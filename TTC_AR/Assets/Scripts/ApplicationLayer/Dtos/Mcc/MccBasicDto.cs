using System;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace ApplicationLayer.Dtos.Mcc
{
    [Preserve]
    public class MccBasicDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("cabinetCode")]
        public string CabinetCode { get; set; }

        [Preserve]

        public MccBasicDto(int id, string cabinetCode)
        {
            Id = id;
            CabinetCode = string.IsNullOrEmpty(cabinetCode) ? throw new ArgumentNullException(nameof(cabinetCode)) : cabinetCode;
        }
    }
}