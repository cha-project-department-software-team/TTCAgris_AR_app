
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationLayer.Dtos.FieldDevice;
using ApplicationLayer.Interfaces;
using ApplicationLayer.UseCases;
using UnityEngine.Purchasing;

namespace ApplicationLayer.Services
{
    //! Không bắt lỗi tại đây
    public class FieldDeviceService : IFieldDeviceService
    {
        private readonly FieldDeviceUseCase _FieldDeviceUseCase;
        public FieldDeviceService(FieldDeviceUseCase FieldDeviceUseCase)
        {
            _FieldDeviceUseCase = FieldDeviceUseCase;
        }

        //! Dữ liệu trả về là Dto

        public async Task<FieldDeviceResponseDto> GetFieldDeviceByIdAsync(int fieldDeviceId)
        {

            return await _FieldDeviceUseCase.GetFieldDeviceByIdAsync(fieldDeviceId);


        }

        public async Task<List<FieldDeviceBasicDto>> GetListFieldDeviceAsync(int grapperId)
        {
            return await _FieldDeviceUseCase.GetListFieldDeviceAsync(grapperId);
        }

        public async Task<bool> CreateNewFieldDeviceAsync(int grapperId, FieldDeviceRequestDto fieldDeviceRequestDto)
        {
            return await _FieldDeviceUseCase.CreateNewFieldDeviceAsync(grapperId, fieldDeviceRequestDto);
        }
        public async Task<bool> UpdateFieldDeviceAsync(int fieldDeviceId, FieldDeviceRequestDto fieldDeviceRequestDto)
        {
            return await _FieldDeviceUseCase.UpdateFieldDeviceAsync(fieldDeviceId, fieldDeviceRequestDto);
        }
        public async Task<bool> DeleteFieldDeviceAsync(int fieldDeviceId)
        {
            return await _FieldDeviceUseCase.DeleteFieldDeviceAsync(fieldDeviceId);
        }
    }

}