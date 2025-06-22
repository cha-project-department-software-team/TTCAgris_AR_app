
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine.Scripting;
#nullable enable

namespace Domain.Entities
{

  [Preserve]
  public class ModuleSpecificationEntity
  {
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("code")]
    public string Code { get; set; } = string.Empty;

    // [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
    [JsonProperty("type")]
    public string? Type { get; set; }

    // [JsonProperty("numOfIO", NullValueHandling = NullValueHandling.Ignore)]
    [JsonProperty("numOfIO")]
    public string? NumOfIO { get; set; }

    // [JsonProperty("signalType", NullValueHandling = NullValueHandling.Ignore)]
    [JsonProperty("signalType")]
    public string? SignalType { get; set; }

    // [JsonProperty("compatibleTBUs", NullValueHandling = NullValueHandling.Ignore)]
    [JsonProperty("compatibleTBUs")]
    public string? CompatibleTBUs { get; set; }

    // [JsonProperty("operatingVoltage", NullValueHandling = NullValueHandling.Ignore)]\
    [JsonProperty("operatingVoltage")]
    public string? OperatingVoltage { get; set; }

    // [JsonProperty("operatingCurrent", NullValueHandling = NullValueHandling.Ignore)]
    [JsonProperty("operatingCurrent")]
    public string? OperatingCurrent { get; set; }

    // [JsonProperty("flexbusCurrent", NullValueHandling = NullValueHandling.Ignore)]
    [JsonProperty("flexbusCurrent")]
    public string? FlexbusCurrent { get; set; }

    // [JsonProperty("alarm", NullValueHandling = NullValueHandling.Ignore)]
    [JsonProperty("alarm")]
    public string? Alarm { get; set; }

    // [JsonProperty("note", NullValueHandling = NullValueHandling.Ignore)]
    [JsonProperty("note")]
    public string? Note { get; set; }

    // [JsonProperty("pdfManual", NullValueHandling = NullValueHandling.Ignore)]
    [JsonProperty("pdfManual")]
    public string? PdfManual { get; set; }


    public bool ShouldSerializeId()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.POSTModuleSpecification.GetDescription(),
        HttpMethodTypeEnum.PUTModuleSpecification.GetDescription(),
      };
      return !apiRequestType.Any(request => allowedRequests.Contains(request));
    }

    public bool ShouldSerializeCode()
    {
      return true;
    }

    public bool ShouldSerializeType()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETModuleSpecification.GetDescription(),
        HttpMethodTypeEnum.POSTModuleSpecification.GetDescription(),
        HttpMethodTypeEnum.PUTModuleSpecification.GetDescription()
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));
    }
    public bool ShouldSerializeNumOfIO()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETModuleSpecification.GetDescription(),
        HttpMethodTypeEnum.POSTModuleSpecification.GetDescription(),
        HttpMethodTypeEnum.PUTModuleSpecification.GetDescription()
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));
    }
    public bool ShouldSerializeSignalType()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETModuleSpecification.GetDescription(),
        HttpMethodTypeEnum.POSTModuleSpecification.GetDescription(),
        HttpMethodTypeEnum.PUTModuleSpecification.GetDescription()
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));
    }
    public bool ShouldSerializeCompatibleTBUs()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETModuleSpecification.GetDescription(),
        HttpMethodTypeEnum.POSTModuleSpecification.GetDescription(),
        HttpMethodTypeEnum.PUTModuleSpecification.GetDescription()
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));
    }
    public bool ShouldSerializeOperatingVoltage()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETModuleSpecification.GetDescription(),
        HttpMethodTypeEnum.POSTModuleSpecification.GetDescription(),
        HttpMethodTypeEnum.PUTModuleSpecification.GetDescription()
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));
    }
    public bool ShouldSerializeOperatingCurrent()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETModuleSpecification.GetDescription(),
        HttpMethodTypeEnum.POSTModuleSpecification.GetDescription(),
        HttpMethodTypeEnum.PUTModuleSpecification.GetDescription()
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));
    }
    public bool ShouldSerializeFlexbusCurrent()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETModuleSpecification.GetDescription(),
        HttpMethodTypeEnum.POSTModuleSpecification.GetDescription(),
        HttpMethodTypeEnum.PUTModuleSpecification.GetDescription()
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));
    }
    public bool ShouldSerializeAlarm()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETModuleSpecification.GetDescription(),
        HttpMethodTypeEnum.POSTModuleSpecification.GetDescription(),
        HttpMethodTypeEnum.PUTModuleSpecification.GetDescription()
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));
    }
    public bool ShouldSerializeNote()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETModuleSpecification.GetDescription(),
        HttpMethodTypeEnum.POSTModuleSpecification.GetDescription(),
        HttpMethodTypeEnum.PUTModuleSpecification.GetDescription()
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));
    }
    public bool ShouldSerializePdfManual()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETModuleSpecification.GetDescription(),
        HttpMethodTypeEnum.POSTModuleSpecification.GetDescription(),
        HttpMethodTypeEnum.PUTModuleSpecification.GetDescription()
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));
    }

    [Preserve]
    public ModuleSpecificationEntity()
    {

    }

    [Preserve]
    public ModuleSpecificationEntity(string code)
    {
      Code = string.IsNullOrEmpty(code) ? throw new ArgumentNullException(nameof(code)) : code;
    }

    [Preserve]
    //! Dùng để Get hoặc làm field lúc đẩy lên ở các Entity khác
    public ModuleSpecificationEntity(int id, string code)
    {
      Id = id;
      Code = string.IsNullOrEmpty(code) ? throw new ArgumentNullException(nameof(code)) : code;
    }

    [Preserve]
    public ModuleSpecificationEntity(
    int id,
    string? code,
    string? type,
    string? numOfIO,
    string? signalType,
    string? compatibleTBUs,
    string? operatingVoltage,
    string? operatingCurrent,
    string? flexbusCurrent,
    string? alarm,
    string? note,
    string? pdfManual)
    {
      Id = id;

      Code = string.IsNullOrEmpty(code) ? throw new ArgumentNullException(nameof(code)) : code;
      Type = string.IsNullOrEmpty(type) ? "Chưa cập nhật" : type;
      NumOfIO = string.IsNullOrEmpty(numOfIO) ? "Chưa cập nhật" : numOfIO;
      SignalType = string.IsNullOrEmpty(signalType) ? "Chưa cập nhật" : signalType;
      CompatibleTBUs = string.IsNullOrEmpty(compatibleTBUs) ? "Chưa cập nhật" : compatibleTBUs;
      OperatingVoltage = string.IsNullOrEmpty(operatingVoltage) ? "Chưa cập nhật" : operatingVoltage;
      OperatingCurrent = string.IsNullOrEmpty(operatingCurrent) ? "Chưa cập nhật" : operatingCurrent;
      FlexbusCurrent = string.IsNullOrEmpty(flexbusCurrent) ? "Chưa cập nhật" : flexbusCurrent;
      Alarm = string.IsNullOrEmpty(alarm) ? "Chưa cập nhật" : alarm;
      Note = string.IsNullOrEmpty(note) ? "Chưa cập nhật" : note;
      PdfManual = string.IsNullOrEmpty(pdfManual) ? "Chưa cập nhật" : pdfManual;
    }
    [Preserve]
    public ModuleSpecificationEntity(
   string? code,
   string? type,
   string? numOfIO,
   string? signalType,
   string? compatibleTBUs,
   string? operatingVoltage,
   string? operatingCurrent,
   string? flexbusCurrent,
   string? alarm,
   string? note,
   string? pdfManual)
    {
      Code = string.IsNullOrEmpty(code) ? throw new ArgumentNullException(nameof(code)) : code;

      Type = string.IsNullOrEmpty(type) ? "Chưa cập nhật" : type;
      NumOfIO = string.IsNullOrEmpty(numOfIO) ? "Chưa cập nhật" : numOfIO;
      SignalType = string.IsNullOrEmpty(signalType) ? "Chưa cập nhật" : signalType;
      CompatibleTBUs = string.IsNullOrEmpty(compatibleTBUs) ? "Chưa cập nhật" : compatibleTBUs;
      OperatingVoltage = string.IsNullOrEmpty(operatingVoltage) ? "Chưa cập nhật" : operatingVoltage;
      OperatingCurrent = string.IsNullOrEmpty(operatingCurrent) ? "Chưa cập nhật" : operatingCurrent;
      FlexbusCurrent = string.IsNullOrEmpty(flexbusCurrent) ? "Chưa cập nhật" : flexbusCurrent;
      Alarm = string.IsNullOrEmpty(alarm) ? "Chưa cập nhật" : alarm;
      Note = string.IsNullOrEmpty(note) ? "Chưa cập nhật" : note;
      PdfManual = string.IsNullOrEmpty(pdfManual) ? "Chưa cập nhật" : pdfManual;
    }

  }
}