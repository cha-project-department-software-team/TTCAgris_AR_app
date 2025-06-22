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
    public class MccResponseDto : MccBasicDto
    {

        [JsonProperty("brand")]
        public string? Brand { get; set; }

        [JsonProperty("listFieldDevices")]
        public List<FieldDeviceBasicDto>? FieldDeviceBasicDtos { get; set; }

        [JsonProperty("note")]
        public string? Note { get; set; }

        [Preserve]

        public MccResponseDto(int id, string cabinetCode, string? brand, List<FieldDeviceBasicDto>? fieldDeviceBasicDtos, string? note) : base(id, cabinetCode)
        {
            Brand = string.IsNullOrEmpty(brand) ? "Chưa cập nhật" : brand;
            FieldDeviceBasicDtos = fieldDeviceBasicDtos.Any() ? fieldDeviceBasicDtos : new List<FieldDeviceBasicDto>();
            Note = string.IsNullOrEmpty(note) ? "Chưa cập nhật" : note;
        }
    }
}