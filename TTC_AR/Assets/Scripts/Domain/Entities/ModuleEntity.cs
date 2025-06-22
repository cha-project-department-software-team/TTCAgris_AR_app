using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine.Scripting;

#nullable enable
//Các kiểu tham chiếu không được chú thích với ? (nullable) được coi là non-nullable (không thể là null).
namespace Domain.Entities
{
  public class ModuleEntity
  {
    [JsonProperty("id")]
    public int Id { get; set; } // non-nullable

    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty; // non-nullable

    // [JsonProperty("rack", NullValueHandling = NullValueHandling.Ignore)]
    [JsonProperty("rack")]
    public RackEntity? RackEntity { get; set; }

    // [JsonProperty("grapper", NullValueHandling = NullValueHandling.Ignore)]
    [JsonProperty("grapper")]
    public GrapperEntity? GrapperEntity { get; set; }

    // [JsonProperty("jBs", NullValueHandling = NullValueHandling.Ignore)]
    [JsonProperty("listJBs")]
    public List<JBEntity>? JBEntities { get; set; }

    // [JsonProperty("Devices", NullValueHandling = NullValueHandling.Ignore)]
    [JsonProperty("listDevices")]
    public List<DeviceEntity>? DeviceEntities { get; set; }

    // [JsonProperty("moduleSpecification", NullValueHandling = NullValueHandling.Ignore)]
    [JsonProperty("moduleSpecification")]
    public ModuleSpecificationEntity? ModuleSpecificationEntity { get; set; }
    // [JsonProperty("adapterSpecification", NullValueHandling = NullValueHandling.Ignore)]
    [JsonProperty("adapterSpecification")]
    public AdapterSpecificationEntity? AdapterSpecificationEntity { get; set; }



    public bool ShouldSerializeId()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.POSTModule.GetDescription(),
        HttpMethodTypeEnum.PUTModule.GetDescription(),
      };

      return !apiRequestType.Any(request => allowedRequests.Contains(request));
    }
    public bool ShouldSerializeName()
    {
      return true;
    }
    public bool ShouldSerializeRackEntity()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETModule.GetDescription(),
        HttpMethodTypeEnum.POSTModule.GetDescription(),
        HttpMethodTypeEnum.PUTModule.GetDescription(),
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));
    }
    public bool ShouldSerializeGrapperEntity()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETModule.GetDescription(),
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));
    }
    public bool ShouldSerializeJBEntities()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETModule.GetDescription(),
        HttpMethodTypeEnum.POSTModule.GetDescription(),
        HttpMethodTypeEnum.PUTModule.GetDescription(),
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));
    }

    public bool ShouldSerializeDeviceEntities()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETModule.GetDescription(),
        HttpMethodTypeEnum.POSTModule.GetDescription(),
        HttpMethodTypeEnum.PUTModule.GetDescription(),
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));
    }


    public bool ShouldSerializeModuleSpecificationEntity()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETModule.GetDescription(),
        HttpMethodTypeEnum.POSTModule.GetDescription(),
        HttpMethodTypeEnum.PUTModule.GetDescription(),
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));
    }

    public bool ShouldSerializeAdapterSpecificationEntity()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETModule.GetDescription(),
        HttpMethodTypeEnum.POSTModule.GetDescription(),
        HttpMethodTypeEnum.PUTModule.GetDescription(),
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));
    }


    [Preserve]
    public ModuleEntity()
    {
      // DeviceEntities = new List<DeviceEntity>?();
      // JBEntities = new List<JBEntity>?();
    }


    [Preserve]
    public ModuleEntity(int id, string name)
    {
      Id = id;
      Name = string.IsNullOrEmpty(name) ? throw new ArgumentNullException(nameof(name)) : name;
    }

    [Preserve]
    public ModuleEntity(string name, GrapperEntity grapperEntity, RackEntity? rack)
    {
      Name = string.IsNullOrEmpty(name) ? throw new ArgumentNullException(nameof(name)) : name;
      GrapperEntity = grapperEntity == null ? throw new ArgumentNullException(nameof(grapperEntity)) : grapperEntity;
      RackEntity = rack;
    }
    [Preserve]
    public ModuleEntity(string name)
    {
      Name = string.IsNullOrEmpty(name) ? throw new ArgumentNullException(nameof(name)) : name;
    }

    [Preserve]
    public ModuleEntity(int id, GrapperEntity grapperEntity, string name, RackEntity? rack, List<DeviceEntity>? deviceEntities, List<JBEntity>? jbEntities, ModuleSpecificationEntity? moduleSpecificationEntity, AdapterSpecificationEntity? adapterSpecificationEntity)
    {
      Id = id;
      Name = string.IsNullOrEmpty(name) ? throw new ArgumentNullException(nameof(name)) : name;
      GrapperEntity = grapperEntity == null ? throw new ArgumentNullException(nameof(grapperEntity)) : grapperEntity;

      RackEntity = rack ?? null;

      DeviceEntities = (deviceEntities == null || (deviceEntities != null && !deviceEntities.Any()))

      ? new List<DeviceEntity>() : deviceEntities;

      JBEntities = (jbEntities == null || (jbEntities != null && !jbEntities.Any()))

      ? new List<JBEntity>() : jbEntities;

      ModuleSpecificationEntity = moduleSpecificationEntity ?? null;

      AdapterSpecificationEntity = adapterSpecificationEntity ?? null;
    }

    [Preserve]
    public ModuleEntity(string name, RackEntity? rack, List<DeviceEntity>? deviceEntities, List<JBEntity>? jbEntities, ModuleSpecificationEntity? moduleSpecificationEntity, AdapterSpecificationEntity? adapterSpecificationEntity)
    {
      Name = string.IsNullOrEmpty(name) ? throw new ArgumentNullException(nameof(name)) : name;

      RackEntity = rack ?? null;

      DeviceEntities = (deviceEntities == null || (deviceEntities != null && !deviceEntities.Any()))

      ? new List<DeviceEntity>() : deviceEntities;

      JBEntities = (jbEntities == null || (jbEntities != null && !jbEntities.Any()))

      ? new List<JBEntity>() : jbEntities;

      ModuleSpecificationEntity = moduleSpecificationEntity ?? null;

      AdapterSpecificationEntity = adapterSpecificationEntity ?? null;
    }
  }
}