
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine.Scripting;
#nullable enable
namespace Domain.Entities
{
  [Preserve]
  public class ImageEntity
  {
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    // [JsonProperty("Url")]
    // public string? Url { get; set; }

    public bool ShouldSerializeId()
    {
      List<string> apiRequestType = GlobalVariable.APIRequestType;
      HashSet<string> allowedRequests = new HashSet<string>
      {
        HttpMethodTypeEnum.POSTImage.GetDescription(),
        HttpMethodTypeEnum.GETListImage.GetDescription(),
      };
      return !apiRequestType.Any(request => allowedRequests.Contains(request));
    }
    public bool ShouldSerializeName()
    {
      return true;
    }
    // public bool ShouldSerializeUrl()
    // {
    //   List<string> apiRequestType = GlobalVariable.APIRequestType;
    //   HashSet<string> allowedRequests = new HashSet<string>
    //   {
    //     HttpMethodTypeEnum.GETImage.GetDescription(),
    //     HttpMethodTypeEnum.GETJB.GetDescription(),
    //     HttpMethodTypeEnum.GETListJBInformation.GetDescription(),
    //     HttpMethodTypeEnum.GETDevice.GetDescription(),
    //     HttpMethodTypeEnum.GETListDeviceInformationFromGrapper.GetDescription(),
    //     HttpMethodTypeEnum.GETListDeviceInformationFromModule.GetDescription(),
    //     HttpMethodTypeEnum.GETFieldDevice.GetDescription(),
    //   };
    //   return apiRequestType.Any(request => allowedRequests.Contains(request));
    // }

    [Preserve]
    public ImageEntity()
    {
    }

    [Preserve]
    public ImageEntity(int id, string name)
    {
      Id = id;
      Name = string.IsNullOrEmpty(name) ? throw new ArgumentNullException(nameof(name)) : name;
    }

    [Preserve]
    public ImageEntity(string name)
    {
      Name = string.IsNullOrEmpty(name) ? throw new ArgumentNullException(nameof(name)) : name;
    }


  }
}