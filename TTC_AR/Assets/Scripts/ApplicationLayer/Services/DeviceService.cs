
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationLayer.Dtos;
using ApplicationLayer.Dtos.Device;
using ApplicationLayer.Interfaces;
using ApplicationLayer.UseCases;

namespace ApplicationLayer.Services

{

    //! Không bắt lỗi tại đây



    public class DeviceService : IDeviceService
    {
        private readonly DeviceUseCase _DeviceUseCase;
        public DeviceService(DeviceUseCase DeviceUseCase)
        {
            _DeviceUseCase = DeviceUseCase;
        }

        //! Tham số là Dto, Dữ liệu trả về là Dto
        public async Task<DeviceResponseDto> GetDeviceByIdAsync(int id)
        {
            UnityEngine.Debug.Log("Run Service");

            return await _DeviceUseCase.GetDeviceByIdAsync(id);
        }
        public async Task<List<DeviceBasicDto>> GetListDeviceGeneralAsync(int grapperId)
        {
            return await _DeviceUseCase.GetListDeviceGeneralAsync(grapperId);
        }
        public async Task<List<DeviceResponseDto>> GetListDeviceInformationFromGrapperAsync(int grapperId)
        {
            return await _DeviceUseCase.GetListDeviceInformationFromGrapperAsync(grapperId);
        }
        public async Task<List<DeviceBasicDto>> GetListDeviceInformationFromModuleAsync(int moduleId)
        {
            return await _DeviceUseCase.GetListDeviceInformationFromModuleAsync(moduleId);
        }
        public async Task<bool> CreateNewDeviceAsync(int grapperId, DeviceRequestDto DeviceRequestDto)
        {
            return await _DeviceUseCase.CreateNewDeviceAsync(grapperId, DeviceRequestDto);
        }
        public async Task<bool> UpdateDeviceAsync(int deviceId, DeviceRequestDto DeviceRequestDto)
        {
            return await _DeviceUseCase.UpdateDeviceAsync(deviceId, DeviceRequestDto);
        }
        public async Task<bool> DeleteDeviceAsync(int deviceId)
        {
            return await _DeviceUseCase.DeleteDeviceAsync(deviceId);
        }
    }

}