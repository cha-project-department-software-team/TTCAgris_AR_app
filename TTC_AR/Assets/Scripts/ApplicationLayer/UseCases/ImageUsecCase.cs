using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ApplicationLayer.Dtos.Image;

using Domain.Entities;
using Domain.Interfaces;
using UnityEngine;


namespace ApplicationLayer.UseCases
{
    public class ImageUseCase
    {
        private IImageRepository _IImageRepository;

        public ImageUseCase(IImageRepository IImageRepository)
        {
            _IImageRepository = IImageRepository;
        }
        public async Task<List<ImageBasicDto>> GetListImageAsync(int grapperId)
        {
            try
            {
                var ImageEntities = await _IImageRepository.GetListImageAsync(grapperId);

                if (ImageEntities == null)
                {
                    throw new ApplicationException("Failed to get Image list");
                }
                else
                {  // Ánh xạ từ ImageBasicDto sang ImageEntity (nếu cần logic nghiệp vụ) rồi sang ImageBasicDto
                   // var ImageEntities = ImageListDtos.Select(dto => new ImageEntity(dto.Name)
                   // {
                   //     Id = dto.Id,

                    //     Location = dto.Location,

                    //     DeviceEntities = dto.DeviceBasicDtos.Select(d => new DeviceEntity(d.Id, d.Code)).ToList(),

                    //     ModuleEntities = dto.ModuleBasicDtos.Select(m => new ModuleEntity(m.Id, m.Name)).ToList(),

                    //     OutdoorImageEntity = dto.OutdoorImageBasicDto != null
                    //         ? new ImageEntity(dto.OutdoorImageBasicDto.Id, dto.OutdoorImageBasicDto.Name, dto.OutdoorImageBasicDto.Url)
                    //         : null,

                    //     ConnectionImageEntities = dto.ConnectionImageBasicDtos.Select(i => new ImageEntity(i.Id, i.Name, i.Url)).ToList()

                    // }).ToList();

                    // // Ánh xạ từ ImageEntity sang ImageBasicDto
                    var ImageBasicDtos = ImageEntities.Select(ImageEntity => MapToBasicDto(ImageEntity)).ToList();
                    GlobalVariable.temp_Dictionary_ImageInformationModel = ImageBasicDtos.ToDictionary(dto => dto.Name, dto => new ImageInformationModel(id: dto.Id, name: dto.Name
                    // , url: ""
                    ));

                    GlobalVariable.temp_ListImageInformationModel = ImageBasicDtos.Select(dto => new ImageInformationModel(dto.Id, dto.Name
                    // , ""
                    )).ToList();

                    // foreach (var item in GlobalVariable.temp_Dictionary_ImageInformationModel)
                    // {
                    //     UnityEngine.Debug.Log("Key: " + item.Key + " Value: " + item.Value.Id + " " + item.Value.Name);
                    // }

                    return ImageBasicDtos;
                }

            }

            catch (ArgumentException)
            {
                throw new ApplicationException("Failed to get Image list"); // Ném lại lỗi validation cho Unity xử lý

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to get Image list", ex); // Bao bọc lỗi từ Repository
            }
        }
        public async Task<ImageBasicDto> GetImageByIdAsync(int ImageId)
        {
            try
            {
                var ImageEntity = await _IImageRepository.GetImageByIdAsync(ImageId);

                if (ImageEntity == null)
                {
                    throw new ApplicationException("Failed to get Image");
                }
                else
                {
                    // Ánh xạ từ ImageEntity sang ImageEntity để check các lỗi nghiệp vụ
                    var ImageBasicDto = MapToResponseDto(ImageEntity);
                    // Ánh xạ từ ImageEntity sang ImageBasicDto để đưa giá trị trả về
                    return ImageBasicDto;
                }
            }
            catch (ArgumentException)
            {
                throw new ApplicationException("Failed to get Image"); // Ném lại lỗi validation cho Unity xử lý
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to get Image", ex); // Bao bọc lỗi từ Repository
            }

        }
        public async Task<bool> CreateNewImageAsync(int grapperId, ImageRequestDto requestDto)
        {
            try
            {
                // Validate
                if (string.IsNullOrEmpty(requestDto.Name))
                {
                    throw new ArgumentException("name cannot be empty");
                }
                // Ánh xạ từ ImageRequestDto sang ImageEntity để check các nghiệp vụ
                var ImageEntity = MapRequestToEntity(requestDto);

                // var requestData = MapToRequestDto(ImageEntity);

                if (ImageEntity == null)
                {
                    throw new ApplicationException("Failed to create Image cause ImageEntity is Null");
                }

                else
                {
                    return await _IImageRepository.CreateNewImageAsync(grapperId, ImageEntity);
                }

            }
            catch (ArgumentException)
            {
                throw new ApplicationException("Failed to create Image cause name is empty"); // Ném lại lỗi validation cho Unity xử lý

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to create Image", ex); // Bao bọc lỗi từ Repository
            }
        }
        public async Task<bool> UploadImageFromGallery(int grapperId, Texture2D texture, string fileName, string filePath)
        {
            try
            {
                var mimeType = GetMimeType(filePath);
                Debug.Log("Run UseCase");
                Debug.Log($"UploadNewImageFromGallery:  {filePath} {mimeType}");
                var result = await _IImageRepository.UploadNewImageFromGallery(grapperId, texture, fileName, filePath, mimeType);
                // TryDeleteFile(filePath); // Xóa file tạm sau khi upload
                return result;
            }
            catch (ArgumentException)
            {
                throw new ApplicationException("Failed to upload Image cause name is empty"); // Ném lại lỗi validation cho Unity xử lý
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to upload Image from gallery", ex); // Bao bọc lỗi từ Repository
            }
        }
        public async Task<bool> UploadImageFromCamera(int grapperId, Texture2D texture, string fileName)
        {
            try
            {
                Debug.Log("Run UseCase");
                Debug.Log($"UploadNewImageFromCamera: {grapperId} {texture}  {fileName}");

                var result = await _IImageRepository.UploadNewImageFromCamera(grapperId, texture, fileName);
                return result;
            }
            catch (ArgumentException)
            {
                throw new ApplicationException("Failed to upload Image cause name is empty"); // Ném lại lỗi validation cho Unity xử lý
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to upload Image from camera", ex); // Bao bọc lỗi từ Repository
            }
        }

        public async Task<bool> DeleteImageAsync(int ImageId)
        {
            try
            {
                var deletedImageResult = await _IImageRepository.DeleteImageAsync(ImageId);
                return deletedImageResult;
            }
            catch (ArgumentException)
            {
                throw new ApplicationException("Failed to delete Image"); // Ném lại lỗi validation cho Unity xử lý
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to delete Image", ex); // Bao bọc lỗi từ Repository
            }
        }
        private string GetMimeType(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();
            switch (extension)
            {
                case ".png": return "image/png";
                case ".jpg": return "image/jpg";
                case ".jpeg": return "image/jpeg";
                default: return "application/octet-stream"; // MIME type mặc định
            }
        }

        private void TryDeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                    Debug.Log($"Đã xóa file tạm: {filePath}");
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Lỗi khi xóa file tạm: {ex.Message}");
                }
            }
        }

        //! Dto => Entity
        private ImageEntity MapRequestToEntity(ImageRequestDto ImageRequestDto)
        {
            return new ImageEntity(
                name: ImageRequestDto.Name
            );
        }

        //! Entity => Dto
        private ImageBasicDto MapToResponseDto(ImageEntity ImageEntity)
        {
            return new ImageBasicDto(
                ImageEntity.Id,
                ImageEntity.Name
                // ,
                //  ImageEntity.Url
                )
                ;
        }

        private ImageBasicDto MapToBasicDto(ImageEntity ImageEntity)
        {
            return new ImageBasicDto(
                ImageEntity.Id,

                ImageEntity.Name
                 );
        }

    }
}