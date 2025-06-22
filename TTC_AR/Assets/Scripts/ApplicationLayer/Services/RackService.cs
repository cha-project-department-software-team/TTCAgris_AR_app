
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationLayer.Dtos.Rack;
using ApplicationLayer.Interfaces;
using ApplicationLayer.UseCases;

namespace ApplicationLayer.Services

{
    public class RackService : IRackService
    {
        private readonly RackUseCase _RackUseCase;
        public RackService(RackUseCase RackUseCase)
        {
            _RackUseCase = RackUseCase;
        }

        //! Tham số là Dto, Dữ liệu trả về là Dto
        public async Task<RackResponseDto> GetRackByIdAsync(int id)
        {
            return await _RackUseCase.GetRackByIdAsync(id);
        }
        public async Task<IEnumerable<RackBasicDto>> GetListRackAsync(int grapperId)
        {
            return await _RackUseCase.GetListRackAsync(grapperId);
        }
        public async Task<bool> CreateNewRackAsync(int grapperId, RackRequestDto RackRequestDto)
        {
            return await _RackUseCase.CreateNewRackAsync(grapperId, RackRequestDto);
        }
        public async Task<bool> UpdateRackAsync(int rackId, RackRequestDto RackRequestDto)
        {
            return await _RackUseCase.UpdateRackAsync(rackId, RackRequestDto);
        }
        public async Task<bool> DeleteRackAsync(int rackId)
        {
            return await _RackUseCase.DeleteRackAsync(rackId);
        }
    }

}