
// Domain/Repositories/IFieldDeviceRepository.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationLayer.Dtos;
using ApplicationLayer.Dtos.FieldDevice;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IFieldDeviceRepository
    {
        // Task<FieldDeviceResponseDto> GetFieldDeviceByIdAsync(int fieldDeviceId);
        // Task<List<FieldDeviceResponseDto>> GetListFieldDeviceAsync(int grapperId);
        Task<FieldDeviceEntity> GetFieldDeviceByIdAsync(int fieldDeviceId);
        Task<List<FieldDeviceEntity>> GetListFieldDeviceAsync(int grapperId);
        Task<bool> CreateNewFieldDeviceAsync(int grapperId, FieldDeviceEntity fieldDeviceEntity);
        Task<bool> UpdateFieldDeviceAsync(int fieldDeviceId, FieldDeviceEntity fieldDeviceEntity);
        Task<bool> DeleteFieldDeviceAsync(int fieldDeviceId);
    }
}