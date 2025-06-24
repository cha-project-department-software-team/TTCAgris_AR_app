using EasyUI.Progress;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class PickPhotoFromCamera : MonoBehaviour
{
    public RawImage confirmImage;
    public Button camera_Option_Btn;

    private void Start()
    {
        camera_Option_Btn.onClick.RemoveAllListeners();
        camera_Option_Btn.onClick.AddListener(PickPhoto);
    }

    public void UpdateConfirmImage(Texture2D savedPhoto)
    {
        if (savedPhoto == null) return;

        // Texture2D rotatedTexture = RotateTexture90(savedPhoto);
        // confirmImage.texture = rotatedTexture;
        confirmImage.texture = savedPhoto;

        StartCoroutine(Resize_GameObject_Function.Set_NativeSize_For_GameObject(confirmImage));
    }


    //Chỉ giữ nếu thực sự cần xoay ảnh trong tương lai
    private Texture2D RotateTexture90(Texture2D originalTexture)
    {
        int width = originalTexture.width;
        int height = originalTexture.height;

        // Tạo texture mới có chiều hoán đổi
        Texture2D rotatedTexture = new Texture2D(height, width, originalTexture.format, false);
        Color[] originalPixels = originalTexture.GetPixels();
        Color[] rotatedPixels = new Color[originalPixels.Length];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // Vị trí pixel trong ảnh gốc
                int originalIndex = y * width + x;

                // Vị trí tương ứng trong ảnh xoay 90 độ
                int rotatedX = height - 1 - y;
                int rotatedY = x;
                int rotatedIndex = rotatedY * height + rotatedX;

                rotatedPixels[rotatedIndex] = originalPixels[originalIndex];
            }
        }

        rotatedTexture.SetPixels(rotatedPixels);
        rotatedTexture.Apply();
        return rotatedTexture;
    }


    private void OpenCameraToTakePhoto()
    {        // GlobalVariable.PickPhotoFromCamera = true;
        WebCamPhotoCamera.Instance.ConfirmImageCanvas.SetActive(false);
        WebCamPhotoCamera.Instance.StartCameraToTakePhoto(this);
    }

    public void PickPhoto()
    {
        GlobalVariable.PickPhotoFromCamera = true;
        if (Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            OpenCameraToTakePhoto();
        }
        else
        {
            Debug.LogWarning("Permission Not Granted");
            AskPermission();
        }
    }
    public void AskPermission()
    {
        Permission.RequestUserPermission(Permission.Camera);
        PickPhoto();
    }
}
