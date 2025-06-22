
// Domain/Repositories/IGrapperRepository.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IGrapperRepository
    {
        Task<GrapperEntity> GetGrapperByIdAsync(int grapperId);
        Task<List<GrapperEntity>> GetListGrapperAsync(int companyId);
        Task<bool> CreateNewGrapperAsync(int companyId, GrapperEntity grapperEntity);
        Task<bool> UpdateGrapperAsync(int grapperId, GrapperEntity grapperEntity);
        Task<bool> DeleteGrapperAsync(int grapperId);
    }
}