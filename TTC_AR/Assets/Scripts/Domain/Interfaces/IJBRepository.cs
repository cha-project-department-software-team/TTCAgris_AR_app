
// Domain/Repositories/IJBRepository.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationLayer.Dtos.JB;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IJBRepository
    {
        //!  Do kết quả server trả về là tập hợp con của DeviceEntity nên sẽ lựa chọn hàm trả veỀ DeviceResponseDto
        //! Tham số là Entity
        Task<JBEntity> GetJBByIdAsync(int JBId);
        Task<List<JBEntity>> GetListJBGeneralAsync(int grapperId);
        Task<List<JBEntity>> GetListJBInformationAsync(int grapperId);

        Task<bool> CreateNewJBAsync(int grapperId, JBEntity jBEntity);
        Task<bool> UpdateJBAsync(int JBId, JBEntity jBEntity);
        Task<bool> DeleteJBAsync(int JBId);
    }
    //! Được Implement ở JBRepository.cs trong Infrastructure Layer
}