
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationLayer.Dtos.Device;
using Domain.Entities;

namespace ApplicationLayer.Interfaces
{
    public interface IDeviceService

    {
        //! Tham số là Dto, Dữ liệu trả về là Dto
        Task<DeviceResponseDto> GetDeviceByIdAsync(int deviceId);
        Task<List<DeviceBasicDto>> GetListDeviceGeneralAsync(int grapperId);
        Task<List<DeviceResponseDto>> GetListDeviceInformationFromGrapperAsync(int grapperId);
        Task<List<DeviceBasicDto>> GetListDeviceInformationFromModuleAsync(int moduleId);
        Task<bool> CreateNewDeviceAsync(int grapperId, DeviceRequestDto deviceRequestDto);
        Task<bool> UpdateDeviceAsync(int deviceId, DeviceRequestDto deviceRequestDto);
        Task<bool> DeleteDeviceAsync(int deviceId);
    }
}