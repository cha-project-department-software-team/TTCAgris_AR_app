using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using EasyUI.Progress;

public class GetMCCInformation : MonoBehaviour
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private EventPublisher eventPublisher;

    private void Awake()
    {
        eventPublisher.OnButtonClicked += GetMCCInformationModel;
    }

    private void Destroy()
    {
        eventPublisher.OnButtonClicked -= GetMCCInformationModel;
    }

    public async void GetMCCInformationModel()
    {
        try
        {
            StaticVariable.ready_To_Nav_New_Scene = false;
            StaticVariable.ready_To_Update_MCC_UI = false;

            var firstSpaceIndex = title.text.IndexOf(' ');
            var mccCabinetCode = title.text.Substring(firstSpaceIndex + 1);

            //? Mcc tương ứng
            var mcc = StaticVariable.temp_ListMCCInformationModel.Find(mcc => mcc.CabinetCode == mccCabinetCode);

            if (mcc == null)
            {
                // Show_Toast.Instance.ShowToast("error", "Không tìm thấy thông tin mcc!");
                Debug.LogError("Không tìm thấy thông tin mcc!");
                StartCoroutine(ShowToastError.Instance.ShowToast("Vui lòng chọn lại khu vực phù hợp. Thông tin bạn cần tìm không có ở khu vực đã chọn"));
                return;
            }

            await Move_On_Main_Thread.RunOnMainThread(() =>
                      {
                          ShowProgressBar("Đang tải dữ liệu...", "Vui lòng chờ...");
                      });

            await APIManagerOpenCV.Instance.GetMcc(StaticVariable.GetMCCUrl + "/" + mcc.Id);

            await Move_On_Main_Thread.RunOnMainThread(() =>
               {
                   HideProgressBar();
               });

            StaticVariable.ready_To_Nav_New_Scene = true;
            StaticVariable.ready_To_Update_MCC_UI = true;

        }
        catch (Exception)
        {
            StaticVariable.ready_To_Nav_New_Scene = false;
            // Xử lý lỗi và hiển thị thông báo
            await Move_On_Main_Thread.RunOnMainThread(() =>
             {
                 Show_Toast.Instance.ShowToast("failure", "Đã có lỗi xảy ra");
             });
            await Task.Delay(1000);
            await Move_On_Main_Thread.RunOnMainThread(() =>
              {
                  HideProgressBar();
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
