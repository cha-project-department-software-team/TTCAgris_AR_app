using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseGrapperSettingOnClick : MonoBehaviour
{
    public List<string> grapperNames = new List<string>();
    public List<Button> onClickButtons;

    public string previousSceneName = "";
    public string menuSettingName = "";
    // private string  grapperName = "";

    private void Start()
    {

    }

    private void OnEnable()
    {
        for (int i = 0; i < onClickButtons.Count; i++)
        {
            int index = i; // Capture index for closure
            if (onClickButtons[index] != null)
            {
                onClickButtons[index].onClick.AddListener(() => StartHandleButtonClick(index));
            }
        }
    }
    private void StartHandleButtonClick(int index)
    {
        if (index < 0 || index >= grapperNames.Count)
        {
            Debug.LogError("Invalid index for grapperNames.");
            return;
        }

        // !
        // grapperName = grapperNames[index];

        // var grapperInfo = GlobalVariable.temp_Dictionary_GrapperInformationModel.TryGetValue(grapperName, out var info) ? info : null;


        // if (grapperInfo != null)
        // {
        //     GlobalVariable.GrapperId = grapperInfo.Id;
        //     GlobalVariable.GrapperName = grapperInfo.Name;
        // }
        // else
        // {
        //     Debug.LogError($"Grapper information not found for name: {grapperName}");
        //     return;
        // }
        //!

        GlobalVariable.recentScene = menuSettingName;

        GlobalVariable.previousScene = previousSceneName;

        GlobalVariable.ready_To_Nav_New_Scene = true;

        SceneManager.LoadScene(menuSettingName);
    }

    private void OnDisable()
    {
        foreach (var button in onClickButtons)
        {
            if (button != null)
            {
                button.onClick.RemoveAllListeners();
            }
        }
    }
}