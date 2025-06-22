
using System;
using UnityEngine;
using System.Net.Http;
using ApplicationLayer.Dtos.Image;
using ApplicationLayer.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;

public class ImageManager : MonoBehaviour
{


    //! Tương tác người dùng sẽ gọi trực tiếp ImageManager 
    //! ImageManager sẽ gọi ImageService thông qua ServiceLocator
    //! ImageService sẽ gọi ImageRepository thông qua ServiceLocator

    #region Services
    public IImageService _IImageService;
    #endregion

    private void Start()
    {
        //! Dependency Injection
        _IImageService = ServiceLocator.Instance.ImageService;
    }
    public async void GetImageList(int companyId)
    {
        try
        {
            var ImageList = await _IImageService.GetListImageAsync(companyId); //! Gọi _IImageService từ Application Layer
            if (ImageList != null && ImageList.Any())
            {
                foreach (var Image in ImageList)
                    Debug.Log($"Image: {Image.Name}");
            }
            else
            {
                Debug.Log("No Images found");
            }
        }
        catch (ArgumentException ex) // Lỗi validation
        {
            Debug.LogError($"Validation error: {ex.Message}");
            //? hiển thị Dialog hoặc showToast tại đây

        }
        catch (HttpRequestException ex) // Lỗi mạng/HTTP
        {
            Debug.LogError($"Network error: {ex.Message}");
            //? hiển thị Dialog hoặc showToast tại đây

        }
        catch (Exception ex) // Lỗi khác
        {
            Debug.LogError($"Unexpected error: {ex.Message}");
            //? hiển thị Dialog hoặc showToast tại đây

        }
    }

    public async void GetImageById(int ImageId)
    {
        try
        {
            var Image = await _IImageService.GetImageByIdAsync(ImageId); // Gọi Service
            if (Image != null)
            {
                Debug.Log($"Image: {Image.Name}");
            }
            else
            {
                Debug.Log("Image not found");
            }
        }

        catch (ArgumentException ex) // Lỗi validation
        {
            Debug.LogError($"Validation error: {ex.Message}");
            //? hiển thị Dialog hoặc showToast tại đây

        }
        catch (HttpRequestException ex) // Lỗi mạng/HTTP
        {
            Debug.LogError($"Network error: {ex.Message}");
            //? hiển thị Dialog hoặc showToast tại đây

        }
        catch (Exception ex) // Lỗi khác
        {
            Debug.LogError($"Unexpected error: {ex.Message}");
            //? hiển thị Dialog hoặc showToast tại đây

        }
    }

    public async void CreateNewImage(int companyId, ImageRequestDto ImageRequestDto)
    {
        try
        {
            bool result = await _IImageService.CreateNewImageAsync(companyId, ImageRequestDto);
            Debug.Log(result ? "Image created successfully" : "Failed to create Image");
            //? hiển thị Dialog hoặc showToast tại đây
        }

        catch (ArgumentException ex) // Lỗi validation
        {
            Debug.LogError($"Validation error: {ex.Message}");
            //? hiển thị Dialog hoặc showToast tại đây


        }
        catch (HttpRequestException ex) // Lỗi mạng/HTTP
        {
            Debug.LogError($"Network error: {ex.Message}");
            //? hiển thị Dialog hoặc showToast tại đây

        }
        catch (Exception ex) // Lỗi khác
        {
            Debug.LogError($"Unexpected error: {ex.Message}");
            //? hiển thị Dialog hoặc showToast tại đây

        }
    }

    public async void DeleteImage(int ImageId)
    {
        try
        {
            bool result = await _IImageService.DeleteImageAsync(ImageId);
            Debug.Log(result ? "Image deleted successfully" : "Failed to delete Image");
        }

        catch (ArgumentException ex) // Lỗi validation
        {
            Debug.LogError($"Validation error: {ex.Message}");
            //? hiển thị Dialog hoặc showToast tại đây

        }
        catch (HttpRequestException ex) // Lỗi mạng/HTTP
        {
            Debug.LogError($"Network error: {ex.Message}");
            //? hiển thị Dialog hoặc showToast tại đây

        }
        catch (Exception ex) // Lỗi khác
        {
            Debug.LogError($"Unexpected error: {ex.Message}");
            //? hiển thị Dialog hoặc showToast tại đây

        }
    }

    public async void UploadNewImageFromGallery(int grapperId, Texture2D texture, string filePath, string fileName)
    {
        try
        {
            await _IImageService.UploadNewImageFromGallery(grapperId, texture, filePath, fileName);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Upload error: {ex.Message}");
        }
    }
    public async void UploadNewImageFromCamera(int grapperId, Texture2D texture, string fileName)
    {
        try
        {
            bool result = await _IImageService.UploadNewImageFromCamera(grapperId, texture, fileName);
            Debug.Log(result ? "Image uploaded successfully" : "Failed to upload Image");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Upload error: {ex.Message}");
        }
    }
}