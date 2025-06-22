
// Domain/Repositories/IModuleRepository.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationLayer.Dtos;
using ApplicationLayer.Dtos.Module;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IModuleRepository
    {
        Task<ModuleEntity> GetModuleByIdAsync(int moduleId);
        Task<List<ModuleEntity>> GetListModuleAsync(int grapperId);
        Task<bool> CreateNewModuleAsync(int grapperId, ModuleEntity moduleEntity);
        Task<bool> UpdateModuleAsync(int moduleId, ModuleEntity moduleEntity);
        Task<bool> DeleteModuleAsync(int moduleId);
    }
}