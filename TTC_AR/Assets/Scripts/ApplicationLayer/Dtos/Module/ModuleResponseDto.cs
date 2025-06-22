
using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationLayer.Dtos.AdapterSpecification;
using ApplicationLayer.Dtos.Device;
using ApplicationLayer.Dtos.Grapper;
using ApplicationLayer.Dtos.JB;
using ApplicationLayer.Dtos.ModuleSpecification;
using ApplicationLayer.Dtos.Rack;
using Newtonsoft.Json;
using UnityEngine.Scripting;
#nullable enable

namespace ApplicationLayer.Dtos.Module
{

    [Preserve]
    public class ModuleResponseDto : ModuleBasicDto
    {
        [JsonProperty("grapper")] public GrapperBasicDto GrapperBasicDto { get; set; }
        [JsonProperty("rack")] public RackBasicDto? RackBasicDto { get; set; }
        [JsonProperty("listJBs")] public List<JBBasicDto>? JBBasicDtos { get; set; }
        [JsonProperty("listDevices")] public List<DeviceBasicDto>? DeviceBasicDtos { get; set; }
        [JsonProperty("moduleSpecification")] public ModuleSpecificationBasicDto? ModuleSpecificationBasicDto { get; set; }
        [JsonProperty("adapterSpecification")] public AdapterSpecificationBasicDto? AdapterSpecificationBasicDto { get; set; }

        [Preserve]
        public ModuleResponseDto(int id, string name, GrapperBasicDto grapperBasicDto, RackBasicDto? rackBasicDto, List<DeviceBasicDto>? deviceBasicDtos, List<JBBasicDto>? jbBasicDtos, ModuleSpecificationBasicDto? moduleSpecificationBasicDto, AdapterSpecificationBasicDto? adapterSpecificationBasicDto) : base(id, name)
        {
            GrapperBasicDto = grapperBasicDto ?? throw new ArgumentNullException(nameof(grapperBasicDto));
            RackBasicDto = rackBasicDto != null ? rackBasicDto : null;
            DeviceBasicDtos = deviceBasicDtos.Any() ? deviceBasicDtos : new List<DeviceBasicDto>();
            JBBasicDtos = jbBasicDtos.Any() ? jbBasicDtos : new List<JBBasicDto>();
            ModuleSpecificationBasicDto = moduleSpecificationBasicDto != null ? moduleSpecificationBasicDto : null;
            AdapterSpecificationBasicDto = adapterSpecificationBasicDto != null ? adapterSpecificationBasicDto : null;
        }

    }
}