using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PimDeWitte.UnityMainThreadDispatcher;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class APIManagerOpenCV : MonoBehaviour
{
    public static APIManagerOpenCV Instance { get; private set; }

    private Dictionary<string, ModuleInformationModel> Dic_ModuleInformationModels = new Dictionary<string, ModuleInformationModel>();
    private Dictionary<string, MccInformationModel> Dic_MccInformationModels = new Dictionary<string, MccInformationModel>();
    private Dictionary<string, GrapperInformationModel> Dic_GrapperInformationModels = new Dictionary<string, GrapperInformationModel>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public async Task<List<ModuleInformationModel>> GetListModule(string url)
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                string response = await client.GetStringAsync(url);
                // if (response.IsSuccessStatusCode)
                // {
                // string json = await response.Content.ReadAsStringAsync();
                // Debug.Log("json: " + response);

                var listModuleInformationModel = JsonConvert.DeserializeObject<List<ModuleInformationModel>>(response);

                // Debug.Log("listModuleInformationModel.Count: " + listModuleInformationModel.Count);

                if (listModuleInformationModel != null && listModuleInformationModel.Count > 0)
                {
                    StaticVariable.temp_ListModuleInformationModel = listModuleInformationModel;
                    Dic_ModuleInformationModels.Clear();

                    foreach (var Module in listModuleInformationModel)
                    {
                        Dic_ModuleInformationModels.TryAdd(Module.Name, Module);
                    }

                    StaticVariable.Dic_ModuleInformationModel = Dic_ModuleInformationModels;
                    // Debug.Log("Dic_ModuleInformationModels.Count: " + Dic_ModuleInformationModels.Count);
                }
                return listModuleInformationModel;
            }
        }
        catch (Exception ex)
        {
            HandleRequestError($"Unexpected error: {ex.Message + " " + ex.StackTrace + " " + ex.InnerException}");
            return null;
        }
    }

    public async Task<ModuleInformationModel> GetModule(string url)
    {
        StaticVariable.ActiveCloseCanvasButton = false;
        try
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    // Debug.Log("json: " + json);
                    var moduleInformationModel = JsonConvert.DeserializeObject<ModuleInformationModel>(json);

                    if (moduleInformationModel != null)
                    {
                        // StaticVariable.ModuleId = moduleInformationModel.Id;
                        // StaticVariable.ModuleSpecificationId = moduleInformationModel.ModuleSpecificationModel.Id;
                        // StaticVariable.AdapterSpecificationId = moduleInformationModel.AdapterSpecificationModel.Id;

                        StaticVariable.temp_ListJBInformationModelFromModule.Clear();
                        StaticVariable.temp_ListDeviceInformationModelFromModule.Clear();

                        StaticVariable.temp_ModuleInformationModel = null;
                        StaticVariable.temp_ModuleSpecificationModel = null;
                        StaticVariable.temp_AdapterSpecificationModel = null;

                        StaticVariable.temp_ListJBInformationModelFromModule = moduleInformationModel.ListJBInformationModel;
                        StaticVariable.temp_ListDeviceInformationModelFromModule = moduleInformationModel.ListDeviceInformationModel;

                        StaticVariable.temp_ModuleSpecificationModel = moduleInformationModel.ModuleSpecificationModel;
                        StaticVariable.temp_AdapterSpecificationModel = moduleInformationModel.AdapterSpecificationModel;

                        // Debug.Log("temp_ListJBInformationModelFromModule.Count: " + StaticVariable.temp_ListJBInformationModelFromModule.Count);
                        // Debug.Log("temp_ListDeviceInformationModelFromModule.Count: " + StaticVariable.temp_ListDeviceInformationModelFromModule.Count);
                        return moduleInformationModel;
                    }
                    else
                    {
                        HandleRequestError("Failed to get JB information.");
                        return null;
                    }
                }
                else
                {
                    HandleRequestError($"Failed to get JB information. Status code: {response.StatusCode}");
                    return null;
                }
            }
        }
        catch (JsonException jsonEx)
        {
            HandleRequestError(jsonEx.Message);
            return null;

        }
        catch (Exception ex)
        {
            HandleRequestError(ex.Message);
            return null;

        }
    }

    public async Task<List<MccInformationModel>> GetListMCCs(string url)
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                string response = await client.GetStringAsync(url);
                // Debug.Log("response: " + response);

                var listMccInformationModel = JsonConvert.DeserializeObject<List<MccInformationModel>>(response);

                StaticVariable.temp_ListMCCInformationModel.Clear();
                StaticVariable.Dic_MccInformationModel.Clear();

                if (listMccInformationModel != null && listMccInformationModel.Count > 0)
                {
                    StaticVariable.temp_ListMCCInformationModel = listMccInformationModel;
                    Dic_MccInformationModels.Clear();

                    foreach (var Mcc in listMccInformationModel)
                    {
                        Dic_MccInformationModels.TryAdd(Mcc.CabinetCode, Mcc);
                    }

                    StaticVariable.Dic_MccInformationModel = Dic_MccInformationModels;
                }
                return listMccInformationModel;
            }
        }
        catch (Exception ex)
        {
            HandleRequestError($"Unexpected error: {ex.Message + " " + ex.StackTrace + " " + ex.InnerException}");
            return null;
        }
    }

    public async Task<MccInformationModel> GetMcc(string url)
    {
        StaticVariable.ActiveCloseCanvasButton = false;
        try
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    // Debug.Log("json: " + json);
                    var mccInformationModel = JsonConvert.DeserializeObject<MccInformationModel>(json);

                    if (mccInformationModel != null)
                    {
                        // StaticVariable.MccId = mccInformationModel.Id;

                        StaticVariable.temp_ListFieldDeviceModelFromMCC.Clear();
                        StaticVariable.temp_MccInformationModel = null;

                        StaticVariable.temp_ListFieldDeviceModelFromMCC = mccInformationModel.ListFieldDeviceInformation;
                        StaticVariable.temp_MccInformationModel = mccInformationModel;

                        return mccInformationModel;
                    }
                    else
                    {
                        HandleRequestError("Failed to get JB information.");
                        return null;
                    }
                }
                else
                {
                    HandleRequestError($"Failed to get JB information. Status code: {response.StatusCode}");
                    return null;
                }
            }
        }
        catch (JsonException jsonEx)
        {
            HandleRequestError(jsonEx.Message);
            return null;

        }
        catch (Exception ex)
        {
            HandleRequestError(ex.Message);
            return null;

        }
    }

    private void HandleRequestError(string error)
    {
        Debug.LogError($"Request error: {error}");
        throw new Exception(error);
    }

    public async Task<List<DeviceInformationModel>> GetListDevice(string url)
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    // Debug.Log("json: " + json);
                    var deviceInformationModel = JsonConvert.DeserializeObject<List<DeviceInformationModel>>(json);

                    if (deviceInformationModel != null)
                    {

                        if (StaticVariable.ready_To_Reset_ListDevice)
                        {
                            StaticVariable.temp_ListDeviceInformationModelFromDeviceName.Clear();
                            StaticVariable.temp_ListAdditionalImageFromDevice.Clear();
                            // Debug.Log("Clear temp_ListDeviceInformationModelFromDeviceName");
                        }

                        foreach (var device in deviceInformationModel)
                        {
                            StaticVariable.temp_ListDeviceInformationModelFromDeviceName.Add(device.Code, device);

                            if (device.AdditionalConnectionImages.Count == 0)
                            {
                                continue;
                            }
                            StaticVariable.temp_ListAdditionalImageFromDevice.Add(device.Code, device.AdditionalConnectionImages.Select(image => image.Name).ToList());
                            // Debug.Log("device.Code: " + device.Code + " device.AdditionalConnectionImages.Count: " + device.AdditionalConnectionImages.Count);
                        }
                    }
                    return deviceInformationModel;
                }
                else
                {
                    HandleRequestError($"Failed to get JB information. Status code: {response.StatusCode}");
                    return null;
                }
            }
        }
        catch (Exception ex)
        {
            HandleRequestError($"Unexpected error: {ex.Message}");
            return null;
        }
    }

    public async Task<JBInformationModel> GetJBInformation(string url)
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    // Debug.Log("json: " + json);
                    var jBInformationModel = JsonConvert.DeserializeObject<JBInformationModel>(json);

                    StaticVariable.temp_JBInformationModel = jBInformationModel;
                    StaticVariable.device_Code = "1";

                    return jBInformationModel;
                }
                else
                {
                    HandleRequestError($"Failed to get JB information. Status code: {response.StatusCode}");
                    return null;
                }
            }
        }
        catch (Exception ex)
        {
            HandleRequestError($"Unexpected error: {ex.Message}");
            return null;
        }
    }

    public async Task<FieldDeviceInformationModel> GetFieldDeviceInformation(string url)
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    // Debug.Log("json: " + json);
                    var fieldDeviceInformation = JsonConvert.DeserializeObject<FieldDeviceInformationModel>(json);

                    StaticVariable.temp_FieldDeviceInformationModel = fieldDeviceInformation;

                    return fieldDeviceInformation;
                }
                else
                {
                    HandleRequestError($"Failed to get JB information. Status code: {response.StatusCode}");
                    return null;
                }
            }
        }
        catch (Exception ex)
        {
            HandleRequestError($"Unexpected error: {ex.Message}");
            return null;
        }
    }

    public async Task<List<GrapperInformationModel>> GetListGrapper(string url)
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                string response = await client.GetStringAsync(url);
                // Debug.Log("response: " + response);

                var listGrapperInformationModel = JsonConvert.DeserializeObject<List<GrapperInformationModel>>(response);

                if (listGrapperInformationModel != null && listGrapperInformationModel.Count > 0)
                {
                    StaticVariable.temp_ListGrapperInformationModel = listGrapperInformationModel;
                    // Debug.Log("temp_ListGrapperInformationModel.Count: " + StaticVariable.temp_ListGrapperInformationModel.Count);
                    Dic_GrapperInformationModels.Clear();

                    foreach (var grapper in listGrapperInformationModel)
                    {
                        Dic_GrapperInformationModels.TryAdd(grapper.Name, grapper);
                    }

                    StaticVariable.Dic_GrapperInformationModel = Dic_GrapperInformationModels;
                }
                return listGrapperInformationModel;
            }
        }
        catch (Exception ex)
        {
            HandleRequestError($"Unexpected error: {ex.Message + " " + ex.StackTrace + " " + ex.InnerException}");
            return null;
        }
    }
}
