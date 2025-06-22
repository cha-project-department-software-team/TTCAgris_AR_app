
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Threading;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Net.Http;
using ApplicationLayer.Dtos;
using ApplicationLayer.Dtos.Device;
using ApplicationLayer.Interfaces;
public class DeviceManager : MonoBehaviour
{


    //! Tương tác người dùng sẽ gọi trực tiếp DeviceManager 
    //! DeviceManager sẽ gọi DeviceService thông qua ServiceLocator
    //! DeviceService sẽ gọi DeviceRepository thông qua ServiceLocator

    #region Services
    public IDeviceService _IDeviceService;
    #endregion

    private void Start()
    {
        //! Dependency Injection
        _IDeviceService = ServiceLocator.Instance.DeviceService;
    }
    public async void GetListDeviceGeneral(int grapperId)
    {
        try
        {
            var deviceBasicDtos = await _IDeviceService.GetListDeviceGeneralAsync(grapperId); //! Gọi _IDeviceService từ Application Layer
            if (deviceBasicDtos != null && deviceBasicDtos.ToList().Any())
            {
                foreach (var Device in deviceBasicDtos)
                    Debug.Log($"Device: {Device.Code}");
            }
            else
            {
                Debug.Log("No Devices found");
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
    public async void GetDeviceInformationFromGrapperList(int grapperId)
    {
        try
        {
            var deviceResponseDtos = await _IDeviceService.GetListDeviceInformationFromGrapperAsync(grapperId); //! Gọi _IDeviceService từ Application Layer
            if (deviceResponseDtos != null && deviceResponseDtos.ToList().Any())
            {
                foreach (var Device in deviceResponseDtos)
                    Debug.Log($"Device: {Device.Code}");
            }
            else
            {
                Debug.Log("No Devices found");
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

    public async void GetDeviceInformationFromModuleList(int moduleId)
    {
        try
        {
            var deviceResponseDtos = await _IDeviceService.GetListDeviceInformationFromModuleAsync(moduleId); //! Gọi _IDeviceService từ Application Layer
            if (deviceResponseDtos != null && deviceResponseDtos.ToList().Any())
            {
                foreach (var Device in deviceResponseDtos)
                    Debug.Log($"Device: {Device.Code}");
            }
            else
            {
                Debug.Log("No Devices found");
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


    public async void GetDeviceById(int deviceId)
    {
        try
        {
            var Device = await _IDeviceService.GetDeviceByIdAsync(deviceId); // Gọi Service
            if (Device != null)
            {
                Debug.Log($"Device: {Device.Code}");
            }
            else
            {
                Debug.Log("Device not found");
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

    public async void CreateNewDevice(int grapperId, DeviceRequestDto DeviceRequestDto)
    {
        try
        {
            bool result = await _IDeviceService.CreateNewDeviceAsync(grapperId, DeviceRequestDto);
            Debug.Log(result ? "Device created successfully" : "Failed to create Device");
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
    public async void UpdateDevice(int deviceId, DeviceRequestDto DeviceRequestDto)
    {
        deviceId = GlobalVariable.deviceId;
        try
        {
            bool result = await _IDeviceService.UpdateDeviceAsync(deviceId, DeviceRequestDto);
            Debug.Log(result ? "Device updated successfully" : "Failed to update Device");
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
    public async void DeleteDevice(int deviceId)
    {
        deviceId = GlobalVariable.deviceId;
        try
        {
            bool result = await _IDeviceService.DeleteDeviceAsync(deviceId);
            Debug.Log(result ? "Device deleted successfully" : "Failed to delete Device");
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