using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine.Scripting;
#nullable enable
namespace Domain.Entities
{
  [Preserve]
  public class RackEntity
  {
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("listModules")]
    public List<ModuleEntity>? ModuleEntities { get; set; }


    public bool ShouldSerializeId()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.POSTRack.GetDescription(),
        HttpMethodTypeEnum.PUTRack.GetDescription(),
      };
      return !apiRequestType.Any(request => allowedRequests.Contains(request));
    }

    public bool ShouldSerializeName()
    {
      return true;
    }

    public bool ShouldSerializeModuleEntities()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.GETRack.GetDescription(),
        HttpMethodTypeEnum.PUTRack.GetDescription(),
        HttpMethodTypeEnum.POSTRack.GetDescription()
      };
      return apiRequestType.Any(request => allowedRequests.Contains(request));
    }

    [Preserve]
    public RackEntity()
    {
      // ModuleEntities = new List<ModuleEntity>();
    }


    [Preserve]
    public RackEntity(int id, string name, List<ModuleEntity>? moduleEntities)
    {
      Id = id;
      Name = string.IsNullOrEmpty(name) ? throw new ArgumentNullException(nameof(name)) : name;
      ModuleEntities = !moduleEntities.Any() ? new List<ModuleEntity>() : moduleEntities;
    }

    [Preserve]
    public RackEntity(int id, string name)
    {
      Id = id;
      Name = string.IsNullOrEmpty(name) ? throw new ArgumentNullException(nameof(name)) : name;
    }

    [Preserve]
    public RackEntity(string name)
    {
      Name = string.IsNullOrEmpty(name) ? throw new ArgumentNullException(nameof(name)) : name;
    }


    [Preserve]
    public RackEntity(string name, List<ModuleEntity>? moduleEntities)
    {

      Name = string.IsNullOrEmpty(name) ? throw new ArgumentNullException(nameof(name)) : name;

      ModuleEntities = !moduleEntities.Any() ? new List<ModuleEntity>() : moduleEntities;
    }


  }

}