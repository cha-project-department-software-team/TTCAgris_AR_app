
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.CSharp;
using System;
//! Tạo List Option, nhưng chưa tạo Button để tương tác

public class Initialize_JB_List_Option_Selection : MonoBehaviour
{
    [Header("Canvas")]
    public GameObject Selection_Option_Canvas;

    [Header("list Selection Panels")]
    public GameObject selection_List_Device_Panel;
    public GameObject selection_List_ModuleIO_Panel;
    public GameObject selection_List_Location_Image_Panel;
    public GameObject selection_List_Connection_Image_Panel;

    [Header("list Selection Option Contents")]
    public Transform Device_List_Selection_Option_Content_Transform;
    public Transform Module_List_Selection_Option_Content_Transform;
    public Transform Location_Image_List_Selection_Option_Content_Transform;
    public Transform Connection_Image_List_Selection_Option_Content_Transform;

    public Dictionary<string, GameObject> initialSelectionOptions = new Dictionary<string, GameObject>();
    private List<DeviceInformationModel> deviceInformationModels = new List<DeviceInformationModel>();

    private List<ModuleInformationModel> moduleInformationModels = new List<ModuleInformationModel>();
    private List<ImageInformationModel> locationImageInformationModels = new List<ImageInformationModel>();
    private List<ImageInformationModel> connectionImageInformationModels = new List<ImageInformationModel>();


    private void Awake()
    {
        InitializeItemOptions();
        deviceInformationModels = GlobalVariable.temp_ListDeviceInformationModel;
        moduleInformationModels = GlobalVariable.temp_ListModuleInformationModel;
        locationImageInformationModels = GlobalVariable.temp_ListImageInformationModel;
        connectionImageInformationModels = GlobalVariable.temp_ListImageInformationModel;
    }
    private void Start()
    {
        PopulateListSelection();
    }

    private void InitializeItemOptions()
    {
        initialSelectionOptions["Devices"] = Device_List_Selection_Option_Content_Transform.GetChild(0).gameObject;
        initialSelectionOptions["Modules"] = Module_List_Selection_Option_Content_Transform.GetChild(0).gameObject;
        initialSelectionOptions["Location_Image"] = Location_Image_List_Selection_Option_Content_Transform.GetChild(0).gameObject;
        initialSelectionOptions["Connection_Images"] = Connection_Image_List_Selection_Option_Content_Transform.GetChild(0).gameObject;
    }
    void PopulateListSelection()
    {
        PopulateSelectionPanel("Devices", deviceInformationModels, selection_List_Device_Panel,
            Device_List_Selection_Option_Content_Transform, device => device.Code); // Lấy Code cho Device

        PopulateSelectionPanel("Modules", moduleInformationModels, selection_List_ModuleIO_Panel,
            Module_List_Selection_Option_Content_Transform, module => module.Name); // Lấy Name cho Module

        PopulateSelectionPanel("Location_Image", locationImageInformationModels, selection_List_Location_Image_Panel,
            Location_Image_List_Selection_Option_Content_Transform, image => image.Name); // Lấy Name cho Location Image

        PopulateSelectionPanel("Connection_Images", connectionImageInformationModels, selection_List_Connection_Image_Panel,
            Connection_Image_List_Selection_Option_Content_Transform, image => image.Name); // Lấy Name cho Connection Image
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


    // private void AddOption(GameObject option, string text, string field)
    // {
    //     SetOptionText(option, text);
    // }

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
