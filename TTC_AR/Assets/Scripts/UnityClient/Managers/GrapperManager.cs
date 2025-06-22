
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
using PimDeWitte.UnityMainThreadDispatcher;
using System.Linq;
using System.Net.Http;
using ApplicationLayer.Interfaces;
using ApplicationLayer.Dtos;
using ApplicationLayer.Dtos.Grapper;
public class GrapperManager : MonoBehaviour
{


    //! Tương tác người dùng sẽ gọi trực tiếp GrapperManager 
    //! GrapperManager sẽ gọi GrapperService thông qua ServiceLocator
    //! GrapperService sẽ gọi GrapperRepository thông qua ServiceLocator

    #region Services
    public IGrapperService _IGrapperService;
    #endregion

    private void Start()
    {
        //! Dependency Injection
        _IGrapperService = ServiceLocator.Instance.GrapperService;
    }
    public async void GetGrapperList(int grapperId)
    {
        try
        {
            var GrapperList = await _IGrapperService.GetListGrapperAsync(grapperId); //! Gọi _IGrapperService từ Application Layer
            if (GrapperList != null && GrapperList.Any())
            {
                foreach (var Grapper in GrapperList)
                    Debug.Log($"Grapper: {Grapper.Name}");
            }
            else
            {
                Debug.Log("No Grappers found");
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

    public async void GetGrapperById(int GrapperId)
    {
        try
        {
            var Grapper = await _IGrapperService.GetGrapperByIdAsync(GrapperId); // Gọi Service
            if (Grapper != null)
            {
                Debug.Log($"Grapper: {Grapper.Name}");
            }
            else
            {
                Debug.Log("Grapper not found");
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

    public async void CreateNewGrapper(int grapperId, GrapperRequestDto GrapperRequestDto)
    {
        try
        {
            bool result = await _IGrapperService.CreateNewGrapperAsync(grapperId, GrapperRequestDto);
            Debug.Log(result ? "Grapper created successfully" : "Failed to create Grapper");
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
    public async void UpdateGrapper(int GrapperId, GrapperRequestDto GrapperRequestDto)
    {
        try
        {
            bool result = await _IGrapperService.UpdateGrapperAsync(GrapperId, GrapperRequestDto);
            Debug.Log(result ? "Grapper updated successfully" : "Failed to update Grapper");
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
    public async void DeleteGrapper(int GrapperId)
    {
        try
        {
            bool result = await _IGrapperService.DeleteGrapperAsync(GrapperId);
            Debug.Log(result ? "Grapper deleted successfully" : "Failed to delete Grapper");
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