using System;
using System.Collections.Generic;
using ApplicationLayer.Dtos.Device;
using ApplicationLayer.Dtos.FieldDevice;
using ApplicationLayer.Dtos.JB;
using ApplicationLayer.Dtos.Mcc;
using ApplicationLayer.Dtos.Rack;
using Newtonsoft.Json;
using UnityEngine.Scripting;

#nullable enable
namespace ApplicationLayer.Dtos.Grapper
{
    [Preserve]
    public class GrapperRequestDto
    {
        [JsonProperty("name")] public string Name { get; set; }

        [Preserve]

        public GrapperRequestDto(string name)
        {
            Name = string.IsNullOrEmpty(name) ? throw new ArgumentNullException(nameof(name)) : name;
        }
    }
}