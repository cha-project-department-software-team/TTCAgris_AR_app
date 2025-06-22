using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationLayer.Dtos.Mcc;
using ApplicationLayer.Interfaces;
using ApplicationLayer.Dtos.FieldDevice;

public class MccPresenter
{
    private readonly IMccView _view;
    private readonly IMccService _service;

    public MccPresenter(
        IMccView view,
        IMccService service)
    {
        _view = view;
        _service = service;
    }

    public async void LoadListMcc(int grapperId)
    {
        GlobalVariable.APIRequestType.Add("GET_Mcc_List");
        _view.ShowLoading("Đang tải dữ liệu...");
        try
        {
            var MccBasicDto = await _service.GetListMccAsync(grapperId);
            if (MccBasicDto != null)
            {
                if (MccBasicDto.Any())
                {
                    var models = MccBasicDto.Select(dto => ConvertFromBasicDto(dto)).ToList();
                    _view.DisplayList(models);

                }
                else
                {
                    var models = new List<MccInformationModel>();
                    _view.DisplayList(models);
                }
                _view.ShowSuccess();

            }
            else
            {
                _view.ShowError("No Mccs found");
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
            GlobalVariable.APIRequestType.Remove("GET_Mcc_List");
        }
    }

    public async void LoadDetailById(int mccId)
    {
        GlobalVariable.APIRequestType.Add("GET_Mcc");
        _view.ShowLoading("Đang tải dữ liệu...");
        try
        {
            var dto = await _service.GetMccByIdAsync(mccId);
            if (dto != null)
            {
                var model = ConvertFromResponseDto(dto);
                _view.DisplayDetail(model);
                _view.ShowSuccess();
            }
            else
            {
                _view.ShowError("Mcc not found");
            }
        }
        catch (Exception ex)
        {
            _view.ShowError($"Error: {ex.Message}");
        }
        finally
        {
            _view.HideLoading();
            GlobalVariable.APIRequestType.Remove("GET_Mcc");
        }
    }
    public async void CreateNewMcc(int grapperId, MccInformationModel model)
    {
        GlobalVariable.APIRequestType.Add("POST_Mcc");
        _view.ShowLoading("Đang thực hiện...");
        try
        {
            var dto = ConvertToRequestDto(model);
            var result = await _service.CreateNewMccAsync(grapperId, dto);
            if (result)
            {
                _view.ShowSuccess(); // Chỉ hiển thị thành công nếu result == true
            }
            else
            {
                _view.ShowError("Create New Mcc failed");
            }
        }
        catch (Exception ex)
        {
            _view.ShowError($"Error: {ex.Message}");
        }
        finally
        {
            _view.HideLoading();
            GlobalVariable.APIRequestType.Remove("POST_Mcc");
        }
    }


    public async void UpdateMcc(int mccId, MccInformationModel model)
    {
        GlobalVariable.APIRequestType.Add("PUT_Mcc");
        _view.ShowLoading("Đang thực hiện...");

        try
        {
            var dto = ConvertToRequestDto(model);
            var result = await _service.UpdateMccAsync(mccId, dto);
            if (result)
            {
                _view.ShowSuccess(); // Chỉ hiển thị thành công nếu result == true
            }
            else
            {
                _view.ShowError("Update Mcc failed");
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
            GlobalVariable.APIRequestType.Remove("PUT_Mcc");
        }
    }
    public async void DeleteMcc(int mccId)
    {
        GlobalVariable.APIRequestType.Add("DELETE_Mcc");
        _view.ShowLoading("Đang thực hiện...");

        try
        {
            var result = await _service.DeleteMccAsync(mccId);
            if (result)
            {
                _view.ShowSuccess(); // Chỉ hiển thị thành công nếu result == true
            }
            else
            {
                _view.ShowError("Delete Mcc failed");
            }
        }
        catch (Exception ex)
        {
            _view.ShowError($"Error: {ex.Message}");
        }
        finally
        {
            _view.HideLoading();
            GlobalVariable.APIRequestType.Remove("DELETE_Mcc");
        }
    }


    //! Dto => Model
    private MccInformationModel ConvertFromResponseDto(MccResponseDto dto)
    {
        return new MccInformationModel(
            id: dto.Id,
            cabinetCode: dto.CabinetCode,
            brand: dto.Brand,
            listFieldDeviceInformation: dto.FieldDeviceBasicDtos?.Select(fieldDeviceDto => new FieldDeviceInformationModel(
                id: fieldDeviceDto.Id,
                name: fieldDeviceDto.Name
               )).ToList(),
            note: dto.Note
        );
    }
    private MccInformationModel ConvertFromBasicDto(MccBasicDto dto)
    {
        return new MccInformationModel(
            id: dto.Id,
            cabinetCode: dto.CabinetCode
        );
    }

    //! Model => Dto
    private MccRequestDto ConvertToRequestDto(MccInformationModel model)
    {
        return new MccRequestDto(
            cabinetCode: model.CabinetCode,
            brand: model.Brand,
            fieldDeviceBasicDtos: model.ListFieldDeviceInformation?.Select(fieldDeviceModel => new FieldDeviceBasicDto(
                id: fieldDeviceModel.Id,
                name: fieldDeviceModel.Name
            )).ToList(),
            note: model.Note
        )
       ;
    }
}