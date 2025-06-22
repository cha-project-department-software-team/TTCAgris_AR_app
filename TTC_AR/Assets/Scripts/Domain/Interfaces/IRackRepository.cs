
// Domain/Repositories/IRackRepository.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IRackRepository
    {
        Task<RackEntity> GetRackByIdAsync(int rackId);
        Task<List<RackEntity>> GetListRackAsync(int grapperId);
        Task<bool> CreateNewRackAsync(int grapperId, RackEntity rackEntity);
        Task<bool> UpdateRackAsync(int rackId, RackEntity rackEntity);
        Task<bool> DeleteRackAsync(int rackId);
    }
}