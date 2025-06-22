using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Domain.Interfaces;
using System.Collections;
using UnityEngine.Networking;
using System.IO;
using UnityEngine;
using Unity.VisualScripting;


namespace Infrastructure.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly HttpClient _httpClient;


        public ImageRepository(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            // _httpClient.BaseAddress = new Uri(GlobalVariable.baseUrl);
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        //! TRả về ImageEntity
        public async Task<ImageEntity> GetImageByIdAsync(int ImageId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{GlobalVariable.baseUrl}/Images/{ImageId}/info");

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Failed to get Image. Status: {response.StatusCode}");
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Debug.Log(content);
                    var entity = JsonConvert.DeserializeObject<ImageEntity>(content);
                    return entity;
                }
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException("Failed to fetch Image", ex); // Ném lỗi HTTP lên UseCase
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unexpected error during HTTP request", ex); // Bao bọc lỗi khác
            }

        }

        //! Trả về List<ImageEntity>
        public async Task<List<ImageEntity>> GetListImageAsync(int grapperId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{GlobalVariable.baseUrl}/Grappers/{grapperId}/images");
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException($"Failed to get Image list. Status: {response.StatusCode}");
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Debug.Log(content);
                    var entities = JsonConvert.DeserializeObject<List<ImageEntity>>(content);
                    return entities;
                }

            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException("Failed to fetch Image list", ex); // Ném lỗi HTTP lên UseCase
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unexpected error during HTTP request", ex); // Bao bọc lỗi khác
            }
        }


        public async Task<bool> CreateNewImageAsync(int grapperId, ImageEntity ImageEntity)
        {
            try
            {
                if (ImageEntity == null)
                    throw new ArgumentNullException(nameof(ImageEntity), "Request data cannot be null");

                // var json = JsonConvert.SerializeObject(ImageEntity);
                // Tạo dữ liệu tối giản gửi lên server với tên property khớp yêu cầu
                // var ImageImageEntity = ConvertImageImageEntity(ImageEntity);
                var json = JsonConvert.SerializeObject(ImageEntity);

                Debug.Log(json);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{GlobalVariable.baseUrl}/Images/grapperId?grapperId={grapperId}&fileName={ImageEntity.Name}", content);

                var temp = await response.Content.ReadAsStringAsync();
                Debug.Log(temp);
                var result = JsonConvert.DeserializeObject<bool>(temp);
                return result;
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException("Failed to create Image", ex); // Ném lỗi HTTP lên UseCase
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unexpected error during HTTP request", ex); // Bao bọc lỗi khác
            }
        }

        public async Task<bool> DeleteImageAsync(int ImageId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{GlobalVariable.baseUrl}/Images/{ImageId}");
                var temp = await response.Content.ReadAsStringAsync();
                Debug.Log(temp);
                var result = JsonConvert.DeserializeObject<bool>(temp);
                return result;
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException("Failed to delete Image", ex); // Ném lỗi HTTP lên UseCase
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unexpected error during HTTP request", ex); // Bao bọc lỗi khác
            }
        }

        public async Task<bool> UploadNewImageFromGallery(int grapperId, Texture2D texture, string fileName, string filePath, string mimeType)
        {
            try
            {
                if (!fileName.Contains(".png") && !fileName.Contains(".jpg"))
                {
                    fileName += ".png";
                }
                if (fileName.Contains(".jpg"))
                {
                    Debug.Log("fileName.Contain jpg");
                    fileName = fileName.Replace(".jpg", ".png");
                }

                Debug.Log("Run Repository 1");

                int targetHeight = 2560;
                if (texture.height == 0) throw new Exception("Texture height is zero!");

                float aspectRatio = (float)texture.width / texture.height;
                int targetWidth = Mathf.Max(1, Mathf.RoundToInt(targetHeight * aspectRatio));

                // Tạo texture mới (readable)
                Texture2D resizedTexture = new Texture2D(targetWidth, targetHeight, TextureFormat.RGBA32, false);

                // Đảm bảo texture gốc readable
                Color[] pixels = texture.GetPixels();
                Color[] resizedPixels = new Color[targetWidth * targetHeight];

                // Bilinear scaling
                for (int y = 0; y < targetHeight; y++)
                {
                    for (int x = 0; x < targetWidth; x++)
                    {
                        float u = x / (float)(targetWidth - 1);
                        float v = y / (float)(targetHeight - 1);
                        int x0 = Mathf.Clamp((int)(u * (texture.width - 1)), 0, texture.width - 1);
                        int y0 = Mathf.Clamp((int)(v * (texture.height - 1)), 0, texture.height - 1);
                        resizedPixels[y * targetWidth + x] = pixels[y0 * texture.width + x0];
                    }
                }
                Debug.Log("Run Repository 1.5");

                resizedTexture.SetPixels(resizedPixels);

                Debug.Log("Run Repository 1.6");
                resizedTexture.Apply();

                Debug.Log("Run Repository 2");
                byte[] fileData = File.ReadAllBytes(filePath);
                Debug.Log("Run Repository 3");
                WWWForm form = new WWWForm();
                Debug.Log("Run Repository 4");
                form.AddBinaryData("file", fileData, fileName, mimeType);
                Debug.Log("Run Repository 5");

                Debug.Log($"imageSize: {resizedTexture.width} x {resizedTexture.height}");

                {
                    using (UnityWebRequest request = UnityWebRequest.Post($"{GlobalVariable.baseUrl}/Images/grapperId?grapperId={grapperId}&fileName={fileName}", form))
                    {
                        Debug.Log("Run Repository 6");
                        request.SendWebRequest();
                        while (!request.isDone)
                        {
                            await Task.Delay(50);
                        }

                        if (request.result != UnityWebRequest.Result.Success)
                        {
                            throw new Exception($"Lỗi upload ảnh: {request.error}");
                        }
                        else
                        {

                            return true;
                        }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                Debug.LogError($"❌ Lỗi upload ảnh: {ex.Message}");
                throw new ApplicationException("Failed to upload image", ex);
            }
            catch (Exception ex)
            {
                Debug.LogError($"❌ Lỗi upload ảnh: {ex.Message}");
                throw new ApplicationException($"Lỗi upload ảnh: {ex.Message}", ex);
            }
        }
        public async Task<bool> UploadNewImageFromCamera(int grapperId, Texture2D texture, string fileName)
        {
            try
            {
                // Ensure proper file extension
                if (!fileName.Contains(".png") && !fileName.Contains(".jpg"))
                {
                    fileName += ".png";
                }
                if (fileName.Contains(".jpg"))
                {
                    fileName = fileName.Replace(".jpg", ".png");
                }

                // Resize the texture to height 2560 while maintaining aspect ratio
                int targetHeight = 2560;
                float aspectRatio = (float)texture.width / texture.height;
                int targetWidth = Mathf.RoundToInt(targetHeight * aspectRatio);

                Texture2D resizedTexture = new Texture2D(targetWidth, targetHeight, texture.format, false);
                Color[] pixels = texture.GetPixels();
                Color[] resizedPixels = new Color[targetWidth * targetHeight];

                // Simple bilinear scaling
                for (int y = 0; y < targetHeight; y++)
                {
                    for (int x = 0; x < targetWidth; x++)
                    {
                        float u = x / (float)(targetWidth - 1);
                        float v = y / (float)(targetHeight - 1);
                        int x0 = (int)(u * (texture.width - 1));
                        int y0 = (int)(v * (texture.height - 1));
                        resizedPixels[y * targetWidth + x] = pixels[y0 * texture.width + x0];
                    }
                }

                resizedTexture.SetPixels(resizedPixels);
                resizedTexture.Apply();

                // Encode the resized texture to PNG
                byte[] imageData = resizedTexture.EncodeToPNG();

                // Clean up
                UnityEngine.Object.Destroy(resizedTexture);

                // Create form data
                WWWForm form = new WWWForm();
                form.AddBinaryData("file", imageData, fileName, "image/png");

                Debug.Log($"UploadNewImageFromCamera: {grapperId} + {fileName}");
                Debug.Log($"imageSize: {resizedTexture.width} x {resizedTexture.height}");

                using (UnityWebRequest request = UnityWebRequest.Post(
                    $"{GlobalVariable.baseUrl}/Images/grapperId?grapperId={grapperId}&fileName={fileName}",
                    form))
                {
                    request.SendWebRequest();

                    while (!request.isDone)
                    {
                        await Task.Delay(50);
                    }

                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        throw new ApplicationException($"Lỗi upload ảnh: {request.error}");
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                Debug.LogError($"❌ Lỗi upload ảnh: {ex.Message}");
                throw new ApplicationException("Failed to upload image", ex);
            }
            catch (Exception ex)
            {
                Debug.LogError($"❌ Lỗi upload ảnh: {ex.Message}");
                throw new ApplicationException("Unexpected error during HTTP request", ex);
            }
        }
    }

    // public IEnumerator UploadNewImage()
    // {
    //     // Chụp ảnh màn hình
    //     string filePath = Path.Combine(Application.persistentDataPath, "screenshot.png");

    //     yield return new WaitForEndOfFrame();
    //     // Đọc dữ liệu ảnh
    //     byte[] imageBytes = File.ReadAllBytes(filePath);

    //     // Tạo DTO
    //     ImageDto imageDto = new ImageDto
    //     {
    //         FileName = "screenshot.png",
    //         ContentType = "image/png",
    //         Size = imageBytes.Length,
    //         Description = "Ảnh chụp từ Unity"
    //     };

    //     // Chuyển DTO thành JSON
    //     string jsonDto = JsonUtility.ToJson(imageDto);

    //     // Tạo form để gửi cả file và DTO
    //     WWWForm form = new WWWForm();
    //     form.AddBinaryData("file", imageBytes, "screenshot.png", "image/png");

    //     form.AddField("metadata", jsonDto);

    //     using (UnityWebRequest www = UnityWebRequest.Post(GlobalVariable.baseUrl, form))
    //     {
    //         yield return www.SendWebRequest();

    //         if (www.result == UnityWebRequest.Result.Success)
    //         {
    //         }
    //         else
    //         {
    //         }
    //     }

    //     // Xóa file tạm
    //     File.Delete(filePath);
    // }
    // private object ConvertImageRequestData(ImageEntity ImageEntity)
    // {
    //     return new
    //     {
    //         name = ImageEntity.Name,
    //         Location = ImageEntity.Location ?? "",
    //         ListDevices = ImageEntity.DeviceEntities?
    //             .Where(d => d != null)
    //             .Select(d => new DeviceBasicDto(d.Id, d.Code))
    //             .ToList() ?? new List<DeviceBasicDto>(),
    //         ListModules = ImageEntity.ModuleEntities
    //             .Where(m => m != null)
    //             .Select(m => new ModuleBasicDto(m.Id, m.Name))
    //             .ToList() ?? new List<ModuleBasicDto>(),
    //         OutdoorImage = ImageEntity.OutdoorImageEntity != null
    //             ? new ImageBasicDto(ImageEntity.OutdoorImageEntity.Id, ImageEntity.OutdoorImageEntity.Name)
    //             : null,
    //         ListConnectionImages = ImageEntity.ConnectionImageEntities?
    //             .Where(i => i != null)
    //             .Select(i => new ImageBasicDto(i.Id, i.Name))
    //             .ToList() ?? new List<ImageBasicDto>()
    //     };
    // }



}