using System;
using System.Collections.Generic;
using ApplicationLayer.Dtos.Image;
using Newtonsoft.Json;
using UnityEngine.Scripting;

#nullable enable

namespace ApplicationLayer.Dtos.Device
{
    [Preserve]
    public class DeviceBasicDto
    {
        [JsonProperty("id")] public int Id { get; set; }
        [JsonProperty("code")] public string Code { get; set; }

        [Preserve]

        public DeviceBasicDto(int id, string code)
        {
            Id = id;
            Code = string.IsNullOrEmpty(code) ? throw new ArgumentException(nameof(code)) : code;
        }
    }



}