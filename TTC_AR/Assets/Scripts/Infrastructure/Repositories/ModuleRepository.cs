using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ApplicationLayer.Dtos.Module;
using Domain.Interfaces;


namespace Infrastructure.Repositories
{
    public class ModuleRepository : IModuleRepository
    {
        private readonly HttpClient _httpClient;


        public ModuleRepository(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            // _httpClient.BaseAddress = new Uri(GlobalVariable.baseUrl);
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        //! TRả về ModuleResponseDto do kết quả server trả chỉ là một tập hợp con của Entity
        public async Task<ModuleEntity> GetModuleByIdAsync(int moduleId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{GlobalVariable.baseUrl}/Modules/{moduleId}");
                if (!response.IsSuccessStatusCode)
                {
                    UnityEngine.Debug.LogError($"Failed to get Module. Status: {response.StatusCode}");
                    throw new HttpRequestException($"Failed to get Module. Status: {response.StatusCode}");
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    UnityEngine.Debug.Log("Module content: " + content);

                    if (string.IsNullOrEmpty(content))
                    {
                        UnityEngine.Debug.LogError("Received empty response from server");
                        throw new ApplicationException("Received empty response from server");
                    }
                    else
                    {
                        var entity = JsonConvert.DeserializeObject<ModuleEntity>(content);
                        return entity;
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException("Failed to fetch Module", ex); // Ném lỗi HTTP lên UseCase
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unexpected error during HTTP request", ex); // Bao bọc lỗi khác
            }

        }

        //! Trả về List<ModuleGeneralDto> do kết quả server trả chỉ là một tập hợp con của Entity
        public async Task<List<ModuleEntity>> GetListModuleAsync(int grapperId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{GlobalVariable.baseUrl}/Grappers/{grapperId}/modules");
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException($"Failed to get Module list. Status: {response.StatusCode}");
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    UnityEngine.Debug.Log("Module list content: " + content);

                    var entities = JsonConvert.DeserializeObject<List<ModuleEntity>>(content);

                    return entities;

                }

            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException("Failed to fetch Module list", ex); // Ném lỗi HTTP lên UseCase
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unexpected error during HTTP request", ex); // Bao bọc lỗi khác
            }
        }

        public async Task<bool> CreateNewModuleAsync(int grapperId, ModuleEntity requestData)
        {
            try
            {

                var json = JsonConvert.SerializeObject(requestData);

                UnityEngine.Debug.Log("json: " + json);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{GlobalVariable.baseUrl}/Modules/{grapperId}", content);

                var temp = await response.Content.ReadAsStringAsync();

                UnityEngine.Debug.Log("Create Module response: " + temp);

                var result = JsonConvert.DeserializeObject<bool>(temp);
                return result;
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException("Failed to create Module", ex); // Ném lỗi HTTP lên UseCase
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unexpected error during HTTP request", ex); // Bao bọc lỗi khác
            }

        }

        public async Task<bool> UpdateModuleAsync(int moduleId, ModuleEntity requestData)
        {
            try
            {
                var json = JsonConvert.SerializeObject(requestData);
                UnityEngine.Debug.Log("json: " + json);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"{GlobalVariable.baseUrl}/Modules/{moduleId}", content);

                var temp = await response.Content.ReadAsStringAsync();

                UnityEngine.Debug.Log("Update Module response: " + temp);

                var result = JsonConvert.DeserializeObject<bool>(temp);
                return result;
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException("Failed to update Module", ex); // Ném lỗi HTTP lên UseCase

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unexpected error during HTTP request", ex); // Bao bọc lỗi khác
            }
        }

        public async Task<bool> DeleteModuleAsync(int moduleId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{GlobalVariable.baseUrl}/Modules/{moduleId}");

                var temp = await response.Content.ReadAsStringAsync();

                UnityEngine.Debug.Log("Delete Module response: " + temp);

                var result = JsonConvert.DeserializeObject<bool>(temp);
                return result;
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException("Failed to delete Module", ex); // Ném lỗi HTTP lên UseCase
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unexpected error during HTTP request", ex); // Bao bọc lỗi khác
            }

        }
    }


}