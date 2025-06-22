
using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationLayer.Dtos.Rack;
using ApplicationLayer.Interfaces;
using ApplicationLayer.Dtos.Image;
using Unity.VisualScripting;
using ApplicationLayer.Dtos.JB;
using System.Diagnostics;
using ApplicationLayer.Dtos.Device;
using ApplicationLayer.Dtos.AdapterSpecification;
using ApplicationLayer.Dtos.Module;
public class RackPresenter
{
    private readonly IRackView _view;
    private readonly IRackService _service;

    public RackPresenter(
        IRackView view,
        IRackService service)
    {
        _view = view;
        _service = service;
    }

    //! Get list Rack chỉ có Id và Code
    public async void LoadListRack(int grapperId)
    {
        GlobalVariable.APIRequestType.Add("GET_Rack_List");
        _view.ShowLoading("Đang tải dữ liệu...");
        try
        {
            var RackBasicDtos = await _service.GetListRackAsync(grapperId);
            if (RackBasicDtos != null)
            {
                if (RackBasicDtos.Any())
                {
                    var models = RackBasicDtos.Select(dto => ConvertFromBasicDto(dto)).ToList();

                    _view.DisplayList(models);
                }
                else
                {
                    var models = new List<RackInformationModel>();
                    _view.DisplayList(models);
                }
                _view.ShowSuccess();
            }
            else
            {
                _view.ShowError("No Racks found");
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
            GlobalVariable.APIRequestType.Remove("GET_Rack_List_General");
        }
    }

    //! GET Rack Detail với đầy đủ thông tin
    public async void LoadDetailById(int rackId)
    {
        GlobalVariable.APIRequestType.Add("GET_Rack");
        _view.ShowLoading("Đang tải dữ liệu...");
        try
        {
            UnityEngine.Debug.Log("Run Presenter");
            var RackResponseDto = await _service.GetRackByIdAsync(rackId);
            if (RackResponseDto != null)
            {
                var model = ConvertFromResponseDto(RackResponseDto);
                if (model != null)
                {
                    UnityEngine.Debug.Log("Get Rack Detail Successfully");
                }
                UnityEngine.Debug.Log(model.Name + model.Id);
                _view.DisplayDetail(model);
                _view.ShowSuccess();
            }
            else
            {
                _view.ShowError("Rack not found");
            }
        }
        catch (Exception ex)
        {
            _view.ShowError($"Error: {ex.Message}");
        }
        finally
        {
            _view.HideLoading();
            GlobalVariable.APIRequestType.Remove("GET_Rack");
        }
    }
    public async void CreateNewRack(int grapperId, RackInformationModel model)
    {
        GlobalVariable.APIRequestType.Add("POST_Rack");
        _view.ShowLoading("Đang thực hiện...");
        try
        {
            var dto = ConvertToRequestDto(model);

            var result = await _service.CreateNewRackAsync(grapperId, dto);

            if (result)
            {
                _view.ShowSuccess(); // Chỉ hiển thị thành công nếu result == true
            }
            else
            {
                _view.ShowError("Create New Rack failed");
            }
        }
        catch (Exception ex)
        {
            _view.ShowError($"Error: {ex.Message}");
        }
        finally
        {
            _view.HideLoading();
            GlobalVariable.APIRequestType.Remove("POST_Rack");
        }
    }

    public async void UpdateRack(int rackId, RackInformationModel model)
    {
        GlobalVariable.APIRequestType.Add("PUT_Rack");
        _view.ShowLoading("Đang thực hiện...");
        try
        {
            UnityEngine.Debug.Log("Run Presenter");
            var dto = ConvertToRequestDto(model);
            UnityEngine.Debug.Log("Convert to Request DTO Successfully");
            var result = await _service.UpdateRackAsync(rackId, dto);
            UnityEngine.Debug.Log("Run Service Successfully");

            if (result)
            {
                _view.ShowSuccess(); // Chỉ hiển thị thành công nếu result == true
            }
            else
            {
                _view.ShowError("Update Rack failed");
            }
        }
        catch (Exception ex)
        {
            _view.ShowError($"Error: {ex.Message}");
        }
        finally
        {
            _view.HideLoading();
            GlobalVariable.APIRequestType.Remove("PUT_Rack");
        }
    }
    public async void DeleteRack(int rackId)
    {
        GlobalVariable.APIRequestType.Add("DELETE_Rack");
        _view.ShowLoading("Đang thực hiện...");
        try
        {
            var result = await _service.DeleteRackAsync(rackId);
            if (result)
            {
                _view.ShowSuccess(); // Chỉ hiển thị thành công nếu result == true
            }
            else
            {
                _view.ShowError("Delete Rack failed");
            }
        }
        catch (Exception ex)
        {
            _view.ShowError($"Error: {ex.Message}");
        }
        finally
        {
            _view.HideLoading();
            GlobalVariable.APIRequestType.Remove("DELETE_Rack");
        }
    }



    //! Dto => Model
    private RackInformationModel ConvertFromResponseDto(RackResponseDto dto)
    {
        return new RackInformationModel(
            id: dto.Id,
            name: dto.Name,
           listModuleInformationModel: dto.ModuleBasicDtos.Any() ? dto.ModuleBasicDtos.Select(
                module => new ModuleInformationModel(
                    id: module.Id,
                    name: module.Name
                )
            ).ToList() : new List<ModuleInformationModel>());
    }
    private RackInformationModel ConvertFromBasicDto(RackBasicDto dto)
    {
        return new RackInformationModel(
             id: dto.Id,
             name: dto.Name);
    }

    //! Model => Dto
    private RackRequestDto ConvertToRequestDto(RackInformationModel model)
    {
        return new RackRequestDto
        (
            name: model.Name,
            moduleBasicDtos: model.ListModuleInformationModel.Any() ? model.ListModuleInformationModel.Select(
                module => new ModuleBasicDto(
                    id: module.Id,
                    name: module.Name
                )
            ).ToList() : new List<ModuleBasicDto>()

        );
    }
}
