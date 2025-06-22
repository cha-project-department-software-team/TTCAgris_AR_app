
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine.Scripting;

#nullable enable
namespace Domain.Entities
{
  [Preserve]
  public class FieldDeviceEntity
  {
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    // [JsonProperty("mcc", NullValueHandling = NullValueHandling.Ignore)]
    [JsonProperty("mcc")]
    public MccEntity? MccEntity { get; set; }

    // [JsonProperty("ratedPower", NullValueHandling = NullValueHandling.Ignore)]
    [JsonProperty("ratedPower")]
    public string? RatedPower { get; set; }

    // [JsonProperty("ratedCurrent", NullValueHandling = NullValueHandling.Ignore)]
    [JsonProperty("ratedCurrent")]
    public string? RatedCurrent { get; set; }

    // [JsonProperty("activeCurrent", NullValueHandling = NullValueHandling.Ignore)]
    [JsonProperty("activeCurrent")]
    public string? ActiveCurrent { get; set; }

    // [JsonProperty("note", NullValueHandling = NullValueHandling.Ignore)]
    [JsonProperty("note")]
    public string? Note { get; set; } = string.Empty;

    // [JsonProperty("listConnectionImages", NullValueHandling = NullValueHandling.Ignore)]
    [JsonProperty("listConnectionImages")]
    public List<ImageEntity>? ConnectionImageEntities { get; set; }

    // public bool ShouldSerializeId()
    // {
    //   List<string> apiRequestType = GlobalVariable.APIRequestType;
    //   HashSet<string> allowedRequests = new HashSet<string>
    //   {
    //     HttpMethodTypeEnum.PUTFieldDevice.GetDescription(),        
    //   };
    //   return !allowedRequests.Contains(apiRequestType);
    // }

    public bool ShouldSerializeId()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.POSTFieldDevice.GetDescription(),
        HttpMethodTypeEnum.PUTFieldDevice.GetDescription(),
      };
      return !apiRequestType.Any(request => allowedRequests.Contains(request));
    }
    public bool ShouldSerializeName()
    {
      return true;
    }
    public bool ShouldSerializeMccEntity()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETFieldDevice.GetDescription(),
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));
    }
    public bool ShouldSerializeRatedPower()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETFieldDevice.GetDescription(),
        HttpMethodTypeEnum.POSTFieldDevice.GetDescription(),
        HttpMethodTypeEnum.PUTFieldDevice.GetDescription(),


      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));
    }

    public bool ShouldSerializeRatedCurrent()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETFieldDevice.GetDescription(),
        HttpMethodTypeEnum.POSTFieldDevice.GetDescription(),
        HttpMethodTypeEnum.PUTFieldDevice.GetDescription(),
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));
    }

    public bool ShouldSerializeActiveCurrent()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETFieldDevice.GetDescription(),
        HttpMethodTypeEnum.POSTFieldDevice.GetDescription(),
        HttpMethodTypeEnum.PUTFieldDevice.GetDescription(),
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));
    }

    public bool ShouldSerializeConnectionImageEntities()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETFieldDevice.GetDescription(),
        HttpMethodTypeEnum.POSTFieldDevice.GetDescription(),
        HttpMethodTypeEnum.PUTFieldDevice.GetDescription(),
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));
    }

    public bool ShouldSerializeNote()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETFieldDevice.GetDescription(),
         HttpMethodTypeEnum.POSTFieldDevice.GetDescription(),
        HttpMethodTypeEnum.PUTFieldDevice.GetDescription(),
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));
    }


    [Preserve]
    public FieldDeviceEntity()
    {
      // ConnectionImageEntities = new List<ImageEntity>();
    }


    [Preserve]  //! Dùng khi FieldDeviceEntity được dùng làm Field để Post cho Mcc
    public FieldDeviceEntity(int id, string name)
    {
      Id = id;
      Name = string.IsNullOrEmpty(name) ? throw new ArgumentNullException(nameof(name)) : name;
    }

    [Preserve]
    public FieldDeviceEntity(string name, MccEntity? mccEntity, List<ImageEntity>? connectionImageEntities)
    {
      Name = string.IsNullOrEmpty(name) ? throw new ArgumentNullException(nameof(name)) : name;
      MccEntity = mccEntity ?? throw new ArgumentNullException(nameof(mccEntity));
      ConnectionImageEntities = (connectionImageEntities == null || (connectionImageEntities != null && connectionImageEntities.Count <= 0)) ? new List<ImageEntity>() : connectionImageEntities;
    }

    [Preserve]
    //! Dùng khi Get FieldDeviceEntity từ Grapper
    public FieldDeviceEntity(int id, string name, MccEntity? mccEntity, List<ImageEntity>? connectionImageEntities, string? ratedPower, string? ratedCurrent, string? activeCurrent, string? note)
    {
      Id = id;
      Name = string.IsNullOrEmpty(name) ? throw new ArgumentNullException(nameof(name)) : name;
      MccEntity = mccEntity ?? null;
      RatedPower = string.IsNullOrEmpty(ratedPower) ? "Chưa cập nhật" : ratedPower;
      RatedCurrent = string.IsNullOrEmpty(ratedCurrent) ? "Chưa cập nhật" : ratedCurrent;
      ActiveCurrent = string.IsNullOrEmpty(activeCurrent) ? "Chưa cập nhật" : activeCurrent;
      ConnectionImageEntities = (connectionImageEntities == null || (connectionImageEntities != null && connectionImageEntities.Count <= 0)) ? new List<ImageEntity>() : connectionImageEntities;
      Note = string.IsNullOrEmpty(note) ? "Chưa cập nhật" : note;
    }


    [Preserve]
    public FieldDeviceEntity(string name, List<ImageEntity>? connectionImageEntities, string? ratedPower, string? ratedCurrent, string? activeCurrent, string? note)
    {
      Name = string.IsNullOrEmpty(name) ? throw new ArgumentNullException(nameof(name)) : name;
      RatedPower = string.IsNullOrEmpty(ratedPower) ? "Chưa cập nhật" : ratedPower;
      RatedCurrent = string.IsNullOrEmpty(ratedCurrent) ? "Chưa cập nhật" : ratedCurrent;
      ActiveCurrent = string.IsNullOrEmpty(activeCurrent) ? "Chưa cập nhật" : activeCurrent;
      ConnectionImageEntities = (connectionImageEntities == null || (connectionImageEntities != null && connectionImageEntities.Count <= 0)) ? new List<ImageEntity>() : connectionImageEntities;
      Note = string.IsNullOrEmpty(note) ? "Chưa cập nhật" : note;
    }
  }
}