using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationLayer.Interfaces;
using Unity.VisualScripting;
using ApplicationLayer.Dtos.JB;
using ApplicationLayer.Dtos.Grapper;
using ApplicationLayer.Dtos.AdapterSpecification;
using ApplicationLayer.Dtos.Rack;
using System.Threading.Tasks;
using UnityEngine.UI;
using ApplicationLayer.Dtos.Company;
using UnityEngine;
using ApplicationLayer.Dtos.Image;
public class LoginPresenter
{
    private readonly ILoginView _view;
    private readonly ICompanyService _companyService;
    private readonly IGrapperService _grapperService;
    private readonly IJBService _jbService;
    private readonly IImageService _imageService;
    private readonly IModuleService _moduleService;
    private readonly IMccService _mccService;
    private readonly IFieldDeviceService _fieldDeviceService;
    private readonly IDeviceService _deviceService;
    private readonly IModuleSpecificationService _moduleSpecificationService;
    private readonly IAdapterSpecificationService _adapterSpecificationService;
    private readonly IRackService _rackService;

    public LoginPresenter(
        ILoginView view,
        ICompanyService CompanyService,
        IGrapperService GrapperService,
        IJBService JBService,
        IImageService ImageService
        , IModuleService ModuleService
        , IMccService MccService
        , IFieldDeviceService FieldDeviceService
        , IDeviceService DeviceService
        , IModuleSpecificationService ModuleSpecificationService
        , IAdapterSpecificationService AdapterSpecificationService
        , IRackService RackService
        )
    {
        _view = view;
        _companyService = CompanyService;
        _grapperService = GrapperService;
        _jbService = JBService;
        _imageService = ImageService;
        _moduleService = ModuleService;
        _mccService = MccService;
        _fieldDeviceService = FieldDeviceService;
        _deviceService = DeviceService;
        _moduleSpecificationService = ModuleSpecificationService;
        _adapterSpecificationService = AdapterSpecificationService;
        _rackService = RackService;

    }

    //! Get list Login chỉ có Id và Code
    public async void LoginProcess()
    {
        GlobalVariable.APIRequestType.Add("GET_Company");
        GlobalVariable.APIRequestType.Add("GET_Grapper_List");
        GlobalVariable.APIRequestType.Add("GET_JB_List_Information");
        GlobalVariable.APIRequestType.Add("GET_Image_List");
        GlobalVariable.APIRequestType.Add("GET_Module_List");
        GlobalVariable.APIRequestType.Add("GET_Device_List_General");
        GlobalVariable.APIRequestType.Add("GET_MCC_List");
        GlobalVariable.APIRequestType.Add("GET_FieldDevice_List");
        GlobalVariable.APIRequestType.Add("GET_ModuleSpecification_List");
        GlobalVariable.APIRequestType.Add("GET_AdapterSpecification_List");
        GlobalVariable.APIRequestType.Add("GET_Rack_List");

        _view.ShowLoading("Đang đăng nhập...");

        try
        {
            var companyTask = _companyService.GetCompanyByIdAsync(1);
            var grapperTask = _grapperService.GetListGrapperAsync(1);
            var jbTask = _jbService.GetListJBInformationAsync(1);
            var imageTask = _imageService.GetListImageAsync(1);
            var moduleTask = _moduleService.GetListModuleAsync(1);
            var mccTask = _mccService.GetListMccAsync(1);
            var fieldDeviceTask = _fieldDeviceService.GetListFieldDeviceAsync(1);
            var deviceTask = _deviceService.GetListDeviceGeneralAsync(1);
            var moduleSpecificationTask = _moduleSpecificationService.GetListModuleSpecificationAsync(1);
            var adapterSpecificationTask = _adapterSpecificationService.GetListAdapterSpecificationAsync(1);
            var rackTask = _rackService.GetListRackAsync(1);

            await Task.WhenAll(companyTask, grapperTask, jbTask, imageTask, moduleTask, mccTask, fieldDeviceTask, deviceTask, moduleSpecificationTask, adapterSpecificationTask, rackTask);

            var companyDto = companyTask.Result;
            var grapperDtos = grapperTask.Result;
            var jbDtos = jbTask.Result;
            var imageDtos = imageTask.Result;
            var moduleDtos = moduleTask.Result;
            var mccDtos = mccTask.Result;
            var fieldDeviceDtos = fieldDeviceTask.Result;
            var deviceDtos = deviceTask.Result;
            var moduleSpecificationDtos = moduleSpecificationTask.Result;
            var adapterSpecificationDtos = adapterSpecificationTask.Result;
            var rackDtos = rackTask.Result;

            if (companyDto == null || grapperDtos == null
            || jbDtos == null || imageDtos == null
            || moduleDtos == null
            || fieldDeviceDtos == null
            || deviceDtos == null
            || moduleSpecificationDtos == null
            || adapterSpecificationDtos == null
            || mccDtos == null
            || rackDtos == null)
            {
                _view.ShowError("Tải dữ liệu thất bại!");
                return;
            }

            if (companyDto != null)
            {
                var models = ConvertCompanyModelFromDto(companyDto);
                if (models != null)
                {
                    GlobalVariable.temp_CompanyInformationModel = models;
                }
            }
            else
            {
                _view.ShowError("Tải dữ liệu thất bại!");
            }
            if (grapperDtos != null)
            {
                var models = new List<GrapperInformationModel>();
                if (grapperDtos.Any())
                {
                    models = grapperDtos.Select(dto => ConvertGrapperFromResponseDto(dto)).ToList();
                }
                if (models != null)
                {
                    GlobalVariable.temp_ListGrapperInformationModel = models;
                    GlobalVariable.temp_Dictionary_GrapperInformationModel = models.ToDictionary(m => m.Name, m => m);
                }
            }
            else
            {
                _view.ShowError("Tải dữ liệu thất bại!");

            }

            if (jbDtos != null)
            {
                if (jbDtos.Any())
                {
                    var models = jbDtos.Select(dto => ConvertFromResponseDto(dto)).ToList();
                    GlobalVariable.temp_ListJBInformationModel = models;
                    GlobalVariable.temp_Dictionary_JBInformationModel = models.ToDictionary(m => m.Name, m => m);
                    Debug.Log(GlobalVariable.temp_Dictionary_JBInformationModel.Count);
                }

            }
            else
            {
                _view.ShowError("Tải dữ liệu thất bại!");

            }
            if (imageDtos != null)
            {
                if (imageDtos.Any())
                {
                    var models = imageDtos.Select(dto => ConvertImageFromBasicDto(dto)).ToList();
                    GlobalVariable.temp_ListImageInformationModel = models;
                    GlobalVariable.temp_Dictionary_ImageInformationModel = models.ToDictionary(m => m.Name, m => m);
                    Debug.Log(GlobalVariable.temp_Dictionary_ImageInformationModel.Count);
                }
            }
            else
            {
                _view.ShowError("Tải dữ liệu thất bại!");

            }
            if (moduleDtos != null)
            {
                if (moduleDtos.Any())
                {
                    var models = moduleDtos.Select(dto => new ModuleInformationModel(
                        id: dto.Id,
                        name: dto.Name
                    )).ToList();
                    GlobalVariable.temp_ListModuleInformationModel = models;
                    GlobalVariable.temp_Dictionary_ModuleInformationModel = models.ToDictionary(m => m.Name, m => m);
                    Debug.Log(GlobalVariable.temp_Dictionary_ModuleInformationModel.Count);
                }
            }
            else
            {
                _view.ShowError("Tải dữ liệu thất bại!");
            }
            if (mccDtos != null)
            {
                if (mccDtos.Any())
                {
                    var models = mccDtos.Select(dto => new MccInformationModel(
                        id: dto.Id,
                        cabinetCode: dto.CabinetCode
                    )).ToList();
                    GlobalVariable.temp_ListMCCInformationModel = models;
                    GlobalVariable.temp_Dictionary_MCCInformationModel = models.ToDictionary(m => m.CabinetCode, m => m);
                    Debug.Log(GlobalVariable.temp_Dictionary_MCCInformationModel.Count);
                }
            }
            else
            {
                _view.ShowError("Tải dữ liệu thất bại!");
            }
            if (fieldDeviceDtos != null)
            {
                if (fieldDeviceDtos.Any())
                {
                    var models = fieldDeviceDtos.Select(dto => new FieldDeviceInformationModel(
                        id: dto.Id,
                        name: dto.Name
                    )).ToList();
                    GlobalVariable.temp_ListFieldDeviceInformationModel = models;
                    Debug.Log("FieldDevices: " + models.Count);

                    GlobalVariable.temp_Dictionary_FieldDeviceInformationModel.Clear();
                    //!
                    // foreach (var fieldDevice in models)
                    // {
                    //     if (!GlobalVariable.temp_Dictionary_FieldDeviceInformationModel.ContainsKey(fieldDevice.Name))
                    //     {
                    //         GlobalVariable.temp_Dictionary_FieldDeviceInformationModel[fieldDevice.Name] = new List<FieldDeviceInformationModel>()
                    //         {
                    //             fieldDevice
                    //         };
                    //     }
                    //     if (GlobalVariable.temp_Dictionary_FieldDeviceInformationModel.ContainsKey(fieldDevice.Name))
                    //     {
                    //         if (GlobalVariable.temp_Dictionary_FieldDeviceInformationModel[fieldDevice.Name].Any())
                    //         {
                    //             GlobalVariable.temp_Dictionary_FieldDeviceInformationModel[fieldDevice.Name].Add(fieldDevice);
                    //         }

                    //     }
                    // }
                    //!
                    GlobalVariable.temp_Dictionary_FieldDeviceInformationModel = models
                      .GroupBy(fd => fd.Name)
                      .ToDictionary(g => g.Key, g => g.ToList());
                    Debug.Log(GlobalVariable.temp_Dictionary_FieldDeviceInformationModel.Count);
                }
            }
            else
            {
                _view.ShowError("Tải dữ liệu thất bại!");
            }

            if (deviceDtos != null)
            {
                if (deviceDtos.Any())
                {
                    var models = deviceDtos.Select(dto => new DeviceInformationModel(
                        id: dto.Id,
                        code: dto.Code
                    )).ToList();
                    GlobalVariable.temp_ListDeviceInformationModel = models;
                    GlobalVariable.temp_Dictionary_DeviceInformationModel = models.ToDictionary(m => m.Code, m => m);
                    Debug.Log("Devices: " + models.Count);
                }
            }
            else
            {
                _view.ShowError("Tải dữ liệu thất bại!");
            }
            if (moduleSpecificationDtos != null)
            {
                if (moduleSpecificationDtos.Any())
                {
                    var models = moduleSpecificationDtos.Select(dto => new ModuleSpecificationModel(
                        id: dto.Id,
                        code: dto.Code
                    )).ToList();
                    GlobalVariable.temp_ListModuleSpecificationModel = models;
                    GlobalVariable.temp_Dictionary_ModuleSpecificationModel = models.ToDictionary(m => m.Code, m => m);
                    Debug.Log("ModuleSpecifications: " + models.Count);
                }
            }
            else
            {
                _view.ShowError("Tải dữ liệu thất bại!");
            }
            if (adapterSpecificationDtos != null)
            {
                if (adapterSpecificationDtos.Any())
                {
                    var models = adapterSpecificationDtos.Select(dto => new AdapterSpecificationModel(
                        id: dto.Id,
                        code: dto.Code
                    )).ToList();
                    GlobalVariable.temp_ListAdapterSpecificationModel = models;
                    GlobalVariable.temp_Dictionary_AdapterSpecificationModel = models.ToDictionary(m => m.Code, m => m);
                    Debug.Log("AdapterSpecifications: " + models.Count);
                }
            }
            else
            {
                _view.ShowError("Tải dữ liệu thất bại!");
            }

            if (rackDtos != null)
            {
                if (rackDtos.Any())
                {
                    var models = rackDtos.Select(dto => new RackInformationModel(
                        id: dto.Id,
                        name: dto.Name
                    )).ToList();
                    GlobalVariable.temp_ListRackInformationModel = models;
                    GlobalVariable.temp_Dictionary_RackInformationModel = models.ToDictionary(m => m.Name, m => m);
                    Debug.Log("Racks: " + models.Count);
                }
            }
            else
            {
                _view.ShowError("Tải dữ liệu thất bại!");
            }
            _view.ShowSuccess(); // Chỉ hiển thị thành công nếu result == true

            GlobalVariable.GrapperId = 1;
            GlobalVariable.companyId = 1;

        }
        catch (Exception ex)
        {
            _view.ShowError("Tải dữ liệu thất bại!");
            Debug.LogError("Lỗi khi tải dữ liệu: " + ex.Message);
        }
        finally
        {
            _view.HideLoading();
            GlobalVariable.APIRequestType.Remove("GET_Company");
            GlobalVariable.APIRequestType.Remove("GET_Grapper_List");
            GlobalVariable.APIRequestType.Remove("GET_JB_List_Information");
            GlobalVariable.APIRequestType.Remove("GET_Image_List");
            GlobalVariable.APIRequestType.Remove("GET_Module_List");
            GlobalVariable.APIRequestType.Remove("GET_Device_List_General");
            GlobalVariable.APIRequestType.Remove("GET_MCC_List");
            GlobalVariable.APIRequestType.Remove("GET_FieldDevice_List");
            GlobalVariable.APIRequestType.Remove("GET_ModuleSpecification_List");
            GlobalVariable.APIRequestType.Remove("GET_AdapterSpecification_List");
            GlobalVariable.APIRequestType.Remove("GET_Rack_List");

        }
    }

    private CompanyInformationModel ConvertCompanyModelFromDto(CompanyResponseDto dto)
    {
        return new CompanyInformationModel(
            id: dto.Id,
            name: dto.Name,
            listGrapperInformationModel: dto.GrapperBasicDtos.Select(
                grapper => new GrapperInformationModel(
                    id: grapper.Id,
                    name: grapper.Name
                )).ToList(),
            listModuleSpecificationModel: dto.ModuleSpecificationBasicDtos.Select(
                module => new ModuleSpecificationModel(
                    id: module.Id,
                    code: module.Code
                )).ToList(),
            listAdapterSpecificationModel: dto.AdapterSpecificationBasicDtos.Select(
                adapter => new AdapterSpecificationModel(
                    id: adapter.Id,
                    code: adapter.Code
                )).ToList()
        );
    }

    private JBInformationModel ConvertFromResponseDto(JBGeneralDto dto)
    {
        return new JBInformationModel(
            id: dto.Id,
            name: dto.Name,
            location: string.IsNullOrEmpty(dto.Location) ? "Được ghi chú trên sơ đồ" : dto.Location,
            outdoorImage: dto.OutdoorImageBasicDto != null ? new ImageInformationModel(
                id: dto.OutdoorImageBasicDto.Id,
                name: dto.OutdoorImageBasicDto.Name
            ) : null,
            listConnectionImages: dto.ConnectionImageBasicDtos.Any() ? dto.ConnectionImageBasicDtos.Select(device => new ImageInformationModel(
                id: device.Id,
                name: device.Name
            )).ToList() : new List<ImageInformationModel>()
        );
    }
    //! Dto => Model
    private GrapperInformationModel ConvertGrapperFromResponseDto(GrapperBasicDto dto)
    {
        return new GrapperInformationModel(
            id: dto.Id,
            name: dto.Name);
    }

    //! Dto => Model
    private ImageInformationModel ConvertImageFromBasicDto(ImageBasicDto dto)
    {
        return new ImageInformationModel(
            id: dto.Id,
            name: dto.Name);
    }
}
