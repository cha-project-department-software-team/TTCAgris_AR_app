
// Domain/Repositories/IModuleRepository.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationLayer.Dtos;
using ApplicationLayer.Dtos.Module;
using Domain.Entities;

namespace ApplicationLayer.Interfaces
{
    public interface IModuleService 
    {
        Task<ModuleResponseDto> GetModuleByIdAsync(int moduleId);
        Task<List<ModuleBasicDto>> GetListModuleAsync(int grapperId);
        Task<bool> CreateNewModuleAsync(int grapperId, ModuleRequestDto moduleRequestDto);
        Task<bool> UpdateModuleAsync(int moduleId, ModuleRequestDto moduleRequestDto);
        Task<bool> DeleteModuleAsync(int moduleId);
    }
}