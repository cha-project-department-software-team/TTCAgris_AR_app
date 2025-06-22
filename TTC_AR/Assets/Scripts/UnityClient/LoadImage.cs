using System;
using System.Net.Http;
using System.Threading.Tasks;
using Domain.Entities;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class LoadImage : MonoBehaviour
{
    public static LoadImage Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public Texture2D LoadImageFromFile(string path)
    {
        // Đọc dữ liệu ảnh từ file
        byte[] fileData = System.IO.File.ReadAllBytes(path);
        // Tạo một Texture2D mới
        Texture2D texture = new Texture2D(2, 2);
        // Load ảnh từ fileData
        texture.LoadImage(fileData);
        return texture;
    }
    public async Task LoadImageFromUrlAsync(string imageName, Image image, bool convertToSprite = true)
    {
        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture($"{GlobalVariable.baseUrl}/Images/{imageName}"))
        {
            image.gameObject.SetActive(true);

            if (!await SendWebRequestAsync(webRequest))
            {
                HandleRequestError(webRequest.error);
                return;
            }
            try
            {
                Debug.Log("LoadImageFromUrlAsync");
                Texture2D texture = DownloadHandlerTexture.GetContent(webRequest);
                if (convertToSprite)
                {
                    Sprite sprite = Texture_To_Sprite.ConvertTextureToSprite(texture);
                    image.sprite = sprite;
                }
                Debug.Log("LoadImageFromUrlAsync Done");
            }
            catch (JsonException jsonEx)
            {
                Debug.LogError($"Error parsing JSON from URL: {webRequest.url}, Error: {jsonEx.Message}");
                return;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Unexpected error from URL: {webRequest.url}, Error: {ex}");
                return;
            }
        }
    }

    private async Task<bool> SendWebRequestAsync(UnityWebRequest request)
    {
        try
        {
            request.timeout = 20;
            var operation = request.SendWebRequest();
            while (!operation.isDone)
            {
                await Task.Yield();
            }
            bool isSuccess = request.result == UnityWebRequest.Result.Success;
            GlobalVariable.API_Status = isSuccess;
            Debug.Log($"Request completed with status:" + isSuccess);
            return isSuccess;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error during web request: {ex}");
            GlobalVariable.API_Status = false;
            return false;
        }
    }

    private void HandleRequestError(string error)
    {
        Debug.LogError($"Request error: {error}");
        // throw new Exception(error);
    }
}

