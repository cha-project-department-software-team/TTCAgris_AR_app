using System;
using System.Collections.Generic;
using System.Linq;
using EasyUI.Progress;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateModuleSettingView : MonoBehaviour, IModuleView
{
    public Initialize_Module_List_Option_Selection initialize_Module_List_Option_Selection;
    private ModulePresenter _presenter;

    [SerializeField] private ModuleInformationModel ModuleInformationModel;

    [Header("Input Fields")]
    [SerializeField] private TMP_InputField Name_TextField;

    [Header("Basic")]
    public ScrollRect scrollRect;
    public RectTransform viewPortTransform;
    public GameObject parent_Content_Vertical_Group;
    private Transform temp_Item_Transform;

    [Header("LayOutGroup")]
    public GameObject List_Racks_Parent_Vertical_Layout_Group;
    public GameObject List_Devices_Parent_Grid_Layout_Group;
    public GameObject List_JBs_Parent_Grid_Layout_Group;
    public GameObject List_ModuleSpecification_Parent_Vertical_Layout_Group;
    public GameObject List_AdapterSpecification_Parent_Vertical_Layout_Group;

    [Header("Prefab")]
    public GameObject Device_Item_Prefab;
    public GameObject JB_Item_Prefab;
    public GameObject Rack_Item_Prefab;
    public GameObject ModuleSpecification_Item_Prefab;
    public GameObject AdapterSpecification_Item_Prefab;


    [Header("Buttons")]
    [SerializeField] private Button submitButton;
    [SerializeField] private Button backButton;

    [SerializeField] private Button backButtonRackListSelection;
    [SerializeField] private Button backButtonDeviceListSelection;
    [SerializeField] private Button backButtonJBListSelection;
    [SerializeField] private Button backButtonModuleSpecificationListSelection;
    [SerializeField] private Button backButtonAdapterSpecificationListSelection;

    [SerializeField] private GameObject addRackButton;
    [SerializeField] private GameObject addModuleSpeButton;
    [SerializeField] private GameObject addAdapterSpeButton;

    public GameObject DialogOneButton;
    public GameObject DialogTwoButton;

    [Header("Canvas")]
    public GameObject List_Module_Canvas;
    public GameObject Add_New_Module_Canvas;
    public GameObject Update_Module_Canvas;

    private readonly Dictionary<string, RackInformationModel> temp_Dictionary_RackModel = new();
    private readonly Dictionary<string, DeviceInformationModel> temp_Dictionary_DeviceModel = new();
    private readonly Dictionary<string, JBInformationModel> temp_Dictionary_JBModel = new();
    private readonly Dictionary<string, ModuleSpecificationModel> temp_Dictionary_ModuleSpecificationModel = new();
    private readonly Dictionary<string, AdapterSpecificationModel> temp_Dictionary_AdapterSpecificationModel = new();
    private Sprite successConfirmButtonSprite;
    private int grapperId;

    private readonly Dictionary<string, List<GameObject>> selectedGameObjects = new()
    {   { "Racks", new List<GameObject>() },
        { "Devices", new List<GameObject>() },
        { "JBs", new List<GameObject>() },
        { "ModuleSpecifications", new List<GameObject>() },
        { "AdapterSpecifications", new List<GameObject>() },
    };

    private readonly Dictionary<string, int> selectedCounts = new()
    {
        { "Racks", 0 },
        { "Devices", 0 },
        { "JBs", 0 },
        { "ModuleSpecifications", 0 },
        { "AdapterSpecifications", 0 }
    };
    void Awake()
    {
        _presenter = new ModulePresenter(this, ManagerLocator.Instance.ModuleManager._IModuleService);
    }

    void OnEnable()
    {
        grapperId = GlobalVariable.GrapperId;
        successConfirmButtonSprite = Resources.Load<Sprite>("images/UIimages/Success_Back_Button_Background");
        Debug.Log(successConfirmButtonSprite);
        RenewView();

        AddButtonListeners(initialize_Module_List_Option_Selection.Rack_List_Selection_Option_Content_Transform, "Racks");
        AddButtonListeners(initialize_Module_List_Option_Selection.Device_List_Selection_Option_Content_Transform, "Devices");
        AddButtonListeners(initialize_Module_List_Option_Selection.JB_List_Selection_Option_Content_Transform, "JBs");
        AddButtonListeners(initialize_Module_List_Option_Selection.ModuleSpecification_List_Selection_Option_Content_Transform, "ModuleSpecifications");
        AddButtonListeners(initialize_Module_List_Option_Selection.AdapterSpecification_List_Selection_Option_Content_Transform, "AdapterSpecifications");

        backButton.onClick.RemoveAllListeners();
        submitButton.onClick.RemoveAllListeners();

        backButtonRackListSelection.onClick.RemoveAllListeners();
        backButtonDeviceListSelection.onClick.RemoveAllListeners();
        backButtonJBListSelection.onClick.RemoveAllListeners();
        backButtonModuleSpecificationListSelection.onClick.RemoveAllListeners();
        backButtonAdapterSpecificationListSelection.onClick.RemoveAllListeners();

        backButtonRackListSelection.onClick.AddListener(() => CloseListSelectionFromBackButton("Racks"));
        backButtonDeviceListSelection.onClick.AddListener(() => CloseListSelectionFromBackButton("Devices"));
        backButtonJBListSelection.onClick.AddListener(() => CloseListSelectionFromBackButton("JBs"));
        backButtonModuleSpecificationListSelection.onClick.AddListener(() => CloseListSelectionFromBackButton("ModuleSpecifications"));
        backButtonAdapterSpecificationListSelection.onClick.AddListener(() => CloseListSelectionFromBackButton("AdapterSpecifications"));

        backButton.onClick.AddListener(CloseAddCanvas);
        submitButton.onClick.AddListener(OnSubmitButtonClick);

        scrollRect.verticalNormalizedPosition = 1;
    }

    void OnDisable()
    {
        RenewView();
    }

    private void OnSubmitButtonClick()
    {
        if (string.IsNullOrEmpty(Name_TextField.text))
        {
            OpenErrorDialog("Vui lòng nhập mã Module");
            return;
        }
        if (GlobalVariable.temp_Dictionary_ModuleInformationModel.ContainsKey(Name_TextField.text))
        {
            OpenErrorDialog("Module này đã tồn tại", "Vui lòng nhập mã Module khác");
            return;
        }
        ModuleInformationModel = new ModuleInformationModel(
            name: Name_TextField.text,
            rack: temp_Dictionary_RackModel.Any() ? temp_Dictionary_RackModel.Values.ToList().LastOrDefault() : null,
            listDeviceInformationModel: temp_Dictionary_DeviceModel.Any() ? temp_Dictionary_DeviceModel.Values.ToList() : new List<DeviceInformationModel>(),
            listJBInformationModel: temp_Dictionary_JBModel.Any() ? temp_Dictionary_JBModel.Values.ToList() : new List<JBInformationModel>(),
            moduleSpecificationModel: temp_Dictionary_ModuleSpecificationModel.Any() ? temp_Dictionary_ModuleSpecificationModel.Values.ToList().LastOrDefault() : null,
            adapterSpecificationModel: temp_Dictionary_AdapterSpecificationModel.Any() ? temp_Dictionary_AdapterSpecificationModel.Values.ToList().LastOrDefault() : null
            );

        if (ModuleInformationModel != null)
        {
            _presenter.CreateNewModule(grapperId, ModuleInformationModel);
        }
    }

    private void RenewView()
    {
        addRackButton.SetActive(true);
        addModuleSpeButton.SetActive(true);
        addAdapterSpeButton.SetActive(true);

        Device_Item_Prefab.SetActive(false);
        JB_Item_Prefab.SetActive(false);
        Rack_Item_Prefab.SetActive(false);
        ModuleSpecification_Item_Prefab.SetActive(false);
        AdapterSpecification_Item_Prefab.SetActive(false);

        ResetAllInputFields();

        ClearActiveChildren(List_Racks_Parent_Vertical_Layout_Group);
        ClearActiveChildren(List_Devices_Parent_Grid_Layout_Group);
        ClearActiveChildren(List_JBs_Parent_Grid_Layout_Group);
        ClearActiveChildren(List_ModuleSpecification_Parent_Vertical_Layout_Group);
        ClearActiveChildren(List_AdapterSpecification_Parent_Vertical_Layout_Group);


        temp_Dictionary_RackModel.Clear();
        temp_Dictionary_DeviceModel.Clear();
        temp_Dictionary_JBModel.Clear();
        temp_Dictionary_ModuleSpecificationModel.Clear();
        temp_Dictionary_AdapterSpecificationModel.Clear();

        selectedGameObjects["Racks"].Clear();
        selectedGameObjects["Devices"].Clear();
        selectedGameObjects["ModuleSpecifications"].Clear();
        selectedGameObjects["JBs"].Clear();
        selectedGameObjects["AdapterSpecifications"].Clear();

        selectedCounts["Racks"] = 0;
        selectedCounts["Devices"] = 0;
        selectedCounts["JBs"] = 0;
        selectedCounts["ModuleSpecifications"] = 0;
        selectedCounts["AdapterSpecifications"] = 0;

        scrollRect.verticalNormalizedPosition = 1;
        // scrollRect.content.localPosition = new Vector3(0, 0, 0);
    }
    private void ResetAllInputFields()
    {
        Name_TextField.text = "";
    }
    public void CloseAddCanvas()
    {
        Add_New_Module_Canvas.SetActive(false);
        Update_Module_Canvas.SetActive(false);
        List_Module_Canvas.SetActive(true);
    }


    private void AddButtonListeners(Transform contentTransform, string field)
    {
        foreach (Transform option in contentTransform)
        {
            AddButtonListener(option, () => SelectOption(option.gameObject.GetComponentInChildren<TMP_Text>().text, field));
        }
    }

    private void AddButtonListener(Transform option, UnityEngine.Events.UnityAction action)
    {
        var button = option.gameObject.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(action);
        }
    }

    private void SelectOption(string optionTextValue, string field)
    {
        SetItemTextValue(temp_Item_Transform, optionTextValue, field);
        CloseListSelection(field);

        selectedCounts[field]++;
        if (!selectedGameObjects[field].Contains(temp_Item_Transform.gameObject))
        {
            selectedGameObjects[field].Add(temp_Item_Transform.gameObject);
        }
        else
        {
            Destroy(temp_Item_Transform.gameObject);
        }
    }

    private void SetItemTextValue(Transform temp_Item_Transform, string textValue, string field)
    {
        var itemText = temp_Item_Transform.GetComponentInChildren<TMP_Text>();

        if (itemText == null) return;

        itemText.text = textValue;

        switch (field)
        {
            case "Racks":
                if (GlobalVariable.temp_Dictionary_RackInformationModel.TryGetValue(textValue, out var RackInfo))
                {
                    if (!temp_Dictionary_RackModel.ContainsKey(textValue))
                    {
                        temp_Dictionary_RackModel[textValue] = new RackInformationModel(RackInfo.Id, RackInfo.Name);
                    }
                    else
                    {
                        Destroy(temp_Item_Transform.gameObject);
                    }
                }
                break;
            case "Devices":
                if (GlobalVariable.temp_Dictionary_DeviceInformationModel.TryGetValue(textValue, out var deviceInfo))
                {
                    if (!temp_Dictionary_DeviceModel.ContainsKey(textValue))
                    {
                        temp_Dictionary_DeviceModel[textValue] = new DeviceInformationModel(deviceInfo.Id, deviceInfo.Code);
                    }
                    else
                    {
                        Destroy(temp_Item_Transform.gameObject);
                    }
                }
                break;

            case "JBs":
                if (GlobalVariable.temp_Dictionary_JBInformationModel.TryGetValue(textValue, out var jbInfo))
                {
                    if (!temp_Dictionary_JBModel.ContainsKey(textValue))
                    {
                        temp_Dictionary_JBModel[textValue] = new JBInformationModel(jbInfo.Id, jbInfo.Name);
                    }
                    else
                    {
                        Destroy(temp_Item_Transform.gameObject);
                    }
                }
                break;

            case "ModuleSpecifications":
                if (GlobalVariable.temp_Dictionary_ModuleSpecificationModel.TryGetValue(textValue, out var moduleSpecificationModel))
                {
                    if (!temp_Dictionary_ModuleSpecificationModel.ContainsKey(textValue))
                    {
                        temp_Dictionary_ModuleSpecificationModel[textValue] = new ModuleSpecificationModel(moduleSpecificationModel.Id, moduleSpecificationModel.Code);
                    }
                    else
                    {
                        Destroy(temp_Item_Transform.gameObject);
                    }
                }
                break;

            case "AdapterSpecifications":
                if (GlobalVariable.temp_Dictionary_AdapterSpecificationModel.TryGetValue(textValue, out var adapterSpecificationModel))
                {
                    if (!temp_Dictionary_AdapterSpecificationModel.ContainsKey(textValue))
                    {
                        temp_Dictionary_AdapterSpecificationModel[textValue] = new AdapterSpecificationModel(adapterSpecificationModel.Id, adapterSpecificationModel.Code);
                    }
                    else
                    {
                        Destroy(temp_Item_Transform.gameObject);
                    }
                }
                break;
        }
    }
    public void OpenListRackSelection() => OpenListSelection("Racks", Rack_Item_Prefab, List_Racks_Parent_Vertical_Layout_Group);
    public void OpenListDeviceSelection() => OpenListSelection("Devices", Device_Item_Prefab, List_Devices_Parent_Grid_Layout_Group);
    public void OpenLisJBSelection() => OpenListSelection("JBs", JB_Item_Prefab, List_JBs_Parent_Grid_Layout_Group);
    public void OpenListModuleSpecificationSelection() => OpenListSelection("ModuleSpecifications", ModuleSpecification_Item_Prefab, List_ModuleSpecification_Parent_Vertical_Layout_Group);
    public void OpenListAdapterSpecificationSelection() => OpenListSelection("AdapterSpecifications", AdapterSpecification_Item_Prefab, List_AdapterSpecification_Parent_Vertical_Layout_Group);

    public void OpenListSelection(string field, GameObject itemPrefab, GameObject parentGroup)
    {
        // if (!initialize_Module_List_Option_Selection.Selection_Option_Canvas.activeSelf)
        //     initialize_Module_List_Option_Selection.Selection_Option_Canvas.SetActive(true);
        if (field == "Racks")
        {
            addRackButton.SetActive(false);
        }
        if (field == "ModuleSpecifications")
        {
            addModuleSpeButton.SetActive(false);
        }
        if (field == "AdapterSpecifications")
        {
            addAdapterSpeButton.SetActive(false);
        }
        var newItem = Instantiate(itemPrefab, parentGroup.transform);
        newItem.SetActive(true);
        temp_Item_Transform = newItem.transform;
        var button = newItem.GetComponentInChildren<Button>();
        button.onClick.AddListener(() => DeselectItem(newItem, field));
        GetSelectionPanel(field).SetActive(true);
    }

    private void DeselectItem(GameObject item, string field)
    {
        selectedCounts[field]--;
        selectedGameObjects[field].Remove(item);
        temp_Dictionary_RackModel.Remove(item.GetComponentInChildren<TMP_Text>().text);
        temp_Dictionary_DeviceModel.Remove(item.GetComponentInChildren<TMP_Text>().text);
        temp_Dictionary_JBModel.Remove(item.GetComponentInChildren<TMP_Text>().text);
        temp_Dictionary_ModuleSpecificationModel.Remove(item.GetComponentInChildren<TMP_Text>().text);
        temp_Dictionary_AdapterSpecificationModel.Remove(item.GetComponentInChildren<TMP_Text>().text);
        Destroy(item);

        if (field == "Racks")
        {
            addRackButton.SetActive(true);
        }
        if (field == "ModuleSpecifications")
        {
            addModuleSpeButton.SetActive(true);
        }
        if (field == "AdapterSpecifications")
        {
            addAdapterSpeButton.SetActive(true);
        }
    }

    public void CloseListSelection(string field)
    {
        switch (field)
        {
            case "Racks":
                initialize_Module_List_Option_Selection.selection_List_Rack_Panel.SetActive(false);
                break;
            case "Devices":
                initialize_Module_List_Option_Selection.selection_List_Device_Panel.SetActive(false);
                break;
            case "JBs":
                initialize_Module_List_Option_Selection.selection_List_JB_Panel.SetActive(false);
                break;
            case "ModuleSpecifications":
                initialize_Module_List_Option_Selection.selection_List_ModuleSpecification_Panel.SetActive(false);
                break;
            case "AdapterSpecifications":
                initialize_Module_List_Option_Selection.selection_List_AdapterSpecification_Panel.SetActive(false);
                break;
        }
        // initialize_Module_List_Option_Selection.Selection_Option_Canvas.SetActive(false);
    }

    public void CloseListSelectionFromBackButton(string field)
    {
        temp_Item_Transform.gameObject.SetActive(false);
        switch (field)
        {
            case "Racks":
                initialize_Module_List_Option_Selection.selection_List_Rack_Panel.SetActive(false);
                addRackButton.SetActive(true);
                break;
            case "Devices":
                initialize_Module_List_Option_Selection.selection_List_Device_Panel.SetActive(false);
                break;
            case "JBs":
                initialize_Module_List_Option_Selection.selection_List_JB_Panel.SetActive(false);
                break;
            case "ModuleSpecifications":
                initialize_Module_List_Option_Selection.selection_List_ModuleSpecification_Panel.SetActive(false);
                addModuleSpeButton.SetActive(true);
                break;
            case "AdapterSpecifications":
                initialize_Module_List_Option_Selection.selection_List_AdapterSpecification_Panel.SetActive(false);
                addAdapterSpeButton.SetActive(true);
                break;
        }
        // initialize_Module_List_Option_Selection.Selection_Option_Canvas.SetActive(false);
    }

    private void ClearDeActiveChildren(GameObject parentGroup)
    {
        foreach (Transform child in parentGroup.transform)
        {
            if (!child.gameObject.activeSelf && parentGroup.transform.childCount > 1)
                Destroy(child.gameObject);
        }
    }

    private void ClearActiveChildren(GameObject parentGroup)
    {
        foreach (Transform child in parentGroup.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private GameObject GetSelectionPanel(string field)
    {
        return field switch
        {
            "Racks" => initialize_Module_List_Option_Selection.selection_List_Rack_Panel,
            "Devices" => initialize_Module_List_Option_Selection.selection_List_Device_Panel,
            "JBs" => initialize_Module_List_Option_Selection.selection_List_JB_Panel,
            "ModuleSpecifications" => initialize_Module_List_Option_Selection.selection_List_ModuleSpecification_Panel,
            "AdapterSpecifications" => initialize_Module_List_Option_Selection.selection_List_AdapterSpecification_Panel,

            _ => throw new ArgumentException("Invalid field Name")
        };
    }

    private void OpenErrorDialog(string title = "Tạo Module IO mới thất bại", string message = "Đã có lỗi xảy ra khi tạo Module IO mới. Vui lòng thử lại sau.")
    {
        DialogOneButton.SetActive(true);
        var backButton = DialogOneButton.transform.Find("Background/Back_Button").GetComponent<Button>();
        backButton.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/UIimages/Error_Back_Button_Background");
        DialogOneButton.transform.Find("Background/Dialog_Status_Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("images/UIimages/Error_Icon_For_Dialog");
        DialogOneButton.transform.Find("Background/Dialog_Content").GetComponent<TMP_Text>().text = message;
        DialogOneButton.transform.Find("Background/Dialog_Title").GetComponent<TMP_Text>().text = title;

        backButton.onClick.RemoveAllListeners();
        backButton.onClick.AddListener(() =>
        {
            DialogOneButton.SetActive(false);
        });
    }

    private void OpenSuccessDialog(ModuleInformationModel model)
    {
        DialogTwoButton.SetActive(true);

        var backgroundTransform = DialogTwoButton.transform.Find("Background");
        var horizontalGroupTransform = backgroundTransform.Find("Horizontal_Group");

        backgroundTransform.Find("Dialog_Status_Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("images/UIimages/Success_Icon_For_Dialog");
        backgroundTransform.Find("Dialog_Content").GetComponent<TMP_Text>().text = $"Bạn đã thành công thêm Module IO <b><color=#004C8A>{model.Name}</b></color> vào hệ thống";
        backgroundTransform.Find("Dialog_Title").GetComponent<TMP_Text>().text = "Thêm Module IO mới thành công";

        var confirmButton = horizontalGroupTransform.Find("Confirm_Button").GetComponent<Button>();
        var backButton = horizontalGroupTransform.Find("Back_Button").GetComponent<Button>();


        var confirmButtonSprite = confirmButton.GetComponent<Image>();
        confirmButtonSprite.sprite = successConfirmButtonSprite;

        var confirmButtonText = confirmButton.GetComponentInChildren<TMP_Text>();
        var backButtonText = backButton.GetComponentInChildren<TMP_Text>();

        // var colors = confirmButton.colors;
        // colors.normalColor = new Color32(92, 237, 115, 255); // #5CED73 in RGB
        // confirmButton.colors = colors;

        confirmButtonText.text = "Tiếp tục thêm mới";
        backButtonText.text = "Trở lại danh sách";

        confirmButton.onClick.RemoveAllListeners();
        backButton.onClick.RemoveAllListeners();

        confirmButton.onClick.AddListener(() =>
        {
            ResetAllInputFields();
            DialogTwoButton.SetActive(false);
            scrollRect.verticalNormalizedPosition = 1;
            RenewView();
        });

        backButton.onClick.AddListener(() =>
        {
            ResetAllInputFields();
            DialogTwoButton.SetActive(false);
            RenewView();
            CloseAddCanvas();
        });
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
        if (GlobalVariable.APIRequestType.Contains("POST_Module"))
        {
            OpenErrorDialog();
        }
    }

    public void ShowSuccess(string message)
    {
        if (GlobalVariable.APIRequestType.Contains("POST_Module"))
        {
            Show_Toast.Instance.ShowToast("success", message);
            OpenSuccessDialog(ModuleInformationModel);
        }

        StartCoroutine(Show_Toast.Instance.Set_Instance_Status_False(1f));
    }

    public void DisplayList(List<ModuleInformationModel> models) { }
    public void DisplayCreateResult(bool success) { }
    public void DisplayUpdateResult(bool success) { }
    public void DisplayDeleteResult(bool success) { }
    public void DisplayDetail(ModuleInformationModel model) { }
}
