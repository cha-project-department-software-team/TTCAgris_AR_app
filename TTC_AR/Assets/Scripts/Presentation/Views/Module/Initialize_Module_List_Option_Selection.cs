
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.CSharp;
using System;
//! Tạo List Option, nhưng chưa tạo Button để tương tác

public class Initialize_Module_List_Option_Selection : MonoBehaviour
{
    [Header("Canvas")]
    public GameObject Selection_Option_Canvas;

    [Header("list Selection Panels")]
    public GameObject selection_List_Rack_Panel;
    public GameObject selection_List_Device_Panel;
    public GameObject selection_List_JB_Panel;
    public GameObject selection_List_ModuleSpecification_Panel;
    public GameObject selection_List_AdapterSpecification_Panel;

    [Header("list Selection Option Contents")]
    public Transform Rack_List_Selection_Option_Content_Transform;
    public Transform Device_List_Selection_Option_Content_Transform;
    public Transform JB_List_Selection_Option_Content_Transform;
    public Transform ModuleSpecification_List_Selection_Option_Content_Transform;
    public Transform AdapterSpecification_List_Selection_Option_Content_Transform;

    public Dictionary<string, GameObject> initialSelectionOptions = new Dictionary<string, GameObject>();

    private List<RackInformationModel> rackInformationModels = new List<RackInformationModel>();
    private List<DeviceInformationModel> deviceInformationModels = new List<DeviceInformationModel>();
    private List<JBInformationModel> jbInformationModels = new List<JBInformationModel>();
    private List<ModuleSpecificationModel> moduleSpecificationModels = new List<ModuleSpecificationModel>();
    private List<AdapterSpecificationModel> adapterSpecificationModels = new List<AdapterSpecificationModel>();
    private void Awake()
    {
        InitializeItemOptions();
        deviceInformationModels = GlobalVariable.temp_ListDeviceInformationModel;
        jbInformationModels = GlobalVariable.temp_ListJBInformationModel;
        rackInformationModels = GlobalVariable.temp_ListRackInformationModel;
        moduleSpecificationModels = GlobalVariable.temp_ListModuleSpecificationModel;
        adapterSpecificationModels = GlobalVariable.temp_ListAdapterSpecificationModel;
    }
    private void Start()
    {
        PopulateListSelection();
    }

    private void InitializeItemOptions()
    {
        initialSelectionOptions["Racks"] = Rack_List_Selection_Option_Content_Transform.GetChild(0).gameObject;
        initialSelectionOptions["Devices"] = Device_List_Selection_Option_Content_Transform.GetChild(0).gameObject;
        initialSelectionOptions["JBs"] = JB_List_Selection_Option_Content_Transform.GetChild(0).gameObject;
        initialSelectionOptions["ModuleSpecifications"] = ModuleSpecification_List_Selection_Option_Content_Transform.GetChild(0).gameObject;
        initialSelectionOptions["AdapterSpecifications"] = AdapterSpecification_List_Selection_Option_Content_Transform.GetChild(0).gameObject;
    }
    void PopulateListSelection()
    {
        PopulateSelectionPanel("Racks", rackInformationModels, selection_List_Rack_Panel,
            Rack_List_Selection_Option_Content_Transform, Rack => Rack.Name); // Lấy Code cho Device

        PopulateSelectionPanel("Devices", deviceInformationModels, selection_List_Device_Panel,
            Device_List_Selection_Option_Content_Transform, device => device.Code); // Lấy Code cho Device

        PopulateSelectionPanel("JBs", jbInformationModels, selection_List_JB_Panel,
            JB_List_Selection_Option_Content_Transform, image => image.Name); // Lấy Name cho Location Image

        PopulateSelectionPanel("ModuleSpecifications", moduleSpecificationModels, selection_List_ModuleSpecification_Panel,
                 ModuleSpecification_List_Selection_Option_Content_Transform, module => module.Code); // Lấy Name cho Module

        PopulateSelectionPanel("AdapterSpecifications", adapterSpecificationModels, selection_List_AdapterSpecification_Panel,
            AdapterSpecification_List_Selection_Option_Content_Transform, image => image.Code); // Lấy Name cho Connection Image
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
                newOption.SetActive(true);
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
