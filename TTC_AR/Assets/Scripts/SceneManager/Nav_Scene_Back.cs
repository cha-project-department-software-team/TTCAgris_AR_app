using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Nav_Scene_Back : MonoBehaviour
{
    public string previousSceneName;
    public string recentSceneName;
    // [SerializeField]
    // private bool isOrientation = false;
    [SerializeField]
    private GameObject detail_Image;
    private void Awake()
    {
        Scene_Manager.Instance.SetScreenOrientation();
    }
    private void Update()
    {
        if (IsEscapePressed())
        {
            if (SceneManager.GetActiveScene().name.Contains("PLCBox") && detail_Image.activeSelf)
            {
                detail_Image.gameObject.SetActive(false);
            }
            else NavigatePop();
        }
    }

    private bool IsEscapePressed()
    {
        return Input.GetKeyDown(KeyCode.Escape) ||
               (Gamepad.current?.buttonEast?.wasPressedThisFrame ?? false) ||
               (Keyboard.current?.escapeKey.wasPressedThisFrame ?? false);
    }

    public void NavigatePop()
    {
        GlobalVariable.ready_To_Nav_New_Scene = true;
        if (previousSceneName == "UpdateImageScene"
        || previousSceneName == "UpdateGrapperScene")
        {
            var temp = GlobalVariable.previousScene;
            GlobalVariable.previousScene = previousSceneName;
            GlobalVariable.recentScene = temp;
            SceneManager.LoadScene(temp);
            return;
        }
        else if (previousSceneName == "ControlDeviceARScene")
        {
            recentSceneName = GlobalVariable.previousScene;
            GlobalVariable.previousScene = previousSceneName;
            GlobalVariable.recentScene = recentSceneName;
            SceneManager.LoadScene(recentSceneName);
            return;
        }
        else
        {
            Scene_Manager.Instance.NavigateToScene(recentSceneName, previousSceneName);
            return;
        }
    }
}