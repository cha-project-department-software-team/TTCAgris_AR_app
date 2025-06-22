
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
//! Tạo List Option, nhưng chưa tạo Button để tương tác

public class Initialize_Device_List_Option_Selection : MonoBehaviour
{
    [Header("Canvas")]
    public GameObject Selection_Option_Canvas;

    [Header("list Selection Panels")]
    public GameObject selection_List_JB_Panel;
    public GameObject selection_List_ModuleIO_Panel;
    public GameObject selection_List_Additional_Connection_Image_Panel;

    [Header("list Selection Option Contents")]
    public Transform JB_List_Selection_Option_Content_Transform;
    public Transform Module_List_Selection_Option_Content_Transform;
    public Transform Additional_Connection_Image_List_Selection_Option_Content_Transform;

    public Dictionary<string, GameObject> initialSelectionOptions = new Dictionary<string, GameObject>();

    private List<JBInformationModel> jBInformationModels = new List<JBInformationModel>();
    private List<ModuleInformationModel> moduleInformationModels = new List<ModuleInformationModel>();
    private List<ImageInformationModel> additional_ConnectionImageInformationModels = new List<ImageInformationModel>();


    private void Awake()
    {
        InitializeItemOptions();
        jBInformationModels = GlobalVariable.temp_ListJBInformationModel;
        moduleInformationModels = GlobalVariable.temp_ListModuleInformationModel;
        additional_ConnectionImageInformationModels = GlobalVariable.temp_ListImageInformationModel;
    }
    private void Start()
    {
        PopulateListSelection();
    }

    private void InitializeItemOptions()
    {
        initialSelectionOptions["JB"] = JB_List_Selection_Option_Content_Transform.GetChild(0).gameObject;
        initialSelectionOptions["Module"] = Module_List_Selection_Option_Content_Transform.GetChild(0).gameObject;
        initialSelectionOptions["Additional_Connection_Images"] = Additional_Connection_Image_List_Selection_Option_Content_Transform.GetChild(0).gameObject;
    }
    void PopulateListSelection()
    {
        PopulateSelectionPanel("JB", jBInformationModels, selection_List_JB_Panel,
            JB_List_Selection_Option_Content_Transform, jb => jb.Name);

        PopulateSelectionPanel("Module", moduleInformationModels, selection_List_ModuleIO_Panel,
            Module_List_Selection_Option_Content_Transform, module => module.Name);


        PopulateSelectionPanel("Additional_Connection_Images", additional_ConnectionImageInformationModels, selection_List_Additional_Connection_Image_Panel,
            Additional_Connection_Image_List_Selection_Option_Content_Transform, image => image.Name);
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
