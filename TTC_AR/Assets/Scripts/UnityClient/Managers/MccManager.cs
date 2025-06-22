
using System;
using UnityEngine;
using System.Net.Http;
using ApplicationLayer.Dtos;
using ApplicationLayer.Dtos.Mcc;
using ApplicationLayer.Interfaces;
using System.Linq;
public class MccManager : MonoBehaviour
{


    //! Tương tác người dùng sẽ gọi trực tiếp MccManager 
    //! MccManager sẽ gọi MccService thông qua ServiceLocator
    //! MccService sẽ gọi MccRepository thông qua ServiceLocator

    #region Services
    public IMccService _IMccService;
    #endregion

    private void Start()
    {
        //! Dependency Injection
        _IMccService = ServiceLocator.Instance.MccService;
    }
    public async void GetMccList(int grapperId)
    {
        try
        {
            var MccList = await _IMccService.GetListMccAsync(grapperId); //! Gọi _IMccService từ Application Layer
            if (MccList != null && MccList.Any())
            {
                foreach (var Mcc in MccList)
                    Debug.Log($"Mcc: {Mcc.CabinetCode}");
            }
            else
            {
                Debug.Log("No Mccs found");
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

    public async void GetMccById(int mccId)
    {
        try
        {
            var Mcc = await _IMccService.GetMccByIdAsync(mccId); // Gọi Service
            if (Mcc != null)
            {
                Debug.Log($"Mcc: {Mcc.CabinetCode}");
            }
            else
            {
                Debug.Log("Mcc not found");
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

    public async void CreateNewMcc(int grapperId, MccRequestDto MccRequestDto)
    {
        try
        {
            bool result = await _IMccService.CreateNewMccAsync(grapperId, MccRequestDto);
            Debug.Log(result ? "Mcc created successfully" : "Failed to create Mcc");
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
    public async void UpdateMcc(int mccId, MccRequestDto MccRequestDto)
    {
        mccId = GlobalVariable.mccId;
        try
        {
            bool result = await _IMccService.UpdateMccAsync(mccId, MccRequestDto);
            Debug.Log(result ? "Mcc updated successfully" : "Failed to update Mcc");
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
    public async void DeleteMcc(int mccId)
    {
        mccId = GlobalVariable.mccId;
        try
        {
            bool result = await _IMccService.DeleteMccAsync(mccId);
            Debug.Log(result ? "Mcc deleted successfully" : "Failed to delete Mcc");
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