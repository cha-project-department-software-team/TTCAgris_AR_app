using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationLayer.Dtos.AdapterSpecification;
using ApplicationLayer.Dtos.Device;
using ApplicationLayer.Dtos.JB;
using ApplicationLayer.Dtos.ModuleSpecification;
using ApplicationLayer.Dtos.Rack;
using Newtonsoft.Json;
using UnityEngine.Scripting;
#nullable enable

namespace ApplicationLayer.Dtos.Module
{
    [Preserve]
    public class ModuleRequestDto
    {
        [JsonProperty("name")] public string Name { get; set; } = string.Empty;
        [JsonProperty("rack")] public RackBasicDto? RackBasicDto { get; set; }
        [JsonProperty("listDevices")] public List<DeviceBasicDto>? DeviceBasicDtos { get; set; }
        [JsonProperty("listJBs")] public List<JBBasicDto>? JBBasicDtos { get; set; }
        [JsonProperty("moduleSpecification")] public ModuleSpecificationBasicDto? ModuleSpecificationBasicDto { get; set; }
        [JsonProperty("adapterSpecification")] public AdapterSpecificationBasicDto? AdapterSpecificationBasicDto { get; set; }

        [Preserve]
        public ModuleRequestDto(string name, RackBasicDto? rackBasicDto, List<DeviceBasicDto>? deviceBasicDtos, List<JBBasicDto>? jBBasicDtos, ModuleSpecificationBasicDto? moduleSpecificationBasicDto, AdapterSpecificationBasicDto? adapterSpecificationBasicDto)
        {
            Name = string.IsNullOrEmpty(name) ? throw new ArgumentNullException(nameof(name)) : name;
            RackBasicDto = rackBasicDto != null ? rackBasicDto : null;
            DeviceBasicDtos = deviceBasicDtos.Any() ? deviceBasicDtos : new List<DeviceBasicDto>();
            JBBasicDtos = jBBasicDtos.Any() ? jBBasicDtos : new List<JBBasicDto>();
            ModuleSpecificationBasicDto = moduleSpecificationBasicDto != null ? moduleSpecificationBasicDto : null;
            AdapterSpecificationBasicDto = adapterSpecificationBasicDto != null ? adapterSpecificationBasicDto : null;
        }

    }
}