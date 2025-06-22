
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationLayer.Dtos.Grapper;
using Domain.Entities;

namespace ApplicationLayer.Interfaces
{
    public interface IGrapperService
    {
        Task<GrapperResponseDto> GetGrapperByIdAsync(int grapperId);
        Task<List<GrapperBasicDto>> GetListGrapperAsync(int companyId);
        Task<bool> CreateNewGrapperAsync(int companyId, GrapperRequestDto grapperRequestDto);
        Task<bool> UpdateGrapperAsync(int grapperId, GrapperRequestDto grapperRequestDto);
        Task<bool> DeleteGrapperAsync(int grapperId);
    }
}