
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine.Scripting;
#nullable enable
namespace Domain.Entities
{
  [Preserve]
  public class MccEntity
  {
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("cabinetCode")]
    public string CabinetCode { get; set; } = string.Empty;

    // [JsonProperty("brand", NullValueHandling = NullValueHandling.Ignore)]
    [JsonProperty("brand")]
    public string? Brand { get; set; }

    // [JsonProperty("fieldDevices", NullValueHandling = NullValueHandling.Ignore)]
    [JsonProperty("listFieldDevices")]
    public List<FieldDeviceEntity>? FieldDeviceEntities { get; set; }

    // [JsonProperty("note", NullValueHandling = NullValueHandling.Ignore)]
    [JsonProperty("note")]
    public string? Note { get; set; }


    public bool ShouldSerializeId()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.POSTMcc.GetDescription(),
        HttpMethodTypeEnum.PUTMcc.GetDescription(),
      };
      return !apiRequestType.Any(request => allowedRequests.Contains(request));
    }
    public bool ShouldSerializeCabinetCode()
    {
      return true;
    }

    public bool ShouldSerializeBrand()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETMcc.GetDescription(),
        HttpMethodTypeEnum.GETListMcc.GetDescription(),
        HttpMethodTypeEnum.PUTMcc.GetDescription(),
        HttpMethodTypeEnum.POSTMcc.GetDescription(),
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));
    }

    public bool ShouldSerializeFieldDeviceEntities()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETMcc.GetDescription(),
        HttpMethodTypeEnum.PUTMcc.GetDescription(),
        HttpMethodTypeEnum.POSTMcc.GetDescription(),
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));
    }

    public bool ShouldSerializeNote()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETMcc.GetDescription(),
        HttpMethodTypeEnum.PUTMcc.GetDescription(),
        HttpMethodTypeEnum.POSTMcc.GetDescription(),
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));
    }


    [Preserve]
    public MccEntity()
    {
    }

    [Preserve]
    public MccEntity(string cabinetCode, List<FieldDeviceEntity>? fieldDeviceEntities)
    {
      CabinetCode = string.IsNullOrEmpty(cabinetCode) ? throw new ArgumentNullException(nameof(cabinetCode)) : cabinetCode;

      FieldDeviceEntities = (fieldDeviceEntities == null
      || (fieldDeviceEntities != null && fieldDeviceEntities.Count <= 0))
      ? new List<FieldDeviceEntity>() : fieldDeviceEntities; ;
    }

    [Preserve]
    public MccEntity(string cabinetCode)
    {
      CabinetCode = string.IsNullOrEmpty(cabinetCode) ? throw new ArgumentNullException(nameof(cabinetCode)) : cabinetCode;
    }

    [Preserve]
    public MccEntity(int id, string cabinetCode, List<FieldDeviceEntity>? fieldDeviceEntities, string? brand, string? note)
    {
      Id = id;
      CabinetCode = string.IsNullOrEmpty(cabinetCode) ? throw new ArgumentNullException(nameof(cabinetCode)) : cabinetCode;
      Brand = string.IsNullOrEmpty(brand) ? "Chưa cập nhật" : brand;
      FieldDeviceEntities = (fieldDeviceEntities == null
        || (fieldDeviceEntities != null && fieldDeviceEntities.Count <= 0))
        ? new List<FieldDeviceEntity>() : fieldDeviceEntities; ;
      Note = string.IsNullOrEmpty(note) ? "Chưa cập nhật" : note;
    }
    [Preserve]
    public MccEntity(string cabinetCode, string? brand, List<FieldDeviceEntity>? fieldDeviceEntities, string? note)
    {
      CabinetCode = string.IsNullOrEmpty(cabinetCode) ? throw new ArgumentNullException(nameof(cabinetCode)) : cabinetCode;
      Brand = string.IsNullOrEmpty(brand) ? "Chưa cập nhật" : brand;
      FieldDeviceEntities = (fieldDeviceEntities == null
        || (fieldDeviceEntities != null && fieldDeviceEntities.Count <= 0))
        ? new List<FieldDeviceEntity>() : fieldDeviceEntities; ;
      Note = string.IsNullOrEmpty(note) ? "Chưa cập nhật" : note;
    }
  }
}