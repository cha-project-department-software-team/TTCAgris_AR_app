
using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationLayer.Dtos.Module;
using Newtonsoft.Json;
using UnityEngine.Scripting;
#nullable enable    
namespace ApplicationLayer.Dtos.Rack
{
    [Preserve]
    public class RackRequestDto
    {
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("listModules")] public List<ModuleBasicDto>? ModuleBasicDtos { get; set; }

        [Preserve]

        public RackRequestDto(string name, List<ModuleBasicDto>? moduleBasicDtos)
        {
            Name = string.IsNullOrEmpty(name) ? throw new ArgumentException(nameof(name)) : name;
            ModuleBasicDtos = moduleBasicDtos.Any() ? moduleBasicDtos : new List<ModuleBasicDto>();
        }

    }
}