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
    public class GrapperRepository : IGrapperRepository
    {
        private readonly HttpClient _httpClient;


        public GrapperRepository(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            // _httpClient.BaseAddress = new Uri(GlobalVariable.baseUrl);
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        //! TRả về GrapperEntity
        public async Task<GrapperEntity> GetGrapperByIdAsync(int GrapperId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{GlobalVariable.baseUrl}/Grappers/{GrapperId}");

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Failed to get Grapper. Status: {response.StatusCode}");
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    UnityEngine.Debug.Log(content);
                    var entity = JsonConvert.DeserializeObject<GrapperEntity>(content);
                    return entity;

                }
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException("Failed to fetch Grapper", ex); // Ném lỗi HTTP lên UseCase
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unexpected error during HTTP request", ex); // Bao bọc lỗi khác
            }
        }

        //! Trả về List<GrapperEntity>
        public async Task<List<GrapperEntity>> GetListGrapperAsync(int companyId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{GlobalVariable.baseUrl}/Companies/{companyId}/grappers");
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException($"Failed to get Grapper list. Status: {response.StatusCode}");
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    UnityEngine.Debug.Log(content);
                    var entities = JsonConvert.DeserializeObject<List<GrapperEntity>>(content);
                    return entities;
                }

            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException("Failed to fetch Grapper list", ex); // Ném lỗi HTTP lên UseCase
            }
            catch (JsonException ex)
            {
                throw new ApplicationException("Failed to deserialize JSON", ex); // Xử lý lỗi deserialize
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unexpected error during HTTP request", ex); // Bao bọc lỗi khác
            }
        }


        public async Task<bool> CreateNewGrapperAsync(int companyId, GrapperEntity grapperEntity)
        {
            try
            {
                if (grapperEntity == null)
                    throw new ArgumentNullException(nameof(grapperEntity), "Request data cannot be null");

                var json = JsonConvert.SerializeObject(grapperEntity);

                UnityEngine.Debug.Log(json);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{GlobalVariable.baseUrl}/Grappers/add/{companyId}", content);

                var temp = await response.Content.ReadAsStringAsync();
                UnityEngine.Debug.Log(temp);
                var result = JsonConvert.DeserializeObject<bool>(temp);
                return result;
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException("Failed to create Grapper", ex); // Ném lỗi HTTP lên UseCase
            }
            catch (JsonException ex)
            {
                throw new ApplicationException("Failed to deserialize JSON", ex); // Xử lý lỗi deserialize
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unexpected error during HTTP request", ex); // Bao bọc lỗi khác
            }
        }

        public async Task<bool> UpdateGrapperAsync(int GrapperId, GrapperEntity grapperEntity)
        {
            try
            {
                if (grapperEntity == null)
                    throw new ArgumentNullException(nameof(grapperEntity), "Request data cannot be null");

                var json = JsonConvert.SerializeObject(grapperEntity);
                UnityEngine.Debug.Log(json);

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"{GlobalVariable.baseUrl}/Grappers/update/{GrapperId}", content);
                var temp = await response.Content.ReadAsStringAsync();
                UnityEngine.Debug.Log(temp);
                var result = JsonConvert.DeserializeObject<bool>(temp);
                return result;
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException("Failed to update Grapper", ex); // Ném lỗi HTTP lên UseCase
            }
            catch (JsonException ex)
            {
                throw new ApplicationException("Failed to deserialize JSON", ex); // Xử lý lỗi deserialize
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unexpected error during HTTP request", ex); // Bao bọc lỗi khác
            }

        }

        public async Task<bool> DeleteGrapperAsync(int GrapperId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{GlobalVariable.baseUrl}/Grappers/delete/{GrapperId}");

                var temp = await response.Content.ReadAsStringAsync();
                UnityEngine.Debug.Log(temp);
                var result = JsonConvert.DeserializeObject<bool>(temp);
                return result;
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException("Failed to delete Grapper", ex); // Ném lỗi HTTP lên UseCase
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unexpected error during HTTP request", ex); // Bao bọc lỗi khác
            }
        }
        // private object ConvertGrapperRequestData(GrapperEntity GrapperEntity)
        // {
        //     return new
        //     {
        //         name = GrapperEntity.Name,
        //         Location = GrapperEntity.Location ?? "",
        //         ListDevices = GrapperEntity.DeviceEntities?
        //             .Where(d => d != null)
        //             .Select(d => new DeviceBasicDto(d.Id, d.Code))
        //             .ToList() ?? new List<DeviceBasicDto>(),
        //         ListModules = GrapperEntity.ModuleEntities
        //             .Where(m => m != null)
        //             .Select(m => new ModuleBasicDto(m.Id, m.Name))
        //             .ToList() ?? new List<ModuleBasicDto>(),
        //         OutdoorImage = GrapperEntity.OutdoorImageEntity != null
        //             ? new ImageBasicDto(GrapperEntity.OutdoorImageEntity.Id, GrapperEntity.OutdoorImageEntity.Name)
        //             : null,
        //         ListConnectionImages = GrapperEntity.ConnectionImageEntities?
        //             .Where(i => i != null)
        //             .Select(i => new ImageBasicDto(i.Id, i.Name))
        //             .ToList() ?? new List<ImageBasicDto>()
        //     };
        // }
    }


}