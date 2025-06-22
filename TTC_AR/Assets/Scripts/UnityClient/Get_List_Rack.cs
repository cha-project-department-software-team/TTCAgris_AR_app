
// using UnityEngine;
// using System;
// using System.Threading.Tasks;
// using PimDeWitte.UnityMainThreadDispatcher;
// public class Get_List_Rack : MonoBehaviour
// {
//     public int grapperId = 1;
//     private void Awake()
//     {
//         grapperId = GlobalVariable.GrapperId;
//         Get_List_Rack_Basic_Models();
//     }
//     private void Start()
//     {

//     }
//     public async void Get_List_Rack_Basic_Models()
//     {
//         try
//         {
//             await Move_On_Main_Thread.RunOnMainThread(() =>
//              {

//                  Show_Toast.Instance.ShowToast("loading", "Đang tải dữ liệu...");
//              });
//             await Task.WhenAll(
//             APIManager.Instance.GetListRacks(url: $"{GlobalVariable.baseUrl}Grappers/{grapperId}/racks")
//             );
//             UnityMainThreadDispatcher.Instance.Enqueue(() =>
//               {
//                   StartCoroutine(Show_Toast.Instance.Set_Instance_Status_False());

//               });
//             Debug.Log("Get_List_Grapper_Models completed.");
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
