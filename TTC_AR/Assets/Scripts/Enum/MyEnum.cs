using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection;

public enum MyEnum
{
    [Description("GrapperAScanScene")]
    GrapperAScanScene,
    [Description("GrapperBScanScene")]
    GrapperBScanScene,
    [Description("GrapperCScanScene")]
    GrapperCScanScene,
    [Description("LHScanScene")]
    LHScanScene,
    [Description("FieldDevicesScene")]
    FieldDevicesScene,
    [Description("LoginScene")]
    LoginScene,
    [Description("PLCBoxGrapA")]
    PLCBoxGrapA,
    [Description("PLCBoxGrapB")]
    PLCBoxGrapB,
    [Description("PLCBoxGrapC")]
    PLCBoxGrapC,
    [Description("PLCBoxLH")]
    PLCBoxLH,
    [Description("MenuScene")]
    MenuScene,
    [Description("SelectionsScene")]
    SelectionsScene,
}

public enum HttpMethodTypeEnum
{
    //?Company
    [Description("GET_Company_List")] GETListCompany,
    [Description("GET_Company")] GETCompany,
    //?Grapper
    [Description("GET_Grapper_List")] GETListGrapper,
    [Description("GET_Grapper")] GETGrapper,
    [Description("POST_Grapper")] POSTGrapper,
    [Description("PUT_Grapper")] PUTGrapper,
    [Description("DELETE_Grapper")] DELETEGrapper,

    //?Rack
    [Description("GET_Rack_List")] GETListRack,
    [Description("GET_Rack")] GETRack,

    [Description("POST_Rack")] POSTRack,
    [Description("PUT_Rack")] PUTRack,
    [Description("DELETE_Rack")] DELETERack,

    //?Module
    [Description("GET_Module_List")] GETListModule,
    [Description("GET_Module")] GETModule,
    [Description("POST_Module")] POSTModule,
    [Description("PUT_Module")] PUTModule,
    [Description("DELETE_Module")] DELETEModule,

    //?JB
    [Description("GET_JB_List_General")] GETListJBGeneral,
    [Description("GET_JB_List_Information")] GETListJBInformation,
    [Description("GET_JB")] GETJB,
    [Description("POST_JB")] POSTJB,
    [Description("PUT_JB")] PUTJB,
    [Description("DELETE_JB")] DELETEJB,

    //?Device
    [Description("GET_Device_List_General")] GETListDeviceGeneral,
    [Description("GET_Device_List_Information_FromModule")] GETListDeviceInformationFromModule,
    [Description("GET_Device_List_Information_FromGrapper")] GETListDeviceInformationFromGrapper,
    [Description("GET_Device")] GETDevice,
    [Description("POST_Device")] POSTDevice,
    [Description("PUT_Device")] PUTDevice,
    [Description("DELETE_Device")] DELETEDevice,

    //?ModuleSpecification
    [Description("GET_ModuleSpecification_List")] GETListModuleSpecification,
    [Description("GET_ModuleSpecification")] GETModuleSpecification,

    [Description("POST_ModuleSpecification")] POSTModuleSpecification,
    [Description("PUT_ModuleSpecification")] PUTModuleSpecification,
    [Description("DELETE_ModuleSpecification")] DELETEModuleSpecification,

    //?AdapterSpecification
    [Description("GET_AdapterSpecification_List")] GETListAdapterSpecification,
    [Description("GET_AdapterSpecification")] GETAdapterSpecification,
    [Description("POST_AdapterSpecification")] POSTAdapterSpecification,
    [Description("PUT_AdapterSpecification")] PUTAdapterSpecification,
    [Description("DELETE_AdapterSpecification")] DELETEAdapterSpecification,

    //?Mcc
    [Description("GET_Mcc_List")] GETListMcc,
    [Description("GET_Mcc")] GETMcc,
    [Description("POST_Mcc")] POSTMcc,
    [Description("PUT_Mcc")] PUTMcc,
    [Description("DELETE_Mcc")] DELETEMcc,

    //?FieldDevice (Có lỗi tên ở phần gốc)
    [Description("GET_FieldDevice_List")] GETListFieldDevice,
    [Description("GET_FieldDevice")] GETFieldDevice,
    [Description("POST_FieldDevice")] POSTFieldDevice,
    [Description("PUT_FieldDevice")] PUTFieldDevice,
    [Description("DELETE_FieldDevice")] DELETEFieldDevice,

    //?Image (Có lỗi tên ở phần gốc)
    [Description("GET_Image_List")] GETListImage,
    [Description("GET_Image")] GETImage,
    [Description("POST_Image")] POSTImage,
    [Description("DELETE_Image")] DELETEImage,

}

public static class EnumExtensions
{
    // Dùng ConcurrentDictionary để lưu cache, tránh việc truy vấn Reflection nhiều lần
    private static readonly ConcurrentDictionary<Enum, string> _descriptionCache = new ConcurrentDictionary<Enum, string>();

    public static string GetDescription<T>(this T value) where T : Enum
    {
        return _descriptionCache.GetOrAdd(value, v =>
        {
            FieldInfo field = typeof(T).GetField(v.ToString());
            DescriptionAttribute attribute = field?.GetCustomAttribute<DescriptionAttribute>();
            return attribute?.Description ?? v.ToString();
        });
    }
}
