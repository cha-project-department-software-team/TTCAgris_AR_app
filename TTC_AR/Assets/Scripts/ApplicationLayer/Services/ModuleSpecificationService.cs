
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationLayer.Dtos;
using ApplicationLayer.Dtos.ModuleSpecification;
using ApplicationLayer.Interfaces;
using ApplicationLayer.UseCases;

namespace ApplicationLayer.Services
{
    //! Không bắt lỗi tại đây
    public class ModuleSpecificationService : IModuleSpecificationService
    {
        private readonly ModuleSpecificationUseCase _ModuleSpecificationUseCase;
        public ModuleSpecificationService(ModuleSpecificationUseCase ModuleSpecificationUseCase)
        {
            _ModuleSpecificationUseCase = ModuleSpecificationUseCase;
        }

        //! Dữ liệu trả về là Dto

        public async Task<ModuleSpecificationResponseDto> GetModuleSpecificationByIdAsync(int moduleSpecificationId)
        {
            return await _ModuleSpecificationUseCase.GetModuleSpecificationByIdAsync(moduleSpecificationId);
        }

        public async Task<IEnumerable<ModuleSpecificationBasicDto>> GetListModuleSpecificationAsync(int companyId)
        {
            return await _ModuleSpecificationUseCase.GetListModuleSpecificationAsync(companyId);
        }

        public async Task<bool> CreateNewModuleSpecificationAsync(int companyId, ModuleSpecificationRequestDto ModuleSpecificationRequestDto)
        {
            return await _ModuleSpecificationUseCase.CreateNewModuleSpecificationAsync(companyId, ModuleSpecificationRequestDto);
        }
        public async Task<bool> UpdateModuleSpecificationAsync(int moduleSpecificationId, ModuleSpecificationRequestDto ModuleSpecificationRequestDto)
        {
            return await _ModuleSpecificationUseCase.UpdateModuleSpecificationAsync(moduleSpecificationId, ModuleSpecificationRequestDto);
        }
        public async Task<bool> DeleteModuleSpecificationAsync(int moduleSpecificationId)
        {
            return await _ModuleSpecificationUseCase.DeleteModuleSpecificationAsync(moduleSpecificationId);
        }
    }

}