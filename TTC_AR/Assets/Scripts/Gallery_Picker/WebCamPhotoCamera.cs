using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using EasyUI.Progress;

public class WebCamPhotoCamera : MonoBehaviour
{
    public static WebCamPhotoCamera Instance { get; private set; }

    [Header("Camera Settings")]
    [SerializeField] private WebCamTexture webCamTexture;
    [SerializeField] private Texture2D capturedPhoto;

    [Header("Canvas")]
    public GameObject ConfirmImageCanvas;

    [Header("Take Photo Module")]
    public GameObject TakePhotoModule;
    public RawImage CameraScreen;
    public Button TakeButton;
    public Button CancelTakePhotoButton;
    public AspectRatioFitter CaptureScreenAspectRatioFitter;

    [Header("Preview Photo Module")]
    public GameObject PreviewPhotoModule;
    public RawImage PreviewImage;
    public Button AcceptButton;
    public Button RetakeButton;
    public AspectRatioFitter PreviewImageAspectRatioFitter;

    // [Header("Flash Control")]
    // [SerializeField] private Button FlashToggleButton;
    // [SerializeField] private Sprite FlashOnSprite;
    // [SerializeField] private Sprite FlashOffSprite;

    [Header("Brightness Control")]
    public Slider BrightnessSlider;
    public float BrightnessValue = 1.5f; // Default brightness value
    private PickPhotoFromCamera pickPhotoFromCamera;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void OnEnable()
    {
        RegisterUIEvents();
        InitializeBrightnessSlider();
        SetUIElementsActive(false);
    }

    private void OnDisable()
    {
        UnregisterUIEvents();
    }

    private void RegisterUIEvents()
    {
        TakeButton.onClick.AddListener(TakePhoto);
        AcceptButton.onClick.AddListener(() => StartCoroutine(AcceptPhoto()));
        RetakeButton.onClick.AddListener(RetakePhoto);
        CancelTakePhotoButton.onClick.AddListener(StopCamera);
        BrightnessSlider.onValueChanged.AddListener(OnBrightnessChanged);
    }

    private void UnregisterUIEvents()
    {
        TakeButton.onClick.RemoveAllListeners();
        AcceptButton.onClick.RemoveAllListeners();
        RetakeButton.onClick.RemoveAllListeners();
        CancelTakePhotoButton.onClick.RemoveAllListeners();
        // FlashToggleButton.onClick.RemoveAllListeners();
        BrightnessSlider.onValueChanged.RemoveAllListeners();
    }

    private void InitializeBrightnessSlider()
    {
        BrightnessSlider.maxValue = 3f;
        BrightnessSlider.minValue = 0f;
        BrightnessSlider.value = BrightnessValue;
    }

    public void SetUIElementsActive(bool isActive)
    {
        TakePhotoModule.SetActive(isActive);
        PreviewPhotoModule.SetActive(isActive);
    }

    public void StartCameraToTakePhoto(PickPhotoFromCamera pickPhotoFromCamera)
    {
        this.pickPhotoFromCamera = pickPhotoFromCamera;
        SetUIElementsActive(true);

        if (WebCamTexture.devices.Length > 0)
        {
            WebCamDevice selectedDevice = SelectCameraDevice();
            webCamTexture = new WebCamTexture(selectedDevice.name, 1440, 3088, 30);
            CameraScreen.texture = webCamTexture;
            webCamTexture.Play();
            // StartCoroutine(AdjustAspectRatio());
        }
        else
        {
            Debug.LogError("No camera found!");
        }
    }

    private WebCamDevice SelectCameraDevice()
    {
        foreach (WebCamDevice device in WebCamTexture.devices)
        {
            if (!device.isFrontFacing)
                return device;
        }
        return WebCamTexture.devices[0];
    }

    public void StopCamera()
    {
        SetUIElementsActive(false);
        if (webCamTexture != null)
        {
            webCamTexture.Stop();
            webCamTexture = null;
        }
        pickPhotoFromCamera = null;
    }

    // private IEnumerator AdjustAspectRatio()
    // {
    //     yield return new WaitUntil(() => webCamTexture.width > 100);
    //     float videoRatio = (float)webCamTexture.width / webCamTexture.height;
    //     CaptureScreenAspectRatioFitter.aspectRatio = videoRatio;
    //     PreviewImageAspectRatioFitter.aspectRatio = videoRatio;
    // }

    private void TakePhoto()
    {
        StartCoroutine(CapturePhoto());
        TakePhotoModule.SetActive(false);
        PreviewPhotoModule.SetActive(true);
    }

    private IEnumerator CapturePhoto()
    {
        yield return new WaitForEndOfFrame();
        capturedPhoto = new Texture2D(webCamTexture.width, webCamTexture.height);
        capturedPhoto.SetPixels(webCamTexture.GetPixels());
        capturedPhoto.Apply();
        PreviewImage.texture = capturedPhoto;
    }

    public IEnumerator AcceptPhoto()
    {
        ShowProgressBar("Đang tải ảnh...", "...");

        if (capturedPhoto != null && pickPhotoFromCamera != null)
        {
            ConfirmImageCanvas.SetActive(true);
            pickPhotoFromCamera.UpdateConfirmImage(capturedPhoto);
        }
        yield return new WaitForSeconds(1f);

        StopCamera();

        yield return new WaitForSeconds(0.5f);
        HideProgressBar();
    }


    private void ShowProgressBar(string title, string details)
    {
        Progress.Show(title, ProgressColor.Blue, true);
        Progress.SetDetailsText(details);
    }
    private void HideProgressBar()
    {
        Progress.Hide();
    }
    public void RetakePhoto()
    {
        TakePhotoModule.SetActive(true);
        PreviewPhotoModule.SetActive(false);
        webCamTexture.Play();
    }




    private void UpdateFlashButtonUI()
    {
        // FlashToggleButton.GetComponent<Image>().sprite = isFlashOn ? FlashOnSprite : FlashOffSprite;
    }



    private void OnBrightnessChanged(float value)
    {
        BrightnessValue = value;
        if (CameraScreen.material != null)
        {
            CameraScreen.material.SetFloat("_Brightness", BrightnessValue);
        }
    }

    private void OnDestroy()
    {
    }
}
