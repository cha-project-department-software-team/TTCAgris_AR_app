using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ApplicationLayer.Dtos;
using ApplicationLayer.Dtos.AdapterSpecification;
using ApplicationLayer.Dtos.Company;
using ApplicationLayer.Dtos.Grapper;
using ApplicationLayer.Dtos.ModuleSpecification;
using Domain.Entities;
using Domain.Interfaces;

namespace ApplicationLayer.UseCases
{
    public class CompanyUseCase
    {
        private ICompanyRepository _ICompanyRepository;

        public CompanyUseCase(ICompanyRepository ICompanyRepository)
        {
            _ICompanyRepository = ICompanyRepository;
        }

        public async Task<List<CompanyResponseDto>> GetListCompanyAsync()
        {
            try
            {
                var CompanyEntities = await _ICompanyRepository.GetListCompanyAsync();

                if (CompanyEntities == null)
                {
                    throw new ApplicationException("Failed to get Company list");
                }
                else
                {

                    var companyResponseDtos = CompanyEntities.Select(MapToResponseDto).ToList();
                    if (companyResponseDtos == null || companyResponseDtos.Count == 0)
                    {
                        throw new ApplicationException("Failed to get Company list");
                    }
                    else
                    {
                        // GlobalVariable.temp_Dictionary_CompanyInformationModel = companyResponseDtos.ToDictionary(dto => dto.Name, dto => MapToCompanyModel(dto));
                        // GlobalVariable.temp_List_CompanyInformationModel = companyResponseDtos.Select(dto => MapToCompanyModel(dto)).ToList();
                        return companyResponseDtos;
                    }
                }
            }
            catch (ArgumentException exception)
            {
                throw new ApplicationException("Failed to get Company list", exception); // Ném lại lỗi validation cho Unity xử lý
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to get Company list", ex); // Bao bọc lỗi từ Repository
            }

        }
        public async Task<CompanyResponseDto> GetCompanyByIdAsync(int CompanyId)
        {
            try
            {
                var companyEntity = await _ICompanyRepository.GetCompanyByIdAsync(CompanyId);

                if (companyEntity == null)
                {
                    throw new ApplicationException("Failed to get Company");
                }
                else
                {
                    var companyResponseDto = MapToResponseDto(companyEntity);
                    if (companyResponseDto == null)
                    {
                        throw new ApplicationException("Failed to get Company");
                    }
                    else
                    {
                        // GlobalVariable.temp_CompanyInformationModel = MapToCompanyModel(companyResponseDto);
                        return companyResponseDto;
                    }
                }
            }
            catch (ArgumentException exception)
            {
                throw new ApplicationException("Failed to get Company", exception); // Ném lại lỗi validation cho Unity xử lý
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to get Company", ex); // Bao bọc lỗi từ Repository
            }
        }


        //! Entity => Dto
        private CompanyResponseDto MapToResponseDto(CompanyEntity companyEntity)
        {
            return new CompanyResponseDto(
                id: companyEntity.Id,
                name: companyEntity.Name,
                grapperBasicDtos: companyEntity.GrapperEntities.Any() ? companyEntity.GrapperEntities.Select(grapperEntity => new GrapperBasicDto(grapperEntity.Id, grapperEntity.Name)).ToList() : new List<GrapperBasicDto>(),
                moduleSpecificationBasicDtos: companyEntity.ModuleSpecificationEntities.Any() ? companyEntity.ModuleSpecificationEntities.Select(moduleSpecificationEntity => new ModuleSpecificationBasicDto(moduleSpecificationEntity.Id, moduleSpecificationEntity.Code)).ToList() : new List<ModuleSpecificationBasicDto>(),
                adapterSpecificationBasicDtos: companyEntity.AdapterSpecificationEntities.Any() ? companyEntity.AdapterSpecificationEntities.Select(adapterSpecificationEntity => new AdapterSpecificationBasicDto(adapterSpecificationEntity.Id, adapterSpecificationEntity.Code)).ToList() : new List<AdapterSpecificationBasicDto>()
            );
        }

        //! Dto => Model
        private CompanyInformationModel MapToCompanyModel(CompanyResponseDto companyResponseDto)
        {
            return new CompanyInformationModel(
                id: companyResponseDto.Id,
                name: companyResponseDto.Name,
                listGrapperInformationModel: companyResponseDto.GrapperBasicDtos.Any() ? companyResponseDto.GrapperBasicDtos.Select(g => new GrapperInformationModel(g.Id, g.Name)).ToList() : new List<GrapperInformationModel>(),
                listModuleSpecificationModel: companyResponseDto.ModuleSpecificationBasicDtos.Any() ? companyResponseDto.ModuleSpecificationBasicDtos.Select(m => new ModuleSpecificationModel(m.Id, m.Code)).ToList() : new List<ModuleSpecificationModel>(),
                listAdapterSpecificationModel: companyResponseDto.AdapterSpecificationBasicDtos.Any() ? companyResponseDto.AdapterSpecificationBasicDtos.Select(a => new AdapterSpecificationModel(a.Id, a.Code)).ToList() : new List<AdapterSpecificationModel>()
            );
        }
    }
}