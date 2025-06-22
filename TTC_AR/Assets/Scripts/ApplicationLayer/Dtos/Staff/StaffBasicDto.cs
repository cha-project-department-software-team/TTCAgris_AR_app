// using System;
// using Newtonsoft.Json;
// using UnityEngine.Scripting;

// namespace ApplicationLayer.Dtos
// {
//   public class StaffResponseDto
//   {
//     [JsonProperty("id")] public string UserId { get; set; }
//     [JsonProperty("name")] public string UserName { get; set; } = string.Empty;
//     [JsonProperty("role")] public string Role { get; set; } = string.Empty;

//     [Preserve]
//     
//     public StaffResponseDto(string userId, string userName, string role)
//     {
//       UserId = userId;
//       UserName = userName;
//       Role = role;
//     }
//   }

//   [Preserve]
//   public class StaffRequestDto
//   {
//     [JsonProperty("name")] public string UserName { get; set; } = string.Empty;
//     [JsonProperty("password")] public string Password { get; set; } = string.Empty;
//     [Preserve]
//     
//     public StaffRequestDto(string userName, string password)
//     {
//       UserName = userName;
//       Password = password;
//     }
//   }
// }


