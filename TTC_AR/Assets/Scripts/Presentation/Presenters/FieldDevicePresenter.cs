using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ApplicationLayer.Dtos.FieldDevice;
using ApplicationLayer.Dtos.Image;
using ApplicationLayer.Interfaces;
using Unity.VisualScripting;

public class FieldDevicePresenter
{
    private readonly IFieldDeviceView _view;
    private readonly IFieldDeviceService _service;

    public FieldDevicePresenter(
        IFieldDeviceView view,
        IFieldDeviceService service)
    {
        _view = view;
        _service = service;
    }

    public async void LoadListFieldDevice(int grapperId)
    {
        GlobalVariable.APIRequestType.Add("GET_FieldDevice_List");
        _view.ShowLoading("Đang tải dữ liệu...");

        try
        {
            var FieldDeviceBasicDto = await _service.GetListFieldDeviceAsync(grapperId);
            if (FieldDeviceBasicDto != null)
            {
                if (FieldDeviceBasicDto.Any())
                {
                    var models = FieldDeviceBasicDto.Select(dto => ConvertFromBasicDto(dto)).ToList();

                    _view.DisplayList(models);

                }
                else
                {
                    var models = new List<FieldDeviceInformationModel>();
                    _view.DisplayList(models);
                }
                _view.ShowSuccess();

            }
            else
            {
                _view.ShowError("No FieldDevices found");
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
            GlobalVariable.APIRequestType.Remove("GET_FieldDevice_List");
        }
    }

    public async void LoadDetailById(int fieldDeviceId)
    {
        GlobalVariable.APIRequestType.Add("GET_FieldDevice");
        _view.ShowLoading("Đang tải dữ liệu...");

        try
        {
            var dto = await _service.GetFieldDeviceByIdAsync(fieldDeviceId);
            if (dto != null)
            {
                var model = ConvertFromResponseDto(dto);
                if (model != null)
                {
                    _view.DisplayDetail(model);
                    _view.ShowSuccess();
                }
                else
                {
                    UnityEngine.Debug.Log("FieldDevice not found1");
                    _view.ShowError("FieldDevice not found");
                }
            }
            else
            {
                UnityEngine.Debug.Log("FieldDevice not found2");
                _view.ShowError("FieldDevice not found");
            }
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.Log("Error: " + ex.Message);
            _view.ShowError($"Error: {ex.Message}");
        }
        finally
        {
            _view.HideLoading();
            GlobalVariable.APIRequestType.Remove("GET_FieldDevice");
        }
    }
    public async void CreateNewFieldDevice(int grapperId, FieldDeviceInformationModel model)
    {
        GlobalVariable.APIRequestType.Add("POST_FieldDevice");
        _view.ShowLoading("Đang thực hiện...");

        try
        {
            var dto = ConvertToRequestDto(model);
            var result = await _service.CreateNewFieldDeviceAsync(grapperId, dto);
            if (result)
            {
                _view.ShowSuccess(); // Chỉ hiển thị thành công nếu result == true
            }
            else
            {
                _view.ShowError("Create New FieldDevice failed");
            }
        }
        catch (Exception ex)
        {
            _view.ShowError($"Error: {ex.Message}");
        }
        finally
        {
            _view.HideLoading();
            GlobalVariable.APIRequestType.Remove("POST_FieldDevice");
        }
    }


    public async void UpdateFieldDevice(int fieldDeviceId, FieldDeviceInformationModel model)
    {
        GlobalVariable.APIRequestType.Add("PUT_FieldDevice");
        _view.ShowLoading("Đang thực hiện...");

        try
        {
            var dto = ConvertToRequestDto(model);
            var result = await _service.UpdateFieldDeviceAsync(fieldDeviceId, dto);
            if (result)
            {
                _view.ShowSuccess(); // Chỉ hiển thị thành công nếu result == true
            }
            else
            {
                _view.ShowError("Update FieldDevice failed");
            }
        }
        catch (Exception ex)
        {
            _view.ShowError($"Error: {ex.Message}");
        }
        finally
        {
            _view.HideLoading();
            GlobalVariable.APIRequestType.Remove("PUT_FieldDevice");
        }
    }
    public async void DeleteFieldDevice(int fieldDeviceId)
    {
        GlobalVariable.APIRequestType.Add("DELETE_FieldDevice");
        _view.ShowLoading("Đang thực hiện...");

        try
        {
            var result = await _service.DeleteFieldDeviceAsync(fieldDeviceId);
            if (result)
            {
                _view.ShowSuccess(); // Chỉ hiển thị thành công nếu result == true
            }
            else
            {
                _view.ShowError("Delete FieldDevice failed");
            }
        }
        catch (Exception ex)
        {
            _view.ShowError($"Error: {ex.Message}");
        }
        finally
        {
            _view.HideLoading();
            GlobalVariable.APIRequestType.Remove("DELETE_FieldDevice");

        }
    }


    //! Dto => Model
    private FieldDeviceInformationModel ConvertFromResponseDto(FieldDeviceResponseDto dto)
    {
        return new FieldDeviceInformationModel(
            id: dto.Id,
            name: dto.Name,
           mcc: dto.Mcc != null ? new MccInformationModel(
                id: dto.Mcc.Id,
                cabinetCode: dto.Mcc.CabinetCode
            ) : null,
            ratedPower: dto.RatedPower,
            ratedCurrent: dto.RatedCurrent,
            activeCurrent: dto.ActiveCurrent,
            listConnectionImages: dto.ConnectionImages.Any() ? dto.ConnectionImages.Select(imageDto => new ImageInformationModel(
                id: imageDto.Id,
                name: imageDto.Name
            // url: imageDto.Url
            )).ToList() : new List<ImageInformationModel>(),
            note: dto.Note
        );
    }
    private FieldDeviceInformationModel ConvertFromBasicDto(FieldDeviceBasicDto dto)
    {
        return new FieldDeviceInformationModel(
            id: dto.Id,
            name: dto.Name
        );
    }

    //! Model => Dto
    private FieldDeviceRequestDto ConvertToRequestDto(FieldDeviceInformationModel model)
    {
        return new FieldDeviceRequestDto(
            name: model.Name,
            ratedPower: model.RatedPower,
            ratedCurrent: model.RatedCurrent,
            activeCurrent: model.ActiveCurrent,
            connectionImageBasicDtos: model.ListConnectionImages.Any() ? model.ListConnectionImages?.Select(imageModel => new ImageBasicDto(
                id: imageModel.Id,
                name: imageModel.Name
            )).ToList() : new List<ImageBasicDto>(),
            note: model.Note
        )
       ;
    }
}