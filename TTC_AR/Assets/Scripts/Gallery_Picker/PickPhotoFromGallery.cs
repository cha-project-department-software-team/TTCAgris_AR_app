using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Threading.Tasks;
using static NativeGallery;

public class PickPhotoFromGallery : MonoBehaviour
{
    public Permission permission; // Permission to access Camera Roll
    public string imagePath;
    public RawImage image;
    public Button Gallery_Option_Btn;

    [Header("Canvas")]
    public GameObject imageCanvas;
    void Start()
    {
        Gallery_Option_Btn.onClick.RemoveAllListeners();
        Gallery_Option_Btn.onClick.AddListener(() => PickPhoto(image));
    }
    public async void SavePhotoToCameraRoll(Texture2D myTexture, string albumName, string filename)
    {
        Permission result = await Task.Run(() => SaveImageToGallery(myTexture, albumName, filename));
        if (result != Permission.Granted)
        {
            Debug.LogError("Failed to save!");
        }
        else
        {
            //Debug"Photo is saved to Camera Roll on phone device.");
            // Function_onSaved_Return.Invoke(); // Triggered [On Unity Event] in Visual Scripting
        }
    }

    public void PickPhoto(RawImage imageForUpload)
    {
        GlobalVariable.PickPhotoFromCamera = false;
        image.gameObject.SetActive(false);
        if (permission == Permission.Granted)
        {
            GetImageFromGallery((path)
            => HandlePickedPhoto(path, imageForUpload),
            "Select an image", "image/*");
        }
        else
        {
            Debug.LogWarning("Permission Not Granted");
            imagePath = null;
            AskPermission();
        }
    }

    private async void HandlePickedPhoto(string path, RawImage imageForUpload)
    {
        if (string.IsNullOrEmpty(path))
        {
            imagePath = null;
            imageForUpload.texture = null;
        }
        else
        {
            imagePath = path;
            imageForUpload.texture = await LoadSpriteFromPathAsync(path);
            image.gameObject.SetActive(true);
            imageCanvas.SetActive(true);
            StartCoroutine(
          Resize_GameObject_Function.Set_NativeSize_For_GameObject(
            imageForUpload
          )
       );
            // Function_onPicked_Return.Invoke(); // Triggered [On Unity Event] in Visual Scripting
        }
    }

    private async Task<Texture> LoadSpriteFromPathAsync(string path)
    {
        byte[] imageData = await Task.Run(() => File.ReadAllBytes(path));
        Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
        texture.LoadImage(imageData);
        return texture;
    }

    private Texture2D SpriteToTexture2D(Sprite sprite)
    {
        Texture2D texture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height, TextureFormat.RGBA32, false);
        Color[] pixels = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                  (int)sprite.textureRect.y,
                                                  (int)sprite.textureRect.width,
                                                  (int)sprite.textureRect.height);
        texture.SetPixels(pixels);
        texture.Apply();
        return texture;
    }

    public void AskPermission()
    {
        Permission permissionResult = RequestPermission(PermissionType.Read, MediaType.Image);
        permission = permissionResult;

        if (permission == Permission.Granted)
        {
            PickPhoto(image);
        }
    }
}
