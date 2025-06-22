using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ApplicationLayer.Dtos;
using ApplicationLayer.Dtos.AdapterSpecification;
using ApplicationLayer.Dtos.Device;
using ApplicationLayer.Dtos.Grapper;
using ApplicationLayer.Dtos.JB;
using ApplicationLayer.Dtos.Module;
using ApplicationLayer.Dtos.ModuleSpecification;
using ApplicationLayer.Dtos.Rack;
using Domain.Entities;
using Domain.Interfaces;
using Unity.VisualScripting;

namespace ApplicationLayer.UseCases
{
    public class ModuleUseCase
    {
        private IModuleRepository _IModuleRepository;

        public ModuleUseCase(IModuleRepository IModuleRepository)
        {
            _IModuleRepository = IModuleRepository;
        }
        public async Task<List<ModuleBasicDto>> GetListModuleAsync(int grapperId)
        {
            try
            {
                var moduleEntities = await _IModuleRepository.GetListModuleAsync(grapperId);

                if (moduleEntities == null)
                {
                    throw new ApplicationException("Failed to get Module list");
                }
                else
                {
                    int count = moduleEntities.Count;
                    // var listModuleInfo = new List<ModuleInformationModel>(count);
                    // var dictModuleInfo = new Dictionary<string, ModuleInformationModel>(count);
                    var moduleBasicDtos = new List<ModuleBasicDto>(count);
                    foreach (var moduleEntity in moduleEntities)
                    {
                        var dto = MapEntityToBasicDto(moduleEntity);
                        // var model = new ModuleInformationModel(dto.Id, dto.Name);
                        moduleBasicDtos.Add(dto);
                        // listModuleInfo.Add(model);
                        // dictModuleInfo[dto.Name] = model;
                    }

                    // GlobalVariable.temp_ListModuleInformationModel = listModuleInfo;
                    // GlobalVariable.temp_Dictionary_ModuleInformationModel = dictModuleInfo;
                    return moduleBasicDtos;
                }

            }
            catch (ArgumentException exception)
            {
                throw new ApplicationException("Failed to get Module list", exception); // Ném lại lỗi validation cho Unity xử lý
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to get Module list", ex); // Bao bọc lỗi từ Repository
            }

        }

        public async Task<ModuleResponseDto> GetModuleByIdAsync(int moduleId)
        {
            try
            {
                var moduleEntity = await _IModuleRepository.GetModuleByIdAsync(moduleId) ??
                    throw new ApplicationException("Failed to get Module");

                return MapEntityToResponseDto(moduleEntity);

            }
            catch (ArgumentException exception)
            {
                throw new ApplicationException("Failed to get Module", exception); // Ném lại lỗi validation cho Unity xử lý
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to get Module", ex); // Bao bọc lỗi từ Repository
            }

        }

        public async Task<bool> CreateNewModuleAsync(int grapperId, ModuleRequestDto requestDto)
        {
            try
            {
                //UnityEngine.Debug.Log("Run UseCase");
                // Ánh xạ từ ModuleRequestDto sang ModuleEntity để check các nghiệp vụ
                var ModuleEntity = MapRequestToModuleEntity(requestDto);

                //UnityEngine.Debug.Log("UseCase Send to Repository");

                if (ModuleEntity == null)
                {
                    throw new ApplicationException("Failed to create Module cause ModuleEntity is Null");
                }
                else
                {
                    var createdModuleResult = await _IModuleRepository.CreateNewModuleAsync(grapperId, ModuleEntity);

                    if (createdModuleResult == false)
                    {
                        throw new ApplicationException("Failed to update Module");
                    }
                    else return true;
                }

            }
            catch (ArgumentException)
            {
                throw new ApplicationException("Failed to create Module cause ArgumentException"); // Ném lại lỗi validation cho Unity xử lý
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to create Module", ex); // Bao bọc lỗi từ Repository
            }
        }

        public async Task<bool> UpdateModuleAsync(int moduleId, ModuleRequestDto requestDto)
        {
            try
            {
                // Ánh xạ từ ModuleRequestDto sang ModuleEntity để check các nghiệp vụ
                //UnityEngine.Debug.Log("Run UseCase");
                var ModuleEntity = MapRequestToModuleEntity(requestDto);
                //UnityEngine.Debug.Log("Convert to Entity Successfully");
                if (ModuleEntity == null)
                {
                    throw new ApplicationException("Failed to create Module cause ModuleEntity is Null");
                }
                else
                {
                    var updatedModuleResult = await _IModuleRepository.UpdateModuleAsync(moduleId, ModuleEntity);

                    if (updatedModuleResult == false)
                    {
                        throw new ApplicationException("Failed to update Module");
                    }
                    else return true;
                }
            }
            catch (ArgumentException)
            {
                throw new ApplicationException("Failed to update Module cause ArgumentException"); // Ném lại lỗi validation cho Unity xử lý
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to update Module", ex); // Bao bọc lỗi từ Repository
            }
        }

        public async Task<bool> DeleteModuleAsync(int moduleId)
        {
            try
            {
                var deletedModuleResult = await _IModuleRepository.DeleteModuleAsync(moduleId);
                return deletedModuleResult;
            }
            catch (ArgumentException)
            {
                throw new ApplicationException("Failed to delete Module cause ArgumentException"); // Ném lại lỗi validation cho Unity xử lý
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to delete Module", ex); // Bao bọc lỗi từ Repository
            }


        }

        //! Entity => Dto
        private ModuleBasicDto MapEntityToBasicDto(ModuleEntity moduleEntity)
        {
            return new ModuleBasicDto(moduleEntity.Id, moduleEntity.Name);
        }
        private ModuleRequestDto MapEntityToRequestDto(ModuleEntity moduleEntity)
        {
            var deviceEntities = moduleEntity.DeviceEntities ?? Enumerable.Empty<DeviceEntity>();
            var jbEntities = moduleEntity.JBEntities ?? Enumerable.Empty<JBEntity>();

            return new ModuleRequestDto(
          name: moduleEntity.Name,
          rackBasicDto: new RackBasicDto(moduleEntity.RackEntity.Id, moduleEntity.RackEntity.Name),
          deviceBasicDtos: deviceEntities.Any()
          ? moduleEntity.DeviceEntities.Select(d => new DeviceBasicDto(d.Id, d.Code)).ToList()
          : new List<DeviceBasicDto>(),
           jBBasicDtos: jbEntities.Any()
           ? moduleEntity.JBEntities.Select(j => new JBBasicDto(j.Id, j.Name)).ToList()
           : new List<JBBasicDto>(),
           moduleSpecificationBasicDto: new ModuleSpecificationBasicDto(moduleEntity.ModuleSpecificationEntity.Id, moduleEntity.ModuleSpecificationEntity.Code),
           adapterSpecificationBasicDto: new AdapterSpecificationBasicDto(moduleEntity.AdapterSpecificationEntity.Id, moduleEntity.AdapterSpecificationEntity.Code)
            );
        }

        private ModuleResponseDto MapEntityToResponseDto(ModuleEntity moduleEntity)
        {
            var jbEntities = moduleEntity.JBEntities.Any() ? moduleEntity.JBEntities : new List<JBEntity>();
            var deviceEntities = moduleEntity.DeviceEntities.Any() ? moduleEntity.DeviceEntities : new List<DeviceEntity>();
            return new ModuleResponseDto(
                id: moduleEntity.Id,
                name: moduleEntity.Name,
                grapperBasicDto: moduleEntity.GrapperEntity != null ? new GrapperBasicDto(moduleEntity.GrapperEntity.Id, moduleEntity.GrapperEntity.Name) : null,
                rackBasicDto: moduleEntity.RackEntity != null ? new RackBasicDto(moduleEntity.RackEntity.Id, moduleEntity.RackEntity.Name) : null,
                deviceBasicDtos: deviceEntities.Any()
                    ? new List<DeviceBasicDto>(deviceEntities.Select(d => new DeviceBasicDto(d.Id, d.Code)))
                    : new List<DeviceBasicDto>(),
                jbBasicDtos: jbEntities.Any()
                    ? new List<JBBasicDto>(jbEntities.Select(j => new JBBasicDto(j.Id, j.Name)))
                    : new List<JBBasicDto>(),
                moduleSpecificationBasicDto: moduleEntity.ModuleSpecificationEntity != null ? MapToEntityToModuleSpecificationBasicDto(moduleEntity.ModuleSpecificationEntity) : null,
                adapterSpecificationBasicDto: moduleEntity.AdapterSpecificationEntity != null ? MapToEntityToAdapterSpecificationBasicDto(moduleEntity.AdapterSpecificationEntity) : null
            );
        }

        private ModuleSpecificationBasicDto MapToEntityToModuleSpecificationBasicDto(ModuleSpecificationEntity moduleSpecificationEntity)
        {
            return new ModuleSpecificationBasicDto(
                id: moduleSpecificationEntity.Id,
                code: moduleSpecificationEntity.Code
            );
        }
        private AdapterSpecificationBasicDto MapToEntityToAdapterSpecificationBasicDto(AdapterSpecificationEntity adapterSpecificationEntity)
        {
            return new AdapterSpecificationBasicDto(
                   id: adapterSpecificationEntity.Id,
                    code: adapterSpecificationEntity.Code

                    );
        }



        //! Dto => Entity
        private ModuleEntity MapResponseToModuleEntity(ModuleResponseDto moduleResponseDto)
        {
            return new ModuleEntity
            (moduleResponseDto.Id,
                moduleResponseDto.Name
            )
            {
                RackEntity = new RackEntity(moduleResponseDto.RackBasicDto.Id, moduleResponseDto.RackBasicDto.Name),
                DeviceEntities = moduleResponseDto.DeviceBasicDtos.Select(d => new DeviceEntity(d.Id, d.Code)).ToList(),
                JBEntities = moduleResponseDto.JBBasicDtos.Select(j => new JBEntity(j.Id, j.Name)).ToList(),
                ModuleSpecificationEntity = MapToModuleSpecificationResponseEntity(moduleResponseDto),
                AdapterSpecificationEntity = MapToAdapterSpecificationResponseEntity(moduleResponseDto)
            };
        }

        private ModuleSpecificationEntity MapToModuleSpecificationResponseEntity(ModuleResponseDto moduleResponseDto)
        {
            return new ModuleSpecificationEntity(
                    moduleResponseDto.ModuleSpecificationBasicDto.Id,
                    moduleResponseDto.ModuleSpecificationBasicDto.Code
                    );
        }

        private AdapterSpecificationEntity MapToAdapterSpecificationResponseEntity(ModuleResponseDto moduleResponseDto)
        {
            return new AdapterSpecificationEntity(
                    moduleResponseDto.AdapterSpecificationBasicDto.Id,
                    moduleResponseDto.AdapterSpecificationBasicDto.Code
                    );
        }
        private ModuleEntity MapRequestToModuleEntity(ModuleRequestDto moduleRequestDto)
        {
            return new ModuleEntity
            (
              name: moduleRequestDto.Name,
              rack: moduleRequestDto.RackBasicDto != null ? new RackEntity(
                id: moduleRequestDto.RackBasicDto.Id,
                name: moduleRequestDto.RackBasicDto.Name) : null,
              deviceEntities: moduleRequestDto.DeviceBasicDtos.Any() ? moduleRequestDto.DeviceBasicDtos.Select(d => new DeviceEntity(d.Id, d.Code)).ToList() : new List<DeviceEntity>(),
              jbEntities: moduleRequestDto.JBBasicDtos.Any() ? moduleRequestDto.JBBasicDtos.Select(j => new JBEntity(j.Id, j.Name)).ToList() : new List<JBEntity>(),
              moduleSpecificationEntity: moduleRequestDto.ModuleSpecificationBasicDto != null ? new ModuleSpecificationEntity(moduleRequestDto.ModuleSpecificationBasicDto.Id, moduleRequestDto.ModuleSpecificationBasicDto.Code) : null,
              adapterSpecificationEntity: moduleRequestDto.AdapterSpecificationBasicDto != null ? new AdapterSpecificationEntity(moduleRequestDto.AdapterSpecificationBasicDto.Id, moduleRequestDto.AdapterSpecificationBasicDto.Code) : null);
        }
        private ModuleEntity MapBasicToModuleEntity(ModuleBasicDto moduleGeneralDto)
        {
            return new ModuleEntity
            (moduleGeneralDto.Id,
                moduleGeneralDto.Name
            );
        }

    }
}