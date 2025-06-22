
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.Scripting;
#nullable enable

[Preserve]
public class FieldDeviceInformationModel
{

  [JsonProperty("id")]
  public int Id { get; set; }
  [JsonProperty("name")]
  public string Name { get; set; } = string.Empty;
  [JsonProperty("Mcc")]
  public MccInformationModel? Mcc { get; set; }

  [JsonProperty("ratedPower")]
  public string? RatedPower { get; set; }

  [JsonProperty("ratedCurrent")]
  public string? RatedCurrent { get; set; }

  [JsonProperty("activeCurrent")]
  public string? ActiveCurrent { get; set; }

  [JsonProperty("listConnectionImages")]
  public List<ImageInformationModel>? ListConnectionImages { get; set; }

  [JsonProperty("note")]
  public string? Note { get; set; }
  [Preserve]

  public FieldDeviceInformationModel(int id, MccInformationModel mcc, string name, string? ratedPower, string? ratedCurrent, string? activeCurrent, List<ImageInformationModel>? listConnectionImages, string? note)
  {
    Id = id;
    Mcc = mcc;
    Name = name;
    RatedPower = ratedPower;
    RatedCurrent = ratedCurrent;
    ActiveCurrent = activeCurrent;
    ListConnectionImages = listConnectionImages;
    Note = note;
  }

  public FieldDeviceInformationModel(string name, string? ratedPower, string? ratedCurrent, string? activeCurrent, List<ImageInformationModel>? listConnectionImages, string? note)
  {
    Name = name;
    RatedPower = ratedPower;
    RatedCurrent = ratedCurrent;
    ActiveCurrent = activeCurrent;
    ListConnectionImages = listConnectionImages;
    Note = note;
  }
  public FieldDeviceInformationModel(int id, string name)
  {
    Id = id;
    Name = name;
  }


  [Preserve]
  public FieldDeviceInformationModel()
  {
  }

  public class FieldDevice_Basic_Model
  {
    [JsonProperty("id")]
    public int Id { get; set; }
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("cabinetCode")]
    public string CabinetCode { get; set; }

    public FieldDevice_Basic_Model(int id, string name, string cabinetCode)
    {
      Id = id;
      Name = name;
      CabinetCode = cabinetCode;
    }

  }
}