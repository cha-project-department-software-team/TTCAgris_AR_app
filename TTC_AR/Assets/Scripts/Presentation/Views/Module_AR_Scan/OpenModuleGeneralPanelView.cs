using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyUI.Progress;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpenModuleGeneralPanelView : MonoBehaviour, IModuleView
{
    private string ModuleName;
    private ModulePresenter _presenter;
    [SerializeField] private TMP_Text title;
    private ModuleInformationModel moduleInformationModel;
    void Awake()
    {
        _presenter = new ModulePresenter(this, ManagerLocator.Instance.ModuleManager._IModuleService);
    }
    void OnEnable()
    {
        LoadModuleInfor();
    }
    void OnDisable()
    {
    }


    public void LoadModuleInfor()
    {
        Debug.Log(GlobalVariable.objectName);

        ModuleName = GlobalVariable.objectName;

        GlobalVariable.temp_Dictionary_ModuleInformationModel.TryGetValue(ModuleName, out ModuleInformationModel ModuleInformationModel);

        _presenter.LoadDetailById(ModuleInformationModel.Id);
    }

    public void DisplayList(List<ModuleInformationModel> models)
    {

    }
    public void DisplayDetail(ModuleInformationModel model)
    {
        Debug.Log("Let's display detail");
        if (model != null)
        {
            moduleInformationModel = model;

            Debug.Log("DisplayDetail: " + moduleInformationModel.Name);

            GlobalVariable.moduleId = moduleInformationModel.Id;

            GlobalVariable.temp_ModuleInformationModel = moduleInformationModel;

            //   GlobalVariable.temp_ListDeviceInformationModel_FromModule = moduleInformationModel.ListDeviceInformationModel;

            if (moduleInformationModel.ListJBInformationModel != null)
            {
                if (moduleInformationModel.ListJBInformationModel.Any())
                {
                    GlobalVariable.temp_ListJBInformationModel_FromModule = moduleInformationModel.ListJBInformationModel;
                    Debug.Log("ListJBInformationModel not Empty");

                }
                else
                {
                    GlobalVariable.temp_ListJBInformationModel_FromModule = new List<JBInformationModel>();
                    Debug.Log("ListJBInformationModel Empty");
                }
            }
            if (model.ModuleSpecificationModel != null)
            {
                GlobalVariable.moduleSpecificationId = model.ModuleSpecificationModel.Id;
            }
            else
            {
                GlobalVariable.moduleSpecificationId = 0;
            }
            if (model.AdapterSpecificationModel != null)
            {
                GlobalVariable.adapterSpecificationId = model.AdapterSpecificationModel.Id;
            }
            else
            {
                GlobalVariable.adapterSpecificationId = 0;

            }
            title.text = moduleInformationModel.Name;
        }
    }

    private void ShowProgressBar(string title, string details)
    {
        Progress.Show(title, ProgressColor.Blue, true);
        Progress.SetDetailsText(details);
    }
    private void HideProgressBar()
    {
        Progress.Hide();
    }
    public void ShowLoading(string title) => ShowProgressBar(title, "Đang tải dữ liệu...");
    public void HideLoading() => HideProgressBar();
    public void ShowError(string message)
    {
        if (GlobalVariable.APIRequestType.Contains("GET_Module"))
        {
            Show_Toast.Instance.ShowToast("failure", "Tải dữ liệu thất bại");
        }
        StartCoroutine(Show_Toast.Instance.Set_Instance_Status_False(1f));
    }
    public void ShowSuccess(string message)
    {
        if (GlobalVariable.APIRequestType.Contains("GET_Module"))
        {
            Show_Toast.Instance.ShowToast("success", message);
        }
        StartCoroutine(Show_Toast.Instance.Set_Instance_Status_False(1f));
    }

    // Không dùng trong ListView
    public void DisplayCreateResult(bool success) { }
    public void DisplayUpdateResult(bool success) { }
    public void DisplayDeleteResult(bool success) { }


    // public void DisplayList(List<ModuleInformationModel> models)
    // {
    //     throw new NotImplementedException();
    // }

    // public void DisplayDetail(ModuleInformationModel model)
    // {
    //     throw new NotImplementedException();
    // }
}