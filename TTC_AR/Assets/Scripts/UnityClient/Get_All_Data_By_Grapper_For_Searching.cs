
// using UnityEngine;
// using System;
// using System.Threading.Tasks;
// using PimDeWitte.UnityMainThreadDispatcher;
// public class Get_All_Data_By_Grapper_For_Searching : MonoBehaviour
// {
//     public int grapperId;
//     public EventPublisher eventPublisher; // Tham chiếu đến Publisher
//     private void Awake()
//     {
//         //   if (GlobalVariable.ready_To_Nav_New_Scene) GlobalVariable.ready_To_Nav_New_Scene = false;

//     }
//     private void OnEnable()
//     {
//         if (eventPublisher != null)
//         {
//             eventPublisher.OnButtonClicked += GetAllDataByGrapperForSearching; // Đăng ký sự kiện
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
//             eventPublisher.OnButtonClicked -= GetAllDataByGrapperForSearching; // Hủy đăng ký sự kiện
//         }
//         else
//         {
//             Debug.Log("eventPublisher is null");
//         }
//     }
//     private void Start()
//     {

//     }
//     public async void GetAllDataByGrapperForSearching()
//     {
//         try
//         {

//             GlobalVariable.ready_To_Nav_New_Scene = false;

//             ;//!GlobalVariable.temp_ListRackBasicModels = APIManager.Instance.temp_ListGrapperBasicModels.Find(grapper => grapper.Id == grapperId);

//             Debug.Log(GlobalVariable.temp_ListRackBasicModels.Count);
//             await Task.WhenAll(
//             //GlobalVariable.temp_GrapperBasicModel.Id
//             APIManager.Instance.GetListDeviceData(url: $"{GlobalVariable.baseUrl}Grappers/{grapperId}/devices"),
//             APIManager.Instance.GetListJBData(url: $"{GlobalVariable.baseUrl}Grappers/{grapperId}/jbs")
//                               );
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
