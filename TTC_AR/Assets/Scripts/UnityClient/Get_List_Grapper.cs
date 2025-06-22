
// using UnityEngine;
// using System;
// using System.Threading.Tasks;
// using PimDeWitte.UnityMainThreadDispatcher;
// public class Get_List_Grapper : MonoBehaviour
// {
//     private void Awake()
//     {
//         //Get_List_Grapper_Models();
//     }
//     private void Start()
//     {

//     }

//     public async void Get_List_Grapper_Models()
//     {
//         try
//         {
//             await Move_On_Main_Thread.RunOnMainThread(() =>
//              {

//                  Show_Toast.Instance.ShowToast("loading", "Đang tải dữ liệu...");
//              });
//             await Task.WhenAll(
//             APIManager.Instance.GetListGrappers(url: $"{GlobalVariable.baseUrl}grappers")
//             );
//             await Move_On_Main_Thread.RunOnMainThread(() =>
//             {
//                 StartCoroutine(Show_Toast.Instance.Set_Instance_Status_False());
//             });
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
