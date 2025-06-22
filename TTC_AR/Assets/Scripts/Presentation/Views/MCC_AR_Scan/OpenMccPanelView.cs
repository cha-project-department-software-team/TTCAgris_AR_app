using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyUI.Progress;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpenMccPanelView : MonoBehaviour, IMccView
{
    private string mccName;
    public GameObject List_FieldDevices_Panel;
    public GameObject General_Panel;
    private MccPresenter _presenter;
    [SerializeField] private GameObject fieldDevice_Item_Prefab;
    [SerializeField] private Transform parentTransform;
    [SerializeField] private ScrollRect scroll_Area;
    [SerializeField] private List<TMP_Text> mccInfor_Value = new List<TMP_Text>();
    private List<FieldDeviceInformationModel> listFieldDeviceFromMcc = new List<FieldDeviceInformationModel>();
    private List<GameObject> listFieldDeviceItems = new List<GameObject>();

    void Awake()
    {
        _presenter = new MccPresenter(this, ManagerLocator.Instance.MccManager._IMccService);
    }
    void OnEnable()
    {
        ReFreshMccData();
        LoadMCCInfor();
    }
    void OnDisable()
    {
    }
    void ReFreshMccData()
    {
        fieldDevice_Item_Prefab.SetActive(false);
        foreach (Transform child in parentTransform)
        {
            if (child.gameObject != fieldDevice_Item_Prefab && child.name.Contains("Item"))
                Destroy(child.gameObject);
        }
        listFieldDeviceFromMcc.Clear();
    }

    public void LoadMCCInfor()
    {
        mccName = GlobalVariable.objectName;
        GlobalVariable.temp_Dictionary_MCCInformationModel.TryGetValue(mccName, out MccInformationModel mccInformationModel);
        GlobalVariable.mccId = mccInformationModel.Id;
        _presenter.LoadDetailById(mccInformationModel.Id);
    }
    public void DisplayFieldDeviceList(List<FieldDeviceInformationModel> models)
    {
    }
    public void DisplayList(List<MccInformationModel> models)
    {

    }
    public void DisplayDetail(MccInformationModel model)
    {
        GlobalVariable.temp_MCCInformationModel = model;
        listFieldDeviceFromMcc = model.ListFieldDeviceInformation;
        UpdateUI(model);
    }
    private void UpdateUI(MccInformationModel model)
    {
        fieldDevice_Item_Prefab.SetActive(true);
        UpdateTextValue(mccInfor_Value[0], model.Brand, "Chưa cập nhật");
        UpdateTextValue(mccInfor_Value[1], model.Note, "Chưa cập nhật");

        if (listFieldDeviceFromMcc.Count > 0)
        {
            foreach (var fieldDevice in listFieldDeviceFromMcc)
            {
                var fieldDeviceItem = Instantiate(fieldDevice_Item_Prefab, parentTransform);
                listFieldDeviceItems.Add(fieldDeviceItem);

                if (fieldDeviceItem.TryGetComponent(out FieldDeviceInfor fieldDeviceInfor))
                {
                    fieldDeviceInfor.SetFieldDeviceName(fieldDevice.Name);
                    fieldDeviceInfor.button.onClick.RemoveAllListeners();
                    fieldDeviceInfor.button.onClick.AddListener(() => OpenGeneralPanel(fieldDevice));
                }
            }

            fieldDevice_Item_Prefab.SetActive(false);
            scroll_Area.verticalNormalizedPosition = 1f;
        }
    }

    private void UpdateTextValue(TMP_Text textField, string value, string defaultValue)
    {
        if (string.IsNullOrEmpty(value) || value == defaultValue)
        {
            textField.text = defaultValue;
            textField.color = Color.red;
            textField.fontStyle = FontStyles.Bold;
        }
        else
        {
            textField.text = value;
            textField.color = Color.black; // Reset to default color
            textField.fontStyle = FontStyles.Normal; // Reset to default style
        }
    }


    public void OpenGeneralPanel(FieldDeviceInformationModel model)
    {
        SetUpFieldDeviceData(model);
        General_Panel.SetActive(true);
        List_FieldDevices_Panel.SetActive(false);
    }
    private void SetUpFieldDeviceData(FieldDeviceInformationModel model)
    {
        GlobalVariable.temp_FieldDeviceInformationModel = model;
        GlobalVariable.fieldDeviceId = model.Id;
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
        if (GlobalVariable.APIRequestType.Contains("GET_Mcc"))
        {
            Show_Toast.Instance.ShowToast("failure", "Tải dữ liệu thất bại");
        }
        StartCoroutine(Show_Toast.Instance.Set_Instance_Status_False(1f));
    }
    public void ShowSuccess()
    {
        if (GlobalVariable.APIRequestType.Contains("GET_Mcc"))
        {
            Show_Toast.Instance.ShowToast("success", "Tải dữ liệu thành công");
        }
        StartCoroutine(Show_Toast.Instance.Set_Instance_Status_False(1f));
    }

    // Không dùng trong ListView
    public void DisplayCreateResult(bool success) { }
    public void DisplayUpdateResult(bool success) { }
    public void DisplayDeleteResult(bool success) { }


}