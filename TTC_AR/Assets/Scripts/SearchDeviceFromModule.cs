using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Collections;
using Unity.VisualScripting;
using System;
using System.Linq;
public class SearchDeviceFromModule : MonoBehaviour
{
    public List<TMP_Text> deviceInformation = new List<TMP_Text>();
    private List<DeviceInformationModel> listDeviceFromModule = new List<DeviceInformationModel>();
    private DeviceInformationModel deviceInforModel;
    public TMP_Dropdown dropdown;
    public GameObject contentPanel;
    public Transform jbConnectionParentTransform;
    private List<JBInformationModel> listJBInformationModel = new List<JBInformationModel>();
    public GameObject JB_Item_Prefab;
    // public List<GameObject> list_jb_Detail_Panel_Instantiates = new List<GameObject>();
    public Dictionary<string, JBInformationModel> dic_JBInformationModel = new Dictionary<string, JBInformationModel>();
    public Dictionary<string, GameObject> dic_JBInformationModel_Button = new Dictionary<string, GameObject>();

    private const string noDeviceMessage = "Không có thiết bị kết nối";

    [SerializeField]
    public GameObject module_Canvas;
    public GameObject list_Devices_Panel;
    public GameObject list_JBs_Panel;
    public GameObject jb_Detail_Panel;
    private void Awake()
    {
        list_Devices_Panel ??= module_Canvas.transform.Find("list_Devices").gameObject;
        list_JBs_Panel ??= module_Canvas.transform.Find("JB_TSD_Basic_Panel").gameObject;
        jb_Detail_Panel ??= module_Canvas.transform.Find("Detail_JB_TSD").gameObject;
    }

    private void OnEnable()
    {
        if (GlobalVariable.temp_ListDeviceInformationModel_FromModule.Any())
        {
            listDeviceFromModule = GlobalVariable.temp_ListDeviceInformationModel_FromModule;
        }

        module_Canvas ??= GetComponentInParent<GameObject>();
        StartCoroutine(UpdateUI());
    }

    private IEnumerator UpdateUI()
    {
        yield return null;

        dropdown.options.Clear();

        if (listDeviceFromModule.Any())
        {
            contentPanel.SetActive(true);
            foreach (var model in listDeviceFromModule)
            {
                dropdown.options.Add(new TMP_Dropdown.OptionData(model.Code));
            }
            dropdown.value = 0;
            dropdown.RefreshShownValue();
            dropdown.onValueChanged.AddListener(OnValueChange);
            OnValueChange(0);
        }
        else
        {
            contentPanel.SetActive(false);
            dropdown.options.Add(new TMP_Dropdown.OptionData(noDeviceMessage));
            dropdown.value = 0;
            dropdown.RefreshShownValue();
            ClearDeviceInformation();
        }


    }

    private void OnDisable()
    {

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
            deviceInforModel = listDeviceFromModule[value];
            if (!contentPanel.activeSelf)
            {
                contentPanel.SetActive(true);
            }
            UpdateDeviceInformation(deviceInforModel);
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

        deviceInformation[0].text = device.Code;
        deviceInformation[1].text = device.Function;
        deviceInformation[2].text = device.Range;
        deviceInformation[3].text = device.Unit;
        deviceInformation[4].text = device.IOAddress;

        GlobalVariable.deviceCode = device.Code;
        dic_JBInformationModel.Clear();
        dic_JBInformationModel_Button.Clear();

        if (device.JBInformationModels != null && device.JBInformationModels.Any())
        {
            listJBInformationModel = device.JBInformationModels;
        }

        else if (!listJBInformationModel.Any())
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
                        dic_JBInformationModel.Add(model.Name, model);

                        var new_JB_Item = Instantiate(JB_Item_Prefab, jbConnectionParentTransform);
                        var new_JB_Item_JBInfor = new_JB_Item.GetComponent<JBInfor>();

                        new_JB_Item_JBInfor.SetJBInfor(model);

                        dic_JBInformationModel_Button.Add(model.Name, new_JB_Item);

                        new_JB_Item_JBInfor.button.onClick.RemoveAllListeners();
                        new_JB_Item_JBInfor.button.onClick.AddListener(() =>
                        {
                            GlobalVariable.navigate_from_List_Devices = true;
                            GlobalVariable.navigate_from_list_JBs = false;
                            NavigateJBDetailScreen(model: model);
                        });
                    }
                    dic_JBInformationModel_Button[model.Name].SetActive(true);
                }
                JB_Item_Prefab.SetActive(false);
                return;
            }
            else
            {
                JB_Item_Prefab.SetActive(true);
                var jbInfor = JB_Item_Prefab.GetComponent<JBInfor>();
                jbInfor.SetJBInfor(listJBInformationModel[0]);
                jbInfor.button.onClick.RemoveAllListeners();
                jbInfor.button.onClick.AddListener(() =>
                {
                    GlobalVariable.navigate_from_List_Devices = true;
                    GlobalVariable.navigate_from_list_JBs = false;
                    NavigateJBDetailScreen(model: listJBInformationModel[0]);
                });
            }
        }


    }

    private void ClearDeviceInformation()
    {
        foreach (var infoValue in deviceInformation)
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

        if (GlobalVariable.navigate_from_list_JBs)
        {
            GlobalVariable.navigate_from_JB_TSD_Detail = true;
            list_JBs_Panel.gameObject.SetActive(false);
            jb_Detail_Panel.gameObject.SetActive(true);
        }

        if (GlobalVariable.navigate_from_List_Devices)
        {
            GlobalVariable.navigate_from_JB_TSD_Detail = true;
            list_Devices_Panel.gameObject.SetActive(false);
            jb_Detail_Panel.gameObject.SetActive(true);
        }
    }
}
