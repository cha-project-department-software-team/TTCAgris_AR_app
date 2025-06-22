
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationLayer.Dtos;
using ApplicationLayer.Dtos.Mcc;
using Domain.Entities;

namespace ApplicationLayer.Interfaces
{
    public interface IMccService
    {
        Task<MccResponseDto> GetMccByIdAsync(int mccId);
        Task<List<MccBasicDto>> GetListMccAsync(int grapperId);
        Task<bool> CreateNewMccAsync(int grapperId, MccRequestDto mccRequestDto);
        Task<bool> UpdateMccAsync(int mccId, MccRequestDto mccRequestDto);
        Task<bool> DeleteMccAsync(int mccId);
    }
}