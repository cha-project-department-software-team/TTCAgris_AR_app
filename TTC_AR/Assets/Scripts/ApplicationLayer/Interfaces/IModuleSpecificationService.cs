
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationLayer.Dtos;
using ApplicationLayer.Dtos.ModuleSpecification;
using Domain.Entities;

namespace ApplicationLayer.Interfaces
{
    public interface IModuleSpecificationService
    {   //! Tham số là Dto, tả về Dto
        Task<ModuleSpecificationResponseDto> GetModuleSpecificationByIdAsync(int moduleSpecificationId);
        Task<IEnumerable<ModuleSpecificationBasicDto>> GetListModuleSpecificationAsync(int companyId);
        Task<bool> CreateNewModuleSpecificationAsync(int companyId, ModuleSpecificationRequestDto moduleSpecificationRequestDto);
        Task<bool> UpdateModuleSpecificationAsync(int moduleSpecificationId, ModuleSpecificationRequestDto moduleSpecificationRequestDto);
        Task<bool> DeleteModuleSpecificationAsync(int moduleSpecificationId);
    }
}