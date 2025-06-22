
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationLayer.Dtos;
using ApplicationLayer.Dtos.JB;


namespace ApplicationLayer.Interfaces
{
    public interface IJBService
    {
        //! Tham số là Dto, tả về Dto
        Task<JBResponseDto> GetJBByIdAsync(int JBId);
        Task<List<JBGeneralDto>> GetListJBInformationAsync(int grapperId);
        Task<List<JBBasicDto>> GetListJBGeneralAsync(int grapperId);
        Task<bool> CreateNewJBAsync(int grapperId, JBRequestDto JBRequestDto);
        Task<bool> UpdateJBAsync(int JBId, JBRequestDto JBRequestDto);
        Task<bool> DeleteJBAsync(int JBId);
    }

}