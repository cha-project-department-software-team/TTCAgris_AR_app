using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.Scripting;
#nullable enable

namespace ApplicationLayer.Dtos.AdapterSpecification
{
  [Preserve]

  public class AdapterSpecificationBasicDto
  {
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("code")] public string Code { get; set; }

    [Preserve]

    public AdapterSpecificationBasicDto(int id, string code)
    {
      Id = id;
      Code = string.IsNullOrEmpty(code) ? throw new ArgumentException(nameof(code)) : code;
    }
  }

}

