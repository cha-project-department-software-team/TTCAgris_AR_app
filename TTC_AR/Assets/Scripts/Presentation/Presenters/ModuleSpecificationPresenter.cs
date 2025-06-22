using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ApplicationLayer.Dtos.ModuleSpecification;
using ApplicationLayer.Interfaces;

public class ModuleSpecificationPresenter
{
    private readonly IModuleSpecificationView _view;
    private readonly IModuleSpecificationService _service;

    public ModuleSpecificationPresenter(
        IModuleSpecificationView view,
        IModuleSpecificationService service)
    {
        _view = view;
        _service = service;
    }

    public async void LoadListModuleSpecification(int companyId)
    {
        GlobalVariable.APIRequestType.Add("GET_ModuleSpecification_List");
        _view.ShowLoading("Đang tải dữ liệu...");

        try
        {
            var ModuleSpecificationBasicDto = await _service.GetListModuleSpecificationAsync(companyId);
            if (ModuleSpecificationBasicDto != null)
            {
                if (ModuleSpecificationBasicDto.Any())
                {
                    var models = ModuleSpecificationBasicDto.Select(dto => ConvertFromBasicDto(dto)).ToList();
                    _view.DisplayList(models);
                    _view.ShowSuccess("Tải danh sách thành công");
                }
                else
                {
                    var models = new List<ModuleSpecificationModel>();
                    _view.DisplayList(models);
                    _view.ShowSuccess("Tải danh sách thành công nhưng danh sách trống");
                }
            }
            else
            {
                _view.ShowError("Đã có lỗi xảy ra khi tải danh sách loại Module. Vui lòng thử lại sau");
            }
        }
        catch (Exception)
        {
            _view.ShowError("Đã có lỗi xảy ra khi tải danh sách loại Module. Vui lòng thử lại sau");
        }
        finally
        {
            _view.HideLoading();
            GlobalVariable.APIRequestType.Remove("GET_ModuleSpecification_List");
        }
    }

    public async void LoadDetailById(int moduleId)
    {
        GlobalVariable.APIRequestType.Add("GET_ModuleSpecification");
        _view.ShowLoading("Đang tải dữ liệu...");

        try
        {
            var dto = await _service.GetModuleSpecificationByIdAsync(moduleId);
            if (dto != null)
            {
                var model = ConvertFromResponseDto(dto);
                _view.DisplayDetail(model);
                _view.ShowSuccess("Tải dữ liệu thành công");
            }
            else
            {
                _view.ShowError("Đã có lỗi xảy ra khi tải dữ liệu loại Module. Vui lòng thử lại sau");
            }
        }
        catch (Exception)
        {
            _view.ShowError("Đã có lỗi xảy ra khi tải dữ liệu loại Module. Vui lòng thử lại sau");
        }
        finally
        {
            _view.HideLoading();
            GlobalVariable.APIRequestType.Remove("GET_ModuleSpecification");
        }
    }
    public async void CreateNewModuleSpecification(int companyId, ModuleSpecificationModel model)
    {
        GlobalVariable.APIRequestType.Add("POST_ModuleSpecification");
        _view.ShowLoading("Đang thực hiện...");

        try
        {
            var dto = ConvertToRequestDto(model);
            var result = await _service.CreateNewModuleSpecificationAsync(companyId, dto);

            if (result)
            {
                _view.ShowSuccess("Tạo loại Module mới thành công");
            }
            else
            {
                _view.ShowError("Tạo loại Module mới thất bại");
            }
        }
        catch (Exception)
        {
            _view.ShowError("Tạo loại Module mới thất bại");
        }
        finally
        {
            _view.HideLoading();
            GlobalVariable.APIRequestType.Remove("POST_ModuleSpecification");
        }
    }


    public async void UpdateModuleSpecification(int moduleSpecificationId, ModuleSpecificationModel model)
    {
        GlobalVariable.APIRequestType.Add("PUT_ModuleSpecification");
        _view.ShowLoading("Đang thực hiện...");

        try
        {
            var dto = ConvertToRequestDto(model);
            var result = await _service.UpdateModuleSpecificationAsync(moduleSpecificationId, dto);
            if (result)
            {
                _view.ShowSuccess("Cập nhật loại Module thành công");
            }
            else
            {
                _view.ShowError("Cập nhật loại Module thất bại");
            }
        }
        catch (Exception)
        {
            _view.ShowError("Cập nhật loại Module thất bại");

        }
        finally
        {
            _view.HideLoading();
            GlobalVariable.APIRequestType.Remove("PUT_ModuleSpecification");
        }
    }
    public async void DeleteModuleSpecification(int moduleSpecificationId)
    {
        GlobalVariable.APIRequestType.Add("DELETE_ModuleSpecification");
        _view.ShowLoading("Đang thực hiện...");

        try
        {
            var result = await _service.DeleteModuleSpecificationAsync(moduleSpecificationId);
            if (result)
            {
                _view.ShowSuccess("Xóa loại Module thành công");
            }
            else if (result == false)
            {
                _view.ShowError("Xóa loại Module thất bại");
            }
        }
        catch (Exception ex)
        {
            _view.ShowError($"Error: {ex.Message}");
        }
        finally
        {
            _view.HideLoading();
            GlobalVariable.APIRequestType.Remove("DELETE_ModuleSpecification");
        }
    }

    //! Dto => Model
    private ModuleSpecificationModel ConvertFromResponseDto(ModuleSpecificationResponseDto dto)
    {
        return new ModuleSpecificationModel(
            id: dto.Id,
            code: dto.Code,
            type: dto.Type,
            compatibleTBUs: dto.CompatibleTBUs,
            flexbusCurrent: dto.FlexbusCurrent,
            numOfIO: dto.NumOfIO,
            operatingCurrent: dto.OperatingCurrent,
            operatingVoltage: dto.OperatingVoltage,
            signalType: dto.SignalType,
            alarm: dto.Alarm,
            note: dto.Note,
            pdfManual: dto.PdfManual
        );
    }
    private ModuleSpecificationModel ConvertFromBasicDto(ModuleSpecificationBasicDto dto)
    {
        return new ModuleSpecificationModel(
            id: dto.Id,
            code: dto.Code
        );
    }

    //! Model => Dto
    private ModuleSpecificationRequestDto ConvertToRequestDto(ModuleSpecificationModel model)
    {
        return new ModuleSpecificationRequestDto(
            code: model.Code,
            type: model.Type,
            numOfIO: model.NumOfIO,
            signalType: model.SignalType,
            compatibleTBUs: model.CompatibleTBUs,
            operatingVoltage: model.OperatingVoltage,
            operatingCurrent: model.OperatingCurrent,
            flexbusCurrent: model.FlexbusCurrent,
            alarm: model.Alarm,
            note: model.Note,
            pdfManual: model.PdfManual
        )
       ;
    }
}