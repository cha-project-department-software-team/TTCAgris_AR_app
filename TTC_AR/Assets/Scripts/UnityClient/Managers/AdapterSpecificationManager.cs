
using System;
using UnityEngine;
using System.Net.Http;
using ApplicationLayer.Interfaces;
using ApplicationLayer.Dtos.AdapterSpecification;
using System.Linq;

public class AdapterSpecificationManager : MonoBehaviour
{


    //! Tương tác người dùng sẽ gọi trực tiếp AdapterSpecificationManager 
    //! AdapterSpecificationManager sẽ gọi AdapterSpecificationService thông qua ServiceLocator
    //! AdapterSpecificationService sẽ gọi AdapterSpecificationRepository thông qua ServiceLocator

    #region Services
    public IAdapterSpecificationService _IAdapterSpecificationService;
    #endregion

    private void Start()
    {
        //! Dependency Injection
        _IAdapterSpecificationService = ServiceLocator.Instance.AdapterSpecificationService;
    }
    public async void GetAdapterSpecificationList(int companyId)
    {
        try
        {
            var AdapterSpecificationList = await _IAdapterSpecificationService.GetListAdapterSpecificationAsync(companyId); //! Gọi _IAdapterSpecificationService từ Application Layer
            if (AdapterSpecificationList != null && AdapterSpecificationList.Any())
            {
                foreach (var AdapterSpecification in AdapterSpecificationList)
                    Debug.Log($"AdapterSpecification: {AdapterSpecification.Code}");
            }
            else
            {
                Debug.Log("No AdapterSpecifications found");
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

    public async void GetAdapterSpecificationById(int adapterSpecificationId)
    {
        try
        {
            var AdapterSpecification = await _IAdapterSpecificationService.GetAdapterSpecificationByIdAsync(adapterSpecificationId); // Gọi Service
            if (AdapterSpecification != null)
            {
                Debug.Log($"AdapterSpecification: {AdapterSpecification.Code}");
            }
            else
            {
                Debug.Log("AdapterSpecification not found");
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

    public async void CreateNewAdapterSpecification(int companyId, AdapterSpecificationRequestDto AdapterSpecificationRequestDto)
    {
        try
        {
            bool result = await _IAdapterSpecificationService.CreateNewAdapterSpecificationAsync(companyId, AdapterSpecificationRequestDto);
            Debug.Log(result ? "AdapterSpecification created successfully" : "Failed to create AdapterSpecification");
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
    public async void UpdateAdapterSpecification(int adapterSpecificationId, AdapterSpecificationRequestDto AdapterSpecificationRequestDto)
    {
        try
        {
            bool result = await _IAdapterSpecificationService.UpdateAdapterSpecificationAsync(adapterSpecificationId, AdapterSpecificationRequestDto);
            Debug.Log(result ? "AdapterSpecification updated successfully" : "Failed to update AdapterSpecification");
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
    public async void DeleteAdapterSpecification(int adapterSpecificationId)
    {
        try
        {
            bool result = await _IAdapterSpecificationService.DeleteAdapterSpecificationAsync(adapterSpecificationId);
            Debug.Log(result ? "AdapterSpecification deleted successfully" : "Failed to delete AdapterSpecification");
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
}