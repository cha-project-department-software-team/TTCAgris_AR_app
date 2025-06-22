using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationLayer.Dtos;
using ApplicationLayer.Dtos.Device;
using ApplicationLayer.Dtos.Image;
using ApplicationLayer.Dtos.JB;
using ApplicationLayer.Dtos.Module;
using Domain.Entities;
using Domain.Interfaces;

namespace ApplicationLayer.UseCases
{
    public class JBUseCase
    {
        private IJBRepository _IJBRepository;

        public JBUseCase(IJBRepository IjbRepository)
        {
            _IJBRepository = IjbRepository;
        }
        //! Select(), ToList(), ToDictionary() đề phải duyệt qua toàn bộ danh sách, duyệt tới đâu lưu tới đó 
        //! => dùng Foreach để chỉ quét 1 lần, tối ưu được thời gian xử lý
        public async Task<List<JBBasicDto>> GetListJBGeneralAsync(int grapperId)
        {
            try
            {
                var jBEntities = await _IJBRepository.GetListJBGeneralAsync(grapperId);
                if (jBEntities == null)
                {
                    throw new ApplicationException("Failed to get JB list");
                }

                int count = jBEntities.Count;
                var JBBasicDtos = new List<JBBasicDto>(count);
                var listJBInfo = new List<JBInformationModel>(count);
                var dictJBInfo = new Dictionary<string, JBInformationModel>(count);

                foreach (var JBEntity in jBEntities)
                {
                    var dto = MapToBasicDto(JBEntity);
                    var model = new JBInformationModel(dto.Id, dto.Name);

                    JBBasicDtos.Add(dto);
                    listJBInfo.Add(model);
                    dictJBInfo[dto.Name] = model;
                }

                GlobalVariable.temp_ListJBInformationModel = listJBInfo;
                GlobalVariable.temp_Dictionary_JBInformationModel = dictJBInfo;

                return JBBasicDtos;
            }
            catch (ArgumentException exception)
            {
                throw new ApplicationException("Failed to get JB list", exception); // Ném lại lỗi validation cho Unity xử lý
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to get JB list", ex); // Bao bọc lỗi từ Repository
            }
        }



        //! Dùng IEnumerable<JBGeneralDto thay cho List<JBGeneralDto> vì không cần tạo ra List để sử dụng trong hàm ngay
        public async Task<List<JBGeneralDto>> GetListJBInforAsync(int grapperId)
        {
            try
            {
                var jBEntities = await _IJBRepository.GetListJBInformationAsync(grapperId);
                if (jBEntities == null || !jBEntities.Any())
                {
                    // UnityEngine.Debug.LogWarning("No jBEntities found.");
                    throw new ApplicationException("Failed to get JB list");
                }
                return jBEntities.Select(jBEntity => MapToGeneralDto(jBEntity)).ToList();
            }
            catch (ArgumentException exception)
            {
                throw new ApplicationException("Failed to get JB list", exception); // Ném lại lỗi validation cho Unity xử lý
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to get JB list", ex); // Bao bọc lỗi từ Repository
            }
        }


        public async Task<JBResponseDto> GetJBByIdAsync(int JBId)
        {
            try
            {
                var jbEntity = await _IJBRepository.GetJBByIdAsync(JBId);

                if (jbEntity == null)
                {
                    throw new ApplicationException("Failed to get JB");
                }
                else
                {
                    var jbResponseDto = MapToResponseDto(jbEntity);
                    return jbResponseDto;
                }
            }
            catch (ArgumentException exception)
            {
                throw new ApplicationException("Failed to get JB", exception); // Ném lại lỗi validation cho Unity xử lý
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to get JB", ex); // Bao bọc lỗi từ Repository
            }
        }
        public async Task<bool> CreateNewJBAsync(int grapperId, JBRequestDto requestDto)
        {
            try
            {
                // Validate
                if (string.IsNullOrEmpty(requestDto.Name))
                {
                    throw new ArgumentException("name cannot be empty");
                }
                // Ánh xạ từ JBRequestDto sang JBEntity để check các nghiệp vụ
                var jbEntity = MapRequestToEntity(requestDto);

                // var requestData = MapToRequestDto(jbEntity);

                if (jbEntity == null)
                {
                    throw new ApplicationException("Failed to create JB cause jbEntity is Null");
                }

                else
                {
                    return await _IJBRepository.CreateNewJBAsync(grapperId, jbEntity);
                }

            }
            catch (ArgumentException)
            {
                throw new ApplicationException("Failed to create JB cause name is empty"); // Ném lại lỗi validation cho Unity xử lý
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to create JB", ex); // Bao bọc lỗi từ Repository
            }
        }
        public async Task<bool> UpdateJBAsync(int JBId, JBRequestDto requestDto)
        {
            try
            {
                // Validate
                if (string.IsNullOrEmpty(requestDto.Name))
                {
                    throw new ArgumentException("name cannot be empty");
                }
                // Ánh xạ từ JBRequestDto sang JBEntity để check các nghiệp vụ
                var jbEntity = MapRequestToEntity(requestDto);

                // var requestData = MapToRequestDto(jbEntity);

                if (jbEntity == null)
                {
                    throw new ApplicationException("Failed to update JB cause jbEntity is Null");
                }

                else
                {
                    return await _IJBRepository.UpdateJBAsync(JBId, jbEntity);
                }

            }
            catch (ArgumentException)
            {
                throw new ApplicationException("Failed to update JB cause name is empty"); // Ném lại lỗi validation cho Unity xử lý
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to update JB", ex); // Bao bọc lỗi từ Repository
            }
        }
        public async Task<bool> DeleteJBAsync(int JBId)
        {
            try
            {
                var deletedJBResult = await _IJBRepository.DeleteJBAsync(JBId);
                return deletedJBResult;
            }
            catch (ArgumentException)
            {
                throw new ApplicationException("Failed to delete JB cause name is empty"); // Ném lại lỗi validation cho Unity xử lý
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to delete JB", ex); // Bao bọc lỗi từ Repository
            }

        }


        //! Dto => Entity
        private JBEntity MapRequestToEntity(JBRequestDto jBRequestDto)
        {
            return new JBEntity(
             name: jBRequestDto.Name,
            location: string.IsNullOrEmpty(jBRequestDto.Location) ? "Được ghi chú trên sơ đồ" : jBRequestDto.Location,
            devices: jBRequestDto.DeviceBasicDtos.Any() ? jBRequestDto.DeviceBasicDtos.Select(d => new DeviceEntity(d.Id, d.Code)).ToList() : new List<DeviceEntity>(),
            modules: jBRequestDto.ModuleBasicDtos.Any() ? jBRequestDto.ModuleBasicDtos.Select(m => new ModuleEntity(m.Id, m.Name)).ToList() : new List<ModuleEntity>(),
            outdoorImage: jBRequestDto.OutdoorImageBasicDto != null ? new ImageEntity(jBRequestDto.OutdoorImageBasicDto.Id, jBRequestDto.OutdoorImageBasicDto.Name) : null,
            connectionImages: jBRequestDto.ConnectionImageBasicDtos.Any() ? jBRequestDto.ConnectionImageBasicDtos.Select(i => new ImageEntity(i.Id, i.Name)).ToList() : new List<ImageEntity>()
            );
        }


        //! Entity => Dto
        private JBResponseDto MapToResponseDto(JBEntity jBEntity)
        {
            return new JBResponseDto(
               id: jBEntity.Id,

              name: jBEntity.Name,

              location: string.IsNullOrEmpty(jBEntity.Location) ? "Được ghi chú trong sơ đồ" : jBEntity.Location,
                deviceBasicDtos: jBEntity.DeviceEntities.Any() ?
             jBEntity.DeviceEntities.Select(d => new DeviceBasicDto(d.Id, d.Code)).ToList() : new List<DeviceBasicDto>(),
                moduleBasicDtos: jBEntity.ModuleEntities.Any() ?
                    jBEntity.ModuleEntities.Select(m => new ModuleBasicDto(m.Id, m.Name)).ToList() : new List<ModuleBasicDto>(),
                outdoorImageBasicDto: jBEntity.OutdoorImageEntity != null ?
                        new ImageBasicDto(jBEntity.OutdoorImageEntity.Id, jBEntity.OutdoorImageEntity.Name) : null,
                connectionImageBasicDtos: jBEntity.ConnectionImageEntities.Any() ?
                     jBEntity.ConnectionImageEntities.Select(i => new ImageBasicDto(i.Id, i.Name)).ToList() : new List<ImageBasicDto>()
            );
        }
        private JBGeneralDto MapToGeneralDto(JBEntity jBEntity)
        {
            return new JBGeneralDto(
                id: jBEntity.Id,
              name: jBEntity.Name,
          location: string.IsNullOrEmpty(jBEntity.Location) ? "Được ghi chú trong sơ đồ" : jBEntity.Location,
           outdoorImageBasicDto: jBEntity.OutdoorImageEntity != null ?
                        new ImageBasicDto(jBEntity.OutdoorImageEntity.Id, jBEntity.OutdoorImageEntity.Name) : null,
           connectionImageBasicDtos: jBEntity.ConnectionImageEntities.Any() ?
            jBEntity.ConnectionImageEntities.Select(i => new ImageBasicDto(i.Id, i.Name)).ToList() : new List<ImageBasicDto>()
            );
        }
        private JBBasicDto MapToBasicDto(JBEntity jBEntity)
        {
            return new JBBasicDto(
             id: jBEntity.Id,

           name: jBEntity.Name
                   );
        }
        // private JBRequestDto MapToRequestDto(JBEntity jBEntity)
        // {
        //     return new JBRequestDto(
        //         jBEntity.Name,
        //         jBEntity.Location ?? "chưa cập nhật",
        //         jBEntity.DeviceEntities.Select(d => new DeviceBasicDto(d.Id, d.Code)).ToList(),
        //         jBEntity.ModuleEntities.Select(m => new ModuleBasicDto(m.Id, m.Name)).ToList(),
        //         jBEntity.OutdoorImageEntity != null ? new ImageBasicDto(jBEntity.OutdoorImageEntity.Id, jBEntity.OutdoorImageEntity.Name) : null!,
        //         jBEntity.ConnectionImageEntities.Select(i => new ImageBasicDto(i.Id, i.Name)).ToList()
        //     );
        // }


    }
}