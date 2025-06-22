
// using UnityEngine;
// using System;
// using System.Threading.Tasks;
// using System.Collections;
// using PimDeWitte.UnityMainThreadDispatcher;
// using System.Collections.Generic;
// public class Get_Module_Information : MonoBehaviour
// {
//     public EventPublisher eventPublisher; // Tham chiếu đến Publisher

//     private void Awake()
//     {
//     }

//     private void OnEnable()
//     {
//         if (eventPublisher != null)
//         {
//             eventPublisher.OnButtonClicked += Get_ModuleInformationModel; // Đăng ký sự kiện
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
//             eventPublisher.OnButtonClicked -= Get_ModuleInformationModel; // Hủy đăng ký sự kiện
//         }
//         else
//         {
//             Debug.Log("eventPublisher is null");
//         }
//     }
//     private void Start()
//     {

//     }
//     public async void Get_ModuleInformationModel()
//     {
//         try
//         {

//             GlobalVariable.ready_To_Nav_New_Scene = false;

//             var moduleName = gameObject.name.Split('_')[0];

//             var rackName = $"Rack_{gameObject.name.Substring(1, 1)}";

//             //? Rack tương ứng
//             var rack = GlobalVariable.temp_ListRackBasicModels.Find(rack => rack.Name == rackName);
//             //? Module tương ứng    
//             var module = GlobalVariable.temp_ListModuleBasicModels.Find(module => module.Name == moduleName);

//             if (rack == null || module == null)
//             {
//                 Show_Toast.Instance.ShowToast("error", "Không tìm thấy thông tin rack hoặc module!");
//                 return;
//             }

//             await Move_On_Main_Thread.RunOnMainThread(() =>
//                       {

//                           Show_Toast.Instance.ShowToast("loading", "Đang tải dữ liệu...");
//                       });

//             await APIManager.Instance.GetModuleData(
//                 url: $"{GlobalVariable.baseUrl}Modules/{module.Id}"
//             );
//             await APIManager.Instance.DownloadImagesAsync();

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
