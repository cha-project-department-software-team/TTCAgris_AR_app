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

    public class CompanyResponseDto : CompanyBasicDto
    {
        [JsonProperty("listGrappers")] public List<GrapperBasicDto>? GrapperBasicDtos { get; set; }
        [JsonProperty("listModuleSpecifications")] public List<ModuleSpecificationBasicDto>? ModuleSpecificationBasicDtos { get; set; }
        [JsonProperty("listAdapterSpecifications")] public List<AdapterSpecificationBasicDto>? AdapterSpecificationBasicDtos { get; set; }

        [Preserve]

        public CompanyResponseDto(int id, string name, List<GrapperBasicDto>? grapperBasicDtos, List<ModuleSpecificationBasicDto>? moduleSpecificationBasicDtos, List<AdapterSpecificationBasicDto>? adapterSpecificationBasicDtos) : base(id, name)
        {
            GrapperBasicDtos = grapperBasicDtos;
            ModuleSpecificationBasicDtos = moduleSpecificationBasicDtos;
            AdapterSpecificationBasicDtos = adapterSpecificationBasicDtos;
        }
    }
}