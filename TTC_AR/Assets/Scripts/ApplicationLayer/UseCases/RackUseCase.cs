using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationLayer.Dtos;
using ApplicationLayer.Dtos.Device;
using ApplicationLayer.Dtos.Image;
using ApplicationLayer.Dtos.Rack;
using ApplicationLayer.Dtos.Module;
using Domain.Entities;
using Domain.Interfaces;

namespace ApplicationLayer.UseCases
{
    public class RackUseCase
    {
        private readonly IRackRepository _IRackRepository;

        public RackUseCase(IRackRepository IRackRepository)
        {
            _IRackRepository = IRackRepository;
        }

        public async Task<List<RackBasicDto>> GetListRackAsync(int grapperId)
        {
            try
            {
                // var RackEntities = await _IRackRepository.GetListRackAsync(grapperId) ??
                //     throw new ApplicationException("Failed to get Rack list");
                // return RackEntities.Select(RackEntity => MapToBasicDto(RackEntity)).ToList();

                var RackEntities = await _IRackRepository.GetListRackAsync(grapperId) ??

                throw new ApplicationException("Failed to get Rack list");

                int count = RackEntities.Count;

                var RackBasicDtos = new List<RackBasicDto>(count);

                // var listRackInfo = new List<RackInformationModel>(count);

                // var dictRackInfo = new Dictionary<string, RackInformationModel>(count);

                foreach (var RackEntity in RackEntities)
                {
                    var dto = MapToBasicDto(RackEntity);
                    // var model = new RackInformationModel(dto.Id, dto.Name);

                    RackBasicDtos.Add(dto);
                    // listRackInfo.Add(model);
                    // dictRackInfo[dto.Name] = model;
                }

                // GlobalVariable.temp_ListRackInformationModel = listRackInfo;
                // GlobalVariable.temp_Dictionary_RackInformationModel = dictRackInfo;

                return RackBasicDtos;


            }
            catch (ArgumentException)
            {
                throw new ApplicationException("Failed to get Rack list"); // Ném lại lỗi validation cho Unity xử lý
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to get Rack list", ex); // Bao bọc lỗi từ Repository
            }

        }

        public async Task<RackResponseDto> GetRackByIdAsync(int rackId)
        {
            try
            {
                var RackEntity = await _IRackRepository.GetRackByIdAsync(rackId) ??
                    throw new ApplicationException("Failed to get Rack");

                return MapToResponseDto(RackEntity);
            }
            catch (ArgumentException)
            {
                throw new ApplicationException("Failed to get Rack"); // Ném lại lỗi validation cho Unity xử lý
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to get Rack", ex); // Bao bọc lỗi từ Repository
            }

        }

        public async Task<bool> CreateNewRackAsync(int grapperId, RackRequestDto requestDto)
        {
            try
            {
                // Validate
                if (string.IsNullOrEmpty(requestDto.Name))
                {
                    throw new ArgumentException("name cannot be empty");
                }

                var RackEntity = MapRequestToEntity(requestDto) ??
                    throw new ApplicationException("Failed to create Rack cause RackEntity is Null");


                return await _IRackRepository.CreateNewRackAsync(grapperId, RackEntity);
            }
            catch (ArgumentException)
            {
                throw new ApplicationException("Failed to create Rack"); // Ném lại lỗi validation cho Unity xử lý
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to create Rack", ex); // Bao bọc lỗi từ Repository
            }

        }

        public async Task<bool> UpdateRackAsync(int rackId, RackRequestDto requestDto)
        {
            try
            {
                // Validate
                if (string.IsNullOrEmpty(requestDto.Name))
                {
                    throw new ArgumentException("name cannot be empty");
                }

                var RackEntity = MapRequestToEntity(requestDto) ??
                    throw new ApplicationException("Failed to update Rack cause RackEntity is Null");

                return await _IRackRepository.UpdateRackAsync(rackId, RackEntity);
            }
            catch (ArgumentException)
            {
                throw new ApplicationException("Failed to update Rack"); // Ném lại lỗi validation cho Unity xử lý
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to update Rack", ex); // Bao bọc lỗi từ Repository
            }
        }

        public async Task<bool> DeleteRackAsync(int rackId)
        {
            try
            {
                var deletedRackResult = await _IRackRepository.DeleteRackAsync(rackId);
                return deletedRackResult;
            }
            catch (ArgumentException)
            {
                throw new ApplicationException("Failed to delete Rack"); // Ném lại lỗi validation cho Unity xử lý
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to delete Rack", ex); // Bao bọc lỗi từ Repository
            }
        }


        //! Dto => Entity
        private RackEntity MapRequestToEntity(RackRequestDto RackRequestDto)
        {
            return new RackEntity(
                name: RackRequestDto.Name,
                moduleEntities: RackRequestDto.ModuleBasicDtos.Any()
                    ? RackRequestDto.ModuleBasicDtos.Select(module => new ModuleEntity(module.Id, module.Name)).ToList()
                    : new List<ModuleEntity>()
            );
        }
        //! Entity => Dto
        private RackResponseDto MapToResponseDto(RackEntity rackEntity)
        {

            return new RackResponseDto(
                id: rackEntity.Id,
                name: rackEntity.Name,
                moduleBasicDtos: rackEntity.ModuleEntities.Any()
                    ? rackEntity.ModuleEntities.Select(m => new ModuleBasicDto(m.Id, m.Name)).ToList()
                    : new List<ModuleBasicDto>()
            );
        }


        private RackBasicDto MapToBasicDto(RackEntity RackEntity)
        {
            return new RackBasicDto(
                id: RackEntity.Id,
                name: RackEntity.Name
            );
        }
    }
}
