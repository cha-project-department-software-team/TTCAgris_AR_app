
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.Scripting;
#nullable enable

[Preserve]
public class MccInformationModel
{

  [JsonProperty("id")]
  public int Id { get; set; }
  [JsonProperty("cabinetCode")]
  public string CabinetCode { get; set; } = string.Empty;

  [JsonProperty("Brand")]
  public string? Brand { get; set; }
  [JsonProperty("listFieldDevices")]
  public List<FieldDeviceInformationModel>? ListFieldDeviceInformation { get; set; }

  [JsonProperty("Note")]
  public string? Note { get; set; }

  [Preserve]

  public MccInformationModel(int id, string cabinetCode, string? brand, List<FieldDeviceInformationModel>? listFieldDeviceInformation, string? note)
  {
    Id = id;
    CabinetCode = cabinetCode;
    Brand = brand;
    ListFieldDeviceInformation = listFieldDeviceInformation;
    Note = note;
  }
  public MccInformationModel(string cabinetCode, string? brand, List<FieldDeviceInformationModel>? listFieldDeviceInformation, string? note)
  {
    CabinetCode = cabinetCode;
    Brand = brand;
    ListFieldDeviceInformation = listFieldDeviceInformation;
    Note = note;
  }
  public MccInformationModel(int id, string cabinetCode)
  {
    Id = id;
    CabinetCode = cabinetCode;
  }

  [Preserve]
  public MccInformationModel()
  {
  }
}
