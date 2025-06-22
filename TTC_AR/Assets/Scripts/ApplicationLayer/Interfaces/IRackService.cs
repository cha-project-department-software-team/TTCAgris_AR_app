
// Domain/Repositories/IRackRepository.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationLayer.Dtos.Rack;

namespace ApplicationLayer.Interfaces
{
    public interface IRackService
    {
        Task<RackResponseDto> GetRackByIdAsync(int rackId);
        Task<IEnumerable<RackBasicDto>> GetListRackAsync(int grapperId);
        Task<bool> CreateNewRackAsync(int grapperId, RackRequestDto rackRequestDto);
        Task<bool> UpdateRackAsync(int rackId, RackRequestDto rackRequestDto);
        Task<bool> DeleteRackAsync(int rackId);
    }
}