using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.Scripting;
namespace ApplicationLayer.Dtos.Company
{
  [Preserve]

  public class CompanyBasicDto
  {
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("name")] public string Name { get; set; }

    [Preserve]

    public CompanyBasicDto(int id, string name)
    {
      Id = id;
      Name = string.IsNullOrEmpty(name) ? throw new ArgumentNullException(nameof(name)) : name;
    }
  }


}