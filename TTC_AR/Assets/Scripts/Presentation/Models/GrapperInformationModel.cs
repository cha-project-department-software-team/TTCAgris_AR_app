using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.Scripting;
#nullable  enable
[Preserve]
public class GrapperInformationModel
{
  [JsonProperty("id")] public int Id { get; set; }

  [JsonProperty("name")] public string Name { get; set; } = string.Empty;

  [JsonProperty("listRacks")] public List<RackBasicModel>? List_RackBasicModel { get; set; }

  [JsonProperty("listDevices")] public List<DeviceInformationModel>? ListDeviceInformationModel { get; set; }

  [JsonProperty("listJBs")] public List<JBInformationModel>? ListJBInformationModel { get; set; }

  [JsonProperty("listMCCs")] public List<MccInformationModel>? ListMccInformationModel { get; set; }

  [JsonProperty("listModuleSpecifications")] public List<ModuleSpecificationModel>? ListModuleSpecificationModel { get; set; }

  [JsonProperty("listAdapterSpecifications")] public List<AdapterSpecificationModel>? ListAdapterSpecificationModel { get; set; }


  [Preserve]

  public GrapperInformationModel(int id, string name, List<RackBasicModel> list_RackBasicModel, List<DeviceInformationModel> listDeviceInformationModel, List<JBInformationModel> listJBInformationModel, List<MccInformationModel> listMccInformationModel, List<ModuleSpecificationModel> listModuleSpecificationModel, List<AdapterSpecificationModel> listAdapterSpecificationModel)
  {
    Id = id;
    Name = name;
    List_RackBasicModel = list_RackBasicModel;
    ListDeviceInformationModel = listDeviceInformationModel;
    ListJBInformationModel = listJBInformationModel;
    ListMccInformationModel = listMccInformationModel;
    ListModuleSpecificationModel = listModuleSpecificationModel;
    ListAdapterSpecificationModel = listAdapterSpecificationModel;
  }
  [Preserve]
  public GrapperInformationModel(string name, List<RackBasicModel>? list_RackBasicModel, List<DeviceInformationModel>? listDeviceInformationModel, List<JBInformationModel>? listJBInformationModel, List<MccInformationModel>? listMccInformationModel, List<ModuleSpecificationModel>? listModuleSpecificationModel, List<AdapterSpecificationModel>? listAdapterSpecificationModel)
  {
    Name = name;
    List_RackBasicModel = list_RackBasicModel;
    ListDeviceInformationModel = listDeviceInformationModel;
    ListJBInformationModel = listJBInformationModel;
    ListMccInformationModel = listMccInformationModel;
    ListModuleSpecificationModel = listModuleSpecificationModel;
    ListAdapterSpecificationModel = listAdapterSpecificationModel;
  }
  [Preserve]
  public GrapperInformationModel(int id, string name)
  {
    Id = id;
    Name = name;
  }
  [Preserve]
  public GrapperInformationModel(string name)
  {

    Name = name;
  }

  [Preserve]
  public GrapperInformationModel()
  {

  }
}

[Preserve]
public class GrapperBasicModel
{
  [JsonProperty("id")] public int Id { get; set; }
  [JsonProperty("name")] public string Name { get; set; }

  [Preserve]

  public GrapperBasicModel(int id, string name)
  {
    Id = id;
    Name = name;
  }
}

