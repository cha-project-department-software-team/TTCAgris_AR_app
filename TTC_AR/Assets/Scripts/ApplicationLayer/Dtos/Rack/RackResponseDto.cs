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
    public class RackResponseDto : RackBasicDto
    {
        [JsonProperty("listModules")] public List<ModuleBasicDto> ModuleBasicDtos { get; set; }

        [Preserve]
        public RackResponseDto(int id, string name, List<ModuleBasicDto> moduleBasicDtos) : base(id, name)
        {
            ModuleBasicDtos = moduleBasicDtos.Any() ? moduleBasicDtos : new List<ModuleBasicDto>();
        }
    }
}