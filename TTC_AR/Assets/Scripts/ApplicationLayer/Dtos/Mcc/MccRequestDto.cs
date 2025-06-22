using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationLayer.Dtos.FieldDevice;
using Newtonsoft.Json;
using UnityEngine.Scripting;
#nullable enable
namespace ApplicationLayer.Dtos.Mcc
{
    [Preserve]
    public class MccRequestDto
    {
        [JsonProperty("cabinetCode")]
        public string? CabinetCode { get; set; }

        [JsonProperty("brand")]
        public string? Brand { get; set; }

        [JsonProperty("listFieldDevice")]
        public List<FieldDeviceBasicDto>? FieldDeviceBasicDtos { get; set; }

        [JsonProperty("note")]
        public string? Note { get; set; }

        [Preserve]

        public MccRequestDto(string cabinetCode, string? brand, List<FieldDeviceBasicDto>? fieldDeviceBasicDtos, string? note)
        {
            CabinetCode = string.IsNullOrEmpty(cabinetCode) ? throw new ArgumentNullException(nameof(cabinetCode)) : cabinetCode;
            Brand = string.IsNullOrEmpty(brand) ? "Chưa cập nhật" : brand;
            FieldDeviceBasicDtos = fieldDeviceBasicDtos.Any() ? fieldDeviceBasicDtos : new List<FieldDeviceBasicDto>();
            Note = string.IsNullOrEmpty(note) ? "Chưa cập nhật" : note;
        }
    }
}