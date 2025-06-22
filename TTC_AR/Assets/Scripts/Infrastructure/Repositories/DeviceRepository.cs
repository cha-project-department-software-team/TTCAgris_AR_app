using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using Domain.Interfaces;
using Unity.VisualScripting;

namespace Infrastructure.Repositories
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly HttpClient _httpClient;
        // private const string GlobalVariable.baseUrl = "https://67da8d3b35c87309f52d09f5.mockapi.io/api/v4/ListDevices"; // URL server ngoài thực tế

        public DeviceRepository(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            // _httpClient.BaseAddress = new Uri(GlobalVariable.baseUrl);
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<DeviceEntity> GetDeviceByIdAsync(int deviceId)
        {
            try
            {
                UnityEngine.Debug.Log("Run Repository");

                var response = await _httpClient.GetStringAsync($"{GlobalVariable.baseUrl}/Devices/{deviceId}");

                UnityEngine.Debug.Log(response);

                var entity = JsonConvert.DeserializeObject<DeviceEntity>(response);
                UnityEngine.Debug.Log(entity.Id);
                UnityEngine.Debug.Log(entity.Code);
                return entity;
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException("Failed to fetch Device", ex); // Ném lỗi HTTP lên UseCase
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error during HTTP request", ex); // Bao bọc lỗi khác
            }
        }

        public async Task<List<DeviceEntity>> GetListDeviceInformationFromGrapperAsync(int grapperId)
        {
            try
            {
                // var response = await _httpClient.GetAsync($"{GlobalVariable.baseUrl}/{grapperId}");
                var response = await _httpClient.GetAsync($"{GlobalVariable.baseUrl}/Grappers/{grapperId}/deviceInfos");
                if (!response.IsSuccessStatusCode) return null;

                var content = await response.Content.ReadAsStringAsync();

                UnityEngine.Debug.Log(content);
                var entities = JsonConvert.DeserializeObject<List<DeviceEntity>>(content);
                UnityEngine.Debug.Log(entities.Count);

                return entities;
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException("Failed to fetch Device list", ex); // Ném lỗi HTTP lên UseCase
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error during HTTP request", ex); // Bao bọc lỗi khác
            }
        }
        public async Task<List<DeviceEntity>> GetListDeviceInformationFromModuleAsync(int moduleId)
        {
            try
            {
                // var response = await _httpClient.GetAsync($"{GlobalVariable.baseUrl}/{grapperId}");
                var response = await _httpClient.GetAsync($"{GlobalVariable.baseUrl}/Modules/{moduleId}/devices");
                if (!response.IsSuccessStatusCode) return null;
                var content = await response.Content.ReadAsStringAsync();
                UnityEngine.Debug.Log(content);
                return JsonConvert.DeserializeObject<List<DeviceEntity>>(content);
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException("Failed to fetch Device list", ex); // Ném lỗi HTTP lên UseCase
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error during HTTP request", ex); // Bao bọc lỗi khác
            }
        }
        public async Task<List<DeviceEntity>> GetListDeviceGeneralAsync(int grapperId)
        {
            try
            {
                // var response = await _httpClient.GetAsync($"{GlobalVariable.baseUrl}/{grapperId}");

                var response = await _httpClient.GetStringAsync($"{GlobalVariable.baseUrl}/Grappers/{grapperId}/devicesGeneral");

                if (response == null) return null;
                // if (!response.IsSuccessStatusCode) return null;

                // var content = await response.Content.ReadAsStringAsync();

                //  UnityEngine.Debug.Log(response);

                var entities = JsonConvert.DeserializeObject<List<DeviceEntity>>(response);

                // UnityEngine.Debug.Log(entities.Count);

                // UnityEngine.Debug.Log("Số lượng Request" + GlobalVariable.APIRequestType.Count);

                return entities;


            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException("Failed to fetch Device list", ex); // Ném lỗi HTTP lên UseCase
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error during HTTP request", ex); // Bao bọc lỗi khác
            }
        }

        public async Task<bool> CreateNewDeviceAsync(int grapperId, DeviceEntity deviceEntity)
        {
            try
            {
                var json = JsonConvert.SerializeObject(deviceEntity);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{GlobalVariable.baseUrl}/Devices/add/{grapperId}", content);
                var temp = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<bool>(temp);
                return result;
            }

            catch (HttpRequestException ex)
            {
                throw new ApplicationException("Failed to create Device", ex); // Ném lỗi HTTP lên UseCase
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error during HTTP request", ex); // Bao bọc lỗi khác
            }
        }

        public async Task<bool> UpdateDeviceAsync(int deviceId, DeviceEntity deviceEntity)
        {
            try
            {

                // var deviceRequestData = ConvertDeviceRequestData(deviceEntity);
                // var json = JsonConvert.SerializeObject(deviceEntity, new JsonSerializerSettings
                // {
                //     NullValueHandling = NullValueHandling.Ignore
                // });

                var json = JsonConvert.SerializeObject(deviceEntity);
                UnityEngine.Debug.Log("Json: " + json);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"{GlobalVariable.baseUrl}/Devices/{deviceId}", content);
                var temp = await response.Content.ReadAsStringAsync();

                var temp2 = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(temp), Formatting.Indented);
                UnityEngine.Debug.Log("DeviceId From Repository: " + deviceId);
                UnityEngine.Debug.Log("Response: " + temp2);
                var result = JsonConvert.DeserializeObject<bool>(temp);
                UnityEngine.Debug.Log("Response: " + result);
                return result;
            }
            catch (HttpRequestException ex)
            {
                UnityEngine.Debug.Log("Error: " + ex.Message);
                throw new ApplicationException("Failed to update Device", ex); // Ném lỗi HTTP lên UseCase
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log("Error: " + ex.Message);
                throw new Exception("Unexpected error during HTTP request", ex); // Bao bọc lỗi khác
            }
        }

        public async Task<bool> DeleteDeviceAsync(int deviceId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{GlobalVariable.baseUrl}/Devices/{deviceId}");
                var temp = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<bool>(temp);
                return result;
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException("Failed to delete Device", ex); // Ném lỗi HTTP lên UseCase
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error during HTTP request", ex); // Bao bọc lỗi khác
            }


        }

        // private object ConvertDeviceRequestData(DeviceEntity deviceEntity)
        // {
        //     return new
        //     {
        //         code = deviceEntity.Code,
        //         function = deviceEntity.Function,
        //         range = deviceEntity.Range,
        //         unit = deviceEntity.Unit,
        //         iOAddress = deviceEntity.IOAddress,
        //         JBEntity = new
        //         {
        //             deviceEntity.JBEntity.Id,
        //             deviceEntity.JBEntity.Name
        //         },
        //         Module = new
        //         {
        //             deviceEntity.ModuleEntity.Id,
        //             deviceEntity.ModuleEntity.Name
        //         },
        //         AdditionalImageEntity = deviceEntity.AdditionalConnectionImageEntities.Select
        //         (entity => new
        //         {
        //             entity.Id,
        //             entity.Name
        //         }).ToList()
        //     };
        // }
    }
}