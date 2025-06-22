using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationLayer.Dtos.Image;
using Domain.Entities;
using Newtonsoft.Json;
using UnityEngine.Scripting;

#nullable enable
namespace ApplicationLayer.Dtos.JB
{
    [Preserve]
    public class JBGeneralDto : JBBasicDto
    {
        [JsonProperty("location")] public string? Location { get; set; }
        [JsonProperty("outdoorImage")] public ImageBasicDto? OutdoorImageBasicDto { get; set; }
        [JsonProperty("listConnectionImages")] public List<ImageBasicDto>? ConnectionImageBasicDtos { get; set; }

        [Preserve]

        public JBGeneralDto(int id, string name, string? location, ImageBasicDto? outdoorImageBasicDto, List<ImageBasicDto>? connectionImageBasicDtos) : base(id, name)
        {
            Location = string.IsNullOrEmpty(location) ? "Được ghi chú trên sơ đồ" : location;
            OutdoorImageBasicDto = outdoorImageBasicDto != null ? outdoorImageBasicDto : null;
            ConnectionImageBasicDtos = connectionImageBasicDtos.Any() ? connectionImageBasicDtos : new List<ImageBasicDto>();
        }

        // [Preserve]
        // 
        // public JBGeneralDto(int id, string name, string location, ImageBasicDto outdoorImageBasicDto, List<ImageBasicDto> connectionImageBasicDtos) : base(id, name)
        // {
        //     Id = id;
        //     Location = location == "" ? string.Empty : location;
        //     OutdoorImageBasicDto = outdoorImageBasicDto;
        //     ConnectionImageBasicDtos = connectionImageBasicDtos.Any() ? connectionImageBasicDtos : new List<ImageBasicDto>();
        // }
    }
}