using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine.Scripting;
#nullable enable
[Preserve]
public class CompanyInformationModel
{
  [JsonProperty("id")]
  public int Id { get; set; }

  [JsonProperty("name")]
  public string Name { get; set; } = string.Empty;

  [JsonProperty("listGrappers")]
  public List<GrapperInformationModel>? ListGrapperInformationModel { get; set; }
  [JsonProperty("listModuleSpecifications")]
  public List<ModuleSpecificationModel>? ListModuleSpecificationModel { get; set; }
  [JsonProperty("listAdapterSpecifications")]
  public List<AdapterSpecificationModel>? ListAdapterSpecificationModel { get; set; }
  [Preserve]

  public CompanyInformationModel(int id, string name, List<GrapperInformationModel> listGrapperInformationModel, List<ModuleSpecificationModel> listModuleSpecificationModel, List<AdapterSpecificationModel> listAdapterSpecificationModel)
  {
    Id = id;
    Name = string.IsNullOrEmpty(name) ? throw new ArgumentNullException(nameof(name)) : name;
    ListGrapperInformationModel = listGrapperInformationModel.Any() ? listGrapperInformationModel : new List<GrapperInformationModel>();
    ListModuleSpecificationModel = listModuleSpecificationModel.Any() ? listModuleSpecificationModel : new List<ModuleSpecificationModel>();
    ListAdapterSpecificationModel = listAdapterSpecificationModel.Any() ? listAdapterSpecificationModel : new List<AdapterSpecificationModel>();
  }
  public CompanyInformationModel(int id, string name)
  {
    Id = id;
    Name = string.IsNullOrEmpty(name) ? throw new ArgumentNullException(nameof(name)) : name;
  }

}
