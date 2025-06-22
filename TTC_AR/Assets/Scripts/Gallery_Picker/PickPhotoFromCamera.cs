using EasyUI.Progress;
using UnityEngine;
using UnityEngine.UI;

public class PickPhotoFromCamera : MonoBehaviour
{
    public RawImage confirmImage;
    public Button camera_Option_Btn;


    void Start()
    {
        camera_Option_Btn.onClick.RemoveAllListeners();
        camera_Option_Btn.onClick.AddListener(OpenCameraToTakePhoto);
    }
    public void UpdateConfirmImage(Texture2D savedPhoto)
    {
        if (savedPhoto != null)
        {
            Texture2D rotatedTexture = RotateTexture(savedPhoto, 0f);
            confirmImage.texture = rotatedTexture;
            StartCoroutine(
          Resize_GameObject_Function.Set_NativeSize_For_GameObject(
                confirmImage
            )
       );
        }
    }
  
    private Texture2D RotateTexture(Texture2D originalTexture, float angle)
    {
        int width = originalTexture.width;
        int height = originalTexture.height;
        Texture2D rotatedTexture = new Texture2D(height, width);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                rotatedTexture.SetPixel(y, width - 1 - x, originalTexture.GetPixel(x, y));
            }
        }

        rotatedTexture.Apply();
        return rotatedTexture;
    }


    public void OpenCameraToTakePhoto()
    {
        WebCamPhotoCamera.Instance.ConfirmImageCanvas.SetActive(false);
        GlobalVariable.PickPhotoFromCamera = true;
        WebCamPhotoCamera.Instance.StartCameraToTakePhoto(this);


    }
}
