using System;
using System.Collections.Generic;
using System.Linq;
using EasyUI.Progress;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CreateMccSettingView : MonoBehaviour, IMccView
{
    public Initialize_Mcc_List_Option_Selection initialize_Mcc_List_Option_Selection;
    private MccPresenter _presenter;

    [SerializeField] private MccInformationModel MccInformationModel;

    [Header("Input Fields")]
    [SerializeField] private TMP_InputField CabinetCode_TextField;
    [SerializeField] private TMP_InputField Brand_TextField;
    [SerializeField] private TMP_InputField Note_TextField;

    [Header("Basic")]
    public ScrollRect scrollRect;
    public RectTransform viewPortTransform;
    public GameObject parent_Content_Vertical_Group;
    private Transform temp_Item_Transform;

    [Header("LayOutGroup")]
    public GameObject List_FieldDevices_Parent_GridLayout_Group;

    [Header("Prefab")]
    public GameObject fieldDevice_Item_Prefab;

    [Header("Buttons")]
    [SerializeField] private Button submitButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Button backButtonListSelection;

    public GameObject DialogOneButton;
    public GameObject DialogTwoButton;

    [Header("Canvas")]
    public GameObject List_Mcc_Canvas;
    public GameObject Add_New_Mcc_Canvas;
    public GameObject Update_Mcc_Canvas;
    private int grapperId;
    private Sprite successConfirmButtonSprite;

    private readonly Dictionary<string, FieldDeviceInformationModel> temp_Dictionary_FieldDeviceModel = new();
    private readonly Dictionary<string, List<GameObject>> selectedGameObjects = new()
    {
        { "FieldDevices", new List<GameObject>() }
    };

    private readonly Dictionary<string, int> selectedCounts = new()
    {
        { "FieldDevices", 0 }
    };

    void Awake()
    {
       
        _presenter = new MccPresenter(this, ManagerLocator.Instance.MccManager._IMccService);
    }


    void OnEnable()
    {
        successConfirmButtonSprite = Resources.Load<Sprite>("images/UIimages/Success_Back_Button_Background");
        Debug.Log(successConfirmButtonSprite);
        grapperId = GlobalVariable.GrapperId;

        RenewView();

        AddButtonListeners(initialize_Mcc_List_Option_Selection.FieldDevices_List_Selection_Option_Content_Transform, "FieldDevices");

        backButton.onClick.RemoveAllListeners();
        submitButton.onClick.RemoveAllListeners();
        backButtonListSelection.onClick.RemoveAllListeners();

        backButtonListSelection.onClick.AddListener(CloseListSelectionFromBackButton);
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
        if (string.IsNullOrEmpty(CabinetCode_TextField.text))
        {
            OpenErrorDialog("Vui lòng nhập mã tủ Mcc");
        }
        if (GlobalVariable.temp_Dictionary_MCCInformationModel.ContainsKey(CabinetCode_TextField.text))
        {
            OpenErrorDialog("Mã tủ Mcc đã tồn tại", "Vui lòng nhập mã tủ Mcc khác");
        }

        MccInformationModel = new MccInformationModel(
            cabinetCode: CabinetCode_TextField.text,
            brand: string.IsNullOrEmpty(Brand_TextField.text) ? "Chưa cập nhật" : Brand_TextField.text,
            listFieldDeviceInformation: temp_Dictionary_FieldDeviceModel.Any() ? temp_Dictionary_FieldDeviceModel.Values.ToList() : new List<FieldDeviceInformationModel>(),
            note: string.IsNullOrEmpty(Note_TextField.text) ? "Chưa cập nhật" : Note_TextField.text
        );

        _presenter.CreateNewMcc(grapperId, MccInformationModel);
    }

    private void RenewView()
    {
        ClearActiveChildren(List_FieldDevices_Parent_GridLayout_Group);
        ResetAllInputFields();
        temp_Dictionary_FieldDeviceModel.Clear();
        selectedGameObjects["FieldDevices"].Clear();
        selectedCounts["FieldDevices"] = 0;
        scrollRect.verticalNormalizedPosition = 1;

    }

    public void CloseAddCanvas()
    {
        Add_New_Mcc_Canvas.SetActive(false);
        Update_Mcc_Canvas.SetActive(false);
        List_Mcc_Canvas.SetActive(true);
    }

    private void ResetAllInputFields()
    {
        CabinetCode_TextField.text = "";
        Brand_TextField.text = "";
        Note_TextField.text = "";
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
        CloseListSelection();

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
        if (itemText != null)
        {
            itemText.text = textValue;
            if (field == "FieldDevices")
            {
                if (GlobalVariable.temp_Dictionary_FieldDeviceInformationModel.TryGetValue(textValue, out var listFieldDevice))
                {
                    if (listFieldDevice.Count == 1)
                    {
                        var FieldDeviceInfoModel = new FieldDeviceInformationModel(listFieldDevice[0].Id, listFieldDevice[0].Name);
                        if (!temp_Dictionary_FieldDeviceModel.ContainsKey(textValue))
                        {
                            temp_Dictionary_FieldDeviceModel[textValue] = FieldDeviceInfoModel;
                        }
                        else
                        {
                            Destroy(temp_Item_Transform.gameObject);
                        }
                    }
                    else
                    {
                        // var FieldDeviceInfoModel = new FieldDeviceInformationModel(listFieldDevice[0].Id, listFieldDevice[0].Name);
                        // if (!temp_Dictionary_FieldDeviceModel.ContainsKey(textValue))
                        // {
                        //     temp_Dictionary_FieldDeviceModel[textValue] = FieldDeviceInfoModel;
                        // }
                        // else
                        // {
                        //     Destroy(temp_Item_Transform.gameObject);
                        // }
                        Debug.Log("Multiple FieldDeviceInformationModels found for the same name. Please check your data.");

                    }
                }
            }
        }
        Debug.Log("Text Value: " + textValue);
    }

    public void OpenListFieldDeviceConnectionSelection() => OpenListSelection("FieldDevices", fieldDevice_Item_Prefab, List_FieldDevices_Parent_GridLayout_Group);

    public void OpenListSelection(string field, GameObject itemPrefab, GameObject parentGroup)
    {
        if (!initialize_Mcc_List_Option_Selection.Selection_Option_Canvas.activeSelf)
            initialize_Mcc_List_Option_Selection.Selection_Option_Canvas.SetActive(true);

        var newItem = Instantiate(itemPrefab, parentGroup.transform);
        newItem.SetActive(true);
        temp_Item_Transform = newItem.transform;
        temp_Item_Transform.gameObject.GetComponentInChildren<Button>().onClick.AddListener(() => DeselectItem(newItem.gameObject, field));
        GetSelectionPanel(field).SetActive(true);
    }

    private void DeselectItem(GameObject item, string field)
    {
        selectedCounts[field]--;
        selectedGameObjects[field].Remove(item);
        temp_Dictionary_FieldDeviceModel.Remove(item.GetComponentInChildren<TMP_Text>().text);
        Destroy(item);
    }

    public void CloseListSelection()
    {
        initialize_Mcc_List_Option_Selection.selection_List_FieldDevices_Panel.SetActive(false);
        initialize_Mcc_List_Option_Selection.Selection_Option_Canvas.SetActive(false);
    }

    public void CloseListSelectionFromBackButton()
    {
        ClearDeActiveChildren(List_FieldDevices_Parent_GridLayout_Group);
        temp_Item_Transform.gameObject.SetActive(false);
        initialize_Mcc_List_Option_Selection.selection_List_FieldDevices_Panel.SetActive(false);
        initialize_Mcc_List_Option_Selection.Selection_Option_Canvas.SetActive(false);
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
            "FieldDevices" => initialize_Mcc_List_Option_Selection.selection_List_FieldDevices_Panel,
            _ => throw new ArgumentException("Invalid field CabinetCode")
        };
    }

    private void Update()
    {
        if (initialize_Mcc_List_Option_Selection.selection_List_FieldDevices_Panel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || (Gamepad.current?.buttonEast?.wasPressedThisFrame == true) || (Keyboard.current?.escapeKey.wasPressedThisFrame == true))
            {
                CloseListSelectionFromBackButton();
            }
        }
    }

    private void OpenErrorDialog(string title = "Tạo tủ Mcc mới thất bại", string message = "Đã có lỗi xảy ra khi tạo tủ Mcc mới. Vui lòng thử lại sau.")
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
            RenewView();
            DialogOneButton.SetActive(false);
        });
    }

    private void OpenSuccessDialog(MccInformationModel model)
    {
        DialogTwoButton.SetActive(true);

        var backgroundTransform = DialogTwoButton.transform.Find("Background");
        var horizontalGroupTransform = backgroundTransform.Find("Horizontal_Group");

        backgroundTransform.Find("Dialog_Status_Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("images/UIimages/Success_Icon_For_Dialog");
        backgroundTransform.Find("Dialog_Content").GetComponent<TMP_Text>().text = $"Bạn đã thành công thêm tủ Mcc <color=#004C8A><b>{model.CabinetCode}</b></color> vào hệ thống";

        backgroundTransform.Find("Dialog_Title").GetComponent<TMP_Text>().text = "Thêm tủ Mcc mới thành công";


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
            DialogTwoButton.SetActive(false);
            RenewView();
        });

        backButton.onClick.AddListener(() =>
        {
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
        if (GlobalVariable.APIRequestType.Contains("POST_Mcc"))
        {
            OpenErrorDialog();
        }

    }

    public void ShowSuccess()
    {

        if (GlobalVariable.APIRequestType.Contains("POST_Mcc"))
        {
            Show_Toast.Instance.ShowToast("success", "Thêm tủ Mcc mới thành công");
            OpenSuccessDialog(MccInformationModel);
        }

        StartCoroutine(Show_Toast.Instance.Set_Instance_Status_False(1f));
    }

    public void DisplayList(List<MccInformationModel> models) { }
    public void DisplayCreateResult(bool success) { }
    public void DisplayUpdateResult(bool success) { }
    public void DisplayDeleteResult(bool success) { }

    public void DisplayDetail(MccInformationModel model) { }

    public void DisplayFieldDeviceList(List<FieldDeviceInformationModel> models)
    {
    }
}
