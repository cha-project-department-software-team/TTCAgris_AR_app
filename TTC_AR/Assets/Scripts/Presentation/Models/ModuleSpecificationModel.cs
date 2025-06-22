using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using UnityEngine.Scripting;
#nullable enable

[Preserve]
public class ModuleSpecificationModel
{

  [JsonProperty("id")]
  public int Id { get; set; }
  [JsonProperty("code")]
  public string Code { get; set; } = string.Empty;
  [JsonProperty("type")]
  public string? Type { get; set; }
  [JsonProperty("numOfIO")]
  public string? NumOfIO { get; set; }

  [JsonProperty("signalType")]
  public string? SignalType { get; set; }

  [JsonProperty("compatibleTBUs")]
  public string? CompatibleTBUs { get; set; }

  [JsonProperty("operatingVoltage")]
  public string? OperatingVoltage { get; set; }
  [JsonProperty("operatingCurrent")]
  public string? OperatingCurrent { get; set; }

  [JsonProperty("flexbusCurrent")]
  public string? FlexbusCurrent { get; set; }
  [JsonProperty("alarm")]
  public string? Alarm { get; set; }
  [JsonProperty("note")]
  public string? Note { get; set; }
  [JsonProperty("pdfManual")]
  public string? PdfManual { get; set; }

  [Preserve]

  public ModuleSpecificationModel(int id, string code, string? type, string? numOfIO, string? signalType, string? compatibleTBUs, string? operatingVoltage, string? operatingCurrent, string? flexbusCurrent, string? alarm, string? note, string? pdfManual)
  {
    Id = id;
    Code = code;
    Type = type;
    NumOfIO = numOfIO;
    SignalType = signalType;
    CompatibleTBUs = compatibleTBUs;
    OperatingVoltage = operatingVoltage;
    OperatingCurrent = operatingCurrent;
    FlexbusCurrent = flexbusCurrent;
    Alarm = alarm;
    Note = note;
    PdfManual = pdfManual;
  }
  public ModuleSpecificationModel(int id, string code)
  {
    Id = id;
    Code = code;
  }

  public ModuleSpecificationModel()
  {
  }

  public ModuleSpecificationModel(
   string code, string? type, string? numOfIO, string? signalType, string? compatibleTBUs, string? operatingVoltage, string? operatingCurrent, string? flexbusCurrent, string? alarm, string? note, string? pdfManual)
  {
    Code = code;
    Type = type;
    NumOfIO = numOfIO;
    SignalType = signalType;
    CompatibleTBUs = compatibleTBUs;
    OperatingVoltage = operatingVoltage;
    OperatingCurrent = operatingCurrent;
    FlexbusCurrent = flexbusCurrent;
    Alarm = alarm;
    Note = note;
    PdfManual = pdfManual;
  }
}

[Preserve]
public class ModuleSpecificationBasicModel
{
  [JsonProperty("id")]
  public int Id { get; set; }

  [JsonProperty("code")]
  public string Code { get; set; }
  [Preserve]

  public ModuleSpecificationBasicModel(int id, string code)
  {
    Id = id;
    Code = code;
  }

}