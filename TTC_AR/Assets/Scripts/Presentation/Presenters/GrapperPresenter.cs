using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationLayer.Dtos.Grapper;
using ApplicationLayer.Interfaces;
using ApplicationLayer.Dtos.Image;
using Unity.VisualScripting;
using ApplicationLayer.Dtos.JB;
using System.Diagnostics;
using ApplicationLayer.Dtos.Device;
using ApplicationLayer.Dtos.AdapterSpecification;
using ApplicationLayer.Dtos.Module;
public class GrapperPresenter
{
    private readonly IGrapperView _view;
    private readonly IGrapperService _service;

    public GrapperPresenter(
        IGrapperView view,
        IGrapperService service)
    {
        _view = view;
        _service = service;
    }

    //! Get list Grapper chỉ có Id và Code
    public async void LoadListGrapper(int grapperId)
    {
        GlobalVariable.APIRequestType.Add("GET_Grapper_List");
        _view.ShowLoading("Đang tải dữ liệu...");
        try
        {
            var GrapperBasicDtos = await _service.GetListGrapperAsync(grapperId);
            if (GrapperBasicDtos != null)
            {
                if (GrapperBasicDtos.Any())
                {
                    var models = GrapperBasicDtos.Select(dto => ConvertFromBasicDto(dto)).ToList();

                    _view.DisplayList(models);
                }
                else
                {
                    var models = new List<GrapperInformationModel>();
                    _view.DisplayList(models);
                }
                _view.ShowSuccess();
            }
            else
            {
                _view.ShowError("No Grappers found");
            }
        }
        catch (Exception ex)
        {
            _view.ShowError($"Error: {ex.Message}");
            UnityEngine.Debug.Log("Error: " + ex.Message);
        }
        finally
        {
            _view.HideLoading();
            GlobalVariable.APIRequestType.Remove("GET_Grapper_List_General");
        }
    }

    //! GET Grapper Detail với đầy đủ thông tin
    public async void LoadDetailById(int GrapperId)
    {
        GlobalVariable.APIRequestType.Add("GET_Grapper");
        _view.ShowLoading("Đang tải dữ liệu...");
        try
        {
            UnityEngine.Debug.Log("Run Presenter");
            var GrapperResponseDto = await _service.GetGrapperByIdAsync(GrapperId);
            if (GrapperResponseDto != null)
            {
                var model = ConvertFromResponseDto(GrapperResponseDto);
                if (model != null)
                {
                    UnityEngine.Debug.Log("Get Grapper Detail Successfully");
                }
                UnityEngine.Debug.Log(model.Name + model.Id);
                _view.DisplayDetail(model);
                _view.ShowSuccess();
            }
            else
            {
                _view.ShowError("Grapper not found");
            }
        }
        catch (Exception ex)
        {
            _view.ShowError($"Error: {ex.Message}");
        }
        finally
        {
            _view.HideLoading();
            GlobalVariable.APIRequestType.Remove("GET_Grapper");
        }
    }
    public async void CreateNewGrapper(int grapperId, GrapperInformationModel model)
    {
        GlobalVariable.APIRequestType.Add("POST_Grapper");
        _view.ShowLoading("Đang thực hiện...");
        try
        {
            var dto = ConvertToRequestDto(model);

            var result = await _service.CreateNewGrapperAsync(grapperId, dto);

            if (result)
            {
                _view.ShowSuccess(); // Chỉ hiển thị thành công nếu result == true
            }
            else
            {
                _view.ShowError("Create New Grapper failed");
            }
        }
        catch (Exception ex)
        {
            _view.ShowError($"Error: {ex.Message}");
        }
        finally
        {
            _view.HideLoading();
            GlobalVariable.APIRequestType.Remove("POST_Grapper");
        }
    }

    public async void UpdateGrapper(int GrapperId, GrapperInformationModel model)
    {
        GlobalVariable.APIRequestType.Add("PUT_Grapper");
        _view.ShowLoading("Đang thực hiện...");
        try
        {
            UnityEngine.Debug.Log("Run Presenter");
            var dto = ConvertToRequestDto(model);
            UnityEngine.Debug.Log("Convert to Request DTO Successfully");
            var result = await _service.UpdateGrapperAsync(GrapperId, dto);
            UnityEngine.Debug.Log("Run Service Successfully");

            if (result)
            {
                _view.ShowSuccess(); // Chỉ hiển thị thành công nếu result == true
            }
            else
            {
                _view.ShowError("Update Grapper failed");
            }
        }
        catch (Exception ex)
        {
            _view.ShowError($"Error: {ex.Message}");
        }
        finally
        {
            _view.HideLoading();
            GlobalVariable.APIRequestType.Remove("PUT_Grapper");
        }
    }
    public async void DeleteGrapper(int GrapperId)
    {
        GlobalVariable.APIRequestType.Add("DELETE_Grapper");
        _view.ShowLoading("Đang thực hiện...");
        try
        {
            var result = await _service.DeleteGrapperAsync(GrapperId);
            if (result)
            {
                _view.ShowSuccess(); // Chỉ hiển thị thành công nếu result == true
            }
            else
            {
                _view.ShowError("Delete Grapper failed");
            }
        }
        catch (Exception ex)
        {
            _view.ShowError($"Error: {ex.Message}");
        }
        finally
        {
            _view.HideLoading();
            GlobalVariable.APIRequestType.Remove("DELETE_Grapper");
        }
    }



    //! Dto => Model
    private GrapperInformationModel ConvertFromResponseDto(GrapperResponseDto dto)
    {
        return new GrapperInformationModel(
            id: dto.Id,
            name: dto.Name
           );
    }
    private GrapperInformationModel ConvertFromBasicDto(GrapperBasicDto dto)
    {
        return new GrapperInformationModel(
             id: dto.Id,
             name: dto.Name);
    }

    //! Model => Dto
    private GrapperRequestDto ConvertToRequestDto(GrapperInformationModel model)
    {
        return new GrapperRequestDto
        (
            name: model.Name
        );
    }
}