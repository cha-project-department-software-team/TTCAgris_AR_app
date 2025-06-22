using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine.Scripting;
#nullable enable
namespace Domain.Entities
{
  [Preserve]
  public class AdapterSpecificationEntity
  {
    [JsonProperty("id")]
    public int Id { get; set; }
    [JsonProperty("code")]
    public string Code { get; set; } = string.Empty;

    // [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
    [JsonProperty("type")]
    public string? Type { get; set; }
    // [JsonProperty("communication", NullValueHandling = NullValueHandling.Ignore)]
    [JsonProperty("communication")]
    public string? Communication { get; set; }
    // [JsonProperty("numOfModulesAllowed", NullValueHandling = NullValueHandling.Ignore)]
    [JsonProperty("numOfModulesAllowed")]
    public string? NumOfModulesAllowed { get; set; }
    // [JsonProperty("commSpeed", NullValueHandling = NullValueHandling.Ignore)]
    [JsonProperty("commSpeed")]
    public string? CommSpeed { get; set; }
    // [JsonProperty("inputSupply", NullValueHandling = NullValueHandling.Ignore)]
    [JsonProperty("inputSupply")]
    public string? InputSupply { get; set; }
    // [JsonProperty("outputSupply", NullValueHandling = NullValueHandling.Ignore)]
    [JsonProperty("outputSupply")]
    public string? OutputSupply { get; set; }
    // [JsonProperty("inrushCurrent", NullValueHandling = NullValueHandling.Ignore)]
    [JsonProperty("inrushCurrent")]
    public string? InrushCurrent { get; set; }
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
        HttpMethodTypeEnum.POSTAdapterSpecification.GetDescription(),
        HttpMethodTypeEnum.PUTAdapterSpecification.GetDescription(),
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
        HttpMethodTypeEnum.GETAdapterSpecification.GetDescription(),

        HttpMethodTypeEnum.POSTAdapterSpecification.GetDescription(),
        HttpMethodTypeEnum.PUTAdapterSpecification.GetDescription()
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));
    }
    public bool ShouldSerializeCommunication()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETAdapterSpecification.GetDescription(),

        HttpMethodTypeEnum.POSTAdapterSpecification.GetDescription(),
        HttpMethodTypeEnum.PUTAdapterSpecification.GetDescription()
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));
    }
    public bool ShouldSerializeNumOfModulesAllowed()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETAdapterSpecification.GetDescription(),

        HttpMethodTypeEnum.POSTAdapterSpecification.GetDescription(),
        HttpMethodTypeEnum.PUTAdapterSpecification.GetDescription()
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));
    }
    public bool ShouldSerializeCommSpeed()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETAdapterSpecification.GetDescription(),

        HttpMethodTypeEnum.POSTAdapterSpecification.GetDescription(),
        HttpMethodTypeEnum.PUTAdapterSpecification.GetDescription()
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));
    }
    public bool ShouldSerializeInputSupply()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETAdapterSpecification.GetDescription(),

        HttpMethodTypeEnum.POSTAdapterSpecification.GetDescription(),
        HttpMethodTypeEnum.PUTAdapterSpecification.GetDescription()
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));
    }
    public bool ShouldSerializeOutputSupply()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETAdapterSpecification.GetDescription(),

        HttpMethodTypeEnum.POSTAdapterSpecification.GetDescription(),
        HttpMethodTypeEnum.PUTAdapterSpecification.GetDescription()
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));
    }
    public bool ShouldSerializeInrushCurrent()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETAdapterSpecification.GetDescription(),

        HttpMethodTypeEnum.POSTAdapterSpecification.GetDescription(),
        HttpMethodTypeEnum.PUTAdapterSpecification.GetDescription()
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));
    }
    public bool ShouldSerializeAlarm()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETAdapterSpecification.GetDescription(),

        HttpMethodTypeEnum.POSTAdapterSpecification.GetDescription(),
        HttpMethodTypeEnum.PUTAdapterSpecification.GetDescription()
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));
    }
    public bool ShouldSerializeNote()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETAdapterSpecification.GetDescription(),

        HttpMethodTypeEnum.POSTAdapterSpecification.GetDescription(),
        HttpMethodTypeEnum.PUTAdapterSpecification.GetDescription()
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));
    }
    public bool ShouldSerializePdfManual()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETAdapterSpecification.GetDescription(),

        HttpMethodTypeEnum.POSTAdapterSpecification.GetDescription(),
        HttpMethodTypeEnum.PUTAdapterSpecification.GetDescription()
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));
    }


    [Preserve]
    public AdapterSpecificationEntity()
    {

    }

    [Preserve]
    public AdapterSpecificationEntity(string code)
    {
      Code = string.IsNullOrEmpty(code) ? throw new ArgumentNullException(nameof(code)) : code;
    }

    [Preserve]
    public AdapterSpecificationEntity(int id, string code)
    {
      Id = id;
      Code = string.IsNullOrEmpty(code) ? throw new ArgumentNullException(nameof(code)) : code;
      // Code = string.IsNullOrEmpty(code) ? throw new ArgumentNullException(nameof(code)) : code;
    }

    [Preserve]
    public AdapterSpecificationEntity(int id, string code, string? type, string? communication, string? numOfModulesAllowed, string? commSpeed, string? inputSupply, string? outputSupply, string? inrushCurrent, string? alarm, string? note, string? pdfManual)
    {
      Id = id;
      Code = string.IsNullOrEmpty(code) ? throw new ArgumentNullException(nameof(code)) : code;
      Type = string.IsNullOrEmpty(type) ? "Chưa cập nhật" : type;
      Communication = string.IsNullOrEmpty(communication) ? "Chưa cập nhật" : communication;
      NumOfModulesAllowed = string.IsNullOrEmpty(numOfModulesAllowed) ? "Chưa cập nhật" : numOfModulesAllowed;
      CommSpeed = string.IsNullOrEmpty(commSpeed) ? "Chưa cập nhật" : commSpeed;
      InputSupply = string.IsNullOrEmpty(inputSupply) ? "Chưa cập nhật" : inputSupply;
      OutputSupply = string.IsNullOrEmpty(outputSupply) ? "Chưa cập nhật" : outputSupply;
      InrushCurrent = string.IsNullOrEmpty(inrushCurrent) ? "Chưa cập nhật" : inrushCurrent;
      Alarm = string.IsNullOrEmpty(alarm) ? "Chưa cập nhật" : alarm;
      Note = string.IsNullOrEmpty(note) ? "Chưa cập nhật" : note;
      PdfManual = string.IsNullOrEmpty(pdfManual) ? "Chưa cập nhật" : pdfManual;
    }

    [Preserve]
    public AdapterSpecificationEntity(string code, string type, string? communication, string? numOfModulesAllowed, string? commSpeed, string? inputSupply, string? outputSupply, string? inrushCurrent, string? alarm, string? note, string? pdfManual)
    {
      Code = string.IsNullOrEmpty(code) ? throw new ArgumentNullException(nameof(code)) : code;
      Type = string.IsNullOrEmpty(type) ? "Chưa cập nhật" : type;
      Communication = string.IsNullOrEmpty(communication) ? "Chưa cập nhật" : communication;
      NumOfModulesAllowed = string.IsNullOrEmpty(numOfModulesAllowed) ? "Chưa cập nhật" : numOfModulesAllowed;
      CommSpeed = string.IsNullOrEmpty(commSpeed) ? "Chưa cập nhật" : commSpeed;
      InputSupply = string.IsNullOrEmpty(inputSupply) ? "Chưa cập nhật" : inputSupply;
      OutputSupply = string.IsNullOrEmpty(outputSupply) ? "Chưa cập nhật" : outputSupply;
      InrushCurrent = string.IsNullOrEmpty(inrushCurrent) ? "Chưa cập nhật" : inrushCurrent;
      Alarm = string.IsNullOrEmpty(alarm) ? "Chưa cập nhật" : alarm;
      Note = string.IsNullOrEmpty(note) ? "Chưa cập nhật" : note;
      PdfManual = string.IsNullOrEmpty(pdfManual) ? "Chưa cập nhật" : pdfManual;
    }
  }
}

