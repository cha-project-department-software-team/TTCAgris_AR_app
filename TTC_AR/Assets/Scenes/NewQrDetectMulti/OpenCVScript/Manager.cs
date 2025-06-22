using TMPro;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using OpenCVForUnity.UnityUtils.Helper;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private TMP_Text title;
    [SerializeField] private ARHelperMulti aRHelper;
    [SerializeField] private Button backBtn;
    [SerializeField] private EventPublisher eventPublisher;
    [SerializeField] private Camera mainCamera;


    private string activeScene;

    public bool IsCanvasOpen => canvas.gameObject.activeSelf;
    public bool enableQRCodeDetection = true;
    private bool enableQRCodeDetectionByActiveCameraIcon = true;

    private void Start()
    {
        activeScene = SceneManager.GetActiveScene().name;
        backBtn.onClick.RemoveAllListeners();
        backBtn.onClick.AddListener(CloseCanvas);
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    private void Update()
    {
        DetectClickedObject();
    }


    public void DetectClickedObject()
    {
        Ray ray = new Ray();

        // Check if the left mouse button was clicked
        if (Input.GetMouseButtonDown(0))
        {
            // Create a ray from the camera to the mouse position
            // Camera cam = Camera.main != null ? Camera.main : Camera.current;
            // if (cam != null)
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        }
        else if (Input.touchCount > 0)
        {
            // Get the first touch
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                // Create a ray from the camera to the touch position
                // Camera cam = Camera.main != null ? Camera.main : Camera.current;
                // if (cam != null)
                ray = mainCamera.ScreenPointToRay(touch.position);

            }
        }

        // Perform the raycast
        if (Physics.Raycast(ray, out var hit, Mathf.Infinity, layerMask: LayerMask.GetMask("Game Object", "UI", "Default")))
        {
            // Get the GameObject that was hit
            GameObject clickedObject = hit.collider.gameObject;
            Vector3 hitPosition = hit.point;

            float tolerance = 0.03f;
            var qrMarker = aRHelper.markers.Values.FirstOrDefault(marker => Vector3.Distance(marker.qrPosition, hitPosition) <= tolerance);
            if (qrMarker != null)
            {
                var key = aRHelper.markers.FirstOrDefault(pair => pair.Value == qrMarker).Key;
                if (key != null)
                {
                    string[] parts = key.Split('_');
                    string result = parts[parts.Length - 1]; // Lấy phần tử cuối cùng
                    GlobalVariable.objectName = result;

                    if (activeScene == "NewQRCodeDetectorMulti")
                    {

                        title.text = "Module " + result;
                        // eventPublisher.TriggerEvent_ButtonClicked();
                        if (
                            GlobalVariable.temp_Dictionary_MCCInformationModel.TryGetValue(result, out var model1)
                            || GlobalVariable.temp_Dictionary_ModuleInformationModel.TryGetValue(result, out var model2)

                            )
                        {
                            OpenCanvas();
                        }
                    }
                    else
                    {
                        title.text = "Tủ " + result;
                        // eventPublisher.TriggerEvent_ButtonClicked();
                        if (
                                                  GlobalVariable.temp_Dictionary_MCCInformationModel.TryGetValue(result, out var model1)
                                                  || GlobalVariable.temp_Dictionary_ModuleInformationModel.TryGetValue(result, out var model2)

                                                  )
                        {
                            OpenCanvas();
                        }
                    }
                }
            }
            else
            {
                Debug.LogWarning("No QRMarker found at the hit position.");
            }
        }
    }

    private void OpenCanvas()
    {
        if (enableQRCodeDetection)
        {
            enableQRCodeDetection = false;
        }
        canvas.SetActive(true);
    }

    public void CloseCanvas()
    {
        if (!enableQRCodeDetection)
        {
            if (enableQRCodeDetectionByActiveCameraIcon)
            {
                enableQRCodeDetection = true;
            }
        }
        canvas.SetActive(false);
    }
    public void setQRCodeDetection(bool enable)
    {
        enableQRCodeDetection = enable;
        enableQRCodeDetectionByActiveCameraIcon = enable;
    }

}
