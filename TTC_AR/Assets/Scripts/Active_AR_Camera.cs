
using UnityEngine;
using UnityEngine.UI;
public class Active_AR_Camera : MonoBehaviour
{
    [SerializeField]
    private Button active_ARCamera_Button;
    [SerializeField]
    private Button deActive_ARCamera_Button;

    private void Start()
    {
        AddListenerBtn();
    }

    private void Destroy()
    {
        RemoveListenerBtn();
    }

    private void AddListenerBtn()
    {
        active_ARCamera_Button.onClick.AddListener(ResumeCamera);
        deActive_ARCamera_Button.onClick.AddListener(PauseCamera);
    }

    private void RemoveListenerBtn()
    {
        active_ARCamera_Button.onClick.RemoveAllListeners();
        deActive_ARCamera_Button.onClick.RemoveAllListeners();
    }

    private void ResumeCamera()
    {
        GlobalVariable.isCameraPaused = false;
        deActive_ARCamera_Button.gameObject.SetActive(true);
        active_ARCamera_Button.gameObject.SetActive(false);
    }

    private void PauseCamera()
    {
        GlobalVariable.isCameraPaused = true;
        deActive_ARCamera_Button.gameObject.SetActive(false);
        active_ARCamera_Button.gameObject.SetActive(true);
    }

    //     public UnityEngine.UI.Image image;

    //     [SerializeField]
    //     private Camera mainCamera;

    //     // private VuforiaBehaviour vuforiaBehaviour;
    //     private RenderTexture renderTexture;
    //     private Texture2D screenshot;

    //     void Awake()
    //     {
    //         // vuforiaBehaviour = mainCamera.gameObject.GetComponent<VuforiaBehaviour>();
    //         renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
    //         screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
    //     }
    //     private void Start()
    //     {
    //         if (!deActive_ARCamera_Button.gameObject.activeSelf) deActive_ARCamera_Button.gameObject.SetActive(true);
    //         if (active_ARCamera_Button.gameObject.activeSelf) active_ARCamera_Button.gameObject.SetActive(false);
    //         deActive_ARCamera_Button.onClick.AddListener(() => StartCoroutine(PauseARCamera()));
    //         active_ARCamera_Button.onClick.AddListener(ResumeARCamera);
    //     }
    //     private void OnDestroy()
    //     {
    //         deActive_ARCamera_Button.onClick.RemoveListener(() => StartCoroutine(PauseARCamera()));
    //         active_ARCamera_Button.onClick.RemoveListener(ResumeARCamera);
    //     }
    //     private IEnumerator PauseARCamera()
    //     {
    //         yield return TakeScreenshotCoroutine();
    //         // if (vuforiaBehaviour != null)
    //         // {
    //         //     vuforiaBehaviour.enabled = false;  // Tắt AR Camera
    //         //     image.gameObject.SetActive(true);
    //         //     deActive_ARCamera_Button.gameObject.SetActive(false);
    //         //     active_ARCamera_Button.gameObject.SetActive(true);
    //         // }
    //     }


    //     public void ResumeARCamera()
    //     {
    //         // if (vuforiaBehaviour != null)
    //         // {
    //         //     vuforiaBehaviour.enabled = true; // Bật AR Camera
    //         //     image.gameObject.SetActive(false);
    //         //     deActive_ARCamera_Button.gameObject.SetActive(true);
    //         //     active_ARCamera_Button.gameObject.SetActive(false);
    //         // }
    //     }

    //     private IEnumerator TakeScreenshotCoroutine()
    //     {
    //         // Tạo RenderTexture một lần và tái sử dụng nếu có thể
    //         // Save the original culling mask to restore it later
    //         {
    //             renderTexture?.Release();
    //             renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
    //         }
    //         mainCamera.targetTexture = renderTexture;

    //         // Lưu lại culling mask ban đầu để khôi phục sau này
    //         int originalCullingMask = mainCamera.cullingMask;

    //         // Tắt các layer không cần thiết để render
    //         int backBoxLayer = 1 << LayerMask.NameToLayer("BackBox");
    //         int backBoxContentLayer = 1 << LayerMask.NameToLayer("BackBox_Content");
    //         int canvasFrameLayer = 1 << LayerMask.NameToLayer("Canvas_Frame");

    //         // Sử dụng toán tử AND và NOT để loại bỏ các layer không cần thiết
    //         mainCamera.cullingMask &= ~(backBoxLayer | backBoxContentLayer | canvasFrameLayer);

    //         // Render và chờ kết quả
    //         mainCamera.Render();
    //         if (screenshot.width != Screen.width || screenshot.height != Screen.height)
    //         {
    //             screenshot.Reinitialize(Screen.width, Screen.height);
    //         }
    //         // Cập nhật sprite của hình ảnh khi render hoàn tất
    //         RenderTexture.active = renderTexture;
    //         screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
    //         screenshot.Apply();

    //         // Khôi phục culling mask và giải phóng tài nguyên
    //         mainCamera.targetTexture = null;
    //         RenderTexture.active = null;
    //         // Destroy(renderTexture);
    //         // Khôi phục lại culling mask ban đầu
    //         mainCamera.cullingMask = originalCullingMask;
    //         // Gán ảnh vào image
    //         image.sprite = Sprite.Create(screenshot, new Rect(0, 0, screenshot.width, screenshot.height), new Vector2(0.5f, 0.5f));

    //         yield return null;  // Đảm bảo coroutine hoàn tất trong frame tiếp theo
    //     }

}