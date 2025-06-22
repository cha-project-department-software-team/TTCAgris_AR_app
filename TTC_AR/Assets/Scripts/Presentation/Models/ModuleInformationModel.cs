using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.Scripting;
#nullable enable

[Preserve]
public class ModuleInformationModel
{

  [JsonProperty("id")] public int Id { get; set; }

  [JsonProperty("name")] public string Name { get; set; } = string.Empty;
  [JsonProperty("rack")] public RackInformationModel? Rack { get; set; }

  [JsonProperty("grapper")] public GrapperInformationModel? Grapper { get; set; }
  [JsonProperty("listJBs")] public List<JBInformationModel>? ListJBInformationModel { get; set; }
  [JsonProperty("listDevices")] public List<DeviceInformationModel>? ListDeviceInformationModel { get; set; }
  [JsonProperty("moduleSpecification")] public ModuleSpecificationModel? ModuleSpecificationModel { get; set; }
  [JsonProperty("adapterSpecification")] public AdapterSpecificationModel? AdapterSpecificationModel { get; set; }

  [Preserve]
  public ModuleInformationModel(int id, string name, GrapperInformationModel? grapper, RackInformationModel? rack, List<DeviceInformationModel>? listDeviceInformationModel, List<JBInformationModel>? listJBInformationModel, ModuleSpecificationModel? moduleSpecificationModel, AdapterSpecificationModel? adapterSpecificationModel)
  {
    Id = id;
    Name = name;
    Grapper = grapper;
    Rack = rack;
    ListDeviceInformationModel = listDeviceInformationModel;
    ListJBInformationModel = listJBInformationModel;
    ModuleSpecificationModel = moduleSpecificationModel;
    AdapterSpecificationModel = adapterSpecificationModel;
  }

  [Preserve]
  public ModuleInformationModel(int id, string name)
  {
    Id = id;
    Name = name;
  }

  [Preserve]
  public ModuleInformationModel(string name, RackInformationModel? rack, List<DeviceInformationModel>? listDeviceInformationModel, List<JBInformationModel>? listJBInformationModel, ModuleSpecificationModel? moduleSpecificationModel, AdapterSpecificationModel? adapterSpecificationModel)
  {
    Name = name;
    Rack = rack;
    ListDeviceInformationModel = listDeviceInformationModel;
    ListJBInformationModel = listJBInformationModel;
    ModuleSpecificationModel = moduleSpecificationModel;
    AdapterSpecificationModel = adapterSpecificationModel;
  }
}

[Preserve]
public class ModuleBasicModel//! Module Module có Id, Name và Rack tương ứng (Rack chỉ chứa Id và Name)
{
  [JsonProperty("id")]
  public int Id { get; set; }
  [JsonProperty("name")]
  public string Name { get; set; }
  public ModuleBasicModel(int id, string name)
  {
    Id = id;
    Name = name;
  }
}

[Preserve]
public class ModuleGeneralModel //! Module Module có Id, Name và Rack tương ứng (Rack chỉ chứa Id và Name)
{
  [JsonProperty("id")]
  public int Id { get; set; }
  [JsonProperty("name")]
  public string Name { get; set; }
  [JsonProperty("rack")]
  public RackBasicModel RackBasicModel { get; set; }
  [JsonProperty("listDevices")] public List<DeviceBasicModel> ListDeviceBasicModel { get; set; }

  [JsonProperty("listJBs")] public List<JBBasicModel> ListJBBasicModel { get; set; }

  [JsonProperty("moduleSpecification")] public ModuleSpecificationBasicModel? ModuleSpecificationBasicModel { get; set; }
  [JsonProperty("adapterSpecification")] public AdapterSpecificationBasicModel? AdapterSpecificationBasicModel { get; set; }

  [Preserve]

  public ModuleGeneralModel(int id, string name, RackBasicModel rackBasicModel, List<DeviceBasicModel> listDeviceBasicModel, List<JBBasicModel> listJBBasicModel, ModuleSpecificationBasicModel? moduleSpecificationBasicModel, AdapterSpecificationBasicModel? adapterSpecificationBasicModel)
  {
    Id = id;
    Name = name;
    RackBasicModel = rackBasicModel;
    ListDeviceBasicModel = listDeviceBasicModel;
    ListJBBasicModel = listJBBasicModel;
    ModuleSpecificationBasicModel = moduleSpecificationBasicModel;
    AdapterSpecificationBasicModel = adapterSpecificationBasicModel;
  }

}



