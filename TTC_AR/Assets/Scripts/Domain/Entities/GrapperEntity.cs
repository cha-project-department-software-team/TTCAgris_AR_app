using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine.Scripting;

#nullable enable
namespace Domain.Entities
{
  [Preserve]
  public class GrapperEntity
  {
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    // [JsonProperty("rack", NullValueHandling = NullValueHandling.Ignore)]
    [JsonProperty("listRacks")]
    public List<RackEntity>? RackEntities { get; set; }
    // [JsonProperty("rack", NullValueHandling = NullValueHandling.Ignore)]
    [JsonProperty("listModules")]
    public List<ModuleEntity>? ModuleEntities { get; set; }
    // [JsonProperty("Devices", NullValueHandling = NullValueHandling.Ignore)]
    [JsonProperty("listDevices")]
    public List<DeviceEntity>? DeviceEntities { get; set; }
    // [JsonProperty("jBs", NullValueHandling = NullValueHandling.Ignore)]
    [JsonProperty("listJBs")]
    public List<JBEntity>? JBEntities { get; set; }
    // [JsonProperty("mccs", NullValueHandling = NullValueHandling.Ignore)]
    [JsonProperty("listMccs")]
    public List<MccEntity>? MccEntities { get; set; }
    [JsonProperty("listFieldDevices")]
    public List<FieldDeviceEntity>? FieldDeviceEntities { get; set; }


    public bool ShouldSerializeId()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.POSTGrapper.GetDescription(),
        HttpMethodTypeEnum.PUTGrapper.GetDescription(),
      };
      return !apiRequestType.Any(request => allowedRequests.Contains(request));
    }
    public bool ShouldSerializeName()
    {
      return true;
    }


    public bool ShouldSerializeListRacks()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETGrapper.GetDescription(),
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));
    }
    public bool ShouldSerializeListModules()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETGrapper.GetDescription(),
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));
    }
    public bool ShouldSerializeListDevices()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETGrapper.GetDescription(),
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));
    }
    public bool ShouldSerializeListJBs()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETGrapper.GetDescription(),
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));
    }
    public bool ShouldSerializeListMccs()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETGrapper.GetDescription(),
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));
    }
    public bool ShouldSerializeListFieldDevices()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETGrapper.GetDescription(),
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));
    }



    [Preserve]
    public GrapperEntity()
    {
      // RackEntities = new List<RackEntity>();
      // DeviceEntities = new List<DeviceEntity>();
      // JBEntities = new List<JBEntity>();
      // MccEntities = new List<MccEntity>();
    }




    [Preserve]
    public GrapperEntity(int id, string name)
    {
      Id = id;
      Name = string.IsNullOrEmpty(name) ? throw new ArgumentNullException(nameof(name)) : name;
    }



    [Preserve]
    public GrapperEntity(string name)
    {
      Name = string.IsNullOrEmpty(name) ? throw new ArgumentNullException(nameof(name)) : name;
    }

    [Preserve]
    public GrapperEntity(int id, string name, List<RackEntity>? rackEntities, List<ModuleEntity>? moduleEntities, List<DeviceEntity>? deviceEntities, List<JBEntity>? jbEntities, List<MccEntity>? mccEntities, List<FieldDeviceEntity>? fieldDeviceEntities)
    {
      Id = id;
      Name = string.IsNullOrEmpty(name) ? throw new ArgumentNullException(nameof(name)) : name;
      RackEntities = (rackEntities == null || (rackEntities != null && rackEntities.Count <= 0))
      ? new List<RackEntity>() : rackEntities; ;
      ModuleEntities = (moduleEntities == null || (moduleEntities != null && moduleEntities.Count <= 0))
      ? new List<ModuleEntity>() : moduleEntities;
      DeviceEntities = (deviceEntities == null || (deviceEntities != null && deviceEntities.Count <= 0))
      ? new List<DeviceEntity>() : deviceEntities;
      JBEntities = (jbEntities == null || (jbEntities != null && jbEntities.Count <= 0))
      ? new List<JBEntity>() : jbEntities;
      MccEntities = (mccEntities == null || (mccEntities != null && mccEntities.Count <= 0))
      ? new List<MccEntity>() : mccEntities;
      FieldDeviceEntities = (fieldDeviceEntities == null || (fieldDeviceEntities != null && fieldDeviceEntities.Count <= 0))
      ? new List<FieldDeviceEntity>() : fieldDeviceEntities;
    }
    public GrapperEntity(string name, List<RackEntity>? rackEntities, List<DeviceEntity>? deviceEntities, List<JBEntity>? jbEntities, List<MccEntity>? mccEntities, List<FieldDeviceEntity>? fieldDeviceEntities)
    {
      Name = string.IsNullOrEmpty(name) ? throw new ArgumentNullException(nameof(name)) : name;
      RackEntities = (rackEntities == null || (rackEntities != null && rackEntities.Count <= 0))
      ? new List<RackEntity>() : rackEntities; ;
      DeviceEntities = (deviceEntities == null || (deviceEntities != null && deviceEntities.Count <= 0))
      ? new List<DeviceEntity>() : deviceEntities;
      JBEntities = (jbEntities == null || (jbEntities != null && jbEntities.Count <= 0))
      ? new List<JBEntity>() : jbEntities;
      MccEntities = (mccEntities == null || (mccEntities != null && mccEntities.Count <= 0))
      ? new List<MccEntity>() : mccEntities;
      FieldDeviceEntities = (fieldDeviceEntities == null || (fieldDeviceEntities != null && fieldDeviceEntities.Count <= 0))
      ? new List<FieldDeviceEntity>() : fieldDeviceEntities;
    }
    // public GrapperEntity(int id, string name, List<RackEntity> rackEntities, List<DeviceEntity> deviceEntities, List<JBEntity> jbEntities, List<MccEntity> mccEntities, List<ModuleSpecificationEntity> moduleSpecificationEntities, List<AdapterSpecificationEntity> adapterSpecificationEntities)
    // {
    //   if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("name is required.", nameof(name));
    //   Id = id;
    //   Name = name == "" ? throw new ArgumentNullException(nameof(name)) : name;
    //   RackEntities = rackEntities;
    //   DeviceEntities = deviceEntities;
    //   JBEntities = jbEntities;
    //   MccEntities = mccEntities;
    //   ModuleSpecificationEntities = moduleSpecificationEntities;
    //   AdapterSpecificationEntities = adapterSpecificationEntities;

    // }
  }

}