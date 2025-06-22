
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.CSharp;
using System;
//! Tạo List Option, nhưng chưa tạo Button để tương tác

public class Initialize_Rack_List_Option_Selection : MonoBehaviour
{
    [Header("Canvas")]
    public GameObject Selection_Option_Canvas;

    [Header("list Selection Panels")]
    public GameObject selection_List_Module_Panel;

    [Header("list Selection Option Contents")]
    public Transform Module_List_Selection_Option_Content_Transform;

    public Dictionary<string, GameObject> initialSelectionOptions = new Dictionary<string, GameObject>();

    private List<ModuleInformationModel> moduleInformationModels = new List<ModuleInformationModel>();

    private void Awake()
    {
        InitializeItemOptions();
        moduleInformationModels = GlobalVariable.temp_ListModuleInformationModel;

    }
    private void Start()
    {
        PopulateListSelection();
    }

    private void InitializeItemOptions()
    {
        initialSelectionOptions["Modules"] = Module_List_Selection_Option_Content_Transform.GetChild(0).gameObject;
    }
    void PopulateListSelection()
    {
        PopulateSelectionPanel("Modules", moduleInformationModels, selection_List_Module_Panel,
            Module_List_Selection_Option_Content_Transform, Module => Module.Name); // Lấy Code cho Device
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
