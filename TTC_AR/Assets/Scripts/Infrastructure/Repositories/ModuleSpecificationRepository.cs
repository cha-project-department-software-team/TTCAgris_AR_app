using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine.Networking;
using System.Linq;
using ApplicationLayer.Dtos;
using ApplicationLayer.Dtos.ModuleSpecification;
using Domain.Interfaces;

namespace Infrastructure.Repositories
{
    public class ModuleSpecificationRepository : IModuleSpecificationRepository
    {
        private readonly HttpClient _httpClient;

        public ModuleSpecificationRepository(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            // _httpClient.BaseAddress = new Uri(GlobalVariable.baseUrl);
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        //! Trả về Entity do kết quả server trả về hoàn toàn giống hoặc gần giống với Entity
        public async Task<ModuleSpecificationEntity> GetModuleSpecificationByIdAsync(int moduleSpecificationId)
        {
            try
            {
                // var response = await _httpClient.GetAsync($"/api/ModuleSpecification/{moduleSpecificationId}");
                var response = await _httpClient.GetAsync($"{GlobalVariable.baseUrl}/ModuleSpecifications/{moduleSpecificationId}");
                if (!response.IsSuccessStatusCode)
                {
                    UnityEngine.Debug.Log("ErrorStatus Code: " + response.StatusCode);
                    throw new HttpRequestException($"Failed to get ModuleSpecification. Status: {response.StatusCode}");
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    UnityEngine.Debug.Log(content);
                    var entity = JsonConvert.DeserializeObject<ModuleSpecificationEntity>(content);
                    UnityEngine.Debug.Log(entity.Id);
                    return entity;
                }
            }
            catch (HttpRequestException ex)
            {
                UnityEngine.Debug.Log("Error: " + ex.Message);
                throw new ApplicationException("Failed to fetch ModuleSpecification", ex); // Ném lỗi HTTP lên UseCase
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log("Error: " + ex.Message);
                throw new ApplicationException("Unexpected error during HTTP request", ex); // Bao bọc lỗi khác
            }
        }

        //! Trả về List<Entity> do kết quả server trả về hoàn toàn giống hoặc gần giống với Entity
        public async Task<List<ModuleSpecificationEntity>> GetListModuleSpecificationAsync(int grapperId)
        {
            try
            {
                // var response = await _httpClient.GetAsync($"/api/ModuleSpecification/grapper/{companyId}");
                var response = await _httpClient.GetAsync($"{GlobalVariable.baseUrl}/Grappers/{grapperId}/moduleSpecificationsGeneral");

                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException($"Failed to get ModuleSpecification list. Status: {response.StatusCode}");
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var entities = JsonConvert.DeserializeObject<List<ModuleSpecificationEntity>>(content);
                    return entities;
                }
            }
            catch (HttpRequestException ex)
            {
                UnityEngine.Debug.Log("Error: " + ex.Message);

                throw new ApplicationException("Failed to fetch ModuleSpecification", ex); // Ném lỗi HTTP lên UseCase
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log("Error: " + ex.Message);

                throw new ApplicationException("Unexpected error during HTTP request", ex); // Bao bọc lỗi khác
            }
        }

        public async Task<bool> CreateNewModuleSpecificationAsync(int companyId, ModuleSpecificationEntity moduleSpecificationEntity)
        {
            try
            {
                var json = JsonConvert.SerializeObject(moduleSpecificationEntity);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{GlobalVariable.baseUrl}/ModuleSpecifications/companyId?companyId={companyId}", content);
                var temp = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<bool>(temp);
                UnityEngine.Debug.Log("Result: " + result);
                return result;
            }
            catch (HttpRequestException ex)
            {
                UnityEngine.Debug.Log("Error: " + ex.Message);

                throw new ApplicationException("Failed to create ModuleSpecification", ex); // Ném lỗi HTTP lên UseCase
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log("Error: " + ex.Message);

                throw new ApplicationException("Unexpected error during HTTP request", ex); // Bao bọc lỗi khác
            }
        }

        public async Task<bool> UpdateModuleSpecificationAsync(int moduleSpecificationId, ModuleSpecificationEntity moduleSpecificationEntity)
        {
            try
            {
                var json = JsonConvert.SerializeObject(moduleSpecificationEntity);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                // var response = await _httpClient.PutAsync($"/api/ModuleSpecification/{moduleSpecificationId}", content);
                var response = await _httpClient.PutAsync($"{GlobalVariable.baseUrl}/ModuleSpecifications/{moduleSpecificationId}", content);

                var temp = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<bool>(temp);
                UnityEngine.Debug.Log("Result: " + result);
                return result;
            }
            catch (HttpRequestException ex)
            {
                UnityEngine.Debug.Log("Error: " + ex.Message);

                throw new ApplicationException("Failed to update ModuleSpecification", ex); // Ném lỗi HTTP lên UseCase
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log("Error: " + ex.Message);

                throw new ApplicationException("Unexpected error during HTTP request", ex); // Bao bọc lỗi khác
            }
        }

        public async Task<bool> DeleteModuleSpecificationAsync(int moduleSpecificationId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{GlobalVariable.baseUrl}/ModuleSpecifications/{moduleSpecificationId}");

                var temp = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<bool>(temp);
                return result;
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException("Failed to delete ModuleSpecification", ex); // Ném lỗi HTTP lên UseCase
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unexpected error during HTTP request", ex); // Bao bọc lỗi khác
            }

        }

    }
}