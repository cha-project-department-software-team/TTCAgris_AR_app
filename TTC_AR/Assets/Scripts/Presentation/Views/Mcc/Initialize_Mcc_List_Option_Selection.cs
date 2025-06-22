using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

//! Tạo List Option, nhưng chưa tạo Button để tương tác

public class Initialize_Mcc_List_Option_Selection : MonoBehaviour
{
    [Header("Canvas")]
    public GameObject Selection_Option_Canvas;

    [Header("list Selection Panels")]
    public GameObject selection_List_FieldDevices_Panel;

    [Header("list Selection Option Contents")]
    public Transform FieldDevices_List_Selection_Option_Content_Transform;

    public Dictionary<string, GameObject> initialSelectionOptions = new Dictionary<string, GameObject>();
    private List<FieldDeviceInformationModel> fieldDeviceInformationModels = new List<FieldDeviceInformationModel>();

    private void Awake()
    {
        InitializeItemOptions();
        fieldDeviceInformationModels = GlobalVariable.temp_ListFieldDeviceInformationModel;
    }
    private void Start()
    {
        PopulateListSelection();
    }

    private void InitializeItemOptions()
    {
        initialSelectionOptions["FieldDevices"] = FieldDevices_List_Selection_Option_Content_Transform.GetChild(0).gameObject;
    }

    void PopulateListSelection()
    {
        PopulateSelectionPanel(
        field: "FieldDevices",
       fieldDeviceInformationModels: fieldDeviceInformationModels,
       list_Option_Panel: selection_List_FieldDevices_Panel,
       list_Option_Content_Transform: FieldDevices_List_Selection_Option_Content_Transform);
    }
    private void PopulateSelectionPanel(string field, List<FieldDeviceInformationModel> fieldDeviceInformationModels, GameObject list_Option_Panel, Transform list_Option_Content_Transform)
    {
        list_Option_Panel.SetActive(true);
        Debug.Log("FieldDevices: " + fieldDeviceInformationModels.Count);

        if (fieldDeviceInformationModels.Any())
        {
            if (fieldDeviceInformationModels.Count == 1)
            {
                SetOptionText(initialSelectionOptions[field], fieldDeviceInformationModels[0].Name);
                //  AddOption(initialSelectionOptions[field], fieldDeviceInformationModels[0].Name, field);
                //! Ví dụ: AddOption(initialSelectionOptions["Device"], "02LT002", "Device");
                //! initialSelectionOptions["Device"] => Initial Option
            }
            else
            {
                foreach (var option in fieldDeviceInformationModels)
                {
                    GameObject newOption = Instantiate(initialSelectionOptions[field], list_Option_Content_Transform);
                    SetOptionText(newOption, option.Name);
                    //    AddOption(newOption, optionName.Name, field);
                }
                initialSelectionOptions[field].SetActive(false); //! Tắt đi Option mặc định
            }
        }
        var Back_Button = list_Option_Panel.transform.Find("Background/Appbar/Back_Button").gameObject.GetComponent<Button>();
        Back_Button.onClick.RemoveAllListeners();
        list_Option_Panel.SetActive(false);
    }

    private void AddOption(GameObject option, string text, string field)
    {
        SetOptionText(option, text);
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
