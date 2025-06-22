

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Newtonsoft.Json;
namespace Infrastructure.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string baseUrl = $"{GlobalVariable.baseUrl}/Companies";

        public CompanyRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CompanyEntity> GetCompanyByIdAsync(int companyId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{baseUrl}/{companyId}");
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Failed to get company. Status: {response.StatusCode}");
                }
                var content = await response.Content.ReadAsStringAsync();
                var entity = JsonConvert.DeserializeObject<CompanyEntity>(content);
                return entity;
            }
            catch (HttpRequestException ex)
            {
                // Xử lý lỗi HTTP
                throw new Exception($"Failed to fetch company data: {ex.Message}");
            }
            catch (JsonException ex)
            {
                // Xử lý lỗi deserialize
                throw new Exception($"Failed to deserialize JSON: {ex.Message}");
            }
        }

        public async Task<List<CompanyEntity>> GetListCompanyAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(baseUrl);
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException($"Failed to get Company list. Status: {response.StatusCode}");
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<CompanyEntity>>(content);
                }

            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Failed to fetch Company list: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error during HTTP request", ex);
            }
        }
    }
}