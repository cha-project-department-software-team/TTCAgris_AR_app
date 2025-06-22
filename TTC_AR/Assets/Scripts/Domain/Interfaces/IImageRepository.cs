
// Domain/Repositories/IImageRepository.cs
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using UnityEngine;

namespace Domain.Interfaces
{
    public interface IImageRepository
    {
        //!  Do kết quả server trả về là tập hợp con của DeviceEntity nên sẽ lựa chọn hàm trả veỀ DeviceResponseDto
        //! Tham số là Entity
        Task<ImageEntity> GetImageByIdAsync(int ImageId);
        Task<List<ImageEntity>> GetListImageAsync(int grapperId);
        Task<bool> CreateNewImageAsync(int grapperId, ImageEntity ImageEntity);
        Task<bool> DeleteImageAsync(int ImageId);
        Task<bool> UploadNewImageFromGallery(int grapperId, Texture2D texture, string fileName, string filePath, string mimeType);
        Task<bool> UploadNewImageFromCamera(int grapperId, Texture2D texture, string fileName);

    }
    //! Được Implement ở ImageRepository.cs trong Infrastructure Layer
}