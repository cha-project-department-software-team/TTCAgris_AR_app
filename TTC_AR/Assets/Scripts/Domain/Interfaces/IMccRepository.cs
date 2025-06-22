
// Domain/Repositories/IMccRepository.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IMccRepository
    {
        Task<MccEntity> GetMccByIdAsync(int mccId);
        Task<List<MccEntity>> GetListMccAsync(int grapperId);
        Task<bool> CreateNewMccAsync(int grapperId, MccEntity mccEntity);
        Task<bool> UpdateMccAsync(int mccId, MccEntity mccEntity);
        Task<bool> DeleteMccAsync(int mccId);
    }
}