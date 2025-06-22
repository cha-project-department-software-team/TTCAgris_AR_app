
// using UnityEngine;
// using System;
// using System.Threading.Tasks;
// using System.Collections;
// using PimDeWitte.UnityMainThreadDispatcher;
// using System.Collections.Generic;
// public class Get_MCC_Information : MonoBehaviour
// {
//     public EventPublisher eventPublisher; // Tham chiếu đến Publisher
//     public int mccId = 1;
//     private void Awake()
//     {
//     }

//     private void OnEnable()
//     {
//         if (eventPublisher != null)
//         {
//             eventPublisher.OnButtonClicked += GetMCCInformation; // Đăng ký sự kiện
//         }
//         else
//         {
//             Debug.Log("eventPublisher is null");
//         }
//     }


//     void OnDisable()
//     {
//         if (eventPublisher != null)
//         {
//             eventPublisher.OnButtonClicked -= GetMCCInformation; // Hủy đăng ký sự kiện
//         }
//         else
//         {
//             Debug.Log("eventPublisher is null");
//         }
//     }
//     private void Start()
//     {

//     }
//     public async void GetMCCInformation()
//     {
//         mccId = GlobalVariable.mccId;
//         try
//         {
//             GlobalVariable.ready_To_Nav_New_Scene = false;
//             await Move_On_Main_Thread.RunOnMainThread(() =>
//                       {

//                           Show_Toast.Instance.ShowToast("loading", "Đang tải dữ liệu...");
//                       });
//             await APIManager.Instance.GetMccInformation(
//                 url: $"{GlobalVariable.baseUrl}Mccs/{mccId}"

//             );
//             await APIManager.Instance.DownloadImagesAsync();
//             await APIManager.Instance.GetMccInformation(
//                 url: $"{GlobalVariable.baseUrl}Mccs/{mccId}"
//             );
//             await Move_On_Main_Thread.RunOnMainThread(() =>
//                {
//                    StartCoroutine(Show_Toast.Instance.Set_Instance_Status_False());
//                });
//             GlobalVariable.ready_To_Nav_New_Scene = true;
//         }
//         catch (Exception)
//         {
//             GlobalVariable.ready_To_Nav_New_Scene = false;
//             // Xử lý lỗi và hiển thị thông báo
//             await Move_On_Main_Thread.RunOnMainThread(() =>
//              {
//                  Show_Toast.Instance.ShowToast("failure", "Đã có lỗi xảy ra");
//              });
//             await Task.Delay(2000);
//             await Move_On_Main_Thread.RunOnMainThread(() =>
//               {
//                   StartCoroutine(Show_Toast.Instance.Set_Instance_Status_False());
//               });
//         }
//     }
// }
