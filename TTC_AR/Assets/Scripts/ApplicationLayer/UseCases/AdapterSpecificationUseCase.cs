using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationLayer.Dtos;
using ApplicationLayer.Dtos.AdapterSpecification;
using Domain.Entities;
using Domain.Interfaces;

namespace ApplicationLayer.UseCases
{
    public class AdapterSpecificationUseCase
    {
        private IAdapterSpecificationRepository _IAdapterSpecificationRepository;

        public AdapterSpecificationUseCase(IAdapterSpecificationRepository IAdapterSpecificationRepository)
        {
            _IAdapterSpecificationRepository = IAdapterSpecificationRepository;
        }
        public async Task<List<AdapterSpecificationBasicDto>> GetListAdapterSpecificationAsync(int companyId)
        {
            try
            {
                var AdapterSpecificationEntities = await _IAdapterSpecificationRepository.GetListAdapterSpecificationAsync(companyId) ??

                                  throw new ApplicationException("Failed to get AdapterSpecification list");

                int count = AdapterSpecificationEntities.Count;

                var AdapterSpecificationBasicDtos = new List<AdapterSpecificationBasicDto>(count);

                var listAdapterSpecificationInfo = new List<AdapterSpecificationModel>(count);

                var dictAdapterSpecificationInfo = new Dictionary<string, AdapterSpecificationModel>(count);

                foreach (var AdapterSpecificationEntity in AdapterSpecificationEntities)
                {
                    var dto = MapToBasicDto(AdapterSpecificationEntity);
                    var model = new AdapterSpecificationModel(dto.Id, dto.Code);

                    AdapterSpecificationBasicDtos.Add(dto);
                    listAdapterSpecificationInfo.Add(model);
                    dictAdapterSpecificationInfo[dto.Code] = model;
                }

                // GlobalVariable.temp_ListAdapterSpecificationModel = listAdapterSpecificationInfo;
                // GlobalVariable.temp_Dictionary_AdapterSpecificationModel = dictAdapterSpecificationInfo;

                return AdapterSpecificationBasicDtos;

            }

            catch (ArgumentException)
            {
                throw new ApplicationException("Failed to get AdapterSpecification list"); // Ném lại lỗi validation cho Unity xử lý
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to get AdapterSpecification list", ex); // Bao bọc lỗi từ Repository
            }

        }
        public async Task<AdapterSpecificationResponseDto> GetAdapterSpecificationByIdAsync(int adapterSpecificationId)
        {
            try
            {
                var AdapterSpecificationEntity = await _IAdapterSpecificationRepository.GetAdapterSpecificationByIdAsync(adapterSpecificationId);
                if (AdapterSpecificationEntity == null)
                {
                    throw new ApplicationException("Failed to get AdapterSpecification");
                }
                else
                {
                    var AdapterSpecificationResponseDto = MapToResponseDto(AdapterSpecificationEntity);
                    return AdapterSpecificationResponseDto;
                }
            }
            catch (ArgumentException)
            {
                throw new ApplicationException("Failed to get AdapterSpecification"); // Ném lại lỗi validation cho Unity xử lý
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to get AdapterSpecification", ex); // Bao bọc lỗi từ Repository
            }
        }
        public async Task<bool> CreateNewAdapterSpecificationAsync(int companyId, AdapterSpecificationRequestDto requestDto)
        {
            try
            {
                // Validate
                if (string.IsNullOrEmpty(requestDto.Code))
                {
                    throw new ArgumentException("Code cannot be empty");
                }
                var createNewAdapterSpecificationEntity = MapRequestToEntity(requestDto);
                // var requestData = adapterSpecificationRequestDto(createNewAdapterSpecificationEntity);
                if (createNewAdapterSpecificationEntity == null)
                {
                    throw new ApplicationException("Failed to create AdapterSpecification cause AdapterSpecificationEntity is Null");
                }
                else
                {
                    return await _IAdapterSpecificationRepository.CreateNewAdapterSpecificationAsync(companyId, createNewAdapterSpecificationEntity);
                }
            }
            catch (ArgumentException)
            {
                throw new ApplicationException("Failed to create AdapterSpecification"); // Ném lại lỗi validation cho Unity xử lý
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to create AdapterSpecification", ex); // Bao bọc lỗi từ Repository
            }
        }
        public async Task<bool> UpdateAdapterSpecificationAsync(int adapterSpecificationId, AdapterSpecificationRequestDto requestDto)
        {
            // adapterSpecificationId = GlobalVariable.adapterSpecificationId;
            try
            {
                // Validate
                if (string.IsNullOrEmpty(requestDto.Code))
                {
                    throw new ArgumentException("Code cannot be empty");
                }
                var updateAdapterSpecificationEntity = MapRequestToEntity(requestDto);

                //var requestData = adapterSpecificationRequestDto(updateAdapterSpecificationEntity);

                if (updateAdapterSpecificationEntity == null)
                {
                    throw new ApplicationException("Failed to update AdapterSpecification cause AdapterSpecificationEntity is Null");
                }
                else
                {
                    return await _IAdapterSpecificationRepository.UpdateAdapterSpecificationAsync(adapterSpecificationId, updateAdapterSpecificationEntity);
                }

            }
            catch (ArgumentException)
            {
                throw new ApplicationException("Failed to update AdapterSpecification"); // Ném lại lỗi validation cho Unity xử lý
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to update AdapterSpecification", ex); // Bao bọc lỗi từ Repository
            }
        }
        public async Task<bool> DeleteAdapterSpecificationAsync(int adapterSpecificationId)
        {
            //adapterSpecificationId = GlobalVariable.adapterSpecificationId;
            try
            {
                var deletedAdapterSpecificationResult = await _IAdapterSpecificationRepository.DeleteAdapterSpecificationAsync(adapterSpecificationId);
                return deletedAdapterSpecificationResult;
            }
            catch (ArgumentException)
            {
                throw new ApplicationException("Failed to delete AdapterSpecification"); // Ném lại lỗi validation cho Unity xử lý
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to delete AdapterSpecification", ex); // Bao bọc lỗi từ Repository
            }
        }


        //! Entity => Dto
        private AdapterSpecificationBasicDto MapToBasicDto(AdapterSpecificationEntity AdapterSpecificationEntity)
        {
            return new AdapterSpecificationBasicDto(AdapterSpecificationEntity.Id, AdapterSpecificationEntity.Code)
            {
                Id = AdapterSpecificationEntity.Id,
                Code = AdapterSpecificationEntity.Code
            };
        }
        private AdapterSpecificationResponseDto MapToResponseDto(AdapterSpecificationEntity AdapterSpecificationEntity)
        {
            return new AdapterSpecificationResponseDto(
               id: AdapterSpecificationEntity.Id,
             code: AdapterSpecificationEntity.Code,
              type: AdapterSpecificationEntity.Type,
             communication: AdapterSpecificationEntity.Communication,
              numOfModulesAllowed: AdapterSpecificationEntity.NumOfModulesAllowed,
          commSpeed: AdapterSpecificationEntity.CommSpeed,
          inputSupply: AdapterSpecificationEntity.InputSupply,
           outputSupply: AdapterSpecificationEntity.OutputSupply,
           inrushCurrent: AdapterSpecificationEntity.InrushCurrent,
           alarm: AdapterSpecificationEntity.Alarm,
          note: AdapterSpecificationEntity.Note,
           pdfManual: AdapterSpecificationEntity.PdfManual
            );
        }

        //! Dto => Entity
        private AdapterSpecificationEntity MapRequestToEntity(AdapterSpecificationRequestDto requestDto)
        {
            return new AdapterSpecificationEntity(
            code: requestDto.Code,
            type: requestDto.Type,
            communication: requestDto.Communication,
            numOfModulesAllowed: requestDto.NumOfModulesAllowed,
            commSpeed: requestDto.CommSpeed,
            inputSupply: requestDto.InputSupply,
            outputSupply: requestDto.OutputSupply,
            inrushCurrent: requestDto.InrushCurrent,
            alarm: requestDto.Alarm,
            note: requestDto.Note,
            pdfManual: requestDto.PdfManual
            );
        }
    }
}