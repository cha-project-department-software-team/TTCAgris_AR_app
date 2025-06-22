using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.Scripting;
using static ModuleInformationModel;
#nullable enable
[Preserve]
public class DeviceInformationModel
{
  [JsonProperty("id")]
  public int Id { get; set; }

  [JsonProperty("code")]
  public string Code { get; set; } = string.Empty;

  [JsonProperty("function")]
  public string? Function { get; set; }

  [JsonProperty("range")]
  public string? Range { get; set; }

  [JsonProperty("unit")]
  public string? Unit { get; set; }

  [JsonProperty("ioAddress")]
  public string? IOAddress { get; set; }

  [JsonProperty("module")]
  public ModuleInformationModel? ModuleInformationModel { get; set; }

  [JsonProperty("jBs")]
  public List<JBInformationModel>? JBInformationModels { get; set; }

  [JsonProperty("additionalConnectionImages")]
  public List<ImageInformationModel>? AdditionalConnectionImages { get; set; }

  [Preserve]

  public DeviceInformationModel(int id, string code, string? function, string? range, string? unit, string? ioAddress, ModuleInformationModel? moduleInformationModel, List<JBInformationModel>? jbInformationModels, List<ImageInformationModel>? additionalConnectionImages)
  {
    Id = id;
    Code = code;
    Function = function;
    Range = range;
    Unit = unit;
    IOAddress = ioAddress;
    ModuleInformationModel = moduleInformationModel;
    JBInformationModels = jbInformationModels;
    JBInformationModels = jbInformationModels;
    AdditionalConnectionImages = additionalConnectionImages;
  }


  public DeviceInformationModel(string code, string? function, string? range, string? unit, string? ioAddress, ModuleInformationModel? moduleInformationModel, List<JBInformationModel>? jbInformationModels, List<ImageInformationModel>? additionalConnectionImages)
  {
    Code = code;
    Function = function;
    Range = range;
    Unit = unit;
    IOAddress = ioAddress;
    ModuleInformationModel = moduleInformationModel;
    JBInformationModels = jbInformationModels;
    JBInformationModels = jbInformationModels;
    AdditionalConnectionImages = additionalConnectionImages;
  }
  public DeviceInformationModel(int id, string code)
  {
    Id = id;
    Code = code;
  }

  public DeviceInformationModel()
  {
  }
}


[Preserve]
public class DeviceGeneralModel
{
  [JsonProperty("id")]
  public string? Id { get; set; }

  [JsonProperty("code")]
  public string? Code { get; set; }

  [JsonProperty("function")]
  public string? Function { get; set; }

  [JsonProperty("range")]
  public string? Range { get; set; }

  [JsonProperty("unit")]
  public string? Unit { get; set; }

  [JsonProperty("ioAddress")]
  public string? IOAddress { get; set; }

  [JsonProperty("module")]
  public ModuleBasicModel? ModuleBasicModel { get; set; }

  [JsonProperty("JB")]
  public JBBasicModel JBBasicModel { get; set; }

  [JsonProperty("additionalConnectionImages")]
  public List<ImageBasicModel> AdditionalImageModels { get; set; }

  [Preserve]

  public DeviceGeneralModel(string? id, string? code, string? function, string? range, string? unit, string? ioAddress, ModuleBasicModel? moduleBasicModel, JBBasicModel jbBasicModel, List<ImageBasicModel> additionalImageModels)
  {
    Id = id;
    Code = code;
    Function = function;
    Range = range;
    Unit = unit;
    IOAddress = ioAddress;
    ModuleBasicModel = moduleBasicModel;
    JBBasicModel = jbBasicModel;
    AdditionalImageModels = additionalImageModels;
  }
}


[Preserve]
public class DevicePostGeneralModel
{
  [JsonProperty("code")]
  public string? Code { get; set; }
  [JsonProperty("function")]
  public string? Function { get; set; }

  [JsonProperty("range")]
  public string? Range { get; set; }

  [JsonProperty("unit")]
  public string? Unit { get; set; }

  [JsonProperty("ioAddress")]
  public string? IOAddress { get; set; }

  [JsonProperty("module")]
  public ModuleBasicModel? ModuleBasicModel { get; set; }

  [JsonProperty("JB")]
  public JBBasicModel JBBasicModel { get; set; }

  [JsonProperty("additionalConnectionImages")]
  public List<ImageBasicModel> AdditionalConnectionBasicModel { get; set; }

  [Preserve]

  public DevicePostGeneralModel(string? code, string? function, string? range, string? unit, string? ioAddress, ModuleBasicModel? moduleBasicModel, JBBasicModel jbBasicModel, List<ImageBasicModel> additionalConnectionBasicModel)
  {
    Code = code;
    Function = function;
    Range = range;
    Unit = unit;
    IOAddress = ioAddress;
    ModuleBasicModel = moduleBasicModel;
    JBBasicModel = jbBasicModel;
    AdditionalConnectionBasicModel = additionalConnectionBasicModel;
  }


}


[Preserve]
public class DeviceBasicModel
{
  [JsonProperty("id")]
  public string? Id { get; set; }
  [JsonProperty("code")]
  public string? Code { get; set; }
  [Preserve]

  public DeviceBasicModel(string? id, string? code)
  {
    Id = id;
    Code = code;
  }

}