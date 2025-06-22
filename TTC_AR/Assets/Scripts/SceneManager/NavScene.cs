using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NavScene : MonoBehaviour
{
    public string previousSceneName;
    public List<string> recentSceneName;
    public List<Button> listButton;
    private void Awake()
    {

    }
    private void Start()
    {
        for (int i = 0; i < listButton.Count; i++)
        {
            int localIndex = i;
            listButton[i].onClick.AddListener(() =>
            {
                StartCoroutine(NavigateNewScene(localIndex));
            });
        }
    }

    private IEnumerator NavigateNewScene(int buttonIndex)
    {
        int index = buttonIndex;
        if (recentSceneName[index] != previousSceneName)
        {
            CheckGrapperId(recentSceneName[index]);

            GlobalVariable.ready_To_Nav_New_Scene = true;

            yield return Scene_Manager.Instance.WaitAndNavigate(recentSceneName[index], previousSceneName);
        }
       ;
    }
    private int CheckGrapperId(string recentSceneName)
    {
        if (GlobalVariable.PLCBoxScene.Contains(recentSceneName))
        {
            switch (recentSceneName)
            {
                case "PLCBoxGrapA":
                    GlobalVariable.GrapperId = 1;
                    return 1;
                case "PLCBoxGrapB":
                    GlobalVariable.GrapperId = 2;
                    return 2;
                case "PLCBoxGrapC":
                    GlobalVariable.GrapperId = 3;
                    return 3;
                case "PLCBoxLHGrapA":
                    GlobalVariable.GrapperId = 4;
                    return 4;
                default:
                    return 0;
            }
        }
        else
        {
            return 0;
        }
    }
}