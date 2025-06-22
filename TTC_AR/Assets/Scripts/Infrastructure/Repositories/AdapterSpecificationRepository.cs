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
using ApplicationLayer.Dtos.AdapterSpecification;
using Domain.Interfaces;

namespace Infrastructure.Repositories
{
    public class AdapterSpecificationRepository : IAdapterSpecificationRepository
    {
        private readonly HttpClient _httpClient;


        public AdapterSpecificationRepository(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            // _httpClient.BaseAddress = new Uri(GlobalVariable.baseUrl);
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        //! Trả về Entity
        public async Task<AdapterSpecificationEntity> GetAdapterSpecificationByIdAsync(int adapterSpecificationId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{GlobalVariable.baseUrl}/AdapterSpecifications/{adapterSpecificationId}");

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Failed to get AdapterSpecification. Status: {response.StatusCode}");
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();

                    UnityEngine.Debug.Log(content);

                    var entity = JsonConvert.DeserializeObject<AdapterSpecificationEntity>(content);

                    return entity;
                }
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException("Failed to fetch AdapterSpecification", ex); // Ném lỗi HTTP lên UseCase
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error during HTTP request", ex); // Bao bọc lỗi khác
            }
        }

        //! Trả về List<Entity>
        public async Task<List<AdapterSpecificationEntity>> GetListAdapterSpecificationAsync(int companyId)
        {
            try
            {
                // var response = await _httpClient.GetAsync($"{GlobalVariable.baseUrl}/{companyId}");
                var response = await _httpClient.GetAsync($"{GlobalVariable.baseUrl}/Grappers/{companyId}/adapterSpecificationsGeneral");

                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException($"Failed to get AdapterSpecification list. Status: {response.StatusCode}");
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<AdapterSpecificationEntity>>(content);
                }
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException("Failed to fetch AdapterSpecification list", ex); // Ném lỗi HTTP lên UseCase
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error during HTTP request", ex);
            }
        }

        public async Task<bool> CreateNewAdapterSpecificationAsync(int companyId, AdapterSpecificationEntity adapterSpecificationEntity)
        {
            try
            {

                if (adapterSpecificationEntity == null)
                    throw new ArgumentNullException(nameof(adapterSpecificationEntity), "Entity cannot be null");
                // Tạo dữ liệu tối giản gửi lên server
                else
                {
                    var json = JsonConvert.SerializeObject(adapterSpecificationEntity);

                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await _httpClient.PostAsync($"{GlobalVariable.baseUrl}/AdapterSpecifications/companyId?companyId={companyId}", content);

                    var temp = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<bool>(temp);
                    return result;

                }
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException("Failed to create AdapterSpecification", ex); // Ném lỗi HTTP lên UseCase
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error during HTTP request", ex); // Bao bọc lỗi khác
            }
        }


        public async Task<bool> UpdateAdapterSpecificationAsync(int adapterSpecificationId, AdapterSpecificationEntity adapterSpecificationEntity)
        {
            try
            {
                if (adapterSpecificationEntity == null)
                    throw new ArgumentNullException(nameof(adapterSpecificationEntity), "Entity cannot be null");
                // Tạo dữ liệu tối giản gửi lên server
                else
                {

                    var json = JsonConvert.SerializeObject(adapterSpecificationEntity);

                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await _httpClient.PutAsync($"{GlobalVariable.baseUrl}/AdapterSpecifications/{adapterSpecificationId}", content);
                    // var response = await _httpClient.PutAsync($"/api/AdapterSpecification/{adapterSpecificationId}", content);

                    var temp = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<bool>(temp);
                    return result;

                }
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException("Failed to update AdapterSpecification", ex); // Ném lỗi HTTP lên UseCase
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error during HTTP request", ex); // Bao bọc lỗi khác
            }
        }

        public async Task<bool> DeleteAdapterSpecificationAsync(int adapterSpecificationId)
        {
            try
            {
                //! var response = await _httpClient.DeleteAsync($"/api/AdapterSpecification/{adapterSpecificationId}");
                var response = await _httpClient.DeleteAsync($"{GlobalVariable.baseUrl}/AdapterSpecifications/{adapterSpecificationId}");
                var temp = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<bool>(temp);
                return result;

            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException("Failed to delete AdapterSpecification", ex); // Ném lỗi HTTP lên UseCase
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error during HTTP request", ex); // Bao bọc lỗi khác
            }

        }

    }
}