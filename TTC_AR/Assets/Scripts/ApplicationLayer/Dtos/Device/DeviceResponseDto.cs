using System;
using System.Collections.Generic;
using ApplicationLayer.Dtos.Image;
using ApplicationLayer.Dtos.JB;
using ApplicationLayer.Dtos.Module;
using Newtonsoft.Json;
using UnityEngine.Scripting;

#nullable enable

namespace ApplicationLayer.Dtos.Device
{
    [Preserve]
    public class DeviceResponseDto : DeviceBasicDto
    {
        [JsonProperty("function")] public string? Function { get; set; }

        [JsonProperty("range")] public string? Range { get; set; }

        [JsonProperty("unit")] public string? Unit { get; set; }
        [JsonProperty("ioAddress")] public string? IOAddress { get; set; }
        [JsonProperty("module")] public ModuleBasicDto? ModuleBasicDto { get; set; }
        [JsonProperty("jBs")] public List<JBBasicDto>? JBBasicDtos { get; set; }
        [JsonProperty("additionalConnectionImages")] public List<ImageBasicDto>? AdditionalImageBasicDtos { get; set; } = new List<ImageBasicDto>();

        [Preserve]

        public DeviceResponseDto(int id, string code, string? function, string? range, string? unit, string? ioAddress, ModuleBasicDto? moduleBasicDto, List<JBBasicDto>? jbBasicDtos, List<ImageBasicDto>? additionalImageBasicDtos) : base(id, code)
        {
            Function = string.IsNullOrEmpty(function) ? "Chưa cập nhật" : function;
            Range = string.IsNullOrEmpty(range) ? "Chưa cập nhật" : range;
            Unit = string.IsNullOrEmpty(unit) ? "Chưa cập nhật" : unit;
            IOAddress = string.IsNullOrEmpty(ioAddress) ? "Chưa cập nhật" : ioAddress;
            ModuleBasicDto = moduleBasicDto;
            JBBasicDtos = jbBasicDtos;
            AdditionalImageBasicDtos = additionalImageBasicDtos ?? new List<ImageBasicDto>();
        }
        // [Preserve]
        // 
        // public DeviceResponseDto(int id, string code, string function, string range, string unit, string ioAddress, ModuleBasicDto moduleBasicDto, JBGeneralDto jbGeneralDto, List<ImageBasicDto> additionalImageBasicDto) : base(id, code)
        // {
        //     Function = function == "" ? string.Empty : function;
        //     Range = range == "" ? string.Empty : range;
        //     Unit = unit == "" ? string.Empty : unit;
        //     IOAddress = ioAddress == "" ? string.Empty : ioAddress;
        //     ModuleBasicDto = moduleBasicDto ?? throw new ArgumentException(nameof(moduleBasicDto));
        //     JBGeneralDto = jbGeneralDto ?? throw new ArgumentException(nameof(jbGeneralDto));
        //     AdditionalImageBasicDto = additionalImageBasicDto ?? new List<ImageBasicDto>();
        // }
    }
}



