
using Newtonsoft.Json;
using UnityEngine.Scripting;
#nullable enable

namespace ApplicationLayer.Dtos.ModuleSpecification
{
  public class ModuleSpecificationBasicDto
  {
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("code")] public string Code { get; set; }

    [Preserve]

    public ModuleSpecificationBasicDto(int id, string code)
    {
      Id = id;
      Code = string.IsNullOrEmpty(code) ? throw new System.ArgumentException(nameof(code)) : code;
    }
  }

}