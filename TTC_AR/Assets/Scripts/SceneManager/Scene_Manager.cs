using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour
{
    public static Scene_Manager Instance { get; private set; }

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

    public void SetScreenOrientation()
    {
        Screen.orientation = GlobalVariable.sceneNamesLandScape.Contains(GlobalVariable.recentScene)
            ? ScreenOrientation.LandscapeLeft
            : ScreenOrientation.Portrait;

    }

    public void NavigateToScene(string recentSceneName, string previousSceneName)
    {
        if (GlobalVariable.recentScene != recentSceneName)
        {
            GlobalVariable.recentScene = recentSceneName;
            GlobalVariable.previousScene = previousSceneName;
            SceneManager.LoadScene(recentSceneName, LoadSceneMode.Single);
        }
    }

    public IEnumerator WaitAndNavigate(string recentSceneName, string previousSceneName)
    {
        yield return new WaitUntil(() => GlobalVariable.ready_To_Nav_New_Scene);
        try
        {
            Show_Toast.Instance.ShowToast("loading", "Đang chuyển trang...");

            if (GlobalVariable.recentScene != recentSceneName)
            {
                Show_Toast.Instance.ShowToast("loading", "Đang chuyển trang...");
                NavigateToScene(recentSceneName, previousSceneName);
            }
        }
        catch
        {
            Debug.LogError("Error navigating to scene: " + recentSceneName);
        }
        finally
        {
            StartCoroutine(Show_Toast.Instance.Set_Instance_Status_False());
        }

    }
}