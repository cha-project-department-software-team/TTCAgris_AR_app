
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.CSharp;
using System;
using System.Linq;
//! Tạo List Option, nhưng chưa tạo Button để tương tác

public class Initialize_Image_List_Option_Selection : MonoBehaviour
{
    [Header("Canvas")]
    public GameObject Selection_Option_Canvas;

    [Header("list Selection Panels")]
    public GameObject selection_List_Device_Panel;
    public GameObject selection_List_JB_Panel;
    public GameObject selection_List_TSD_Panel;
    public GameObject selection_List_FieldDevice_Panel;

    [Header("list Selection Option Contents")]
    public Transform Device_List_Selection_Option_Content_Transform;
    public Transform JB_List_Selection_Option_Content_Transform;
    public Transform TSD_List_Selection_Option_Content_Transform;
    public Transform FieldDevice_List_Selection_Option_Content_Transform;
    public Dictionary<string, GameObject> initialSelectionOptions = new Dictionary<string, GameObject>();

    private List<JBInformationModel> tsdInformationModels = new List<JBInformationModel>();
    private List<DeviceInformationModel> deviceInformationModels = new List<DeviceInformationModel>();
    private List<JBInformationModel> jbInformationModels = new List<JBInformationModel>();
    private List<FieldDeviceInformationModel> fieldDeviceInformationModels = new List<FieldDeviceInformationModel>();
    private void Awake()
    {
        InitializeItemOptions();

        deviceInformationModels = GlobalVariable.temp_ListDeviceInformationModel;

        jbInformationModels = GlobalVariable.temp_ListJBInformationModel
        .Where(jb => jb.Name.Contains("JB"))
            .Select(jb => new JBInformationModel(jb.Id, jb.Name))
            .ToList(); ;

        tsdInformationModels = GlobalVariable.temp_ListJBInformationModel
            .Where(jb => jb.Name.Contains("TSD"))
            .Select(jb => new JBInformationModel(jb.Id, jb.Name))
            .ToList();

        fieldDeviceInformationModels = GlobalVariable.temp_ListFieldDeviceInformationModel;

    }
    private void Start()
    {
        PopulateListSelection();
    }

    private void InitializeItemOptions()
    {
        initialSelectionOptions["Devices"] = Device_List_Selection_Option_Content_Transform.GetChild(0).gameObject;
        initialSelectionOptions["JBs"] = JB_List_Selection_Option_Content_Transform.GetChild(0).gameObject;
        initialSelectionOptions["TSDs"] = TSD_List_Selection_Option_Content_Transform.GetChild(0).gameObject;
        initialSelectionOptions["FieldDevices"] = FieldDevice_List_Selection_Option_Content_Transform.GetChild(0).gameObject;
    }
    void PopulateListSelection()
    {
        PopulateSelectionPanel("Devices", deviceInformationModels, selection_List_Device_Panel,
            Device_List_Selection_Option_Content_Transform, device => device.Code); // Lấy Name cho Device

        PopulateSelectionPanel("JBs", jbInformationModels, selection_List_JB_Panel,
                    JB_List_Selection_Option_Content_Transform, image => image.Name); // Lấy Name cho Location Image

        PopulateSelectionPanel("TSDs", tsdInformationModels, selection_List_TSD_Panel,
            TSD_List_Selection_Option_Content_Transform, TSD => TSD.Name); // Lấy Code cho Device

        PopulateSelectionPanel("FieldDevices", fieldDeviceInformationModels, selection_List_FieldDevice_Panel,
            FieldDevice_List_Selection_Option_Content_Transform, fieldDevice => fieldDevice.Name); // Lấy Name cho FieldDevice
    }


    private void PopulateSelectionPanel<T>(
      string field,
      List<T> models,
      GameObject list_Option_Panel,
      Transform list_Option_Content_Transform,
      Func<T, string> getValue)
    {
        if (models == null || models.Count == 0) return;

        list_Option_Panel.SetActive(true);
        Debug.Log($"{field}s: {models.Count}");

        if (models.Count == 1)
        {
            SetOptionText(initialSelectionOptions[field], getValue(models[0]));
        }
        else
        {
            foreach (var option in models)
            {
                GameObject newOption = Instantiate(initialSelectionOptions[field], list_Option_Content_Transform);
                SetOptionText(newOption, getValue(option));
            }
            initialSelectionOptions[field].SetActive(false); // Tắt option mặc định
        }

        var backButton = list_Option_Panel.transform.Find("Background/Appbar/Back_Button")?.GetComponent<Button>();
        if (backButton != null) backButton.onClick.RemoveAllListeners();

        list_Option_Panel.SetActive(false);
    }
    private void SetOptionText(GameObject option, string text)
    {
        TMP_Text textComponent = option.GetComponentInChildren<TMP_Text>();
        if (textComponent != null)
        {
            textComponent.text = text;
        }
    }
    private void OnEnable()
    {
    }
    private void OnDisable()
    {
    }






}
