// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Threading.Tasks;
// using UnityEngine;
// using Newtonsoft.Json;
// using UnityEngine.Networking;
// using System.Collections;

// public class Get_Devices_By_Grapper : MonoBehaviour
// {
//     public string grapper;
//     private string filePath;
//     private bool isRunning = false;

//     void Start()
//     {
//     }

//     public async void Get_List_Device_By_Grapper()
//     {
//         GlobalVariable.ready_To_Nav_New_Scene = false;
//         filePath = Path.Combine(Application.streamingAssetsPath, $"Device_Grapper{grapper}.json");
//         if (Application.platform == RuntimePlatform.Android)
//         {
//             await LoadJsonFromAndroid(filePath);
//         }
//         else
//         {
//             await LoadJsonFromFile(filePath);
//         }
//         GlobalVariable.ready_To_Nav_New_Scene = true;
//     }

//     private async Task LoadJsonFromFile(string file)
//     {
//         if (File.Exists(file))
//         {
//             try
//             {
//                 string jsonData = await File.ReadAllTextAsync(file);
//                 ProcessJsonData(jsonData);
//             }
//             catch (Exception e)
//             {
//                 Debug.LogError($"Failed to read JSON file: {e.Message}");
//             }
//         }
//         else
//         {
//             Debug.LogError($"File not found: {filePath}");
//         }
//     }

//     private async Task LoadJsonFromAndroid(string file)
//     {
//         isRunning = true;
//         string androidPath = File.Exists(file) && !string.IsNullOrWhiteSpace(await File.ReadAllTextAsync(file)) ? file : $"jar:file://{Application.dataPath}!/assets/Device_Grapper{grapper}.json";
//         using (UnityWebRequest www = UnityWebRequest.Get(androidPath))
//         {
//             www.timeout = 30;
//             var operation = www.SendWebRequest();

//             while (!operation.isDone)
//             {
//                 await Task.Yield();
//             }

//             if (www.result != UnityWebRequest.Result.Success)
//             {
//                 Debug.LogError($"Failed to load JSON data: {www.error}");
//             }
//             else
//             {
//                 try
//                 {
//                     string jsonData = www.downloadHandler.text;
//                     ProcessJsonData(jsonData);
//                 }
//                 catch (Exception e)
//                 {
//                     Debug.LogError($"Error processing JSON: {e.Message}");
//                 }
//             }
//         }
//         isRunning = false;
//     }

//     private void ProcessJsonData(string jsonData)
//     {
//         try
//         {
//             List<DeviceInformationModel> devices = JsonConvert.DeserializeObject<List<DeviceInformationModel>>(jsonData);
//             if (devices != null && devices.Any() && !string.IsNullOrWhiteSpace(devices[1].Function))
//             {
//                 GlobalVariable_Search_Devices.temp_ListDeviceInformationModelFromModule = devices;
//                 ProcessAndSaveDevices(devices);
//             }
//             else
//             {
//                 Debug.LogError("list thiết bị null hoặc không có đủ dữ liệu hợp lệ.");
//             }
//         }
//         catch (JsonException je)
//         {
//             Debug.LogError($"Failed to deserialize JSON data: {je.Message}");
//         }
//         catch (Exception e)
//         {
//             Debug.LogError($"Unexpected error during JSON processing: {e.Message}");
//         }
//     }

//     private void ProcessAndSaveDevices(List<DeviceInformationModel> devices)
//     {
//         List<string> filteredDevices = GetDeviceForFilter(devices);
//         switch (grapper)
//         {
//             case "A":
//                 GlobalVariable_Search_Devices.devices_Model_For_FilterA = filteredDevices;
//                 break;
//             case "B":
//                 GlobalVariable_Search_Devices.devices_Model_For_FilterB = filteredDevices;
//                 break;
//             case "C":
//                 GlobalVariable_Search_Devices.devices_Model_For_FilterC = filteredDevices;
//                 break;
//             case "D":
//                 GlobalVariable_Search_Devices.devices_Model_For_FilterD = filteredDevices;
//                 break;
//             default:
//                 break;
//         }
//         if (filteredDevices != null && filteredDevices.Any())
//         {
//             Debug.Log($"Filtered devices count: {filteredDevices.Count}");
//         }
//         else
//         {
//             Debug.LogError("Danh sách đã lưu có ít hơn 6 phần tử hoặc null");
//         }
//         if (GlobalVariable_Search_Devices.devices_Model_For_FilterA != null)
//         {
//             StartCoroutine(Show_Toast.Instance.Set_Instance_Status_False());
//         }
//     }

//     private List<string> GetDeviceForFilter(List<DeviceInformationModel> DeviceInformationModels)
//     {
//         List<string> devicesForFilter = new List<string>();

//         foreach (var device in DeviceInformationModels)
//         {
//             if (!string.IsNullOrWhiteSpace(device.Code))
//             {
//                 devicesForFilter.Add(device.Code);
//             }
//             if (!string.IsNullOrWhiteSpace(device.Function))
//             {
//                 devicesForFilter.Add(device.Function);
//             }
//         }

//         return devicesForFilter;
//     }
// }
