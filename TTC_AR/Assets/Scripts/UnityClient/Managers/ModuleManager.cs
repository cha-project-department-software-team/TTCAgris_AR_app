
using System;
using UnityEngine;
using System.Net.Http;
using ApplicationLayer.Interfaces;
using ApplicationLayer.Dtos.Module;
using System.Linq;

public class ModuleManager : MonoBehaviour
{


    //! Tương tác người dùng sẽ gọi trực tiếp ModuleManager 
    //! ModuleManager sẽ gọi ModuleService thông qua ServiceLocator
    //! ModuleService sẽ gọi ModuleRepository thông qua ServiceLocator

    #region Services
    public IModuleService _IModuleService;
    #endregion

    private void Start()
    {
        //! Dependency Injection
        _IModuleService = ServiceLocator.Instance.ModuleService;
    }
    public async void GetModuleList(int grapperId)
    {
        try
        {
            var ModuleList = await _IModuleService.GetListModuleAsync(grapperId); //! Gọi _IModuleService từ Application Layer
            if (ModuleList != null && ModuleList.Any())
            {
                foreach (var Module in ModuleList)
                    Debug.Log($"Module: {Module.Name}");
            }
            else
            {
                Debug.Log("No Modules found");
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

    public async void GetModuleById(int moduleId)
    {
        try
        {
            var Module = await _IModuleService.GetModuleByIdAsync(moduleId); // Gọi Service
            if (Module != null)
            {
                Debug.Log($"Module: {Module.Name}");
            }
            else
            {
                Debug.Log("Module not found");
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

    public async void CreateNewModule(int grapperId, ModuleRequestDto ModuleRequestDto)
    {
        try
        {
            bool result = await _IModuleService.CreateNewModuleAsync(grapperId, ModuleRequestDto);
            Debug.Log(result ? "Module created successfully" : "Failed to create Module");
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
    public async void UpdateModule(int moduleId, ModuleRequestDto ModuleRequestDto)
    {
        try
        {
            bool result = await _IModuleService.UpdateModuleAsync(moduleId, ModuleRequestDto);
            Debug.Log(result ? "Module updated successfully" : "Failed to update Module");
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
    public async void DeleteModule(int moduleId)
    {
        try
        {
            bool result = await _IModuleService.DeleteModuleAsync(moduleId);
            Debug.Log(result ? "Module deleted successfully" : "Failed to delete Module");
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