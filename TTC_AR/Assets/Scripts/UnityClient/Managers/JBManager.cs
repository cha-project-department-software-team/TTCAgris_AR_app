
using System;
using UnityEngine;
using System.Net.Http;
using ApplicationLayer.Dtos;
using ApplicationLayer.Dtos.JB;
using ApplicationLayer.Interfaces;
using System.Linq;

public class JBManager : MonoBehaviour
{
    //! Tương tác người dùng sẽ gọi trực tiếp JBManager 
    //! JBManager sẽ gọi JBService thông qua ServiceLocator
    //! JBService sẽ gọi JBRepository thông qua ServiceLocator
    #region Services
    public IJBService _IJBService;
    #endregion

    private void Start()
    {
        //! Dependency Injection
        _IJBService = ServiceLocator.Instance.JBService;
    }
    public async void GetListJBInformation(int grapperId)
    {
        try
        {
            var jBGeneralDtos = await _IJBService.GetListJBInformationAsync(grapperId); //! Gọi _IJBService từ Application Layer
            if (jBGeneralDtos != null && jBGeneralDtos.Any())
            {
                foreach (var jb in jBGeneralDtos)
                {
                    Debug.Log($"jBResponseDto: {jb.Name}, Location: {jb.Location}");
                    if (jb.OutdoorImageBasicDto != null)
                    {
                        Debug.Log($"OutdoorImage: {jb.OutdoorImageBasicDto.Id}, OutdoorImage: {jb.OutdoorImageBasicDto.Name}");

                    }
                    else
                    {
                        Debug.Log("OutdoorImage is null");
                    }
                    if (jb.ConnectionImageBasicDtos != null)
                    {
                        if (jb.ConnectionImageBasicDtos.Any())
                        {
                            foreach (var connectionImage in jb.ConnectionImageBasicDtos)
                            {
                                Debug.Log($"ConnectionImage: {connectionImage.Id}, ConnectionImage: {connectionImage.Name}");
                            }
                        }
                        else
                        {
                            Debug.Log("list ConnectionImage is empty");
                        }
                    }
                    else
                    {
                        Debug.Log("list ConnectionImage is null");
                    }
                }
            }
            else
            {
                Debug.Log("No JBs found");
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

    public async void GetListJBGeneral(int grapperId)
    {
        try
        {
            var jBBasicDtos = await _IJBService.GetListJBGeneralAsync(grapperId); //! Gọi _IJBService từ Application Layer
            if (jBBasicDtos != null && jBBasicDtos.Any())
            {
                Debug.Log($"jBDtos: {jBBasicDtos.Count}");
            }
            else
            {
                Debug.Log("No JBs found");
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

    public async void GetJBById(int JBId)
    {
        try
        {
            var jBResponseDto = await _IJBService.GetJBByIdAsync(JBId); // Gọi Service
            if (jBResponseDto != null)
            {
                Debug.Log($"jBResponseDto: {jBResponseDto.Name}, Location: {jBResponseDto.Location}");
                if (jBResponseDto.DeviceBasicDtos != null)
                {
                    if (jBResponseDto.DeviceBasicDtos.Any())
                    {
                        foreach (var device in jBResponseDto.DeviceBasicDtos)
                        {
                            Debug.Log($"Device: {device.Id}, Device: {device.Code}");
                        }
                    }
                    else
                    {
                        Debug.Log("list Device is empty");
                    }
                }
                else
                {
                    Debug.Log("list Device is null");
                }

                if (jBResponseDto.ModuleBasicDtos != null)
                {
                    if (jBResponseDto.ModuleBasicDtos.Any())
                    {
                        foreach (var module in jBResponseDto.ModuleBasicDtos)
                        {
                            Debug.Log($"Module: {module.Id}, Module: {module.Name}");
                        }
                    }
                    else
                    {
                        Debug.Log("list Module is empty");
                    }
                }
                else
                {
                    Debug.Log("list Module is null");
                }


                if (jBResponseDto.OutdoorImageBasicDto != null)
                {
                    Debug.Log($"OutdoorImage: {jBResponseDto.OutdoorImageBasicDto.Id}, OutdoorImage: {jBResponseDto.OutdoorImageBasicDto.Name}");
                }
                else
                {
                    Debug.Log("OutdoorImage is null");
                }


                if (jBResponseDto.ConnectionImageBasicDtos != null)
                {
                    if (jBResponseDto.ConnectionImageBasicDtos.Any())
                    {
                        foreach (var connectionImage in jBResponseDto.ConnectionImageBasicDtos)
                        {
                            Debug.Log($"ConnectionImage: {connectionImage.Id}, ConnectionImage: {connectionImage.Name}");
                        }
                    }
                    else
                    {
                        Debug.Log("list ConnectionImage is empty");
                    }
                }

                else
                {
                    Debug.Log("list ConnectionImage is null");
                }
            }
            else
            {
                Debug.Log("jBResponseDto not found");
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

    public async void CreateNewJB(int grapperId, JBRequestDto jBRequestDto)
    {
        try
        {
            bool result = await _IJBService.CreateNewJBAsync(grapperId, jBRequestDto);
            Debug.Log(result ? "JB created successfully" : "Failed to create JB");
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
    public async void UpdateJB(int JBId, JBRequestDto jBRequestDto)
    {
        JBId = GlobalVariable.JBId;
        try
        {
            bool result = await _IJBService.UpdateJBAsync(JBId, jBRequestDto);
            Debug.Log(result ? "JB updated successfully" : "Failed to update JB");
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
    public async void DeleteJB(int JBId)
    {
        JBId = GlobalVariable.JBId;
        try
        {
            bool result = await _IJBService.DeleteJBAsync(JBId);
            Debug.Log(result ? "JB deleted successfully" : "Failed to delete JB");
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