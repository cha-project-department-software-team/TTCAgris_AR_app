using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticVariable
{
    //TODO: API URL JB_TSD
    public static string GetListModuleUrl = "https://67d64a84286fdac89bc185e3.mockapi.io/GetListModule";
    public static string GetModuleUrl = "https://67d3e0ea8bca322cc26b6004.mockapi.io/TTC/GetModule";
    public static string GetListDevicesFromModuleUrl = "https://67d3e0ea8bca322cc26b6004.mockapi.io/TTC/GetListDeviceFromModule";
    public static string GetJBUrl = "https://67d64a84286fdac89bc185e3.mockapi.io/GetJB";

    //TODO: API URL MCC
    public static string GetListGrapperUrl = "https://67e1043658cc6bf78523da45.mockapi.io/TTC/GetListGrapper";
    public static string GetListMCCsUrl = "https://67e0fea158cc6bf78523b874.mockapi.io/TTC/GetListMCCs";
    public static string GetMCCUrl = "https://67e0fea158cc6bf78523b874.mockapi.io/TTC/GetMCC";
    public static string GetFieldDevicesFromMCCUrl = "https://67e1043658cc6bf78523da45.mockapi.io/TTC/GetFieldDevice";



    public static List<ModuleInformationModel> temp_ListModuleInformationModel = new List<ModuleInformationModel>();
    public static List<MccInformationModel> temp_ListMCCInformationModel = new List<MccInformationModel>();
    public static List<GrapperInformationModel> temp_ListGrapperInformationModel = new List<GrapperInformationModel>();
    public static Dictionary<string, ModuleInformationModel> Dic_ModuleInformationModel = new Dictionary<string, ModuleInformationModel>();
    public static Dictionary<string, MccInformationModel> Dic_MccInformationModel = new Dictionary<string, MccInformationModel>();
    public static Dictionary<string, GrapperInformationModel> Dic_GrapperInformationModel = new Dictionary<string, GrapperInformationModel>();
    public static bool API_Status = false;
    public static bool ready_To_Nav_New_Scene = false;
    public static bool ActiveCloseCanvasButton = false;

    //? module
    // public static string ModuleId;
    public static string ModuleSpecificationId;
    public static string AdapterSpecificationId;
    public static List<JBInformationModel> temp_ListJBInformationModelFromModule = new List<JBInformationModel>();
    public static List<JBInformationModel> temp_ListJBInformationModel = new List<JBInformationModel>();
    public static List<DeviceInformationModel> temp_ListDeviceInformationModelFromModule = new List<DeviceInformationModel>();
    public static Dictionary<string, DeviceInformationModel> temp_ListDeviceInformationModelFromDeviceName = new Dictionary<string, DeviceInformationModel>();
    public static Dictionary<string, List<string>> temp_ListAdditionalImageFromDevice = new Dictionary<string, List<string>>();
    public static ModuleInformationModel temp_ModuleInformationModel;
    public static ModuleSpecificationModel temp_ModuleSpecificationModel;
    public static AdapterSpecificationModel temp_AdapterSpecificationModel;

    //? mcc
    // public static string MccId;
    public static List<FieldDeviceInformationModel> temp_ListFieldDeviceModelFromMCC = new List<FieldDeviceInformationModel>();
    public static MccInformationModel temp_MccInformationModel;
    public static FieldDeviceInformationModel temp_FieldDeviceInformationModel;
    public static GrapperInformationModel temp_GrapperInformationModel;

    public static JBInformationModel temp_JBInformationModel;

    public static Dictionary<string, DeviceInformationModel> temp_DeviceInformationModel = new Dictionary<string, DeviceInformationModel>();

    //? navigate from ?
    public static bool navigate_from_List_Devices = false;
    public static bool navigate_from_JB_TSD_General = false;
    public static bool navigate_from_JB_TSD_Detail = false;


    public static bool ready_To_Update_ListDevices_UI = false;
    public static bool ready_To_Update_MCC_UI = false;
    public static bool ready_To_Update_Grapper_UI = false;
    // public static bool ready_To_Update_JB_TSD_Images = false;
    // public static bool ready_To_Download_Images_UI = false;
    // public static bool ready_To_Update_Images_UI = false;

    public static bool ready_To_Reset_ListDevice = true;
    // public static bool ready_To_Reset_ListJB = true;


    public static bool is_JB_TSD_Basic_Canvas_Active = false;

    public static bool is_Custom_Camera = false;


    //TODO: JB TSD Detail
    public static string jb_TSD_Title;
    public static string jb_TSD_Name;
    public static string jb_TSD_Location;
    public static string device_Code;

    // public static GameObject generalPanel;

    public static Dictionary<string, List<Texture2D>> temp_listJBConnectionImageFromModule = new Dictionary<string, List<Texture2D>>();
    public static Dictionary<string, Texture2D> temp_listJBLocationImageFromModule = new Dictionary<string, Texture2D>();
}
