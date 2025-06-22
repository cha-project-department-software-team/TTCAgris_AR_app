

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Newtonsoft.Json;
using UnityEngine;
namespace Infrastructure.Repositories
{
    public class FieldDeviceRepository : IFieldDeviceRepository
    {
        private readonly HttpClient _httpClient;

        public FieldDeviceRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<List<FieldDeviceEntity>> GetListFieldDeviceAsync(int grapperId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{GlobalVariable.baseUrl}/Grappers/{grapperId}/fieldDevices");
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException($"Failed to get FieldDevice list. Status: {response.StatusCode}");
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var entities = JsonConvert.DeserializeObject<List<FieldDeviceEntity>>(content);
                    return entities;
                }
            }
            catch (HttpRequestException ex)
            {
                // Xử lý lỗi HTTP
                UnityEngine.Debug.Log("Error: " + ex.Message);

                throw new ApplicationException($"Failed to fetch FieldDevice data: {ex.Message}");
            }
            catch (JsonException ex)
            {
                // Xử lý lỗi deserialize
                UnityEngine.Debug.Log("Error: " + ex.Message);

                throw new ApplicationException($"Failed to deserialize JSON: {ex.Message}");
            }

        }

        public async Task<FieldDeviceEntity> GetFieldDeviceByIdAsync(int fieldDeviceId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{GlobalVariable.baseUrl}/FieldDevices/{fieldDeviceId}");
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException($"Failed to get FieldDevice. Status: {response.StatusCode}");
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    UnityEngine.Debug.Log(content);
                    var entity = JsonConvert.DeserializeObject<FieldDeviceEntity>(content);
                    UnityEngine.Debug.Log(entity.Name + " " + entity.Id);

                    return entity;
                }
            }
            catch (HttpRequestException ex)
            {
                UnityEngine.Debug.Log("Error: " + ex.Message);
                throw new ApplicationException($"Failed to fetch FieldDevice data: {ex.Message}");
            }
            catch (JsonException ex)
            {
                // Xử lý lỗi deserialize
                UnityEngine.Debug.Log("Error: " + ex.Message);
                throw new ApplicationException($"Failed to deserialize JSON: {ex.Message}");
            }


        }

        public async Task<bool> CreateNewFieldDeviceAsync(int grapperId, FieldDeviceEntity fieldDeviceEntity)
        {
            try
            {

                var json = JsonConvert.SerializeObject(fieldDeviceEntity);
                //    UnityEngine.Debug.Log(json.ToString());

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{GlobalVariable.baseUrl}/FieldDevices/add/{grapperId}", content);

                var temp = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<bool>(temp);
                return result;

                // var responseContent = await response.Content.ReadAsStringAsync();
                // return JsonConvert.DeserializeObject<FieldDeviceEntity>(responseContent);
            }
            catch (HttpRequestException ex)
            {
                // Xử lý lỗi HTTP
                UnityEngine.Debug.Log("Error: " + ex.Message);
                throw new ApplicationException($"Failed to create FieldDevice: {ex.Message}");
            }
            catch (JsonException ex)
            {
                // Xử lý lỗi deserialize
                UnityEngine.Debug.Log("Error: " + ex.Message);
                throw new ApplicationException($"Failed to deserialize JSON: {ex.Message}");
            }
        }

        public async Task<bool> UpdateFieldDeviceAsync(int fieldDeviceId, FieldDeviceEntity fieldDeviceEntity)
        {
            try
            {
                // var fieldDeviceRequestData = ConvertFieldDeviceRequestData(fieldDeviceEntity);

                // var json = JsonConvert.SerializeObject(fieldDeviceEntity, new JsonSerializerSettings
                // {
                //     NullValueHandling = NullValueHandling.Ignore
                // });

                // var json = JsonConvert.SerializeObject(fieldDeviceRequestData);
                var json = JsonConvert.SerializeObject(fieldDeviceEntity);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"{GlobalVariable.baseUrl}/FieldDevices/{fieldDeviceId}", content);

                var temp = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<bool>(temp);
                return result;
                // var responseContent = await response.Content.ReadAsStringAsync();
                // return JsonConvert.DeserializeObject<FieldDeviceEntity>(responseContent);
            }
            catch (HttpRequestException ex)
            {
                // Xử lý lỗi HTTP
                UnityEngine.Debug.Log("Error: " + ex.Message);
                throw new ApplicationException($"Failed to update FieldDevice: {ex.Message}");
            }
            catch (JsonException ex)
            {
                // Xử lý lỗi deserialize
                UnityEngine.Debug.Log("Error: " + ex.Message);
                throw new ApplicationException($"Failed to deserialize JSON: {ex.Message}");
            }
        }
        public async Task<bool> DeleteFieldDeviceAsync(int fieldDeviceId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{GlobalVariable.baseUrl}/FieldDevices/{fieldDeviceId}");

                var temp = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<bool>(temp);
                return result;
            }
            catch (HttpRequestException ex)
            {
                // Xử lý lỗi HTTP
                UnityEngine.Debug.Log("Error: " + ex.Message);
                throw new ApplicationException($"Failed to delete FieldDevice: {ex.Message}");
            }
            catch (JsonException ex)
            {
                // Xử lý lỗi deserialize
                UnityEngine.Debug.Log("Error: " + ex.Message);
                throw new ApplicationException($"Failed to deserialize JSON: {ex.Message}");
            }

        }
    }
}