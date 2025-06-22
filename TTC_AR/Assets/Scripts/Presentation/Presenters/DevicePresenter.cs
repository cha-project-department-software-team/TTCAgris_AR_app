using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationLayer.Dtos.Device;
using ApplicationLayer.Interfaces;
using ApplicationLayer.Dtos.Image;
using ApplicationLayer.Dtos.Module;
using Unity.VisualScripting;
using ApplicationLayer.Dtos.JB;
using System.Diagnostics;
public class DevicePresenter
{
    private readonly IDeviceView _view;
    private readonly IDeviceService _service;

    public DevicePresenter(
        IDeviceView view,
        IDeviceService service)
    {
        _view = view;
        _service = service;
    }


    //! Get list Device nhưng chỉ có Id và Name
    public async void LoadListDeviceInformationFromGrapper(int grapperId)
    {
        GlobalVariable.APIRequestType.Add("GET_Device_List_Information_FromGrapper");
        _view.ShowLoading("Đang tải dữ liệu...");
        try
        {
            var DeviceResponseDtos = await _service.GetListDeviceInformationFromGrapperAsync(grapperId);
            if (DeviceResponseDtos != null)
            {
                if (DeviceResponseDtos.Any())
                {
                    var models = DeviceResponseDtos.Select(dto => ConvertFromResponseDto(dto)).ToList();
                    _view.DisplayList(models);
                }
                else
                {
                    var models = new List<DeviceInformationModel>();
                    _view.DisplayList(models);
                }
                _view.ShowSuccess();
            }
            else
            {
                _view.ShowError("No Devices found");
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
            GlobalVariable.APIRequestType.Remove("GET_Device_List_Information_FromGrapper");
        }
    }


    //! Get list Device nhưng chỉ có Id và Name
    public async void LoadListDeviceInformationFromModule(int moduleId)
    {
        GlobalVariable.APIRequestType.Add("GET_Device_List_Information_FromModule");
        _view.ShowLoading("Đang tải dữ liệu...");
        try
        {
            var DeviceResponseDtos = await _service.GetListDeviceInformationFromModuleAsync(moduleId);
            if (DeviceResponseDtos != null)
            {
                if (DeviceResponseDtos.Any())
                {
                    var models = DeviceResponseDtos.Select(dto => ConvertFromBasicDto(dto)).ToList();
                    _view.DisplayList(models);
                }
                else
                {
                    var models = new List<DeviceInformationModel>();
                    _view.DisplayList(models);
                }
                _view.ShowSuccess();
            }
            else
            {
                _view.ShowError("No Devices found");
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
            GlobalVariable.APIRequestType.Remove("GET_Device_List_Information_FromModule");
        }
    }

    //! Get list Device chỉ có Id và Code
    public async void LoadListDeviceGeneral(int grapperId)
    {
        GlobalVariable.APIRequestType.Add("GET_Device_List_General");
        _view.ShowLoading("Đang tải dữ liệu...");
        try
        {
            var DeviceBasicDtos = await _service.GetListDeviceGeneralAsync(grapperId);
            if (DeviceBasicDtos != null)
            {
                if (DeviceBasicDtos.Any())
                {
                    var models = DeviceBasicDtos.Select(dto => ConvertFromBasicDto(dto)).ToList();

                    _view.DisplayList(models);
                }
                else
                {
                    var models = new List<DeviceInformationModel>();
                    _view.DisplayList(models);
                }
                _view.ShowSuccess();
            }
            else
            {
                _view.ShowError("No Devices found");
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
            GlobalVariable.APIRequestType.Remove("GET_Device_List_General");
        }
    }

    //! GET Device Detail với đầy đủ thông tin
    public async void LoadDetailById(int deviceId)
    {
        GlobalVariable.APIRequestType.Add("GET_Device");
        _view.ShowLoading("Đang tải dữ liệu...");
        try
        {
            UnityEngine.Debug.Log("Run Presenter");
            var DeviceResponseDto = await _service.GetDeviceByIdAsync(deviceId);
            if (DeviceResponseDto != null)
            {
                var model = ConvertFromResponseDto(DeviceResponseDto);
                if (model != null)
                {
                    _view.DisplayDetail(model);
                    _view.ShowSuccess();
                }
                else
                {
                    _view.ShowError("Device not found");
                }
            }
            else
            {
                _view.ShowError("Device not found");
            }
        }
        catch (Exception ex)
        {
            _view.ShowError($"Error: {ex.Message}");
        }
        finally
        {
            _view.HideLoading();
            GlobalVariable.APIRequestType.Remove("GET_Device");
        }
    }
    public async void CreateNewDevice(int grapperId, DeviceInformationModel model)
    {
        GlobalVariable.APIRequestType.Add("POST_Device");
        _view.ShowLoading("Đang thực hiện...");
        try
        {
            var dto = ConvertToRequestDto(model);
            var result = await _service.CreateNewDeviceAsync(grapperId, dto);
            if (result)
            {
                _view.ShowSuccess(); // Chỉ hiển thị thành công nếu result == true
            }
            else
            {
                _view.ShowError("Create New Device failed");
            }
        }
        catch (Exception ex)
        {
            _view.ShowError($"Error: {ex.Message}");
        }
        finally
        {
            _view.HideLoading();
            GlobalVariable.APIRequestType.Remove("POST_Device");
        }
    }

    public async void UpdateDevice(int deviceId, DeviceInformationModel model)
    {
        GlobalVariable.APIRequestType.Add("PUT_Device");
        _view.ShowLoading("Đang thực hiện...");
        try
        {
            var dto = ConvertToRequestDto(model);
            var result = await _service.UpdateDeviceAsync(deviceId, dto);
            if (result)
            {
                GlobalVariable.deviceId = deviceId;
                _view.ShowSuccess(); // Chỉ hiển thị thành công nếu result == true
                UnityEngine.Debug.Log("Update Device succeeded");
            }
            else
            {
                _view.ShowError("Update Device failed");
                UnityEngine.Debug.Log("Update Device failed");
            }
        }
        catch (Exception ex)
        {
            _view.ShowError($"Error: {ex.Message}");
            UnityEngine.Debug.Log("Error: " + ex.Message + " " + ex.StackTrace);
        }
        finally
        {
            _view.HideLoading();
            GlobalVariable.APIRequestType.Remove("PUT_Device");
        }
    }
    public async void DeleteDevice(int deviceId)
    {
        GlobalVariable.APIRequestType.Add("DELETE_Device");
        _view.ShowLoading("Đang thực hiện...");
        try
        {
            UnityEngine.Debug.Log(deviceId);

            var result = await _service.DeleteDeviceAsync(deviceId);
            if (result)
            {
                _view.ShowSuccess(); // Chỉ hiển thị thành công nếu result == true
            }
            else
            {
                UnityEngine.Debug.Log("Delete Device failed");
                _view.ShowError("Delete Device failed");
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
            GlobalVariable.APIRequestType.Remove("DELETE_Device");
        }
    }

    //! Dto => Model
    private DeviceInformationModel ConvertFromResponseDto(DeviceResponseDto dto)
    {
        return new DeviceInformationModel(
            id: dto.Id,
            code: dto.Code,
            function: dto.Function,
            range: dto.Range,
            unit: dto.Unit,
            ioAddress: dto.IOAddress,
            jbInformationModels: dto.JBBasicDtos.Any() ? dto.JBBasicDtos.Select(jb => new JBInformationModel(
                id: jb.Id,
                name: jb.Name)).ToList() : new List<JBInformationModel>(),
            moduleInformationModel: dto.ModuleBasicDto != null ? new ModuleInformationModel(
                                        dto.ModuleBasicDto.Id,
                                        dto.ModuleBasicDto.Name
            ) : null,
            additionalConnectionImages: dto.AdditionalImageBasicDtos.Any() ? dto.AdditionalImageBasicDtos?.Select(
                imageDto => new ImageInformationModel(
                                id: imageDto.Id,
                                name: imageDto.Name
                // ,
                // url: imageDto.Url
                )).ToList() : new List<ImageInformationModel>()
            );
    }
    private DeviceInformationModel ConvertFromBasicDto(DeviceBasicDto dto)
    {
        return new DeviceInformationModel(
            id: dto.Id,
           code: dto.Code
        );
    }

    //! Model => Dto
    private DeviceRequestDto ConvertToRequestDto(DeviceInformationModel model)
    {
        return new DeviceRequestDto(
            code: model.Code,
            function: model.Function,
            range: model.Range,
            unit: model.Unit,
            ioAddress: model.IOAddress,
            jbBasicDtos: model.JBInformationModels.Any() ? model.JBInformationModels.Select(jb => new JBBasicDto(
                id: jb.Id,
                name: jb.Name
            )).ToList() : new List<JBBasicDto>(),
            moduleBasicDto: model.ModuleInformationModel != null ? new ModuleBasicDto(
                id: model.ModuleInformationModel.Id,
                name: model.ModuleInformationModel.Name
            ) : null,
         additionalImageBasicDtos: model.AdditionalConnectionImages.Any() ? model.AdditionalConnectionImages.Select(
            imageInfor => new ImageBasicDto(
                id: imageInfor.Id,
                name: imageInfor.Name
            )
         ).ToList() : new List<ImageBasicDto>()
        );
    }
}