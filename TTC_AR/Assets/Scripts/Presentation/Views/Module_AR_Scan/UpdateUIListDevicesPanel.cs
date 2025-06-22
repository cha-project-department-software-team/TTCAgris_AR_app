using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EasyUI.Progress;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpdateUIListDevicesPanel : MonoBehaviour, IDeviceView
{

    [SerializeField] private GameObject main_Canvas;
    [SerializeField] private GameObject generalPanel;
    [SerializeField] private GameObject listDevicesPanel;
    [SerializeField] private GameObject listJBsPanel;
    [SerializeField] private GameObject JBDetailPanel;
    [SerializeField] private Button closeButton;
    public GameObject JB_Item_Prefab;
    public TMP_Dropdown dropdown;
    public GameObject contentPanel;
    public Transform jbConnectionParentTransform;
    public List<TMP_Text> deviceInforValue = new List<TMP_Text>();

    //!
    private List<DeviceInformationModel> listDeviceFromModule = new List<DeviceInformationModel>();
    private DeviceInformationModel deviceInformationModel;
    private List<JBInformationModel> listJBInformationModel = new List<JBInformationModel>();
    public Dictionary<string, JBInformationModel> dic_JBInformationModel = new Dictionary<string, JBInformationModel>();
    public Dictionary<string, GameObject> dic_JBInformationModel_Button = new Dictionary<string, GameObject>();
    private const string noDeviceMessage = "Không có thiết bị kết nối";
    public List<string> devicesCode = new List<string>();
    private DevicePresenter _presenter;
    private bool loadFirstDevice = true;
    private int moduleId;

    void Awake()
    {
        Initialize();
        _presenter = new DevicePresenter(this, ManagerLocator.Instance.DeviceManager._IDeviceService);
    }
    private void Initialize()
    {
        listDevicesPanel ??= main_Canvas.transform.Find("List_Devices_Panel").gameObject;
        listJBsPanel ??= main_Canvas.transform.Find("List_JB_Panel").gameObject;
        JBDetailPanel ??= main_Canvas.transform.Find("Detail_JB_TSD").gameObject;
    }

    private void OnEnable()
    {
        moduleId = GlobalVariable.moduleId;
        if (moduleId != 0)
        {
            _presenter.LoadListDeviceInformationFromModule(moduleId);

        }
    }
    private void OnDisable()
    {
        closeButton.onClick.RemoveAllListeners();
        dropdown.onValueChanged.RemoveAllListeners();
        DestroyAllInstancesExceptPrefab(dic_JBInformationModel_Button.Values.ToList());
        dic_JBInformationModel.Clear();
        dic_JBInformationModel_Button.Clear();
        listJBInformationModel.Clear();
        listDeviceFromModule.Clear();
    }
    public void DisplayList(List<DeviceInformationModel> models)
    {
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(() => CloseListJBPanel());

        if (models.Any())
        {
            GlobalVariable.temp_ListDeviceInformationModel_FromModule = models;
            listDeviceFromModule = models;
        }
        StartCoroutine(UpdateUI());
    }
    public void DisplayDetail(DeviceInformationModel model)
    {
        var jbInfor = JB_Item_Prefab.GetComponent<JBInfor>();
        jbInfor.button.gameObject.SetActive(true);
        jbInfor.value.text = "JB1";
        jbInfor.Location.fontWeight = TMPro.FontWeight.Bold;
        jbInfor.Location.color = Color.black;
        jbInfor.button.onClick.RemoveAllListeners();

        if (!contentPanel.activeSelf)
        {
            contentPanel.SetActive(true);
        }
        UpdateDeviceInformation(model);
    }

    private IEnumerator UpdateUI()
    {
        loadFirstDevice = true;

        yield return null;

        dropdown.options.Clear();

        if (listDeviceFromModule.Any())
        {
            Debug.Log("listDeviceFromModule Count: " + listDeviceFromModule.Count);
            contentPanel.SetActive(true);
            foreach (var model in listDeviceFromModule)
            {
                dropdown.options.Add(new TMP_Dropdown.OptionData(model.Code));
                devicesCode.Add(model.Code);
            }
            dropdown.value = 0;
            dropdown.RefreshShownValue();
            dropdown.onValueChanged.AddListener(OnValueChange);
            OnValueChange(0);
            yield return new WaitForSeconds(2f);
            loadFirstDevice = false;
        }
        else
        {
            Debug.Log("listDeviceFromModule Count: " + 0);
            contentPanel.SetActive(false);
            dropdown.options.Add(new TMP_Dropdown.OptionData(noDeviceMessage));
            dropdown.value = 0;
            dropdown.RefreshShownValue();
            ClearDeviceInformation();
        }
    }

    private void DestroyAllInstancesExceptPrefab(List<GameObject> listInstances)
    {
        foreach (var instance in listInstances)
        {
            Destroy(instance);
        }
    }

    public void OnValueChange(int value)
    {
        if (value < listDeviceFromModule.Count)
        {
            deviceInformationModel = listDeviceFromModule[value];
            _presenter.LoadDetailById(deviceInformationModel.Id);

        }
        else
        {
            ClearDeviceInformation();
            contentPanel.SetActive(false);
        }
    }

    private void UpdateDeviceInformation(DeviceInformationModel device)
    {
        DestroyAllInstancesExceptPrefab(dic_JBInformationModel_Button.Values.ToList());

        deviceInforValue[0].text = device.Code;
        deviceInforValue[1].text = device.Function;
        deviceInforValue[2].text = device.Range;
        deviceInforValue[3].text = device.Unit;
        deviceInforValue[4].text = device.IOAddress;

        GlobalVariable.deviceCode = device.Code;
        if (device.AdditionalConnectionImages != null && device.AdditionalConnectionImages.Any())
        {
            GlobalVariable.temp_List_AdditionalImages = device.AdditionalConnectionImages;
        }
        else
        {
            GlobalVariable.temp_List_AdditionalImages = new List<ImageInformationModel>();
        }

        dic_JBInformationModel.Clear();

        dic_JBInformationModel_Button.Clear();

        if (device.JBInformationModels != null && device.JBInformationModels.Any())
        {
            listJBInformationModel = device.JBInformationModels;
        }

        if (!listJBInformationModel.Any())
        {
            JB_Item_Prefab.SetActive(true);
            JB_Item_Prefab.GetComponent<JBInfor>().HandleEmptyList();
            return;
        }
        else
        {
            if (listJBInformationModel.Count > 1)
            {
                foreach (var model in listJBInformationModel)
                {
                    if (!dic_JBInformationModel.ContainsKey(model.Name))
                    {
                        if (GlobalVariable.temp_Dictionary_JBInformationModel.TryGetValue(model.Name, out var jbInformationModel))
                        {
                            dic_JBInformationModel.Add(jbInformationModel.Name, jbInformationModel);
                            var new_JB_Item = Instantiate(JB_Item_Prefab, jbConnectionParentTransform);
                            var new_JB_Item_JBInfor = new_JB_Item.GetComponent<JBInfor>();
                            new_JB_Item_JBInfor.SetJBInfor(jbInformationModel);
                            dic_JBInformationModel_Button.Add(jbInformationModel.Name, new_JB_Item);
                            new_JB_Item_JBInfor.button.onClick.RemoveAllListeners();
                            new_JB_Item_JBInfor.button.onClick.AddListener(() =>
                            {
                                GlobalVariable.navigate_from_List_Devices = true;
                                GlobalVariable.navigate_from_list_JBs = false;
                                NavigateJBDetailScreen(model: jbInformationModel);
                            });
                        }
                    }
                    dic_JBInformationModel_Button[model.Name].SetActive(true);
                }
                JB_Item_Prefab.SetActive(false);
                return;
            }
            else
            {
                JB_Item_Prefab.SetActive(true);
                if (GlobalVariable.temp_Dictionary_JBInformationModel.TryGetValue(listJBInformationModel[0].Name, out var jbInformationModel))
                {
                    var jbInfor = JB_Item_Prefab.GetComponent<JBInfor>();
                    jbInfor.SetJBInfor(jbInformationModel);
                    jbInfor.button.onClick.RemoveAllListeners();
                    jbInfor.button.onClick.AddListener(() =>
                    {
                        GlobalVariable.navigate_from_List_Devices = true;
                        GlobalVariable.navigate_from_list_JBs = false;
                        NavigateJBDetailScreen(model: jbInformationModel);
                    });
                }
            }
        }


    }

    private void ClearDeviceInformation()
    {
        foreach (var infoValue in deviceInforValue)
        {
            infoValue.text = string.Empty;
        }
    }

    public void NavigateJBDetailScreen(JBInformationModel model)
    {
        GlobalVariable.temp_JBInformationModel = model;

        GlobalVariable.jb_TSD_Title = model.Name; // Name_Location of JB

        GlobalVariable.jb_TSD_Name = model.Name; // jb_name: JB100

        GlobalVariable.jb_TSD_Location = model.Location; // jb_location: Hầm Cáp MCC

        GlobalVariable.JBId = model.Id;

        if (GlobalVariable.navigate_from_list_JBs)
        {
            GlobalVariable.navigate_from_JB_TSD_Detail = true;
            listJBsPanel.gameObject.SetActive(false);
            JBDetailPanel.gameObject.SetActive(true);
        }

        if (GlobalVariable.navigate_from_List_Devices)
        {
            GlobalVariable.navigate_from_JB_TSD_Detail = true;
            listDevicesPanel.gameObject.SetActive(false);
            JBDetailPanel.gameObject.SetActive(true);
        }
    }
    private void CloseListJBPanel()
    {
        generalPanel.SetActive(true);
        listDevicesPanel.SetActive(false);
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

        if (GlobalVariable.APIRequestType.Contains("GET_Device_List_Information_FromModule"))
        {
            Show_Toast.Instance.ShowToast("success", "Tải dữ liệu thành công");
        }
        if (GlobalVariable.APIRequestType.Contains("GET_Device") && !loadFirstDevice)
        {
            Show_Toast.Instance.ShowToast("success", "Tải dữ liệu thành công");
        }
        StartCoroutine(Show_Toast.Instance.Set_Instance_Status_False(0.5f));
    }
    public void ShowError(string message)
    {
        if (GlobalVariable.APIRequestType.Contains("GET_Device_List_Information_FromModule"))
        {
            Show_Toast.Instance.ShowToast("failure", "Tải dữ liệu thất bại");
        }
        if (GlobalVariable.APIRequestType.Contains("GET_Device"))
        {
            Show_Toast.Instance.ShowToast("failure", "Tải dữ liệu thất bại");
        }
        StartCoroutine(Show_Toast.Instance.Set_Instance_Status_False(0.5f));
    }


    public void DisplayCreateResult(bool success)
    {
        throw new NotImplementedException();
    }

    public void DisplayUpdateResult(bool success)
    {
        throw new NotImplementedException();
    }

    public void DisplayDeleteResult(bool success)
    {
        throw new NotImplementedException();
    }
}
