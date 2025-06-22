using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine.Scripting;
#nullable enable
namespace Domain.Entities
{
  public class DeviceEntity
  {
    [JsonProperty("id")]
    public int Id { get; set; }
    [JsonProperty("module")]
    public ModuleEntity? ModuleEntity { get; set; }
    // [JsonProperty("JB",NullValueHandling = NullValueHandling.Ignore)]
    [JsonProperty("code")]
    public string Code { get; set; } = string.Empty;
    // [JsonProperty("function", NullValueHandling = NullValueHandling.Ignore)]
    [JsonProperty("function")]
    public string? Function { get; set; }
    // [JsonProperty("range", NullValueHandling = NullValueHandling.Ignore)]
    [JsonProperty("range")]
    public string? Range { get; set; }
    // [JsonProperty("Unit", NullValueHandling = NullValueHandling.Ignore)]
    [JsonProperty("unit")]
    public string? Unit { get; set; }
    // [JsonProperty("ioAddress", NullValueHandling = NullValueHandling.Ignore)]
    [JsonProperty("ioAddress")]
    public string? IOAddress { get; set; }
    // [JsonProperty("module", NullValueHandling = NullValueHandling.Ignore)]
    // [JsonProperty("additionalConnectionImages", NullValueHandling = NullValueHandling.Ignore)]
    [JsonProperty("additionalConnectionImages")]
    public List<ImageEntity>? AdditionalConnectionImageEntities { get; set; }
    [JsonProperty("jBs")]
    public List<JBEntity>? JBEntities { get; set; }


    public bool ShouldSerializeId()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.POSTDevice.GetDescription(),
        HttpMethodTypeEnum.PUTDevice.GetDescription(),
      };
      return !apiRequestType.Any(request => allowedRequests.Contains(request));
    }
    public bool ShouldSerializeCode()
    {

      return true;
    }
    public bool ShouldSerializeFunction()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETListDeviceInformationFromGrapper.GetDescription(),
        //HttpMethodTypeEnum.GETListDeviceInformationFromModule.GetDescription(),
        HttpMethodTypeEnum.GETDevice.GetDescription(),
        HttpMethodTypeEnum.POSTDevice.GetDescription(),
        HttpMethodTypeEnum.PUTDevice.GetDescription()
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));

    }
    public bool ShouldSerializeRange()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETListDeviceInformationFromGrapper.GetDescription(),
        //HttpMethodTypeEnum.GETListDeviceInformationFromModule.GetDescription(),
        HttpMethodTypeEnum.GETDevice.GetDescription(),
        HttpMethodTypeEnum.POSTDevice.GetDescription(),
        HttpMethodTypeEnum.PUTDevice.GetDescription()
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));

    }
    public bool ShouldSerializeUnit()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETListDeviceInformationFromGrapper.GetDescription(),
        //HttpMethodTypeEnum.GETListDeviceInformationFromModule.GetDescription(),
        HttpMethodTypeEnum.GETDevice.GetDescription(),
        HttpMethodTypeEnum.POSTDevice.GetDescription(),
        HttpMethodTypeEnum.PUTDevice.GetDescription()
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));

    }
    public bool ShouldSerializeIOAddress()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETListDeviceInformationFromGrapper.GetDescription(),
        //HttpMethodTypeEnum.GETListDeviceInformationFromModule.GetDescription(),
        HttpMethodTypeEnum.GETDevice.GetDescription(),
        HttpMethodTypeEnum.POSTDevice.GetDescription(),
        HttpMethodTypeEnum.PUTDevice.GetDescription()
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));

    }

    public bool ShouldSerializeModuleEntity()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETListDeviceInformationFromGrapper.GetDescription(),
        //HttpMethodTypeEnum.GETListDeviceInformationFromModule.GetDescription(),
        HttpMethodTypeEnum.GETDevice.GetDescription(),
        HttpMethodTypeEnum.POSTDevice.GetDescription(),
        HttpMethodTypeEnum.PUTDevice.GetDescription()
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));

    }

    public bool ShouldSerializeJBEntities()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETListDeviceInformationFromGrapper.GetDescription(),
        //HttpMethodTypeEnum.GETListDeviceInformationFromModule.GetDescription(),
        HttpMethodTypeEnum.GETDevice.GetDescription(),
        HttpMethodTypeEnum.POSTDevice.GetDescription(),
        HttpMethodTypeEnum.PUTDevice.GetDescription()
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));
    }

    public bool ShouldSerializeAdditionalConnectionImageEntities()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETListDeviceInformationFromGrapper.GetDescription(),
        //HttpMethodTypeEnum.GETListDeviceInformationFromModule.GetDescription(),
        HttpMethodTypeEnum.GETDevice.GetDescription(),
        HttpMethodTypeEnum.POSTDevice.GetDescription(),
        HttpMethodTypeEnum.PUTDevice.GetDescription()
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));

    }



    [Preserve]
    public DeviceEntity()
    {
      // AdditionalConnectionImageEntities = new List<ImageEntity>();
    }

    [Preserve]
    public DeviceEntity(int id, string code)
    {
      Id = id;
      Code = string.IsNullOrEmpty(code) ? throw new ArgumentNullException(nameof(code)) : code;
    }

    [Preserve]

    public DeviceEntity(string code)
    {
      Code = string.IsNullOrEmpty(code) ? throw new ArgumentNullException(nameof(code)) : code;
    }


    [Preserve]
    public DeviceEntity(int id, string code, string function, string range, string unit, string ioAddress, ModuleEntity? moduleEntity, List<JBEntity>? jbEntities, List<ImageEntity>? additionalConnectionImageEntities)
    {
      Id = id;
      Code = string.IsNullOrEmpty(code) ? throw new ArgumentNullException(nameof(code)) : code;
      Function = string.IsNullOrEmpty(function) ? "Chưa cập nhật" : function;
      Range = string.IsNullOrEmpty(range) ? "Chưa cập nhật" : range;
      Unit = string.IsNullOrEmpty(unit) ? "Chưa cập nhật" : unit;
      IOAddress = string.IsNullOrEmpty(ioAddress) ? "Chưa cập nhật" : ioAddress;
      ModuleEntity = moduleEntity ?? null;
      JBEntities = (jbEntities == null || (jbEntities != null && !jbEntities.Any())) ? new List<JBEntity>() : jbEntities;
      AdditionalConnectionImageEntities = (additionalConnectionImageEntities == null
         || (additionalConnectionImageEntities != null && !additionalConnectionImageEntities.Any()))
         ? new List<ImageEntity>() : additionalConnectionImageEntities;
    }
    [Preserve]
    public DeviceEntity(string code, string function, string range, string unit, string ioAddress, ModuleEntity? moduleEntity, List<JBEntity>? jbEntities, List<ImageEntity>? additionalConnectionImageEntities)
    {
      Code = string.IsNullOrEmpty(code) ? throw new ArgumentNullException(nameof(code)) : code;

      Function = string.IsNullOrEmpty(function) ? "Chưa cập nhật" : function;

      Range = string.IsNullOrEmpty(range) ? "Chưa cập nhật" : range;

      Unit = string.IsNullOrEmpty(unit) ? "Chưa cập nhật" : unit;

      IOAddress = string.IsNullOrEmpty(ioAddress) ? "Chưa cập nhật" : ioAddress;

      ModuleEntity = moduleEntity ?? null;

      JBEntities = (jbEntities == null || (jbEntities != null && !jbEntities.Any())) ? new List<JBEntity>() : jbEntities;

      AdditionalConnectionImageEntities = (additionalConnectionImageEntities == null
      || (additionalConnectionImageEntities != null && !additionalConnectionImageEntities.Any()))
      ? new List<ImageEntity>() : additionalConnectionImageEntities;
    }
  }
}