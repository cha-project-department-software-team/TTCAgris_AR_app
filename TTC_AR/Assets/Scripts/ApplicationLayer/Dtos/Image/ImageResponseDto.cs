// using Newtonsoft.Json;
// using UnityEngine.Scripting;

// namespace ApplicationLayer.Dtos.Image
// {
//     [Preserve]
//     public class ImageBasicDto : ImageBasicDto
//     {
//         [JsonProperty("Url")]
//         public string Url { get; set; }

//         [Preserve]

//         public ImageBasicDto(int id, string name, string url) : base(id, name)
//         {
//             Url = string.IsNullOrEmpty(url) ? "Chưa cập nhật" : url;
//         }
//     }
// }