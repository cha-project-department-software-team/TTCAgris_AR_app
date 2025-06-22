using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using ApplicationLayer.Dtos.AdapterSpecification;
using ApplicationLayer.Dtos.Company;
using ApplicationLayer.Dtos.Device;
using ApplicationLayer.Dtos.FieldDevice;
using ApplicationLayer.Dtos.Grapper;
using ApplicationLayer.Dtos.JB;
using ApplicationLayer.Dtos.Mcc;
using ApplicationLayer.Dtos.Module;
using ApplicationLayer.Dtos.ModuleSpecification;
using ApplicationLayer.Dtos.Rack;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GlobalVariable : MonoBehaviour
{
    public static string baseUrl = "http://20.2.205.72/api";
    public static List<string> APIRequestType = new List<string>()
    {

    };


    public static List<string> AllowedRequests = new List<string>()
    {
    "GET_Grapper_List",
    "POST_Grapper",
    "PUT_Grapper",
    "DELETE_Grapper",

    "GET_Rack_List",
    "POST_Rack",
    "PUT_Rack",
    "DELETE_Rack",

    "GET_Module_List",
    "POST_Module",
    "PUT_Module",
    "DELETE_Module",

    "GET_JB_List_General",
    "GET_JB_List_Information",
    "GET_JB",
    "POST_JB",
    "PUT_JB",
    "DELETE_JB",

   "GET_Device_List_General",
   "GET_Device_List_Information_FromModule",
   "GET_Device_List_Information_Grapper",
    "GET_Device",
    "POST_Device",
    "PUT_Device",
    "DELETE_Device",

    "GET_ModuleSpecification_List",
    "POST_ModuleSpecification",
    "PUT_ModuleSpecification",
    "DELETE_ModuleSpecification",

    "GET_AdapterSpecification_List",
    "GET_AdapterSpecification",
    "POST_AdapterSpecification",
    "PUT_AdapterSpecification",
    "DELETE_AdapterSpecification",

    "GET_Mcc_List",
    "POST_Mcc",
    "PUT_Mcc",
    "DELETE_Mcc",

    "GET_FieldDevice_List",
    "POST_FieldDevice",
    "PUT_FieldDevice",
    "DELETE_FieldDevice",

    "GET_Image_List"
    };
    public static string objectName = "";
    public static bool requestTimeOut = false;
    public static string previousScene;
    public static string recentScene;
    public static string jb_TSD_Title = "";
    public static string jb_TSD_Location = "";
    public static string jb_TSD_Name = "";
    public static List<GameObject> activated_iamgeTargets = new List<GameObject>() { };
    public static bool navigate_from_List_Devices = false;
    public static bool navigate_from_list_JBs = false;
    public static bool navigate_from_JB_TSD_Detail = false;
    public static GameObject generalPanel;
    public static bool loginSuccess = false;
    public static bool isOpenCanvas = false;
    public static bool ready_To_Nav_New_Scene = false;
    public static bool API_Status = false;
    public static string API_Error_Text = "";

    public static bool Load_Initial_Data_From_Selection_Scene = true;

    // //! lưu trữ tên JB và hình ảnh JB (ẢNH JB THỰC TẾ)
    // public static Dictionary<string, List<Sprite>> list_Name_And_Image_JB_Location_A = new Dictionary<string, List<Sprite>>();
    // public static Dictionary<string, List<Sprite>> list_Name_And_Image_JB_Location_B = new Dictionary<string, List<Sprite>>();
    // public static Dictionary<string, List<Sprite>> list_Name_And_Image_JB_Location_C = new Dictionary<string, List<Sprite>>();
    // public static Dictionary<string, List<Sprite>> list_Name_And_Image_JB_Location_D = new Dictionary<string, List<Sprite>>();

    // //! lưu trữ tên JB và đường dẫn ảnh JB (ẢNH JB THỰC TẾ)
    // public static Dictionary<string, List<string>> list_Name_and_Url_JB_Location_A = new Dictionary<string, List<string>>();
    // public static Dictionary<string, List<string>> list_Name_and_Url_JB_Location_B = new Dictionary<string, List<string>>();
    // public static Dictionary<string, List<string>> list_Name_and_Url_JB_Location_C = new Dictionary<string, List<string>>();
    // public static Dictionary<string, List<string>> list_Name_and_Url_JB_Location_D = new Dictionary<string, List<string>>();

    // //! lưu trữ tên JB và hình ảnh JB (Sơ đồ kết nối)
    // public static Dictionary<string, List<Sprite>> list_Name_And_Image_JB_Connection_A = new Dictionary<string, List<Sprite>>();
    // public static Dictionary<string, List<Sprite>> list_Name_And_Image_JB_Connection_B = new Dictionary<string, List<Sprite>>();
    // public static Dictionary<string, List<Sprite>> list_Name_And_Image_JB_Connection_C = new Dictionary<string, List<Sprite>>();
    // public static Dictionary<string, List<Sprite>> list_Name_And_Image_JB_Connection_D = new Dictionary<string, List<Sprite>>();

    // //! lưu trữ tên JB và đường dẫn ảnh JB (Sơ đồ kết nối)
    // public static Dictionary<string, List<string>> list_Name_and_Url_JB_Connection_A = new Dictionary<string, List<string>>();
    // public static Dictionary<string, List<string>> list_Name_and_Url_JB_Connection_B = new Dictionary<string, List<string>>();
    // public static Dictionary<string, List<string>> list_Name_and_Url_JB_Connection_C = new Dictionary<string, List<string>>();
    // public static Dictionary<string, List<string>> list_Name_and_Url_JB_Connection_D = new Dictionary<string, List<string>>();

    // //! Lưu trữ key (Sơ đồ kết nối)
    // public static List<string> list_Key_JB_Connection_A = new List<string>();
    // public static List<string> list_Key_JB_Connection_B = new List<string>();
    // public static List<string> list_Key_JB_Connection_C = new List<string>();
    // public static List<string> list_Key_JB_Connection_D = new List<string>();
    // //! Lưu trữ key (Hình ảnh thực tế)
    // public static List<string> list_Key_JB_Location_A = new List<string>();
    // public static List<string> list_Key_JB_Location_B = new List<string>();
    // public static List<string> list_Key_JB_Location_C = new List<string>();
    // public static List<string> list_Key_JB_Location_D = new List<string>();


    //! Lưu trữ tạm để không cần phải lọc lại
    public static List<Sprite> list_TempJBLocationImage = new List<Sprite>();
    public static List<Sprite> list_TempJBConnectionImage = new List<Sprite>();
    //public static List<Texture2D> list_Image_JB_Location = new List<Texture2D>();
    //    public static List<Texture2D> list_Image_JB_TSD_Wiring = new List<Texture2D>();

    //! Company
    public static int companyId = 0;
    public static CompanyResponseDto temp_CompanyResponseDto;
    public static CompanyBasicDto temp_CompanyBasicDto;
    public static CompanyRequestDto temp_CompanyRequestDto;
    public static List<CompanyBasicDto> temp_ListCompanyBasicDto = new List<CompanyBasicDto>();
    public static Dictionary<string, CompanyBasicDto> temp_Dictionary_CompanyBasicDto = new Dictionary<string, CompanyBasicDto>();

    public static CompanyInformationModel temp_CompanyInformationModel;
    public static List<CompanyInformationModel> temp_List_CompanyInformationModel = new List<CompanyInformationModel>();
    public static Dictionary<string, CompanyInformationModel> temp_Dictionary_CompanyInformationModel = new Dictionary<string, CompanyInformationModel>();

    //!Grapper
    public static int GrapperId = 0;
    public static string GrapperName = "";
    public static List<GrapperBasicDto> temp_ListGrapperBasicDto = new List<GrapperBasicDto>(); // Id, Name, List_Rack_Basic_Dto
    public static GrapperResponseDto temp_GrapperResponseDto; // Id, Name, List_Rack_Basic_Dto
    public static GrapperBasicDto temp_GrapperBasicDto; // Id, Name, List_Rack_Basic_Dto
    public static GrapperRequestDto temp_GrapperRequestDto; // Id, Name, List_Rack_Basic_Dto
    public static Dictionary<string, GrapperBasicDto> temp_Dictionary_GrapperBasicDto = new Dictionary<string, GrapperBasicDto>();

    public static List<GrapperBasicModel> temp_ListGrapperBasicModels = new List<GrapperBasicModel>(); // Id, Name, List_Rack_Basic_Model
    public static GrapperBasicModel temp_GrapperBasicModel; // Id, Name, List_Rack_Basic_Model
    public static List<GrapperInformationModel> temp_ListGrapperInformationModel = new List<GrapperInformationModel>();
    public static Dictionary<string, GrapperInformationModel> temp_Dictionary_GrapperInformationModel = new Dictionary<string, GrapperInformationModel>();

    //!Rack
    public static int rackId = 0;

    public static List<RackBasicDto> temp_ListRackBasicDto = new List<RackBasicDto>(); // Id, Name, List_Rack_Basic_Dto
    public static RackResponseDto temp_RackResponseDto; // Id, Name, List_Rack_Basic_Dto
    public static RackBasicDto temp_RackBasicDto; // Id, Name, List_Rack_Basic_Dto
    public static RackRequestDto temp_RackRequestDto; // Id, Name, List_Rack_Basic_Dto

    public static List<RackBasicModel> temp_ListRackBasicModels = new List<RackBasicModel>(); // Id, Name, List_ ModuleBasicNonRackModel
    public static RackBasicModel temp_RackBasicModel; // Id, Name, List_ ModuleBasicNonRackModel
    public static List<RackInformationModel> temp_ListRackInformationModel = new List<RackInformationModel>();
    public static Dictionary<string, RackInformationModel> temp_Dictionary_RackInformationModel = new Dictionary<string, RackInformationModel>();


    //!Module
    public static int moduleId = 0;
    public static string moduleName = "";
    public static List<ModuleBasicDto> temp_ListModuleBasicDto = new List<ModuleBasicDto>(); // Id, Name, List_Module_Basic_Dto
    public static ModuleResponseDto temp_ModuleResponseDto; // Id, Name, List_Module_Basic_Dto
    public static ModuleBasicDto temp_ModuleBasicDto; // Id, Name, List_Module_Basic_Dto
    public static ModuleRequestDto temp_ModuleRequestDto; // Id, Name, List_Module_Basic_Dto

    public static ModuleBasicModel temp_ModuleBasicModel; // Id, Name, Rack_Non_List_Module_Model
    public static List<ModuleBasicModel> temp_ListModuleBasicModels = new List<ModuleBasicModel>(); // Id, Name, Rack_Non_List_Module_Model
    public static Dictionary<string, ModuleBasicModel> temp_Dictionary_ModuleBasicModel = new Dictionary<string, ModuleBasicModel>();

    public static ModuleInformationModel temp_ModuleInformationModel;
    public static List<ModuleInformationModel> temp_ListModuleInformationModel = new List<ModuleInformationModel>();
    public static Dictionary<string, ModuleInformationModel> temp_Dictionary_ModuleInformationModel = new Dictionary<string, ModuleInformationModel>();

    //! Device
    public static int deviceId = 0;
    public static string deviceCode = "";
    public static List<DeviceBasicDto> temp_ListDeviceBasicDto = new List<DeviceBasicDto>(); // Id, Name, List_Device_Basic_Dto
    public static DeviceResponseDto temp_DeviceResponseDto; // Id, Name, List_Device_Basic_Dto
    public static DeviceBasicDto temp_DeviceBasicDto; // Id, Name, List_Device_Basic_Dto
    public static DeviceRequestDto temp_DeviceRequestDto; // Id, Name, List_Device_Basic_Dto


    public static DeviceInformationModel temp_DeviceInformationModel;
    public static List<DeviceInformationModel> temp_ListDeviceInformationModel = new List<DeviceInformationModel>();
    public static Dictionary<string, DeviceInformationModel> temp_Dictionary_DeviceInformationModel = new Dictionary<string, DeviceInformationModel>();

    public static Dictionary<string, string> temp_Dictionary_DeviceIOAddress = new Dictionary<string, string>();
    public static List<ImageInformationModel> temp_List_AdditionalImages = new List<ImageInformationModel>();



    //! 
    public static int JBId = 0;
    public static string JBName = "";

    public static List<JBBasicDto> temp_ListJBBasicDto = new List<JBBasicDto>(); // Id, Name, List_JB_Basic_Dto
    public static JBResponseDto temp_JBResponseDto; // Id, Name, List_JB_Basic_Dto
    public static JBBasicDto temp_JBBasicDto; // Id, Name, List_JB_Basic_Dto
    public static JBRequestDto temp_JBRequestDto; // Id, Name, List_Device_Basic_Dto


    public static Dictionary<string, JBBasicModel> temp_Dictionary_JBBasicModel = new Dictionary<string, JBBasicModel>();
    public static JBInformationModel temp_JBInformationModel;
    public static List<JBInformationModel> temp_ListJBInformationModel_FromModule = new List<JBInformationModel>();
    public static List<JBInformationModel> temp_ListJBInformationModel = new List<JBInformationModel>();
    public static Dictionary<string, JBInformationModel> temp_Dictionary_JBInformationModel = new Dictionary<string, JBInformationModel>();
    public static bool ActiveCloseCanvasButton = false;


    //! Mccs
    public static int mccId = 0;
    public static string mccCabinetCode = "";
    public static List<MccBasicDto> temp_ListMccBasicDto = new List<MccBasicDto>(); // Id, Name, List_Mcc_Basic_Dto
    public static MccResponseDto temp_MccResponseDto; // Id, Name, List_Mcc_Basic_Dto
    public static MccBasicDto temp_MccBasicDto; // Id, Name, List_Mcc_Basic_Dto
    public static MccRequestDto temp_MccRequestDto; // Id, Name, List_Device_Basic_Dto

    public static MccInformationModel temp_MCCInformationModel;
    public static List<MccInformationModel> temp_ListMCCInformationModel = new List<MccInformationModel>();
    public static Dictionary<string, MccInformationModel> temp_Dictionary_MCCInformationModel = new Dictionary<string, MccInformationModel>();


    //! Field Device
    public static int fieldDeviceId = 0;
    public static string fieldDeviceName = "";

    public static List<FieldDeviceBasicDto> temp_ListFieldDeviceBasicDto = new List<FieldDeviceBasicDto>(); // Id, Name, List_Field_Device_Basic_Dto
    public static FieldDeviceResponseDto temp_FieldDeviceResponseDto; // Id, Name, List_Field_Device_Basic_Dto
    public static FieldDeviceBasicDto temp_FieldDeviceBasicDto; // Id, Name, List_Field_Device_Basic_Dto
    public static FieldDeviceRequestDto temp_FieldDeviceRequestDto; // Id, Name, List_Device_Basic_Dto


    public static FieldDeviceInformationModel temp_FieldDeviceInformationModel;
    public static List<FieldDeviceInformationModel> temp_ListFieldDeviceInformationModel = new List<FieldDeviceInformationModel>();
    public static Dictionary<string, List<FieldDeviceInformationModel>> temp_Dictionary_FieldDeviceInformationModel = new Dictionary<string, List<FieldDeviceInformationModel>>();


    //! ModuleSpecification 

    public static int moduleSpecificationId = 0;
    public static string moduleSpecificationCode = "";

    public static List<ModuleSpecificationBasicDto> temp_ListModuleSpecificationBasicDto = new List<ModuleSpecificationBasicDto>(); // Id, Name, List_Field_Device_Basic_Dto
    public static ModuleSpecificationResponseDto temp_ModuleSpecificationResponseDto; // Id, Name, List_Field_Device_Basic_Dto
    public static ModuleSpecificationBasicDto temp_ModuleSpecificationBasicDto; // Id, Name, List_Field_Device_Basic_Dto
    public static ModuleSpecificationRequestDto temp_ModuleSpecificationRequestDto; // Id, Name, List_Device_Basic_Dto


    public static Dictionary<string, ModuleSpecificationBasicDto> temp_Dictionary_ModuleSpecificationBasicDto = new Dictionary<string, ModuleSpecificationBasicDto>();
    public static ModuleSpecificationBasicModel temp_ModuleSpecificationBasicModel;


    public static ModuleSpecificationModel temp_ModuleSpecificationModel;
    public static List<ModuleSpecificationModel> temp_ListModuleSpecificationModel = new List<ModuleSpecificationModel>();
    public static Dictionary<string, ModuleSpecificationModel> temp_Dictionary_ModuleSpecificationModel = new Dictionary<string, ModuleSpecificationModel>();

    //! AdapterSpecification
    public static int adapterSpecificationId = 0;
    public static string adapterSpecificationCode = "";

    public static List<AdapterSpecificationBasicDto> temp_ListAdapterSpecificationBasicDto = new List<AdapterSpecificationBasicDto>();
    public static AdapterSpecificationResponseDto temp_AdapterSpecificationResponseDto;
    public static AdapterSpecificationBasicDto temp_AdapterSpecificationBasicDto;
    public static AdapterSpecificationRequestDto temp_AdapterSpecificationRequestDto;

    public static Dictionary<string, AdapterSpecificationBasicDto> temp_Dictionary_AdapterSpecificationBasicDto = new Dictionary<string, AdapterSpecificationBasicDto>();

    public static AdapterSpecificationModel temp_AdapterSpecificationModel;
    public static List<AdapterSpecificationModel> temp_ListAdapterSpecificationModel = new List<AdapterSpecificationModel>();
    public static Dictionary<string, AdapterSpecificationModel> temp_Dictionary_AdapterSpecificationModel = new Dictionary<string, AdapterSpecificationModel>();

    //! ImageInformation
    public static int ImageId = 0;
    public static string ImageName = "";
    public static Dictionary<string, ImageBasicModel> temp_Dictionary_ImageBasicModel = new Dictionary<string, ImageBasicModel>();

    public static ImageInformationModel temp_ImageInformationModel;
    public static List<ImageInformationModel> temp_ListImageInformationModel = new List<ImageInformationModel>();

    public static Dictionary<string, ImageInformationModel> temp_Dictionary_ImageInformationModel = new Dictionary<string, ImageInformationModel>();

    public static List<string> temp_ListImage_Name = new List<string>();

    public static bool PickPhotoFromCamera = false;



    public static List<Texture2D> temp_ListFieldDeviceConnectionImages = new List<Texture2D>();
    public static List<DeviceInformationModel> temp_ListDeviceInformationModel_FromModule = new List<DeviceInformationModel>();
    public static Dictionary<string, List<Texture2D>> temp_listJBConnectionImageFromModule = new Dictionary<string, List<Texture2D>>();
    public static Dictionary<string, Texture2D> temp_listJBLocationImageFromModule = new Dictionary<string, Texture2D>();
    public static AccountModel accountModel = new AccountModel("", "");

    public static List<string> PLCBoxScene = new List<string>()
    {
        "PLCBoxGrapA",
        "PLCBoxGrapB",
        "PLCBoxGrapC",
        "PLCBoxLH",
    };
    public static List<string> jBLocation = new List<string>()
    {
        "Hầm cáp MCC búa",              // JB1, JB2
        "Cầu Thang lên Chè Cân",        // JB3
        "Hành Lang Khuếch Tán",         // JB4
        "Dưới chân Che Ép",             // JB5, JB6
        "Trên Vít Khuếch Tán",          // JB7, JB8 Bis, JB8
        "Che Ép",                       // JB14
        "Duới hầm cáp MCC che Ép",      // JB114, JB102, J101, JB111, Jb112
        "Ngay cầu thang lên MCC"        // JB11_Bis
    };

    public static List<string> sceneNamesLandScape = new List<string>()
    {
       "ControlDeviceARScene",
       "FieldDeviceARScene",
    };

    public static List<string> list_jBName = new List<string>()
    {

    };
    public static List<string> list_ModuleIOName = new List<string>();
    public static List<string> list_DeviceCode = new List<string>() { };
    public static List<string> list_ImageName = new List<string>();

    public static bool isCameraPaused = false;


}