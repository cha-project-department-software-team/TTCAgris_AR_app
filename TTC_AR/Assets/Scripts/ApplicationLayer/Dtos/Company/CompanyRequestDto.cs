using System;
using System.Collections.Generic;
using ApplicationLayer.Dtos.AdapterSpecification;
using ApplicationLayer.Dtos.Grapper;
using ApplicationLayer.Dtos.ModuleSpecification;
using Newtonsoft.Json;
using UnityEngine.Scripting;

#nullable enable
namespace ApplicationLayer.Dtos.Company
{
    [Preserve]
    public class CompanyRequestDto
    {
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("listGrappers")] public List<GrapperBasicDto>? GrapperBasicDtos { get; set; }
        [JsonProperty("listModuleSpecifications")] public List<ModuleSpecificationBasicDto>? ModuleSpecificationBasicDtos { get; set; }
        [JsonProperty("listAdapterSpecifications")] public List<AdapterSpecificationBasicDto>? AdapterSpecificationBasicDtos { get; set; }

        [Preserve]

        public CompanyRequestDto(string name, List<GrapperBasicDto> grapperBasicDtos, List<ModuleSpecificationBasicDto> moduleSpecificationBasicDtos, List<AdapterSpecificationBasicDto> adapterSpecificationBasicDtos)
        {
            Name = name ?? throw new ArgumentException(nameof(name));
            GrapperBasicDtos = grapperBasicDtos;
            ModuleSpecificationBasicDtos = moduleSpecificationBasicDtos;
            AdapterSpecificationBasicDtos = adapterSpecificationBasicDtos;
        }
    }
}