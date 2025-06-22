using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationLayer.Dtos.Device;
using ApplicationLayer.Dtos.FieldDevice;
using ApplicationLayer.Dtos.JB;
using ApplicationLayer.Dtos.Mcc;
using ApplicationLayer.Dtos.Module;
using ApplicationLayer.Dtos.Rack;
using Newtonsoft.Json;
using UnityEngine.Scripting;

#nullable enable

namespace ApplicationLayer.Dtos.Grapper
{

    [Preserve]
    public class GrapperResponseDto : GrapperBasicDto
    {
        [JsonProperty("listRacks")] public List<RackBasicDto>? RackBasicDtos { get; set; }
        [JsonProperty("listModules")] public List<ModuleBasicDto>? ModuleGeneralDtos { get; set; }
        [JsonProperty("listDevices")] public List<DeviceBasicDto>? DeviceBasicDtos { get; set; }
        [JsonProperty("listJBs")] public List<JBBasicDto>? JBBasicDtos { get; set; }
        [JsonProperty("listMCCs")] public List<MccBasicDto>? MccBasicDtos { get; set; }
        [JsonProperty("listFieldDevices")] public List<FieldDeviceBasicDto>? FieldDeviceBasicDtos { get; set; }


        [Preserve]
        public GrapperResponseDto(int id, string name, List<RackBasicDto>? rackBasicDtos, List<ModuleBasicDto> moduleBasicDtos, List<DeviceBasicDto>? deviceBasicDtos, List<JBBasicDto>? jBBasicDtos, List<MccBasicDto>? mccBasicDtos, List<FieldDeviceBasicDto>? fieldDeviceBasicDtos) : base(id, name)
        {
            RackBasicDtos = rackBasicDtos.Any() ? rackBasicDtos : new List<RackBasicDto>();
            ModuleGeneralDtos = moduleBasicDtos.Any() ? moduleBasicDtos : new List<ModuleBasicDto>();
            DeviceBasicDtos = deviceBasicDtos.Any() ? deviceBasicDtos : new List<DeviceBasicDto>();
            JBBasicDtos = jBBasicDtos.Any() ? jBBasicDtos : new List<JBBasicDto>();
            MccBasicDtos = mccBasicDtos.Any() ? mccBasicDtos : new List<MccBasicDto>();
            FieldDeviceBasicDtos = fieldDeviceBasicDtos.Any() ? fieldDeviceBasicDtos : new List<FieldDeviceBasicDto>();
        }
    }

}