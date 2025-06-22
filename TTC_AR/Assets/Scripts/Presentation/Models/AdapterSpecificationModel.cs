using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using UnityEngine.Scripting;
#nullable enable

[Preserve]
public class AdapterSpecificationModel
{

  [JsonProperty("id")]
  public int Id { get; set; }

  [JsonProperty("code")]
  public string Code { get; set; } = string.Empty;

  [JsonProperty("type")]
  public string? Type { get; set; }

  [JsonProperty("communication")]
  public string? Communication { get; set; }

  [JsonProperty("numOfModulesAllowed")]
  public string? NumOfModulesAllowed { get; set; }

  [JsonProperty("commSpeed")]
  public string? CommSpeed { get; set; }

  [JsonProperty("inputSupply")]
  public string? InputSupply { get; set; }
  [JsonProperty("outputSupply")]
  public string? OutputSupply { get; set; }

  [JsonProperty("inrushCurrent")]
  public string? InrushCurrent { get; set; }
  [JsonProperty("alarm")]
  public string? Alarm { get; set; }
  [JsonProperty("note")]
  public string? Note { get; set; }
  [JsonProperty("pdfManual")]
  public string? PdfManual { get; set; }
  [Preserve]

  public AdapterSpecificationModel(int id, string code, string? type, string? communication, string? numOfModulesAllowed, string? commSpeed, string? inputSupply, string? outputSupply, string? inrushCurrent, string? alarm, string? note, string? pdfManual)
  {
    Id = id;
    Code = code;
    Type = type;
    Communication = communication;
    NumOfModulesAllowed = numOfModulesAllowed;
    CommSpeed = commSpeed;
    InputSupply = inputSupply;
    OutputSupply = outputSupply;
    InrushCurrent = inrushCurrent;
    Alarm = alarm;
    Note = note;
    PdfManual = pdfManual;
  }
  public AdapterSpecificationModel(int id, string code)
  {
    Id = id;
    Code = code;
  }

  public AdapterSpecificationModel()
  {
  }
  public AdapterSpecificationModel(string code, string? type, string? communication, string? numOfModulesAllowed, string? commSpeed, string? inputSupply, string? outputSupply, string? inrushCurrent, string? alarm, string? note, string? pdfManual)
  {
    Code = code;
    Type = type;
    Communication = communication;
    NumOfModulesAllowed = numOfModulesAllowed;
    CommSpeed = commSpeed;
    InputSupply = inputSupply;
    OutputSupply = outputSupply;
    InrushCurrent = inrushCurrent;
    Alarm = alarm;
    Note = note;
    PdfManual = pdfManual;
  }
}
public class AdapterSpecificationBasicModel
{
  [JsonProperty("id")]
  public int Id { get; set; }
  [JsonProperty("code")]
  public string Code { get; set; }
  [Preserve]

  public AdapterSpecificationBasicModel(int id, string code)
  {
    Id = id;
    Code = code;
  }
}

