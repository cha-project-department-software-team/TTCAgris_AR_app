using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationLayer.Dtos.Module;
using ApplicationLayer.Interfaces;
using ApplicationLayer.Dtos.JB;
using ApplicationLayer.Dtos.Device;
using ApplicationLayer.Dtos.AdapterSpecification;
using ApplicationLayer.Dtos.Rack;
using ApplicationLayer.Dtos.ModuleSpecification;
public class ModulePresenter
{
    private readonly IModuleView _view;
    private readonly IModuleService _service;

    public ModulePresenter(
        IModuleView view,
        IModuleService service)
    {
        _view = view;
        _service = service;
    }

    //! Get list Module chỉ có Id và Code
    public async void LoadListModule(int grapperId)
    {
        GlobalVariable.APIRequestType.Add("GET_Module_List");
        _view.ShowLoading("Đang tải dữ liệu...");
        try
        {
            var ModuleBasicDtos = await _service.GetListModuleAsync(grapperId);
            if (ModuleBasicDtos != null)
            {
                if (ModuleBasicDtos.Any())
                {
                    var models = ModuleBasicDtos.Select(dto => ConvertFromBasicDto(dto)).ToList();

                    _view.DisplayList(models);
                }
                else
                {
                    var models = new List<ModuleInformationModel>();
                    _view.DisplayList(models);
                }
                _view.ShowSuccess("Tải danh sách thành công");
            }
            else
            {
                _view.ShowError("No Modules found");
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
            GlobalVariable.APIRequestType.Remove("GET_Module_List_General");
        }
    }

    //! GET Module Detail với đầy đủ thông tin
    public async void LoadDetailById(int moduleId)
    {
        GlobalVariable.APIRequestType.Add("GET_Module");
        _view.ShowLoading("Đang tải dữ liệu...");
        try
        {

            UnityEngine.Debug.Log(moduleId);
            var ModuleResponseDto = await _service.GetModuleByIdAsync(moduleId);
            if (ModuleResponseDto != null)
            {
                var model = ConvertFromResponseDto(ModuleResponseDto);
                _view.DisplayDetail(model);
                _view.ShowSuccess("Tải dữ liệu thành công");
            }
            else
            {
                _view.ShowError("Module not found");
            }
        }
        catch (Exception ex)
        {
            _view.ShowError($"Error: {ex.Message}");
        }
        finally
        {
            _view.HideLoading();
            GlobalVariable.APIRequestType.Remove("GET_Module");
        }
    }
    public async void CreateNewModule(int grapperId, ModuleInformationModel model)
    {
        GlobalVariable.APIRequestType.Add("POST_Module");
        _view.ShowLoading("Đang thực hiện...");
        try
        {
            var dto = ConvertToRequestDto(model);

            var result = await _service.CreateNewModuleAsync(grapperId, dto);

            if (result)
            {
                _view.ShowSuccess("");
            }
            else
            {
                _view.ShowError("Create New Module failed");
            }
        }
        catch (Exception ex)
        {
            _view.ShowError($"Error: {ex.Message}");
        }
        finally
        {
            _view.HideLoading();
            GlobalVariable.APIRequestType.Remove("POST_Module");
        }
    }

    public async void UpdateModule(int moduleId, ModuleInformationModel model)
    {
        GlobalVariable.APIRequestType.Add("PUT_Module");
        _view.ShowLoading("Đang thực hiện...");
        try
        {
            UnityEngine.Debug.Log("Run Presenter");
            var dto = ConvertToRequestDto(model);
            UnityEngine.Debug.Log("Convert to Request DTO Successfully");
            var result = await _service.UpdateModuleAsync(moduleId, dto);
            UnityEngine.Debug.Log("Run Service Successfully");

            if (result)
            {
                _view.ShowSuccess(""); // Chỉ hiển thị thành công nếu result == true
            }
            else
            {
                _view.ShowError("Update Module failed");
            }
        }
        catch (Exception ex)
        {
            _view.ShowError($"Error: {ex.Message}");
        }
        finally
        {
            _view.HideLoading();
            GlobalVariable.APIRequestType.Remove("PUT_Module");
        }
    }
    public async void DeleteModule(int moduleId)
    {
        GlobalVariable.APIRequestType.Add("DELETE_Module");
        _view.ShowLoading("Đang thực hiện...");
        try
        {
            var result = await _service.DeleteModuleAsync(moduleId);
            if (result)
            {
                _view.ShowSuccess(""); // Chỉ hiển thị thành công nếu result == true
            }
            else
            {
                _view.ShowError("Delete Module failed");
            }
        }
        catch (Exception ex)
        {
            _view.ShowError($"Error: {ex.Message}");
        }
        finally
        {
            _view.HideLoading();
            GlobalVariable.APIRequestType.Remove("DELETE_Module");
        }
    }



    //! Dto => Model
    private ModuleInformationModel ConvertFromResponseDto(ModuleResponseDto dto)
    {
        return new ModuleInformationModel(
            id: dto.Id,
            name: dto.Name,
            rack: dto.RackBasicDto != null ? new RackInformationModel(
                id: dto.RackBasicDto.Id,
                name: dto.RackBasicDto.Name
            ) : null,
            grapper: new GrapperInformationModel(
                id: dto.GrapperBasicDto.Id,
                name: dto.GrapperBasicDto.Name
            ),
            listJBInformationModel: dto.JBBasicDtos.Any() ? dto.JBBasicDtos.Select(
                jb => new JBInformationModel(
                    id: jb.Id,
                    name: jb.Name
                )
            ).ToList() : new List<JBInformationModel>(),
            listDeviceInformationModel: dto.DeviceBasicDtos.Any() ? dto.DeviceBasicDtos.Select(
                Device => new DeviceInformationModel(
                    id: Device.Id,
                    code: Device.Code
                )
            ).ToList() : new List<DeviceInformationModel>(),
            moduleSpecificationModel: dto.ModuleSpecificationBasicDto != null ? new ModuleSpecificationModel(
                id: dto.ModuleSpecificationBasicDto.Id,
                code: dto.ModuleSpecificationBasicDto.Code
            ) : null,
            adapterSpecificationModel: dto.AdapterSpecificationBasicDto != null ? new AdapterSpecificationModel(
                id: dto.AdapterSpecificationBasicDto.Id,
                code: dto.AdapterSpecificationBasicDto.Code
            ) : null);
    }
    private ModuleInformationModel ConvertFromBasicDto(ModuleBasicDto dto)
    {
        return new ModuleInformationModel(
             id: dto.Id,
             name: dto.Name);
    }

    //! Model => Dto
    private ModuleRequestDto ConvertToRequestDto(ModuleInformationModel model)
    {
        return new ModuleRequestDto
        (
            name: model.Name,
            rackBasicDto: model.Rack != null ? new RackBasicDto(
                id: model.Rack.Id,
                name: model.Rack.Name
            ) : null,
            jBBasicDtos: model.ListJBInformationModel.Any() ? model.ListJBInformationModel.Select(
                jb => new JBBasicDto(
                    id: jb.Id,
                    name: jb.Name
                )
            ).ToList() : new List<JBBasicDto>(),
            deviceBasicDtos: model.ListDeviceInformationModel.Any() ? model.ListDeviceInformationModel.Select(
                Device => new DeviceBasicDto(
                    id: Device.Id,
                    code: Device.Code
                )
            ).ToList() : new List<DeviceBasicDto>(),
            moduleSpecificationBasicDto: model.ModuleSpecificationModel != null ? new ModuleSpecificationBasicDto(
                id: model.ModuleSpecificationModel.Id,
                code: model.ModuleSpecificationModel.Code
            ) : null,
            adapterSpecificationBasicDto: model.AdapterSpecificationModel != null ? new AdapterSpecificationBasicDto(
                id: model.AdapterSpecificationModel.Id,
                code: model.AdapterSpecificationModel.Code
            ) : null
        );
    }
}