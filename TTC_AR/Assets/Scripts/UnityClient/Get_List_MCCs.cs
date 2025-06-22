// using UnityEngine;
// using System;
// using System.Threading.Tasks;
// using System.Collections;
// using PimDeWitte.UnityMainThreadDispatcher;
// public class Get_List_MCCs : MonoBehaviour
// {
//     public string GrapperName = "";
//     public int GrapperId = 1;
//     private void Awake()
//     {
//     }

//     public async void GetListMCCModels(int GrapperId)
//     {
//         try
//         {
//             GlobalVariable.ready_To_Nav_New_Scene = false;
//             // Thực thi các tác vụ giao diện trên Main Thread
//             UnityMainThreadDispatcher.Instance.Enqueue(() =>
//             {

//                 Show_Toast.Instance.ShowToast("loading", "Đang tải dữ liệu...");
//             });

//             // Chờ API hoàn thành
//             await Task.WhenAll(
//                 APIManager.Instance.GetListMCCModels(
//                     url: $"{GlobalVariable.baseUrl}Grappers/{GrapperId}/mccs"
//                 // GlobalVariable.temp_ListGrapperBasicModels[0].Id
//                 )
//             );

//             // Đăng thông báo thành công
//             Debug.Log("GetListMCCModels completed.");

//             // Cập nhật trạng thái giao diện
//             UnityMainThreadDispatcher.Instance.Enqueue(() =>
//             {
//                 StartCoroutine(Show_Toast.Instance.Set_Instance_Status_False());
//             });
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
