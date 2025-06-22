using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyUI.Progress;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GetListMCCsFromGrapper : MonoBehaviour
{
    [SerializeField] private EventPublisher eventPublisher;

    private void Start()
    {
        eventPublisher.OnButton_GrapperClicked += GetListMCCsByGrapper;
    }

    private void OnDestroy()
    {
        eventPublisher.OnButton_GrapperClicked -= GetListMCCsByGrapper;
    }

    public async void GetListMCCsByGrapper()
    {
        try
        {
            StaticVariable.ready_To_Nav_New_Scene = false;

            await Move_On_Main_Thread.RunOnMainThread(() =>
                      {
                          ShowProgressBar("Đang tải dữ liệu...", "Vui lòng chờ...");
                      });

            if (StaticVariable.temp_GrapperInformationModel.Name == "GrapperA")
            {
                await APIManagerOpenCV.Instance.GetListMCCs(StaticVariable.GetListMCCsUrl);
                // Debug.Log("GrapperA: " + StaticVariable.temp_ListMCCInformationModel.Count);
            }
            else
            {
                StaticVariable.temp_ListMCCInformationModel.Clear();
                Debug.Log("Grapper null");
            }

            await Move_On_Main_Thread.RunOnMainThread(() =>
               {
                   HideProgressBar();
               });

            StaticVariable.ready_To_Nav_New_Scene = true;
        }
        catch (Exception)
        {
            StaticVariable.ready_To_Nav_New_Scene = false;
            // Xử lý lỗi và hiển thị thông báo
            await Move_On_Main_Thread.RunOnMainThread(() =>
             {
                 Show_Toast.Instance.ShowToast("failure", "Đã có lỗi xảy ra");
             });
            // await Task.Delay(2000);
            await Move_On_Main_Thread.RunOnMainThread(() =>
              {
                  StartCoroutine(Show_Toast.Instance.Set_Instance_Status_False());
              });
        }
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
}
