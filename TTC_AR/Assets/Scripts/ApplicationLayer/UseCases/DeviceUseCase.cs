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
    public class DeviceUseCase
    {
        private IDeviceRepository _IDeviceRepository;

        public DeviceUseCase(IDeviceRepository deviceEntityRepository)
        {
            _IDeviceRepository = deviceEntityRepository;
        }



        //! Get List Device General
        public async Task<List<DeviceBasicDto>> GetListDeviceGeneralAsync(int grapperId)
        {
            // try
            // {
            // //! Gọi _IDeviceRepository từ Infrastructure Layer
            // var deviceEntities = await _IDeviceRepository.GetListDeviceGeneralAsync(grapperId)
            // ?? throw new ApplicationException("Failed to get Device list");
            // return deviceEntities.Select(MapToBasicDto).ToList();

            try
            {
                var DeviceEntities = await _IDeviceRepository.GetListDeviceGeneralAsync(grapperId) ??

                throw new ApplicationException("Failed to get Device list");

                int count = DeviceEntities.Count;

                var DeviceBasicDtos = new List<DeviceBasicDto>(count);

                var listDeviceInfo = new List<DeviceInformationModel>(count);

                var dictDeviceInfo = new Dictionary<string, DeviceInformationModel>(count);

                foreach (var DeviceEntity in DeviceEntities)
                {
                    var dto = MapToBasicDto(DeviceEntity);
                    var model = new DeviceInformationModel(dto.Id, dto.Code);

                    DeviceBasicDtos.Add(dto);
                    listDeviceInfo.Add(model);
                    dictDeviceInfo[dto.Code] = model;
                }

                GlobalVariable.temp_ListDeviceInformationModel = listDeviceInfo;
                GlobalVariable.temp_Dictionary_DeviceInformationModel = dictDeviceInfo;

                return DeviceBasicDtos;
            }
            // }
            catch (ArgumentException)
            {
                throw new ApplicationException("Failed to get Device list"); // Ném lại lỗi validation cho Unity xử lý
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to get Device list", ex); // Bao bọc lỗi từ Repository
            }
        }


        //! Get List Device Information từ Grapper
        public async Task<List<DeviceResponseDto>> GetListDeviceInformationFromGrapperAsync(int grapperId)
        {
            try
            {
                //! Gọi _IDeviceRepository từ Infrastructure Layer

                var deviceEntities = await _IDeviceRepository.GetListDeviceInformationFromGrapperAsync(grapperId);

                List<DeviceResponseDto> deviceResponseDtos =
                    deviceEntities.Select(MapToResponseDto).ToList();

                return deviceResponseDtos;
            }
            catch (ArgumentException ex)
            {
                // UnityEngine.Debug.Log("Error: " + ex.Message);
                throw new ApplicationException("Failed to get Device list", ex); // Bao bọc lỗi từ Repository
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to get Device list", ex); // Bao bọc lỗi từ Repository
            }
        }


        //! Get List Device Information từ Module
        public async Task<List<DeviceBasicDto>> GetListDeviceInformationFromModuleAsync(int moduleId)
        {
            try
            {
                //! Gọi _IDeviceRepository từ Infrastructure Layer
                var deviceEntities = await _IDeviceRepository.GetListDeviceInformationFromModuleAsync(moduleId)
                ?? throw new ApplicationException("Failed to get Device list"); ;
                //! Đưa về Entity để xử lý logic nghiệp vụ 
                return deviceEntities.Select(MapToBasicDto).ToList();
            }
            catch (ArgumentException)
            {
                throw new ApplicationException("Failed to get Device list"); // Ném lại lỗi validation cho Unity xử lý
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to get Device list", ex); // Bao bọc lỗi từ Repository
            }
        }

        public async Task<DeviceResponseDto> GetDeviceByIdAsync(int deviceId)
        {
            try
            {
                // UnityEngine.Debug.Log("Run UseCase");

                var deviceEntity = await _IDeviceRepository.GetDeviceByIdAsync(deviceId);
                // UnityEngine.Debug.Log("DeviceEntity: " + deviceEntity.Code);
                if (deviceEntity == null)
                {
                    // UnityEngine.Debug.Log("Entity null value");
                    throw new ApplicationException("Failed to get Device");
                }
                else
                {
                    //! Đưa về Entity để xử lý logic nghiệp vụ
                    // UnityEngine.Debug.Log("DeviceEntity: " + deviceEntity.Id);
                    // var deviceEntity = MapResponseToEntity(deviceEntity);
                    var deviceResponseDto = MapToResponseDto(deviceEntity);
                    // UnityEngine.Debug.Log("DeviceResponseDto: " + deviceResponseDto.Code);
                    return deviceResponseDto;
                }
            }
            catch (ArgumentException)
            {
                throw new ApplicationException("Failed to get Device"); // Ném lại lỗi validation cho Unity xử lý
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to get Device", ex); // Bao bọc lỗi từ Repository
            }
        }
        public async Task<bool> CreateNewDeviceAsync(int grapperId, DeviceRequestDto requestDto)
        {
            try
            {
                // Validate
                if (string.IsNullOrEmpty(requestDto.Code))
                    throw new ArgumentException("name cannot be empty");
                // Map DTO to Entity
                var deviceEntity = MapRequestToEntity(requestDto);
                //!  var requestData = MapToRequestDto(deviceEntity);
                // var deviceEntity = new DeviceEntity(requestDto.Code)
                // {
                //     Code = requestDto.Code,
                //     Function = requestDto.Function,
                //     Range = requestDto.Range,
                //     Unit = requestDto.Unit,
                //     IOAddress = requestDto.IOAddress,
                //     JBEntity = new JBEntity(requestDto.JBBasicDto.Id, requestDto.JBBasicDto.Name),
                //     ModuleEntity = new ModuleEntity(requestDto.ModuleBasicDto.Id, requestDto.ModuleBasicDto.Name),
                //     AdditionalConnectionImageEntities = requestDto.AdditionalImageBasicDtos.Select(dto => new ImageEntity(dto.Id, dto.Name)).ToList()
                // };

                //! Cứ đưa vào Entity, khi sang Repository, tùy vào ngữ cảnh sẽ filter lại dữ liệu để gửi lên server
                //! Tham số mặc định của Repository sẽ là Entity
                var createdDeviceResult = await _IDeviceRepository.CreateNewDeviceAsync(grapperId, deviceEntity);

                if (!createdDeviceResult)
                {
                    throw new ApplicationException("Failed to create Device");
                }
                return true;
            }
            catch (ArgumentException)
            {
                throw new ApplicationException("Failed to create Device"); // Ném lại lỗi validation cho Unity xử lý
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to create Device", ex); // Bao bọc lỗi từ Repository
            }
        }

        public async Task<bool> UpdateDeviceAsync(int deviceId, DeviceRequestDto requestDto)
        {
            try
            {
                // Validate
                if (string.IsNullOrEmpty(requestDto.Code))
                    throw new ArgumentException("name cannot be empty");
                // Map DTO to Entity
                var deviceEntity = MapRequestToEntity(requestDto);
                UnityEngine.Debug.Log("DeviceId From UseCase: " + deviceEntity.Id);
                // var requestData = MapToRequestDto(deviceEntity);
                // Map DTO to Entity
                // var deviceEntity = new DeviceEntity(requestDto.Code)
                // {
                //     Code = requestDto.Code,
                //     Function = requestDto.Function,
                //     Range = requestDto.Range,
                //     Unit = requestDto.Unit,
                //     IOAddress = requestDto.IOAddress,
                //     JBEntity = new JBEntity(requestDto.JBBasicDto.Id, requestDto.JBBasicDto.Name),
                //     ModuleEntity = new ModuleEntity(requestDto.ModuleBasicDto.Id, requestDto.ModuleBasicDto.Name),
                //     AdditionalConnectionImageEntities = requestDto.AdditionalImageBasicDtos.Select(dto => new ImageEntity(dto.Id, dto.Name)).ToList()
                // };
                //! Cứ đưa vào Entity, khi sang Repository, tùy vào ngữ cảnh sẽ filter lại dữ liệu để gửi lên server
                //!Tham số mặc định của Repository sẽ là Entity
                var updatedDeviceResult = await _IDeviceRepository.UpdateDeviceAsync(deviceId, deviceEntity);
                if (!updatedDeviceResult)
                {
                    UnityEngine.Debug.Log("Update Device failed");
                    throw new ApplicationException("Failed to update Device");
                }
                return true;
            }
            catch (ArgumentException)
            {

                throw new ApplicationException("Failed to update Device"); // Ném lại lỗi validation cho Unity xử lý
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log("Error: " + ex.Message);
                throw new ApplicationException("Failed to update Device", ex); // Bao bọc lỗi từ Repository
            }
        }

        public async Task<bool> DeleteDeviceAsync(int deviceId)
        {
            try
            {
                var deletedDeviceResult = await _IDeviceRepository.DeleteDeviceAsync(deviceId);
                return deletedDeviceResult;
            }
            catch (ArgumentException)
            {
                throw new ApplicationException("Failed to delete Device"); // Ném lại lỗi validation cho Unity xử lý
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to delete Device", ex); // Bao bọc lỗi từ Repository
            }

        }

        //! Dto => Entity
        private DeviceEntity MapRequestToEntity(DeviceRequestDto requestDto)
        {
            return new DeviceEntity(
              code: requestDto.Code,
              function: requestDto.Function,
             range: requestDto.Range,
            unit: requestDto.Unit,
            ioAddress: requestDto.IOAddress,
            moduleEntity: requestDto.ModuleBasicDto != null ? new ModuleEntity(requestDto.ModuleBasicDto.Id, requestDto.ModuleBasicDto.Name) : null,
            jbEntities: requestDto.JBBasicDtos.Any() ? requestDto.JBBasicDtos.Select(jbEntity => new JBEntity(jbEntity.Id, jbEntity.Name)).ToList() : new List<JBEntity>(),
            additionalConnectionImageEntities: requestDto.AdditionalImageBasicDtos.Any() ? requestDto.AdditionalImageBasicDtos.Select(dto => new ImageEntity(dto.Id, dto.Name)).ToList() : new List<ImageEntity>()
            );
        }
        // private DeviceEntity MapResponseToEntity(DeviceResponseDto responseDto)
        // {
        //     return new DeviceEntity(
        //         responseDto.Id,
        //         responseDto.Code,
        //         responseDto.Function,
        //         responseDto.Range,
        //         responseDto.Unit,
        //         responseDto.IOAddress,
        //         new ModuleEntity(responseDto.ModuleBasicDto.Id, responseDto.ModuleBasicDto.Name),
        //         new JBEntity(responseDto.JBGeneralDto.Id, responseDto.JBGeneralDto.Name, responseDto.JBGeneralDto.Location,
        //         new ImageEntity(responseDto.JBGeneralDto.OutdoorImageBasicDto.Id, responseDto.JBGeneralDto.OutdoorImageBasicDto.Name, responseDto.JBGeneralDto.OutdoorImageBasicDto.Url),
        //         responseDto.JBGeneralDto.ConnectionImageBasicDtos.Select(
        //             ImageBasicDto => new ImageEntity(ImageBasicDto.Id, ImageBasicDto.Name, ImageBasicDto.Url)).ToList()
        //         ),
        //         responseDto.AdditionalImageBasicDtos.Select(
        //             ImageBasicDto => new ImageEntity(ImageBasicDto.Id, ImageBasicDto.Name, ImageBasicDto.Url)).ToList()
        //     );
        // }

        //! Entity => Dto

        private DeviceResponseDto MapToResponseDto(DeviceEntity deviceEntity)
        {
            return new DeviceResponseDto(
                id: deviceEntity.Id,
                code: deviceEntity.Code,
                function: deviceEntity.Function ?? "Chưa cập nhật",
                range: deviceEntity.Range ?? "Chưa cập nhật",
                unit: deviceEntity.Unit ?? "Chưa cập nhật",
                ioAddress: deviceEntity.IOAddress ?? "Chưa cập nhật",
                moduleBasicDto: deviceEntity.ModuleEntity != null ? new ModuleBasicDto(
                    id: deviceEntity.ModuleEntity.Id,
                    name: deviceEntity.ModuleEntity.Name) : null,
                jbBasicDtos: (deviceEntity.JBEntities != null && deviceEntity.JBEntities.Any())
                ? deviceEntity.JBEntities.Select(jb => new JBBasicDto(
                       id: jb.Id,
                       name: jb.Name
                   )).ToList() : new List<JBBasicDto>(),
                additionalImageBasicDtos: deviceEntity.AdditionalConnectionImageEntities.Any() ? deviceEntity.AdditionalConnectionImageEntities.Select(imageEntity => new ImageBasicDto(
                        id: imageEntity.Id,
                        name: imageEntity.Name
                      )).ToList() : new List<ImageBasicDto>()
            );
        }
        private DeviceBasicDto MapToBasicDto(DeviceEntity deviceEntity)
        {
            return new DeviceBasicDto(
                id: deviceEntity.Id,
                code: deviceEntity.Code
            );
        }
    }
}