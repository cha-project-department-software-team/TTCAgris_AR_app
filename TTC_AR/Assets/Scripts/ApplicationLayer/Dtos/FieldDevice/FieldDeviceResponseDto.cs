using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationLayer.Dtos.Image;
using ApplicationLayer.Dtos.Mcc;
using Newtonsoft.Json;
using UnityEngine.Scripting;
#nullable enable
namespace ApplicationLayer.Dtos.FieldDevice
{
    [Preserve]
    public class FieldDeviceResponseDto : FieldDeviceBasicDto
    {
        [JsonProperty("mcc")]
        public MccBasicDto? Mcc { get; set; }

        [JsonProperty("ratedPower")]
        public string? RatedPower { get; set; }

        [JsonProperty("ratedCurrent")]
        public string? RatedCurrent { get; set; }

        [JsonProperty("activeCurrent")]
        public string? ActiveCurrent { get; set; }

        [JsonProperty("listConnectionImages")]
        public List<ImageBasicDto>? ConnectionImages { get; set; }

        [JsonProperty("note")]
        public string? Note { get; set; }
        [Preserve]

        public FieldDeviceResponseDto(int id, string? name, MccBasicDto? mcc, string? ratedPower, string? ratedCurrent, string? activeCurrent, List<ImageBasicDto>? connectionImages, string? note) : base(id, name)
        {
            Mcc = mcc ?? null;
            RatedPower = string.IsNullOrEmpty(ratedPower) ? "Chưa cập nhật" : ratedPower;
            RatedCurrent = string.IsNullOrEmpty(ratedCurrent) ? "Chưa cập nhật" : ratedCurrent;
            ActiveCurrent = string.IsNullOrEmpty(activeCurrent) ? "Chưa cập nhật" : activeCurrent;
            ConnectionImages = connectionImages.Any() ? connectionImages : new List<ImageBasicDto>();
            Note = string.IsNullOrEmpty(note) ? "Chưa cập nhật" : note;
        }
    }
}