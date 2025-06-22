
using System.Linq;
using System.Threading.Tasks;
using ApplicationLayer.Dtos.Mcc;
using Domain.Entities;
using ApplicationLayer.Dtos.Image;
using System.Collections.Generic;
using ApplicationLayer.Dtos.FieldDevice;
using Domain.Interfaces;
using System;

namespace ApplicationLayer.UseCases
{
    public class MccUseCase
    {
        private readonly IMccRepository _MccRepository;

        public MccUseCase(IMccRepository MccRepository)
        {
            _MccRepository = MccRepository;
        }

        public async Task<List<MccBasicDto>> GetListMccAsync(int grapperId)
        {
            try
            {

                var entities = await _MccRepository.GetListMccAsync(grapperId)
                               ?? throw new ApplicationException("Failed to load List MccBasicDtos");
                return entities.Select(entity => MapToBasicDto(entity)).ToList();
            }
            catch (ArgumentException exception)
            {
                throw new ApplicationException("Failed to load List MccBasicDtos", exception); // Ném lại lỗi validation cho Unity xử lý
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to load List MccBasicDtos", ex); // Bao bọc lỗi từ Repository
            }
        }



        public async Task<MccResponseDto> GetMccByIdAsync(int mccId)
        {
            try
            {
                var entity = await _MccRepository.GetMccByIdAsync(mccId) ?? throw new ApplicationException("Failed to load MccResponseDto"); ;
                return MapToResponseDto(entity);
            }
            catch (ArgumentException exception)
            {
                throw new ApplicationException("Failed to load MccResponseDto", exception); // Ném lại lỗi validation cho Unity xử lý
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to load MccResponseDto", ex); // Bao bọc lỗi từ Repository
            }
        }

        public async Task<bool> CreateNewMccAsync(int grapperId, MccRequestDto requestDto)
        {
            try
            {
                //! Không truyền List<MccEntity> MccEntities vào MccEntity để cho giá trị bị null
                //! Khi đó MccEntity chỉ có Id và CabinetCode

                var MccEntity = MapRequestToEntity(requestDto);

                var createdEntity = await _MccRepository.CreateNewMccAsync(grapperId, MccEntity);

                return createdEntity;
                // return new MccResponseDto(
                //     createdEntity.Id,
                //     createdEntity.Name,
                //     new MccBasicDto(createdEntity.Mcc.Id, createdEntity.Mcc.CabinetCode),
                //     createdEntity.RatedPower,
                //     createdEntity.RatedCurrent,
                //     createdEntity.ActiveCurrent,
                //     createdEntity.ConnectionImages.Select(img => new ImageBasicDto(img.Id, img.Name, img.Url)).ToList(),
                //     createdEntity.Note
                // );
            }
            catch (ArgumentException exception)
            {
                throw new ApplicationException("Failed to create Mcc", exception); // Ném lại lỗi validation cho Unity xử lý
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to create Mcc", ex); // Bao bọc lỗi từ Repository

            }
        }

        public async Task<bool> UpdateMccAsync(int mccId, MccRequestDto requestDto)
        {
            try
            {

                var MccEntity = MapRequestToEntity(requestDto);

                var updatedEntity = await _MccRepository.UpdateMccAsync(mccId, MccEntity);

                return updatedEntity;

                // return new MccResponseDto(
                //     updatedEntity.Id,
                //     updatedEntity.Name,
                //     new MccBasicDto(updatedEntity.Mcc.Id, updatedEntity.Mcc.CabinetCode),
                //     updatedEntity.RatedPower,
                //     updatedEntity.RatedCurrent,
                //     updatedEntity.ActiveCurrent,
                //     updatedEntity.ConnectionImages.Select(img => new ImageBasicDto(img.Id, img.Name, img.Url)).ToList(),
                //     updatedEntity.Note
                // );
            }
            catch (ArgumentException exception)
            {
                throw new ApplicationException("Failed to update Mcc", exception); // Ném lại lỗi validation cho Unity xử lý
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to update Mcc", ex); // Bao bọc lỗi từ Repository
            }


        }

        public async Task<bool> DeleteMccAsync(int mccId)
        {
            // MccId = GlobalVariable.MccId;
            try
            {
                var deletedEntity = await _MccRepository.DeleteMccAsync(mccId);

                return deletedEntity;
            }
            catch (ArgumentException exception)
            {
                throw new ApplicationException("Failed to delete Mcc", exception); // Ném lại lỗi validation cho Unity xử lý
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to delete Mcc", ex); // Bao bọc lỗi từ Repository
            }
        }

        //! Dto => Entity
        private MccEntity MapRequestToEntity(MccRequestDto MccRequestDto)
        {
            return new MccEntity(
            cabinetCode: MccRequestDto.CabinetCode,

            fieldDeviceEntities: (MccRequestDto.FieldDeviceBasicDtos == null || (MccRequestDto.FieldDeviceBasicDtos != null && MccRequestDto.FieldDeviceBasicDtos.Count <= 0)) ?
            new List<FieldDeviceEntity>()
            : MccRequestDto.FieldDeviceBasicDtos.Select(fd => new FieldDeviceEntity(fd.Id, fd.Name)).ToList(),

            brand: MccRequestDto.Brand,

            note: MccRequestDto.Note
            );
        }
        //! Entity => Dto
        private MccBasicDto MapToBasicDto(MccEntity entity)
        {
            return new MccBasicDto(
                entity.Id,
                entity.CabinetCode
            );
        }
        private MccResponseDto MapToResponseDto(MccEntity entity)
        {
            return new MccResponseDto(
              id: entity.Id,
              cabinetCode: entity.CabinetCode,
              brand: entity.Brand,
              fieldDeviceBasicDtos: entity.FieldDeviceEntities.Any() ? entity.FieldDeviceEntities.Select(fd => new FieldDeviceBasicDto(fd.Id, fd.Name)).ToList() : new List<FieldDeviceBasicDto>(),
              note: entity.Note
            );
        }
    }
}