
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationLayer.Dtos;
using ApplicationLayer.Dtos.Module;
using ApplicationLayer.Interfaces;
using ApplicationLayer.UseCases;

namespace ApplicationLayer.Services
{  //! Không bắt lỗi tại đây
    public class ModuleService : IModuleService
    {
        private readonly ModuleUseCase _moduleUseCase;
        public ModuleService(ModuleUseCase moduleUseCase)
        {
            _moduleUseCase = moduleUseCase;
        }

        //! Dữ liệu trả về là Dto
        public async Task<ModuleResponseDto> GetModuleByIdAsync(int moduleId)
        {
            return await _moduleUseCase.GetModuleByIdAsync(moduleId);
        }

        public async Task<List<ModuleBasicDto>> GetListModuleAsync(int grapperId)
        {
            return await _moduleUseCase.GetListModuleAsync(grapperId);
        }
        public async Task<bool> CreateNewModuleAsync(int grapperId, ModuleRequestDto moduleRequestDto)
        {
            return await _moduleUseCase.CreateNewModuleAsync(grapperId, moduleRequestDto);
        }
        public async Task<bool> UpdateModuleAsync(int moduleId, ModuleRequestDto moduleRequestDto)
        {
            return await _moduleUseCase.UpdateModuleAsync(moduleId, moduleRequestDto);
        }
        public async Task<bool> DeleteModuleAsync(int moduleId)
        {
            return await _moduleUseCase.DeleteModuleAsync(moduleId);
        }
    }

}