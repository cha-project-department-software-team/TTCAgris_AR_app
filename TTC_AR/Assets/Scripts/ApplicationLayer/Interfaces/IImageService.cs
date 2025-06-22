
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationLayer.Dtos;
using ApplicationLayer.Dtos.Image;
using UnityEngine;


namespace ApplicationLayer.Interfaces
{
    public interface IImageService
    {
        //! Tham số là Dto, tả về Dto
        Task<ImageBasicDto> GetImageByIdAsync(int imageId);
        Task<List<ImageBasicDto>> GetListImageAsync(int grapperId);
        Task<bool> CreateNewImageAsync(int grapperId, ImageRequestDto ImageRequestDto);
        Task<bool> DeleteImageAsync(int imageId);
        Task<bool> UploadNewImageFromGallery(int grapperId, Texture2D texture, string filePath, string fileName);
        Task<bool> UploadNewImageFromCamera(int grapperId, Texture2D texture, string fileName);

    }

}