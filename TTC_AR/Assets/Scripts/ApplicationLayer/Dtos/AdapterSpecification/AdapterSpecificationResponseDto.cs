using System;
using Newtonsoft.Json;
using UnityEngine.Scripting;
#nullable enable
namespace ApplicationLayer.Dtos.AdapterSpecification
{
    [Preserve]

    public class AdapterSpecificationResponseDto : AdapterSpecificationBasicDto
    {
        [JsonProperty("type")] public string? Type { get; set; }

        [JsonProperty("communication")] public string? Communication { get; set; }

        [JsonProperty("numOfModulesAllowed")] public string? NumOfModulesAllowed { get; set; }

        [JsonProperty("commSpeed")] public string? CommSpeed { get; set; }

        [JsonProperty("inputSupply")] public string? InputSupply { get; set; }
        [JsonProperty("outputSupply")] public string? OutputSupply { get; set; }

        [JsonProperty("inrushCurrent")] public string? InrushCurrent { get; set; }
        [JsonProperty("alarm")] public string? Alarm { get; set; }
        [JsonProperty("note")] public string? Note { get; set; }
        [JsonProperty("pdfManual")] public string? PdfManual { get; set; }

        [Preserve]

        public AdapterSpecificationResponseDto(int id, string code, string? type, string? communication, string? numOfModulesAllowed, string? commSpeed, string? inputSupply, string? outputSupply, string? inrushCurrent, string? alarm, string? note, string? pdfManual) : base(id, code)
        {
            Type = type;
            Communication = communication;
            NumOfModulesAllowed = numOfModulesAllowed;
            CommSpeed = commSpeed;
            InputSupply = inputSupply;
            OutputSupply = outputSupply;
            InrushCurrent = inrushCurrent;
            Alarm = alarm;
            Note = note;
            PdfManual = pdfManual;
        }


    }
}