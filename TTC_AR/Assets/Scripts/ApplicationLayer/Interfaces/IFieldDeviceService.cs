
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationLayer.Dtos.FieldDevice;

namespace ApplicationLayer.Interfaces
{
    public interface IFieldDeviceService
    {
        Task<FieldDeviceResponseDto> GetFieldDeviceByIdAsync(int fieldDeviceId);
        Task<List<FieldDeviceBasicDto>> GetListFieldDeviceAsync(int grapperId);
        Task<bool> CreateNewFieldDeviceAsync(int grapperId, FieldDeviceRequestDto fieldDeviceRequestDto);
        Task<bool> UpdateFieldDeviceAsync(int fieldDeviceId, FieldDeviceRequestDto fieldDeviceRequestDto);
        Task<bool> DeleteFieldDeviceAsync(int fieldDeviceId);
    }
}