// using UnityEngine;
// using System;
// using System.Threading.Tasks;
// using System.Collections;
// using PimDeWitte.UnityMainThreadDispatcher;

// public class Get_Field_Device : MonoBehaviour
// {
//     public EventPublisher eventPublisher; // Tham chiếu đến Publisher

//     private void Awake()
//     {
//     }

//     private void OnEnable()
//     {
//         if (eventPublisher == null)
//         {
//             Debug.LogError("EventPublisher not found!");
//             return;
//         }
//         eventPublisher.OnButtonClicked += Get_Field_Device_Model;
//     }

//     void OnDisable()
//     {
//         if (eventPublisher != null)
//         {
//             Debug.Log("Disable Get_Field_Device_Model");
//             eventPublisher.OnButtonClicked -= Get_Field_Device_Model; // Hủy đăng ký sự kiện
//         }
//         else
//         {
//             Debug.Log("eventPublisher is null");
//         }
//     }

//     private void Start()
//     {
//     }

//     public async void Get_Field_Device_Model()
//     {
//         Debug.Log("Get_Field_Device_Model");
//         try
//         {
//             var cabinet_Name = gameObject.name.Split('_')[1];
//             FieldDeviceInformationModel cabinet = null;

//             if (GlobalVariable.temp_ListFieldDeviceInformationModel != null)
//             {
//                 cabinet = GlobalVariable.temp_ListFieldDeviceInformationModel.Find(cabinet => cabinet.Name == cabinet_Name);
//             }
//             else
//             {
//                 Debug.Log("Get_Field_Device_Model + list field device đang null");
//                 return;
//             }

//             if (cabinet == null)
//             {
//                 Debug.Log("Field device not found.");
//                 return;
//             }

//             GlobalVariable.ready_To_Nav_New_Scene = false;

//             // Cập nhật giao diện phải trên main thread
//             await Move_On_Main_Thread.RunOnMainThread(() =>
//             {
//                 
//                 Show_Toast.Instance.ShowToast("loading", "Đang tải dữ liệu...");
//             });

//             Debug.Log("Get_Field_Device_Model 0 ");

//             await APIManager.Instance.GetFieldDeviceInformation(
//                 url: $"{GlobalVariable.baseUrl}GetFieldDevice",
//                 grapperId: 1, // Update sau
//                 fieldDeviceId: cabinet.Id
//             );

//             Debug.Log("Get_Field_Device_Model 1 ");

//             await APIManager.Instance.DownloadImagesAsync();



//             // Cập nhật trạng thái giao diện trên main thread
//             await Move_On_Main_Thread.RunOnMainThread(() =>
//             {
//                 StartCoroutine(Show_Toast.Instance.Set_Instance_Status_False());
//             });


//             Debug.Log("Get_Field_Device_Model 2");
//             GlobalVariable.ready_To_Nav_New_Scene = true;
//         }
//         catch (Exception ex)
//         {
//             GlobalVariable.ready_To_Nav_New_Scene = false;

//             Debug.LogError("Get_Field_Device_Model + lỗi: " + ex.Message);

//             // Cập nhật lỗi trên main thread
//             await Move_On_Main_Thread.RunOnMainThread(() =>
//             {
//                 Show_Toast.Instance.ShowToast("failure", $"Lỗi: {ex.Message}");
//             });
//         }
//     }

// }
