using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.Scripting;
#nullable enable
[Preserve]
public class ImageInformationModel
{
  [JsonProperty("id")]
  public int Id { get; set; }

  [JsonProperty("name")]
  public string Name { get; set; } = string.Empty;

  // [JsonProperty("Url")]
  // public string? Url { get; set; }

  [Preserve]
  public ImageInformationModel(int id, string name
  // string url
  )
  {
    Id = id;
    Name = name;
    // Url = url;
  }

  public ImageInformationModel(string name)
  {

    Name = name;
  }
}


[Preserve]
public class ImageBasicModel
{
  [JsonProperty("id")]
  public int Id { get; set; }

  [JsonProperty("name")]
  public string Name { get; set; }

  [Preserve]

  public ImageBasicModel(int id, string name)
  {
    Id = id;
    Name = name;
  }
}