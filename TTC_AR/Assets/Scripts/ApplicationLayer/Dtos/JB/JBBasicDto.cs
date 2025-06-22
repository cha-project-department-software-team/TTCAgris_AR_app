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
    // DTO cơ bản cho JB (dùng làm field trong DTO khác, có Id)
    [Preserve]
    public class JBBasicDto
    {
        [JsonProperty("id")] public int Id { get; set; }
        [JsonProperty("name")] public string Name { get; set; }

        [Preserve]

        public JBBasicDto(int id, string name)
        {
            Id = id;
            Name = string.IsNullOrEmpty(name) ? throw new ArgumentNullException(nameof(name)) : name;
        }
    }

}
