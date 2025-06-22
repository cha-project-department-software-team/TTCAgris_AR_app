// using System;
// using System.Collections.Generic;
// using System.Text;
// using System.Threading.Tasks;
// using Newtonsoft.Json;
// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.Networking;
// using System.Threading;
// using UnityEngine.SceneManagement;
// using PimDeWitte.UnityMainThreadDispatcher;
// using System.Linq;
// using System.Net.Http;
// using ApplicationLayer.Dtos;
// using ApplicationLayer.Dtos.JB;
// using ApplicationLayer.Interfaces;
// using static ModuleInformationModel;

// public class APIManager : MonoBehaviour
// {
//     public static APIManager Instance { get; private set; }

//     #region Services
//     private IGrapperService _grappersService;
//     private IRackService _rackService;
//     private IJBService _jbService;
//     private IDeviceService _deviceService;
//     private IModuleService _moduleService;
//     private IFieldDeviceService _fieldDeviceService;
//     private IMccService _mccService;
//     private IAdapterSpecificationService _adapterSpecificationService;
//     private IModuleSpecificationService _moduleSpecificationService;
//     #endregion

//     public int CompanyId = 1;
//     public int GrapperId = 1;
//     public int rackId = 1;
//     public int moduleId = 1;
//     public int deviceId = 1;
//     public int JBId = 1;
//     public int fieldDeviceId = 1;
//     public int mccId = 1;

//     public List<RackBasicModel> temp_ListRackBasicModels;
//     public List<GrapperBasicModel> temp_ListGrapperBasicModels;
//     public List<ModuleInformationModel> temp_ListModuleInformationModel = new List<ModuleInformationModel>();
//     public ModuleInformationModel temp_ModuleInformationModel;
//     public ModuleSpecificationBasicModel temp_ModuleSpecificationBasicModel;
//     public ModuleSpecificationModel temp_ModuleSpecificationModel;
//     public AdapterSpecificationModel temp_AdapterSpecificationModel;

//     public List<MccInformationModel> temp_ListMCCInformationModel = new List<MccInformationModel>();
//     public MccInformationModel temp_MccInformationModel;
//     public List<FieldDeviceInformationModel> temp_ListFieldDeviceInformationModel = new List<FieldDeviceInformationModel>();
//     public FieldDeviceInformationModel temp_FieldDeviceInformationModel;
//     public List<JBInformationModel> temp_ListJBInformationModelFromDevice = new List<JBInformationModel>();
//     public List<DeviceInformationModel> temp_ListDeviceInformationModelFromModule_FromModule = new List<DeviceInformationModel>();
//     public Dictionary<string, List<Texture2D>> list_JBConnectionImagesFromModule = new Dictionary<string, List<Texture2D>>();
//     public Dictionary<string, Texture2D> list_JBLocationImagesFromModule = new Dictionary<string, Texture2D>();
//     public List<string> imageUrls = new List<string>();
//     public List<Texture2D> textures = new List<Texture2D>();
//     public Dictionary<string, int> Dic_GrapperBasicNonListRackModels = new Dictionary<string, int>();


//     //! Dictionary
//     public Dictionary<string, DeviceInformationModel> Dic_DeviceInformationModels = new Dictionary<string, DeviceInformationModel>();
//     public Dictionary<string, JBInformationModel> Dic_JBInformationModels = new Dictionary<string, JBInformationModel>();
//     public Dictionary<string, ModuleInformationModel> Dic_ModuleInformationModels = new Dictionary<string, ModuleInformationModel>();
//     public Dictionary<string, MccInformationModel> Dic_MCCInformationModels = new Dictionary<string, MccInformationModel>();
//     public Dictionary<string, FieldDeviceInformationModel> Dic_FieldDeviceInformationModels = new Dictionary<string, FieldDeviceInformationModel>();
//     public Dictionary<string, ModuleSpecificationModel> Dic_ModuleSpecificationModels = new Dictionary<string, ModuleSpecificationModel>();
//     public Dictionary<string, AdapterSpecificationModel> Dic_AdapterSpecificationModels = new Dictionary<string, AdapterSpecificationModel>();

//     private void Awake()
//     {
//         if (Instance == null)
//         {
//             Instance = this;
//             DontDestroyOnLoad(gameObject);
//         }
//         else
//         {
//             Destroy(gameObject);
//         }
//     }
//     private void Start()
//     {
//         //! Dependency Injection
//         // Lấy IJBService từ ServiceLocator
//         _jbService = ServiceLocator.Instance.JBService;
//         // Lấy IDeviceService từ ServiceLocator
//         _deviceService = ServiceLocator.Instance.DeviceService;
//         // Lấy IModuleService từ ServiceLocator
//         _moduleService = ServiceLocator.Instance.ModuleService;
//         // Lấy IFieldDeviceService từ ServiceLocator
//         _fieldDeviceService = ServiceLocator.Instance.FieldDeviceService;
//         // Lấy IMccService từ ServiceLocator
//         _mccService = ServiceLocator.Instance.MccService;
//         // Lấy IAdapterSpecificationService từ ServiceLocator
//         _adapterSpecificationService = ServiceLocator.Instance.AdapterSpecificationService;
//         // Lấy IModuleSpecificationService từ ServiceLocator
//         _moduleSpecificationService = ServiceLocator.Instance.ModuleSpecificationService;
//         // Lấy IGrapperService từ ServiceLocator
//         _grappersService = ServiceLocator.Instance.GrapperService;
//         // Lấy IRackService từ ServiceLocator
//         _rackService = ServiceLocator.Instance.RackService;

//     }

//     public async Task GetListGrappers(string url)
//     {
//         using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
//         {
//             if (!await SendWebRequestAsync(webRequest))
//             {
//                 HandleRequestError(webRequest.error);
//                 return;
//             }
//             try
//             {
//                 string json = webRequest.downloadHandler.text;
//                 Debug.Log("GetListGrappers: " + json);

//                 var grapperModels = JsonConvert.DeserializeObject<List<GrapperBasicModel>>(json);
//                 if (grapperModels == null || grapperModels.Count == 0) return;

//                 GlobalVariable.temp_ListGrapperBasicModels = grapperModels;
//                 Dic_GrapperBasicNonListRackModels.Clear();

//                 foreach (var grapper in grapperModels)
//                 {
//                     Dic_GrapperBasicNonListRackModels.TryAdd(grapper.Name, grapper.Id);
//                 }

//                 Debug.Log($"Total grappers: {GlobalVariable.temp_ListGrapperBasicModels.Count}");
//             }
//             catch (JsonException jsonEx)
//             {
//                 HandleRequestError($"JSON error: {jsonEx.Message}");
//             }
//             catch (Exception ex)
//             {
//                 HandleRequestError($"Unexpected error: {ex.Message}");
//             }
//         }
//     }
//     public async Task GetListRacks(string url)
//     {
//         using UnityWebRequest webRequest = UnityWebRequest.Get(url);
//         {
//             if (!await SendWebRequestAsync(webRequest))
//             {
//                 HandleRequestError(webRequest.error);
//                 return;
//             }

//             try
//             {
//                 Debug.Log("GetListRacks:+ " + webRequest.downloadHandler.text);
//                 var rack_models = JsonConvert.DeserializeObject<List<RackBasicModel>>(webRequest.downloadHandler.text);
//                 if (rack_models != null && rack_models.Any())
//                 {
//                     temp_ListRackBasicModels = rack_models;
//                     GlobalVariable.temp_ListRackBasicModels = rack_models;
//                 }
//             }
//             catch (JsonException jsonEx)
//             {
//                 HandleRequestError(jsonEx.Message);
//                 return;
//             }
//             catch (Exception ex)
//             {
//                 HandleRequestError(ex.Message);
//                 return;
//             }
//         }
//     }
//     public async Task GetListMCCModels(string url)
//     {
//         using UnityWebRequest webRequest = UnityWebRequest.Get(url);
//         {
//             if (!await SendWebRequestAsync(webRequest))
//             {
//                 HandleRequestError(webRequest.error);
//                 return;
//             }
//             try
//             {
//                 var List_MCC_Models = JsonConvert.DeserializeObject<List<MccInformationModel>>(webRequest.downloadHandler.text);
//                 if (List_MCC_Models != null)
//                 {
//                     GlobalVariable.temp_ListMCCInformationModel = new List<MccInformationModel>();
//                     temp_ListMCCInformationModel = List_MCC_Models;
//                     GlobalVariable.temp_ListMCCInformationModel = temp_ListMCCInformationModel;
//                     Debug.Log("list_MCC_Models.Count: " + List_MCC_Models.Count);
//                 }
//                 Debug.Log("success");
//             }
//             catch (JsonException jsonEx)
//             {
//                 HandleRequestError(jsonEx.Message);
//                 return;
//             }
//             catch (Exception ex)
//             {
//                 HandleRequestError(ex.Message);
//                 return;
//             }
//         }
//     }
//     public async Task GetMccInformation(string url)
//     {
//         using UnityWebRequest webRequest = UnityWebRequest.Get(url);
//         {
//             if (!await SendWebRequestAsync(webRequest))
//             {
//                 HandleRequestError(webRequest.error);
//                 return;
//             }
//             try
//             {
//                 var mccModel = JsonConvert.DeserializeObject<MccInformationModel>(webRequest.downloadHandler.text);

//                 if (mccModel != null)
//                 {
//                     mccId = mccModel.Id;
//                     GlobalVariable.temp_MCCInformationModel = mccModel;
//                     //!   GlobalVariable.temp_FieldDeviceInformationModel = mccModel.FieldDeviceInformationModel[0];
//                     GlobalVariable.fieldDeviceId = GlobalVariable.temp_FieldDeviceInformationModel.Id;
//                     GlobalVariable.mccId = mccId;
//                 }
//                 Debug.Log("success");
//             }
//             catch (JsonException jsonEx)
//             {
//                 HandleRequestError(jsonEx.Message);
//                 return;
//             }
//             catch (Exception ex)
//             {
//                 HandleRequestError(ex.Message);
//                 return;
//             }
//         }
//     }


//     // public async Task GetListModuleInformation(string url, int grapperId)
//     // {
//     //     using UnityWebRequest webRequest = UnityWebRequest.Get(url);
//     //     {
//     //         if (!await SendWebRequestAsync(webRequest))
//     //         {
//     //             HandleRequestError(webRequest.error);
//     //             return;
//     //         }

//     //         try
//     //         {
//     //             var list_moduleInformationModel = JsonConvert.DeserializeObject<List<ModuleInformationModel>>(webRequest.downloadHandler.text);
//     //             if (list_moduleInformationModel != null)
//     //             {
//     //                 temp_ListModuleInformationModel = list_moduleInformationModel;
//     //                 GlobalVariable.temp_ListModuleInformationModel = temp_ListModuleInformationModel;
//     //                 foreach (var module in list_moduleInformationModel)
//     //                 {
//     //                     Dic_ModuleInformationModels.TryAdd(module.Name, module);
//     //                 }
//     //                 GlobalVariable.temp_Dictionary_ModuleInformationModel = Dic_ModuleInformationModels;
//     //             }

//     //             Debug.Log("success" + temp_ListModuleInformationModel.Count);
//     //         }
//     //         catch (JsonException jsonEx)
//     //         {
//     //             HandleRequestError(jsonEx.Message);
//     //             return;
//     //         }
//     //         catch (Exception ex)
//     //         {
//     //             HandleRequestError(ex.Message);
//     //             return;
//     //         }
//     //     }
//     // }

//     // Get list Devices by Grapper ==> Search Devices
//     // public async Task GetModuleInformation(string url, int moduleId)
//     // {
//     //     using UnityWebRequest webRequest = UnityWebRequest.Get(url);
//     //     {
//     //         GlobalVariable.ActiveCloseCanvasButton = false;
//     //         if (!await SendWebRequestAsync(webRequest))
//     //         {
//     //             HandleRequestError(webRequest.error);
//     //             return;
//     //         }

//     //         try
//     //         {
//     //             var moduleInformationModel = JsonConvert.DeserializeObject<ModuleInformationModel>(webRequest.downloadHandler.text);
//     //             if (moduleInformationModel != null)
//     //             {
//     //                 GlobalVariable.moduleId = 1;
//     //                 GlobalVariable.moduleSpecificationId = 1;
//     //                 GlobalVariable.adapterSpecificationId = 1;

//     //                 GlobalVariable.temp_ListJBInformationModelFromModule_FromModule.Clear();
//     //                 GlobalVariable.temp_ListDeviceInformationModelFromModule_FromModule.Clear();

//     //                 GlobalVariable.temp_ModuleInformationModel = null;
//     //                 GlobalVariable.temp_ModuleSpecificationBasicModel = null;
//     //                 GlobalVariable.temp_AdapterSpecificationModel = null;

//     //                 temp_ModuleInformationModel = moduleInformationModel;
//     //                 temp_ListJBInformationModelFromDevice = temp_ModuleInformationModel.ListJBInformationModel;
//     //                 temp_ListDeviceInformationModelFromModule_FromModule = temp_ModuleInformationModel.ListDeviceInformationModel_FromModule;
//     //                 ModuleId = temp_ModuleInformationModel.Id;
//     //                 temp_ListJBInformationModel_From_Module = temp_ModuleInformationModel.ListJBInformationModel;
//     //                 temp_ListDeviceInformationModel_FromModule = temp_ModuleInformationModel.ListDeviceInformationModel_FromModule;
//     //                 moduleId = temp_ModuleInformationModel.Id;

//     //                 GlobalVariable.temp_ModuleInformationModel = temp_ModuleInformationModel;

//     //                 GlobalVariable.moduleId = moduleId;
//     //                 GlobalVariable.moduleSpecificationId = moduleInformationModel.ModuleSpecificationModel.Id;
//     //                 GlobalVariable.adapterSpecificationId = moduleInformationModel.AdapterSpecificationModel.Id;

//     //                 GlobalVariable.temp_ListJBInformationModelFromModule_FromModule = temp_ListJBInformationModelFromDevice;
//     //                 GlobalVariable.temp_ListDeviceInformationModelFromModule_FromModule = temp_ListDeviceInformationModelFromModule_FromModule;

//     //                 GlobalVariable.temp_ModuleSpecificationModel = moduleInformationModel.ModuleSpecificationModel;
//     //                 GlobalVariable.temp_AdapterSpecificationModel = moduleInformationModel.AdapterSpecificationModel;


//     //             }

//     //             Debug.Log("success");
//     //         }
//     //         catch (JsonException jsonEx)
//     //         {
//     //             HandleRequestError(jsonEx.Message);
//     //             return;
//     //         }
//     //         catch (Exception ex)
//     //         {
//     //             HandleRequestError(ex.Message);
//     //             return;
//     //         }
//     //     }
//     // }
//     public async Task GetModuleSpecification(string url)
//     {
//         using UnityWebRequest webRequest = UnityWebRequest.Get(url);
//         {
//             if (!await SendWebRequestAsync(webRequest))
//             {
//                 HandleRequestError(webRequest.error);
//                 return;
//             }

//             try
//             {
//                 var moduleSpecificationModel = JsonConvert.DeserializeObject<ModuleSpecificationModel>(webRequest.downloadHandler.text);
//                 if (moduleSpecificationModel != null)
//                 {
//                     temp_ModuleSpecificationModel = moduleSpecificationModel;
//                     //!temp_AdapterSpecificationModel = moduleSpecificationModel.Adapter;
//                     GlobalVariable.temp_ModuleSpecificationModel = moduleSpecificationModel;
//                     //!  GlobalVariable.temp_AdapterSpecificationModel = moduleSpecificationModel.Adapter;
//                 }
//                 Debug.Log("success");
//             }
//             catch (JsonException jsonEx)
//             {
//                 HandleRequestError(jsonEx.Message);
//                 return;
//             }
//             catch (Exception ex)
//             {
//                 HandleRequestError(ex.Message);
//                 return;
//             }
//         }
//     }

//     private SemaphoreSlim _semaphore = new SemaphoreSlim(10);

//     public async Task DownloadImagesAsync()
//     {
//         GlobalVariable.ActiveCloseCanvasButton = false;
//         try
//         {

//             if (SceneManager.GetActiveScene().name == MyEnum.GrapperAScanScene.GetDescription())
//             {

//                 if (temp_ListJBInformationModelFromDevice == null || temp_ListJBInformationModelFromDevice.Count == 0)
//                 {
//                     Debug.LogWarning("No JB information models available to download images.");

//                 }
//                 else
//                 {
//                     Debug.Log("temp_ListJBInformationModelFromDevice.Count: " + temp_ListJBInformationModelFromDevice.Count);

//                     var downloadTasks = new List<Task<Texture2D>>();

//                     foreach (var jb in temp_ListJBInformationModelFromDevice)
//                     {
//                         // Khởi tạo các danh sách nếu chưa tồn tại
//                         if (!list_JBConnectionImagesFromModule.ContainsKey(jb.Name))
//                         {
//                             list_JBConnectionImagesFromModule[jb.Name] = new List<Texture2D>();
//                         }

//                         if (!list_JBLocationImagesFromModule.ContainsKey(jb.Name))
//                         {
//                             list_JBLocationImagesFromModule[jb.Name] = new Texture2D(2, 2);
//                         }

//                         //? Tải hình ảnh ngoài trời
//                         if (!string.IsNullOrEmpty(jb.OutdoorImage.Name))
//                         {
//                             downloadTasks.Add(DownloadImageAsync(jb.OutdoorImage.Name));
//                         }

//                         //? Tải danh sách hình ảnh kết nối
//                         foreach (var image in jb.ListConnectionImages)
//                         {
//                             if (!string.IsNullOrEmpty(image.Name))
//                             {
//                                 downloadTasks.Add(DownloadImageAsync(image.Name));
//                             }
//                         }

//                         //? Chờ tất cả hình ảnh kết nối hoàn tất
//                         var downloadedTextures = await Task.WhenAll(downloadTasks);

//                         //? Cập nhật danh sách hình ảnh kết nối trên Main Thread
//                         UnityMainThreadDispatcher.Instance.Enqueue(() =>
//                         {
//                             if (!string.IsNullOrEmpty(jb.OutdoorImage.Name))
//                             {
//                                 list_JBLocationImagesFromModule[jb.Name] = downloadedTextures[0];
//                                 list_JBConnectionImagesFromModule[jb.Name].AddRange(downloadedTextures.Skip(1));
//                             }
//                             else
//                             {
//                                 list_JBConnectionImagesFromModule[jb.Name].AddRange(downloadedTextures);
//                             }
//                         });
//                         //? Dọn danh sách nhiệm vụ sau mỗi JB
//                         downloadTasks.Clear();
//                     }

//                     // Cập nhật biến toàn cục trên Main Thread
//                     UnityMainThreadDispatcher.Instance.Enqueue(() =>
//                     {
//                         GlobalVariable.temp_listJBConnectionImageFromModule = new Dictionary<string, List<Texture2D>>(list_JBConnectionImagesFromModule);
//                         GlobalVariable.temp_listJBLocationImageFromModule = new Dictionary<string, Texture2D>(list_JBLocationImagesFromModule);
//                         Debug.Log("All images downloaded and updated in global variables.");
//                         list_JBConnectionImagesFromModule.Clear();
//                         list_JBLocationImagesFromModule.Clear();
//                     });
//                 }
//             }
//             else if (SceneManager.GetActiveScene().name == MyEnum.FieldDevicesScene.GetDescription())
//             {
//                 temp_FieldDeviceInformationModel = GlobalVariable.temp_FieldDeviceInformationModel;

//                 var downloadTasks = new List<Task<Texture2D>>();

//                 foreach (var image_url in temp_FieldDeviceInformationModel.ListConnectionImages)
//                 {
//                     if (!string.IsNullOrEmpty(image_url.Name))
//                     {
//                         downloadTasks.Add(DownloadImageAsync(image_url.Name));
//                     }
//                 }
//                 var downloadedTextures = await Task.WhenAll(downloadTasks);

//                 // Cập nhật danh sách hình ảnh kết nối trên Main Thread
//                 UnityMainThreadDispatcher.Instance.Enqueue(() =>
//                 {
//                     GlobalVariable.temp_ListFieldDeviceConnectionImages = new List<Texture2D>(downloadedTextures);
//                     Debug.Log("All field device connection images downloaded and updated.");
//                 });

//                 downloadTasks.Clear();
//             }


//             GlobalVariable.ActiveCloseCanvasButton = true;
//         }
//         catch (Exception ex)
//         {
//             Debug.LogError($"Unexpected error in DownloadImagesAsync: {ex.Message}");
//             GlobalVariable.ActiveCloseCanvasButton = false;
//         }
//     }

//     private async Task<Texture2D> DownloadImageAsync(string name)
//     {

//         await _semaphore.WaitAsync();
//         try
//         {
//             using (var request = UnityWebRequestTexture.GetTexture($"{GlobalVariable.baseUrl}files/{name}"))
//             {
//                 if (!await SendWebRequestAsync(request))
//                 {
//                     Debug.LogError($"Request error from URL: {request.url}, Error: {request.error}");
//                 }

//                 var texture = DownloadHandlerTexture.GetContent(request);
//                 Debug.Log($"Image downloaded from: {name}");
//                 return texture;
//             }
//         }
//         catch (Exception ex)
//         {
//             Debug.LogError($"Unexpected error while downloading image from {name}: {ex}");
//             return null;
//         }
//         finally
//         {
//             _semaphore.Release();
//         }
//     }

//     private async Task<bool> SendWebRequestAsync(UnityWebRequest request)
//     {
//         try
//         {
//             request.timeout = 20;
//             var operation = request.SendWebRequest();
//             while (!operation.isDone)
//             {
//                 await Task.Yield();
//             }
//             bool isSuccess = request.result == UnityWebRequest.Result.Success;
//             GlobalVariable.API_Status = isSuccess;
//             return isSuccess;
//         }
//         catch (Exception ex)
//         {
//             Debug.LogError($"Error during web request: {ex}");
//             GlobalVariable.API_Status = false;
//             return false;
//         }
//     }
//     // public async Task GetAllDevicesByGrapper(string url, int grapperId)
//     // {
//     //     using UnityWebRequest webRequest = UnityWebRequest.Get(url);
//     //     {
//     //         if (!await SendWebRequestAsync(webRequest))
//     //         {
//     //             HandleRequestError(webRequest.error);
//     //             return;
//     //         }
//     //         try
//     //         {
//     //             var list_DeviceInformationModel = JsonConvert.DeserializeObject<List<DeviceInformationModel>>(webRequest.downloadHandler.text);
//     //             if (list_DeviceInformationModel != null && list_DeviceInformationModel.Any())
//     //             {
//     //                 GlobalVariable_Search_Devices.temp_ListDeviceInformationModelFromModule = list_DeviceInformationModel;
//     //                 GlobalVariable_Search_Devices.temp_List_Device_For_Fitler = FilterListDevicesForSearching(list_DeviceInformationModel);

//     //                 Dic_DeviceInformationModels.Clear();

//     //                 foreach (var device in list_DeviceInformationModel)
//     //                 {
//     //                     Dic_DeviceInformationModels.TryAdd(device.Code, device);
//     //                 }
//     //                 GlobalVariable.temp_Dictionary_DeviceInformationModel = Dic_DeviceInformationModels;
//     //             }
//     //             else
//     //             {
//     //                 Debug.Log("list_DeviceInformationModel is null");
//     //             }
//     //         }
//     //         catch (JsonException jsonEx)
//     //         {
//     //             HandleRequestError(jsonEx.Message);
//     //             return;
//     //         }
//     //         catch (Exception ex)
//     //         {
//     //             HandleRequestError(ex.Message);
//     //             return;
//     //         }
//     //     }
//     // }

//     private List<string> FilterListDevicesForSearching(List<DeviceInformationModel> DeviceInformationModels)
//     {
//         var list_Devices_For_Filter = new HashSet<string>();
//         foreach (var device in DeviceInformationModels)
//         {
//             if (!string.IsNullOrWhiteSpace(device.Code)) list_Devices_For_Filter.Add(device.Code);
//             if (!string.IsNullOrWhiteSpace(device.Function)) list_Devices_For_Filter.Add(device.Function);
//         }
//         Debug.Log("list_Devices_For_Filter.Count: " + list_Devices_For_Filter.Count);
//         return new List<string>(list_Devices_For_Filter);
//     }
//     public async Task LoadImageFromUrlAsync(string url, Image image, bool convertToSprite = true)
//     {
//         using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(url))
//         {
//             if (!await SendWebRequestAsync(webRequest))
//             {
//                 HandleRequestError(webRequest.error);
//                 return;
//             }

//             try
//             {
//                 Texture2D texture = DownloadHandlerTexture.GetContent(webRequest);
//                 if (convertToSprite)
//                 {
//                     Sprite sprite = Texture_To_Sprite.ConvertTextureToSprite(texture);
//                     image.sprite = sprite;
//                     image.gameObject.SetActive(true);
//                 }
//             }
//             catch (JsonException jsonEx)
//             {
//                 Debug.LogError($"Error parsing JSON from URL: {webRequest.url}, Error: {jsonEx.Message}");
//             }
//             catch (Exception ex)
//             {
//                 Debug.LogError($"Unexpected error from URL: {webRequest.url}, Error: {ex}");
//             }
//         }
//     }

//     //operation.isDone: Đoạn mã này kiểm tra xem tác vụ tải ảnh có hoàn thành chưa.
//     // Điều này là một công việc background — tải ảnh từ stringernet, không ảnh hưởng đến main thread.
//     // await Task.Yield(): Khi bạn gọi await Task.Yield(), điều này có nghĩa là main thread tạm dừng công việc hiện tại của nó(kiểm tra operation.isDone) 
//     //và nhường quyền điều khiển cho hệ thống để thực hiện các tác vụ khác.Main thread không bị block và có thể tiếp tục xử lý các tác vụ khác.
//     //hệ thống sẽ trả quyền điều khiển lại cho main thread khi công việc background hoàn thành, 
//     //hoặc khi có sự kiện mới yêu cầu thực thi trên main thread.

//     private void HandleRequestError(string error)
//     {
//         Debug.LogError($"Request error: {error}");
//         throw new Exception(error);
//     }





//     //! Module

//     public async Task<List<ModuleInformationModel>> GetListModuleData(string url)
//     {
//         try
//         {
//             using (HttpClient client = new HttpClient())
//             {
//                 HttpResponseMessage response = await client.GetAsync(url);
//                 if (response.IsSuccessStatusCode)
//                 {
//                     string json = await response.Content.ReadAsStringAsync();
//                     var listModuleInformationModel = JsonConvert.DeserializeObject<List<ModuleInformationModel>>(json);
//                     Debug.Log("listModuleInformationModel.Count: " + listModuleInformationModel.Count);

//                     if (listModuleInformationModel != null && listModuleInformationModel.Any())
//                     {
//                         GlobalVariable.temp_ListModuleInformationModel = listModuleInformationModel;
//                         GlobalVariable.temp_Dictionary_ModuleBasicModel.Clear();
//                         Dic_ModuleInformationModels.Clear();

//                         foreach (var Module in listModuleInformationModel)
//                         {
//                             Dic_ModuleInformationModels.TryAdd(Module.Name, Module);
//                             var moduleBasicModel = new ModuleBasicModel(
//                          Module.Id,
//                          Module.Name
//                      );
//                             GlobalVariable.temp_Dictionary_ModuleBasicModel.TryAdd("Module", moduleBasicModel);

//                         }

//                         GlobalVariable.temp_Dictionary_ModuleInformationModel = Dic_ModuleInformationModels;
//                         Debug.Log("Dic_ModuleInformationModels.Count: " + Dic_ModuleInformationModels.Count);
//                     }
//                     return listModuleInformationModel;
//                 }
//                 else
//                 {
//                     HandleRequestError($"Failed to get JB information. Status code: {response.StatusCode}");
//                     return null;
//                 }
//             }
//         }
//         catch (Exception ex)
//         {
//             HandleRequestError($"Unexpected error: {ex.Message}");
//             return null;
//         }
//     }
//     public async Task<ModuleInformationModel> GetModuleData(string url)
//     {
//         GlobalVariable.ActiveCloseCanvasButton = false;
//         try
//         {
//             using (HttpClient client = new HttpClient())
//             {
//                 HttpResponseMessage response = await client.GetAsync(url);
//                 if (response.IsSuccessStatusCode)
//                 {
//                     string json = await response.Content.ReadAsStringAsync();
//                     var moduleInformationModel = JsonConvert.DeserializeObject<ModuleInformationModel>(json);
//                     if (moduleInformationModel != null)
//                     {
//                         GlobalVariable.moduleId = 1;
//                         GlobalVariable.moduleSpecificationId = 1;
//                         GlobalVariable.adapterSpecificationId = 1;

//                         // GlobalVariable.temp_ListJBInformationModelFromModule_FromModule.Clear();
//                         // GlobalVariable.temp_ListDeviceInformationModelFromModule_FromModule.Clear();

//                         GlobalVariable.temp_ModuleInformationModel = null;
//                         GlobalVariable.temp_ModuleSpecificationBasicModel = null;
//                         GlobalVariable.temp_AdapterSpecificationModel = null;

//                         temp_ModuleInformationModel = moduleInformationModel;
//                         // temp_ListJBInformationModel_From_Module = temp_ModuleInformationModel.ListJBInformationModel;
//                         // temp_ListDeviceInformationModel_FromModule = temp_ModuleInformationModel.ListDeviceInformationModel;
//                         moduleId = temp_ModuleInformationModel.Id;

//                         // GlobalVariable.temp_ModuleInformationModel = temp_ModuleInformationModel;

//                         GlobalVariable.moduleId = moduleId;
//                         // GlobalVariable.ModuleSpecificationId = moduleInformationModel.ModuleSpecificationModel.Id;
//                         // GlobalVariable.AdapterSpecificationId = moduleInformationModel.AdapterSpecificationModel.Id;
//                         GlobalVariable.moduleId = moduleId;
//                         GlobalVariable.moduleSpecificationId = moduleInformationModel.ModuleSpecificationModel.Id;
//                         GlobalVariable.adapterSpecificationId = moduleInformationModel.AdapterSpecificationModel.Id;

//                         // GlobalVariable.temp_ListJBInformationModelFromModule_FromModule = temp_ListJBInformationModelFromDevice;
//                         // GlobalVariable.temp_ListDeviceInformationModelFromModule_FromModule = temp_ListDeviceInformationModelFromModule_FromModule;

//                         GlobalVariable.temp_ModuleSpecificationModel = moduleInformationModel.ModuleSpecificationModel;
//                         GlobalVariable.temp_AdapterSpecificationModel = moduleInformationModel.AdapterSpecificationModel;
//                         return moduleInformationModel;
//                     }
//                     else
//                     {
//                         HandleRequestError("Failed to get JB information.");
//                         return null;
//                     }
//                 }
//                 else
//                 {
//                     HandleRequestError($"Failed to get JB information. Status code: {response.StatusCode}");
//                     return null;
//                 }
//             }
//         }
//         catch (JsonException jsonEx)
//         {
//             HandleRequestError(jsonEx.Message);
//             return null;

//         }
//         catch (Exception ex)
//         {
//             HandleRequestError(ex.Message);
//             return null;

//         }
//     }


//     //! Device
//     public async Task<List<DeviceInformationModel>> GetListDeviceData(string url)
//     {
//         try
//         {
//             using (HttpClient client = new HttpClient())
//             {
//                 HttpResponseMessage response = await client.GetAsync(url);
//                 if (response.IsSuccessStatusCode)
//                 {
//                     string json = await response.Content.ReadAsStringAsync();
//                     var list_DeviceInformationModel = JsonConvert.DeserializeObject<List<DeviceInformationModel>>(json);
//                     if (list_DeviceInformationModel != null && list_DeviceInformationModel.Any())
//                     {
//                         // GlobalVariable_Search_Devices.temp_ListDeviceInformationModelFromModule = list_DeviceInformationModel;
//                         GlobalVariable_Search_Devices.temp_List_Device_For_Fitler = FilterListDevicesForSearching(list_DeviceInformationModel);

//                         Dic_DeviceInformationModels.Clear();
//                         GlobalVariable.list_DeviceCode.Clear();
//                         GlobalVariable.temp_Dictionary_DeviceIOAddress.Clear();
//                         foreach (var device in list_DeviceInformationModel)
//                         {
//                             Dic_DeviceInformationModels.TryAdd(device.Code, device);
//                             GlobalVariable.temp_Dictionary_DeviceIOAddress.TryAdd(device.Code, device.IOAddress);
//                             GlobalVariable.list_DeviceCode.Add(device.Code);
//                         }
//                         GlobalVariable.temp_Dictionary_DeviceInformationModel = Dic_DeviceInformationModels;
//                         return list_DeviceInformationModel;
//                     }
//                     else
//                     {
//                         Debug.Log("list_DeviceInformationModel is null");
//                         return null;
//                     }
//                 }
//                 else
//                 {
//                     HandleRequestError($"Failed to get device information. Status code: {response.StatusCode}");
//                     return null;
//                 }
//             }
//         }
//         catch (JsonException jsonEx)
//         {
//             HandleRequestError(jsonEx.Message);
//             return null;
//         }
//         catch (Exception ex)
//         {
//             HandleRequestError(ex.Message);
//             return null;
//         }
//     }
//     public async Task<DeviceInformationModel> GetDeviceData(string url)
//     {
//         try
//         {
//             using (HttpClient client = new HttpClient())
//             {
//                 HttpResponseMessage response = await client.GetAsync(url);
//                 if (response.IsSuccessStatusCode)
//                 {
//                     string json = await response.Content.ReadAsStringAsync();
//                     var DeviceInformationModel = JsonConvert.DeserializeObject<DeviceInformationModel>(json);

//                     if (DeviceInformationModel != null)
//                     {
//                         GlobalVariable.temp_DeviceInformationModel = DeviceInformationModel;
//                     }
//                     return DeviceInformationModel;
//                 }
//                 else
//                 {
//                     HandleRequestError($"Failed to get JB information. Status code: {response.StatusCode}");
//                     return null;
//                 }
//             }
//         }
//         catch (Exception ex)
//         {
//             HandleRequestError($"Unexpected error: {ex.Message}");
//             return null;
//         }
//     }
//     public async Task<bool> UpdateDeviceDataAsync(DeviceGeneralModel DeviceInformationModel, string url)
//     {
//         try
//         {
//             using (HttpClient client = new HttpClient())
//             {
//                 string json = JsonConvert.SerializeObject(DeviceInformationModel);
//                 var content = new StringContent(json, Encoding.UTF8, "application/json");
//                 HttpResponseMessage response = await client.PostAsync(url, content);
//                 return response.IsSuccessStatusCode;
//             }
//         }
//         catch (JsonException jsonEx)
//         {
//             Debug.LogError($"Error parsing JSON: {jsonEx.Message}");
//             return false;
//         }
//         catch (Exception ex)
//         {
//             Debug.LogError($"Error posting data: {ex.Message}");
//             return false;
//         }
//     }
//     public async Task<bool> AddNewDeviceAsync(DevicePostGeneralModel devicePostGeneralModel, string url)
//     {
//         try
//         {
//             using (HttpClient client = new HttpClient())
//             {
//                 string json = JsonConvert.SerializeObject(devicePostGeneralModel);
//                 var content = new StringContent(json, Encoding.UTF8, "application/json");
//                 HttpResponseMessage response = await client.PutAsync(url, content);
//                 return response.IsSuccessStatusCode;
//             }
//         }
//         catch (JsonException jsonEx)
//         {
//             Debug.LogError($"Error parsing JSON: {jsonEx.Message}");
//             return false;
//         }
//         catch (Exception ex)
//         {
//             Debug.LogError($"Error posting data: {ex.Message}");
//             return false;
//         }
//     }
//     public async Task<bool> DeleteDeviceData(string url)
//     {
//         try
//         {
//             using (HttpClient client = new HttpClient())
//             {
//                 HttpResponseMessage response = await client.DeleteAsync(url);
//                 return response.IsSuccessStatusCode;
//             }
//         }
//         catch (Exception ex)
//         {
//             Debug.LogError($"Error deleting data: {ex.Message}");
//             return false;
//         }
//     }

//     //! JB
//     public async Task<List<JBInformationModel>> GetListJBData(string url)
//     {
//         try
//         {
//             using (HttpClient client = new HttpClient())
//             {
//                 HttpResponseMessage response = await client.GetAsync(url);
//                 if (response.IsSuccessStatusCode)
//                 {
//                     string json = await response.Content.ReadAsStringAsync();
//                     var listJBInformationModel = JsonConvert.DeserializeObject<List<JBInformationModel>>(json);
//                     Debug.Log("listJBInformationModel.Count: " + listJBInformationModel.Count);

//                     if (listJBInformationModel != null && listJBInformationModel.Any())
//                     {
//                         //  GlobalVariable_Search_Devices.temp_ListJBInformationModelFromModule
//                         // GlobalVariable.temp_ListJBInformationModelFromModule = listJBInformationModel;
//                         Dic_JBInformationModels.Clear();
//                         GlobalVariable.list_jBName.Clear();
//                         GlobalVariable.temp_Dictionary_JBBasicModel.Clear();
//                         foreach (var JB in listJBInformationModel)
//                         {
//                             Dic_JBInformationModels.TryAdd(JB.Name, JB);
//                             GlobalVariable.list_jBName.Add(JB.Name);
//                             var jbBasicModel = new JBBasicModel(
//                                 JB.Id,
//                                 JB.Name
//                             );
//                             GlobalVariable.temp_Dictionary_JBBasicModel.TryAdd(JB.Name, jbBasicModel);
//                         }
//                         GlobalVariable.temp_Dictionary_JBInformationModel = Dic_JBInformationModels;
//                         Debug.Log("Dic_JBInformationModels.Count: " + Dic_JBInformationModels.Count);
//                     }
//                     return listJBInformationModel;
//                 }
//                 else
//                 {
//                     HandleRequestError($"Failed to get JB information. Status code: {response.StatusCode}");
//                     return null;
//                 }
//             }
//         }
//         catch (Exception ex)
//         {
//             HandleRequestError($"Unexpected error: {ex.Message}");
//             return null;
//         }
//     }
//     public async Task<JBInformationModel> GetJBData(string url)
//     {
//         try
//         {
//             using (HttpClient client = new HttpClient())
//             {
//                 HttpResponseMessage response = await client.GetAsync(url);
//                 if (response.IsSuccessStatusCode)
//                 {
//                     string json = await response.Content.ReadAsStringAsync();
//                     var jBInformationModel = JsonConvert.DeserializeObject<JBInformationModel>(json);

//                     if (jBInformationModel != null)
//                     {
//                         GlobalVariable.temp_JBInformationModel = jBInformationModel;
//                     }
//                     return jBInformationModel;
//                 }
//                 else
//                 {
//                     HandleRequestError($"Failed to get JB information. Status code: {response.StatusCode}");
//                     return null;
//                 }
//             }
//         }
//         catch (Exception ex)
//         {
//             HandleRequestError($"Unexpected error: {ex.Message}");
//             return null;
//         }
//     }
//     public async Task<bool> UpdateJBDataAsync(JBGeneralModel jBGeneralModel, string url)
//     {
//         try
//         {
//             using (HttpClient client = new HttpClient())
//             {
//                 string json = JsonConvert.SerializeObject(jBGeneralModel);
//                 var content = new StringContent(json, Encoding.UTF8, "application/json");
//                 HttpResponseMessage response = await client.PostAsync(url, content);
//                 return response.IsSuccessStatusCode;
//             }
//         }
//         catch (JsonException jsonEx)
//         {
//             Debug.LogError($"Error parsing JSON: {jsonEx.Message}");
//             return false;
//         }
//         catch (Exception ex)
//         {
//             Debug.LogError($"Error posting data: {ex.Message}");
//             return false;
//         }
//     }
//     public async Task<bool> AddNewJBAsync(JBPostGeneralModel jBGeneralModel, string url)
//     {
//         try
//         {
//             using (HttpClient client = new HttpClient())
//             {
//                 string json = JsonConvert.SerializeObject(jBGeneralModel);
//                 var content = new StringContent(json, Encoding.UTF8, "application/json");
//                 HttpResponseMessage response = await client.PutAsync(url, content);
//                 return response.IsSuccessStatusCode;
//             }
//         }
//         catch (JsonException jsonEx)
//         {
//             Debug.LogError($"Error parsing JSON: {jsonEx.Message}");
//             return false;
//         }
//         catch (Exception ex)
//         {
//             Debug.LogError($"Error posting data: {ex.Message}");
//             return false;
//         }

//     }
//     public async Task<bool> DeleteJBData(string url)
//     {
//         try
//         {
//             using (HttpClient client = new HttpClient())
//             {
//                 HttpResponseMessage response = await client.DeleteAsync(url);
//                 return response.IsSuccessStatusCode;
//             }
//         }
//         catch (Exception ex)
//         {
//             Debug.LogError($"Error deleting data: {ex.Message}");
//             return false;
//         }
//     }


//     // public async Task GetAllJBsByGrapper(string url, int grapperId)
//     // {
//     //     using UnityWebRequest webRequest = UnityWebRequest.Get(url);
//     //     {
//     //         if (!await SendWebRequestAsync(webRequest))
//     //         {
//     //             HandleRequestError(webRequest.error);
//     //             return;
//     //         }

//     //         try
//     //         {
//     //             var list_JBInformationModel = JsonConvert.DeserializeObject<List<JBInformationModel>>(webRequest.downloadHandler.text);
//     //             if (list_JBInformationModel != null && list_JBInformationModel.Any())
//     //             {
//     //                 GlobalVariable_Search_Devices.temp_ListJBInformationModelFromModule = list_JBInformationModel;
//     //                 Debug.Log("GlobalVariable_Search_Devices.temp_ListJBInformationModelFromDevice.Count: " + GlobalVariable_Search_Devices.temp_ListJBInformationModelFromModule.Count);
//     //                 Dic_JBInformationModels.Clear();

//     //                 foreach (var JB in list_JBInformationModel)
//     //                 {
//     //                     Dic_JBInformationModels.TryAdd(JB.Name, JB);
//     //                 }
//     //                 GlobalVariable.temp_Dictionary_JBInformationModel = Dic_JBInformationModels;

//     //             }

//     //         }
//     //         catch (JsonException jsonEx)
//     //         {
//     //             HandleRequestError(jsonEx.Message);
//     //             return;
//     //         }
//     //         catch (Exception ex)
//     //         {
//     //             HandleRequestError(ex.Message);
//     //             return;
//     //         }
//     //     }}


//     // Create New Device
//     /* public async Task<bool> CreateNewDevice(string url, DeviceInformationModel device, string sceneName)
//      {
//          try
//          {
//              //PrepareDeviceData(device);
//              string jsonData = JsonConvert.SerializeObject(device);

//              if (string.IsNullOrEmpty(jsonData))
//              {
//                  Debug.LogError("Error serializing data.");
//                  return;
//              }

//              using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
//              {
//                  webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonData));
//                  webRequest.downloadHandler = new DownloadHandlerBuffer();
//                  webRequest.SetRequestHeader("Content-Type", "application/json");

//                  if (!await SendWebRequestAsync(webRequest))
//                  {
//                      HandleRequestError(webRequest.error);
//                      return;
//                  }

//                  await HandleSuccessfulPost(device, sceneName);
//              }
//          }
//          catch (Exception ex)
//          {
//              Debug.LogError($"Unexpected error: {ex}");

//          }

//      }*/


//     // public async Task Get_JB_TSD_Information(string url, string grapperName)
//     // {
//     //     await Task.Delay(1500);
//     //     
//     //     Show_Toast.Instance.ShowToast("loading", $"Đang tải dữ liệu...");
//     //     string jsonData = await FetchJsonData(url, grapperName);
//     //     if (jsonData == null) return;
//     //     try
//     //     {
//     //         var jbDataList = JsonConvert.DeserializeObject<List<JB_TSD_Data>>(jsonData);

//     //         GlobalVariable.list_Name_and_Url_JB_Location_A = jbDataList[0].JB_TSD_Location;
//     //         GlobalVariable.list_Name_and_Url_JB_Connection_A = jbDataList[0].JB_TSD_Wiring;

//     //         await Task.WhenAll(
//     //             LoadImagesAsync(GlobalVariable.list_Name_and_Url_JB_Location_A, "get_JB_Location"),
//     //             LoadImagesAsync(GlobalVariable.list_Name_and_Url_JB_Connection_A, "get_JB_Connection")
//     //         );
//     //         Show_Toast.Instance.ShowToast("success", $"Tải dữ liệu thành công");
//     //         Debug.Log(GlobalVariable.list_Name_and_Url_JB_Location_A.Count);
//     //         Debug.Log(GlobalVariable.list_Name_and_Url_JB_Connection_A.Count);
//     //         await Task.Delay(1500);
//     //         Show_Toast.Instance.StartCoroutine(Show_Toast.Instance.Set_Instance_Status_False());
//     //     }
//     //     catch (JsonException jsonEx)
//     //     {
//     //         Debug.LogError($"Error parsing JSON: {jsonEx.Message}");
//     //     }
//     // }


//     // private async Task LoadImagesAsync(Dictionary<string, List<string>> urlDictionary, string action)
//     // {
//     //     List<Task> loadTasks = new List<Task>();

//     //     foreach (KeyValuePair<string, List<string>> kvp in urlDictionary)
//     //     {
//     //         var key = kvp.Key;
//     //         var List_url = kvp.Value;

//     //         if (action == "get_JB_Location" && !GlobalVariable.list_Name_And_Image_JB_Location_A.ContainsKey(key))
//     //         {
//     //             GlobalVariable.list_Key_JB_Location_A.Add(key);

//     //         }
//     //         if (action == "get_JB_Connection" && !GlobalVariable.list_Name_And_Image_JB_Connection_A.ContainsKey(key))
//     //         {
//     //             GlobalVariable.list_Key_JB_Connection_A.Add(key);
//     //         }
//     //         foreach (string link in List_url)
//     //         {
//     //             loadTasks.Add(LoadImageFromUrlAsync(link, key, action));
//     //         }
//     //     }
//     //     await Task.WhenAll(loadTasks);
//     //     Debug.Log("Lưu các Sprite thành công vào Dictionary");
//     // }


//     // private void LogJbTsdData(JB_TSD_Data jbData)
//     // {
//     //     Debug.Log("JB_TSD_Wiring:");
//     //     foreach (var (key, urls) in jbData.JB_TSD_Wiring)
//     //     {
//     //         Debug.Log($"{key}:");
//     //         foreach (var url in urls) Debug.Log($" - {url}");
//     //     }

//     //     Debug.Log("JB_TSD_Location:");
//     //     foreach (var (key, urls) in jbData.JB_TSD_Location)
//     //     {
//     //         Debug.Log($"{key}:");
//     //         foreach (var url in urls) Debug.Log($" - {url}");
//     //     }
//     // }
// }


// public class ApiClient : MonoBehaviour
// {
//     #region Services
//     private IGrapperService _grappersService;
//     private IRackService _rackService;
//     private IJBService _jbService;
//     private IDeviceService _deviceService;
//     private IModuleService _moduleService;
//     private IFieldDeviceService _fieldDeviceService;
//     private IMccService _mccService;
//     private IAdapterSpecificationService _adapterSpecificationService;
//     private IModuleSpecificationService _moduleSpecificationService;
//     #endregion
//     private readonly HttpClient _httpClient = new HttpClient();

//     private void Start()
//     {
//         //! Dependency Injection
//         // Lấy IJBService từ ServiceLocator
//         _jbService = ServiceLocator.Instance.JBService;
//         // Lấy IDeviceService từ ServiceLocator
//         _deviceService = ServiceLocator.Instance.DeviceService;
//         // Lấy IModuleService từ ServiceLocator
//         _moduleService = ServiceLocator.Instance.ModuleService;
//         // Lấy IFieldDeviceService từ ServiceLocator
//         _fieldDeviceService = ServiceLocator.Instance.FieldDeviceService;
//         // Lấy IMccService từ ServiceLocator
//         _mccService = ServiceLocator.Instance.MccService;
//         // Lấy IAdapterSpecificationService từ ServiceLocator
//         _adapterSpecificationService = ServiceLocator.Instance.AdapterSpecificationService;
//         // Lấy IModuleSpecificationService từ ServiceLocator
//         _moduleSpecificationService = ServiceLocator.Instance.ModuleSpecificationService;
//         // Lấy IGrapperService từ ServiceLocator
//         _grappersService = ServiceLocator.Instance.GrapperService;
//         // Lấy IRackService từ ServiceLocator
//         _rackService = ServiceLocator.Instance.RackService;

//     }
//     public async void GetJBList(int grapperId)
//     {
//         try
//         {
//             var jbList = await _jbService.GetListJBInformationAsync(grapperId); // Gọi Service
//             if (jbList != null && jbList.Any())
//             {
//                 foreach (var jb in jbList)
//                     Debug.Log($"JB: {jb.Name}, Location: {jb.Location}");
//             }
//             else
//             {
//                 Debug.Log("No JBs found");
//             }
//         }
//         catch (Exception ex)
//         {
//             Debug.LogError($"Error getting JB list: {ex.Message}");
//             // Có thể hiển thị UI thông báo lỗi cho người chơi
//         }
//     }

//     public async void GetJBById(int jbId)
//     {
//         try
//         {
//             var jb = await _jbService.GetJBByIdAsync(jbId); // Gọi Service
//             if (jb != null)
//             {
//                 Debug.Log($"JB: {jb.Name}, Location: {jb.Location}");
//             }
//             else
//             {
//                 Debug.Log("JB not found");
//             }
//         }
//         catch (Exception ex)
//         {
//             Debug.LogError($"Error getting JB: {ex.Message}");
//             // Có thể hiển thị UI thông báo lỗi cho người chơi
//         }
//     }

//     public async void CreateNewJB(int grapperId, JBRequestDto jBRequestDto)
//     {
//         try
//         {
//             bool result = await _jbService.CreateNewJBAsync(grapperId, jBRequestDto);
//             Debug.Log(result ? "JB created successfully" : "Failed to create JB");
//         }
//         catch (Exception ex)
//         {
//             Debug.LogError($"Error creating JB: {ex.Message}");
//             // Có thể hiển thị UI thông báo lỗi cho người chơi
//         }
//     }
//     public async void UpdateJB(JBRequestDto jBRequestDto)
//     {
//         try
//         {
//             bool result = await _jbService.UpdateJBAsync(GlobalVariable.GrapperId, jBRequestDto);
//             Debug.Log(result ? "JB updated successfully" : "Failed to update JB");
//         }
//         catch (Exception ex)
//         {
//             Debug.LogError($"Error updating JB: {ex.Message}");
//             // Có thể hiển thị UI thông báo lỗi cho người chơi
//         }
//     }
//     public async void DeleteJB(int jbId)
//     {
//         try
//         {
//             bool result = await _jbService.DeleteJBAsync(jbId);
//             Debug.Log(result ? "JB deleted successfully" : "Failed to delete JB");
//         }
//         catch (Exception ex)
//         {
//             Debug.LogError($"Error deleting JB: {ex.Message}");
//             // Có thể hiển thị UI thông báo lỗi cho người chơi
//         }
//     }

// }