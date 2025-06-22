using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.Scripting;
#nullable enable
[Preserve]
public class RackInformationModel
{
  [JsonProperty("id")]
  public int Id { get; set; }

  [JsonProperty("name")]
  public string Name { get; set; } = string.Empty;

  [JsonProperty("listModules")]
  public List<ModuleInformationModel>? ListModuleInformationModel { get; set; }

  [Preserve]
  public RackInformationModel(int id, string name, List<ModuleInformationModel>? listModuleInformationModel)
  {
    Id = id;
    Name = name;
    ListModuleInformationModel = listModuleInformationModel;
  }
  [Preserve]
  public RackInformationModel(int id, string name)
  {
    Id = id;
    Name = name;
  }

  [Preserve]
  public RackInformationModel()
  {

  }
  [Preserve]
  public RackInformationModel(string name, List<ModuleInformationModel>? listModuleInformationModel)
  {
    Name = name;
    ListModuleInformationModel = listModuleInformationModel;
  }
}

[Preserve]
public class RackBasicModel
{
  [JsonProperty("id")] public int Id { get; set; }
  [JsonProperty("name")] public string Name { get; set; }

  [Preserve]

  public RackBasicModel(int id, string name)
  {
    Id = id;
    Name = name;
  }
}
