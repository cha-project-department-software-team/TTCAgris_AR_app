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
public class ChoosingGrapperPresenter
{
    private readonly IChoosingGrapper _view;
    private readonly IGrapperService _grapperService;

    public ChoosingGrapperPresenter(
        IChoosingGrapper view,
        IGrapperService grapperService
    )
    {
        _view = view;
        _grapperService = grapperService;
    }

    //! Get list Login chỉ có Id và Code
    public async void GetGrappersInfor()
    {
        GlobalVariable.APIRequestType.Add("GET_Grapper");
        _view.ShowLoading("Đang tải dữ liệu...");
        try
        {
            List<Task> tasks = new List<Task>();
            foreach (var grapper in GlobalVariable.temp_ListGrapperInformationModel)
            {
                int index = GlobalVariable.temp_ListGrapperInformationModel.IndexOf(grapper);
                var grapperTask = _grapperService.GetGrapperByIdAsync(grapper.Id);
                tasks.Add(grapperTask);
            }
            await Task.WhenAll(
                tasks
            );

            if (
                tasks.Any(t => t.IsFaulted)
            )
            {
                _view.ShowError("Tải dữ liệu thất bại!");
                return;
            }

            var grapperDtos = tasks.Select(t => (t as Task<GrapperResponseDto>).Result).ToList();

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
            _view.Display();
            _view.ShowSuccess(); // Chỉ hiển thị thành công nếu result == true
        }
        catch (Exception ex)
        {
            _view.ShowError("Tải dữ liệu thất bại!");
            Debug.LogError("Lỗi khi tải dữ liệu: " + ex.Message);
        }
        finally
        {
            _view.HideLoading();
            GlobalVariable.APIRequestType.Remove("GET_Grapper");
        }
    }





    //! Dto => Model
    private GrapperInformationModel ConvertGrapperFromResponseDto(GrapperResponseDto dto)
    {
        return new GrapperInformationModel(
            id: dto.Id,
            name: dto.Name,
            list_RackBasicModel: dto.RackBasicDtos.Any() ? dto.RackBasicDtos.Select(
                rack => new RackBasicModel(
                    id: rack.Id,
                    name: rack.Name
                )).ToList() : new List<RackBasicModel>(),
            listDeviceInformationModel: dto.DeviceBasicDtos.Any() ? dto.DeviceBasicDtos.Select(
                device => new DeviceInformationModel(
                    id: device.Id,
                    code: device.Code
                )).ToList() : new List<DeviceInformationModel>(),
            listJBInformationModel: dto.JBBasicDtos.Any() ? dto.JBBasicDtos.Select(
                jb => new JBInformationModel(
                    id: jb.Id,
                    name: jb.Name
                )).ToList() : new List<JBInformationModel>(),
            listMccInformationModel: dto.MccBasicDtos.Any() ? dto.MccBasicDtos.Select(
                mcc => new MccInformationModel(
                    id: mcc.Id,
                    cabinetCode: mcc.CabinetCode
                )).ToList() : new List<MccInformationModel>(),
            listFieldDeviceInformationModel: dto.FieldDeviceBasicDtos.Any() ? dto.FieldDeviceBasicDtos.Select(
                fieldDevice => new FieldDeviceInformationModel(
                    id: fieldDevice.Id,
                    name: fieldDevice.Name
                )).ToList() : new List<FieldDeviceInformationModel>()
        );

    }

    //! Dto => Model
    private ImageInformationModel ConvertImageFromBasicDto(ImageBasicDto dto)
    {
        return new ImageInformationModel(
            id: dto.Id,
            name: dto.Name);
    }
}
