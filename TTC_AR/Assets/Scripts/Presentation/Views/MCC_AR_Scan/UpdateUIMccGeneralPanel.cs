using System;
using System.Collections;
using System.Collections.Generic;
using EasyUI.Progress;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateUIMccGeneralPanel : MonoBehaviour, IFieldDeviceView
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button viewSetupValueButton;
    [SerializeField] private Button viewConnectionButton;

    [SerializeField] private GameObject listFieldDevicesPanel;
    [SerializeField] private GameObject generalPanel;
    [SerializeField] private GameObject setupValuePanel;
    [SerializeField] private GameObject connectionPanel;
    private FieldDevicePresenter _presenter;

    void Awake()
    {
        _presenter = new FieldDevicePresenter(this, ManagerLocator.Instance.FieldDeviceManager._IFieldDeviceService);
    }
    private void OnEnable()
    {
        _presenter.LoadDetailById(GlobalVariable.fieldDeviceId);
    }

    private void NavigateBack()
    {
        generalPanel.SetActive(false);
        listFieldDevicesPanel.SetActive(true);
    }
    private void OpenSetUpValuePanel()
    {
        generalPanel.SetActive(false);
        setupValuePanel.SetActive(true);
    }
    private void OpenConnectionPanel()
    {
        generalPanel.SetActive(false);
        connectionPanel.SetActive(true);
    }

    private void OnDisable()
    {
        closeButton.onClick.RemoveAllListeners();
        viewSetupValueButton.onClick.RemoveAllListeners();
        viewConnectionButton.onClick.RemoveAllListeners();
    }

    public void DisplayDetail(FieldDeviceInformationModel model)
    {
        title.text = model.Name;

        GlobalVariable.temp_FieldDeviceInformationModel = model;

        GlobalVariable.fieldDeviceId = model.Id;

        closeButton.onClick.RemoveAllListeners();
        viewSetupValueButton.onClick.RemoveAllListeners();
        viewConnectionButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(() => NavigateBack());
        viewSetupValueButton.onClick.AddListener(() => OpenSetUpValuePanel());
        viewConnectionButton.onClick.AddListener(() => OpenConnectionPanel());

        Debug.Log(model.Name);
        Debug.Log(model.RatedPower);
        Debug.Log(model.RatedCurrent);
        Debug.Log(model.ActiveCurrent);
        Debug.Log(model.Note);
        Debug.Log(model.ListConnectionImages.Count);
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

    public void ShowSuccess()
    {
        if (GlobalVariable.APIRequestType.Contains("GET_FieldDevice"))
        {
            Show_Toast.Instance.ShowToast("success", "Tải dữ liệu thành công");
        }
        StartCoroutine(Show_Toast.Instance.Set_Instance_Status_False(1f));
    }
    public void ShowError(string message)
    {
        if (GlobalVariable.APIRequestType.Contains("GET_FieldDevice"))
        {
            Show_Toast.Instance.ShowToast("failure", "Tải dữ liệu thất bại");
        }
        StartCoroutine(Show_Toast.Instance.Set_Instance_Status_False(1f));
    }

    public void DisplayList(List<FieldDeviceInformationModel> models)
    {
    }


    public void DisplayUpdateResult(bool success)
    {
    }
    public void DisplayCreateResult(bool success)
    {
    }
    public void DisplayDeleteResult(bool success)
    {
    }
}
