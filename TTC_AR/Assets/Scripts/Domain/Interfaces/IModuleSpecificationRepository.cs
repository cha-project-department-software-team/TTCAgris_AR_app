
// Domain/Repositories/IModuleSpecificationRepository.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationLayer.Dtos;
using ApplicationLayer.Dtos.ModuleSpecification;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IModuleSpecificationRepository
    {
        //! Trả về Entity
        Task<ModuleSpecificationEntity> GetModuleSpecificationByIdAsync(int moduleSpecificationId);
        Task<List<ModuleSpecificationEntity>> GetListModuleSpecificationAsync(int companyId);
        Task<bool> CreateNewModuleSpecificationAsync(int companyId, ModuleSpecificationEntity ModuleSpecificationEntity);
        Task<bool> UpdateModuleSpecificationAsync(int moduleSpecificationId, ModuleSpecificationEntity ModuleSpecificationEntity);
        Task<bool> DeleteModuleSpecificationAsync(int moduleSpecificationId);
    }
}