
// using UnityEngine;
// using System;
// using System.Threading.Tasks;
// using System.Collections;
// using System.Collections.Generic;
// public class Get_List_Modules_By_Grapper : MonoBehaviour
// {
//     public async void GetListModuleByGrapper()
//     {
//         try
//         {
//             GlobalVariable.ready_To_Nav_New_Scene = false;

//             await Move_On_Main_Thread.RunOnMainThread(() =>
//                       {

//                           Show_Toast.Instance.ShowToast("loading", "Đang tải dữ liệu...");
//                       });

//             await APIManager.Instance.GetListModuleData(
//                 url: GlobalVariable.baseUrl + $"Grappers/{GlobalVariable.GrapperId}/modules"
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
