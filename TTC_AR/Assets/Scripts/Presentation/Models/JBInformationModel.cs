
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.Scripting;
using static ModuleInformationModel;
#nullable enable
[Preserve]
public class JBInformationModel
{

  [JsonProperty("id")]
  public int Id { get; set; }
  [JsonProperty("name")]
  public string Name { get; set; } = string.Empty;

  [JsonProperty("location")]
  public string? Location { get; set; } = string.Empty;

  [JsonProperty("listDevices")]
  public List<DeviceInformationModel>? ListDeviceInformation { get; set; }

  [JsonProperty("listModules")]
  public List<ModuleInformationModel>? ListModuleInformation { get; set; }

  [JsonProperty("outdoorImage")]
  public ImageInformationModel? OutdoorImage { get; set; }

  [JsonProperty("listConnectionImages")]
  public List<ImageInformationModel>? ListConnectionImages { get; set; }

  [Preserve]

  public JBInformationModel(int id, string name, string? location, List<DeviceInformationModel>? listDeviceInformation, List<ModuleInformationModel>? listModuleInformation, ImageInformationModel? outdoorImage, List<ImageInformationModel>? listConnectionImages)
  {
    Id = id;
    Name = name;
    Location = location;
    ListDeviceInformation = listDeviceInformation;
    ListModuleInformation = listModuleInformation;
    OutdoorImage = outdoorImage;
    ListConnectionImages = listConnectionImages;
  }
  public JBInformationModel(string name, string? location, List<DeviceInformationModel>? listDeviceInformation, List<ModuleInformationModel>? listModuleInformation, ImageInformationModel? outdoorImage, List<ImageInformationModel>? listConnectionImages)
  {
    Name = name;
    Location = location;
    ListDeviceInformation = listDeviceInformation;
    ListModuleInformation = listModuleInformation;
    OutdoorImage = outdoorImage;
    ListConnectionImages = listConnectionImages;
  }
  public JBInformationModel(int id, string name)
  {
    Id = id;
    Name = name;
  }
  public JBInformationModel(int id, string name, string? location, ImageInformationModel? outdoorImage, List<ImageInformationModel>? listConnectionImages)
  {
    Id = id;
    Name = name;
    Location = location;
    OutdoorImage = outdoorImage;
    ListConnectionImages = listConnectionImages;
  }

  public JBInformationModel()
  {
  }
}

[Preserve]
public class JBGeneralModel
{
#nullable enable
  [JsonProperty("id")]
  public int Id { get; set; }
  [JsonProperty("name")]
  public string Name { get; set; }

  [JsonProperty("location")]
  public string? Location { get; set; }

  [JsonProperty("listDevices")]
  public List<DeviceBasicModel>? ListDevices { get; set; }

  [JsonProperty("listModules")]
  public List<ModuleBasicModel>? ListModules { get; set; }

  [JsonProperty("outdoorImage")]
  public ImageBasicModel OutdoorImage { get; set; }

  [JsonProperty("listConnectionImages")]
  public List<ImageBasicModel>? ListConnectionImages { get; set; }

  [Preserve]

  public JBGeneralModel(int id, string name, string? location, List<DeviceBasicModel>? listDevices, List<ModuleBasicModel>? listModules, ImageBasicModel outdoorImage, List<ImageBasicModel>? listConnectionImages)
  {
    Id = id;
    Name = name;
    Location = location;
    ListDevices = listDevices;
    ListModules = listModules;
    OutdoorImage = outdoorImage;
    ListConnectionImages = listConnectionImages;
  }
}

public class JBBasicModel
{
  [JsonProperty("id")]
  public int Id { get; set; }
  [JsonProperty("name")]
  public string Name { get; set; }

  [Preserve]

  public JBBasicModel(int id, string name)
  {
    Id = id;
    Name = name;
  }
}


[Preserve]
public class JBPostGeneralModel
{
#nullable enable
  [JsonProperty("name")]
  public string Name { get; set; }
  [JsonProperty("location")]
  public string? Location { get; set; }

  [JsonProperty("listDevices")]
  public List<DeviceBasicModel>? ListDevices { get; set; }

  [JsonProperty("listModules")]
  public List<ModuleBasicModel>? ListModules { get; set; }

  [JsonProperty("outdoorImage")]
  public ImageBasicModel OutdoorImage { get; set; }

  [JsonProperty("listConnectionImages")]
  public List<ImageBasicModel>? ListConnectionImages { get; set; }

  [Preserve]

  public JBPostGeneralModel(string name, string? location, List<DeviceBasicModel>? listDevices, List<ModuleBasicModel>? listModules, ImageBasicModel outdoorImage, List<ImageBasicModel>? listConnectionImages)
  {
    Name = name;
    Location = location;
    ListDevices = listDevices;
    ListModules = listModules;
    OutdoorImage = outdoorImage;
    ListConnectionImages = listConnectionImages;
  }


}
