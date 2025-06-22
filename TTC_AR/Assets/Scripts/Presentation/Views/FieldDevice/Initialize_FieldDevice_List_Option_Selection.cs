using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

//! Tạo List Option, nhưng chưa tạo Button để tương tác

public class Initialize_FieldDevice_List_Option_Selection : MonoBehaviour
{
    [Header("Canvas")]
    public GameObject Selection_Option_Canvas;


    [Header("list Selection Panels")]
    public GameObject selection_List_Connection_Image_Panel;


    [Header("list Selection Option Contents")]
    public Transform Connection_Image_List_Selection_Option_Content_Transform;

    public Dictionary<string, GameObject> initialSelectionOptions = new Dictionary<string, GameObject>();

    private void Awake()
    {
        InitializeItemOptions();
    }
    private void Start()
    {
        StartCoroutine(PopulateListSelection());
    }

    private void InitializeItemOptions()
    {
        initialSelectionOptions["Connection_Image"] = Connection_Image_List_Selection_Option_Content_Transform.GetChild(0).gameObject;
    }

    IEnumerator PopulateListSelection()
    {
        while (GlobalVariable.temp_ListImageInformationModel == null)
        {
            Debug.Log("Waiting for GlobalVariable.temp_ModuleSpecificationModel to be assigned...");
            yield return null;
        }
        PopulateSelectionPanel("Connection_Image", GlobalVariable.temp_ListImageInformationModel, selection_List_Connection_Image_Panel, Connection_Image_List_Selection_Option_Content_Transform);
    }
    private void PopulateSelectionPanel(string field, List<ImageInformationModel> imageInformationModels, GameObject list_Option_Panel, Transform list_Option_Content_Transform)
    {
        list_Option_Panel.SetActive(true);

        if (imageInformationModels != null && imageInformationModels.Any())
        {
            if (imageInformationModels.Count == 1)
            {
                SetOptionText(initialSelectionOptions[field], imageInformationModels[0].Name);
                //  AddOption(initialSelectionOptions[field], imageInformationModels[0].Name, field);
                //! Ví dụ: AddOption(initialSelectionOptions["Device"], "02LT002", "Device");
                //! initialSelectionOptions["Device"] => Initial Option
            }
            else
            {
                foreach (var optionName in imageInformationModels)
                {
                    GameObject newOption = Instantiate(initialSelectionOptions[field], list_Option_Content_Transform);
                    SetOptionText(newOption, optionName.Name);
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
