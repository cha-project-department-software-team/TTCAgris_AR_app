using System;
using System.Collections.Generic;
using System.Linq;
using EasyUI.Progress;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CreateFieldDeviceSettingView : MonoBehaviour, IFieldDeviceView
{
    public Initialize_FieldDevice_List_Option_Selection initialize_FieldDevice_List_Option_Selection;
    private FieldDevicePresenter _presenter;

    [SerializeField] private FieldDeviceInformationModel fieldDeviceInformationModel;

    [Header("Input Fields")]
    [SerializeField] private TMP_InputField Name_TextField;
    [SerializeField] private TMP_InputField RatedPower_TextField;
    [SerializeField] private TMP_InputField RatedCurrent_TextField;
    [SerializeField] private TMP_InputField ActiveCurrent_TextField;
    [SerializeField] private TMP_InputField Note_TextField;

    [Header("Basic")]
    public ScrollRect scrollRect;
    public RectTransform viewPortTransform;
    public GameObject parent_Content_Vertical_Group;
    private Transform temp_Item_Transform;

    [Header("LayOutGroup")]
    public GameObject Connection_Image_Parent_VerticalLayout_Group;

    [Header("Prefab")]
    public GameObject Connection_Image_Item_Prefab;

    [Header("Buttons")]
    [SerializeField] private Button submitButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Button backButtonListSelection;

    public GameObject DialogOneButton;
    public GameObject DialogTwoButton;

    [Header("Canvas")]
    public GameObject List_FieldDevice_Canvas;
    public GameObject Add_New_FieldDevice_Canvas;
    public GameObject Update_FieldDevice_Canvas;

    private readonly Dictionary<string, ImageInformationModel> temp_Dictionary_ImageConnectionModel = new();
    private readonly Dictionary<string, List<GameObject>> selectedGameObjects = new()
    {
        { "Connection_Image", new List<GameObject>() }
    };

    private readonly Dictionary<string, int> selectedCounts = new()
    {
        { "Connection_Image", 0 }
    };
    private int grapperId;
    private Sprite successConfirmButtonSprite;

    // void Awake()
    // {
    //     var fieldDeviceManager = FindObjectOfType<FieldDeviceManager>();
    //     _presenter = new FieldDevicePresenter(this, fieldDeviceManager._IFieldDeviceService);
    // }
    void Awake()
    {
        // var DeviceManager = FindObjectOfType<DeviceManager>();
        _presenter = new FieldDevicePresenter(this, ManagerLocator.Instance.FieldDeviceManager._IFieldDeviceService);
        // DeviceManager._IDeviceService
    }

    void OnEnable()
    {
        successConfirmButtonSprite = Resources.Load<Sprite>("images/UIimages/Success_Back_Button_Background");
        Debug.Log(successConfirmButtonSprite);
        grapperId = GlobalVariable.GrapperId;

        RenewView();

        AddButtonListeners(initialize_FieldDevice_List_Option_Selection.Connection_Image_List_Selection_Option_Content_Transform, "Connection_Image");

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
        if (string.IsNullOrEmpty(Name_TextField.text))
        {
            OpenErrorDialog("Vui lòng nhập tên thiết bị trường");
            return;
        }
        if (GlobalVariable.temp_Dictionary_FieldDeviceInformationModel.ContainsKey(Name_TextField.text))
        {
            OpenErrorDialog("Thiết bị trường đã tồn tại", "Vui lòng nhập tên thiết bị trường khác");
            return;
        }
        fieldDeviceInformationModel = new FieldDeviceInformationModel(
            name: Name_TextField.text,
            ratedPower: string.IsNullOrEmpty(RatedPower_TextField.text) ? "Chưa cập nhật" : RatedPower_TextField.text,
            ratedCurrent: string.IsNullOrEmpty(RatedCurrent_TextField.text) ? "Chưa cập nhật" : RatedCurrent_TextField.text,
            activeCurrent: string.IsNullOrEmpty(ActiveCurrent_TextField.text) ? "Chưa cập nhật" : ActiveCurrent_TextField.text,
            listConnectionImages: temp_Dictionary_ImageConnectionModel.Any() ? temp_Dictionary_ImageConnectionModel.Values.ToList() : new List<ImageInformationModel>(),
            note: string.IsNullOrEmpty(Note_TextField.text) ? "Chưa cập nhật" : Note_TextField.text
        );

        _presenter.CreateNewFieldDevice(grapperId, fieldDeviceInformationModel);
    }

    private void RenewView()
    {
        ClearActiveChildren(Connection_Image_Parent_VerticalLayout_Group);
        ResetAllInputFields();
        temp_Dictionary_ImageConnectionModel.Clear();
        selectedGameObjects["Connection_Image"].Clear();
        selectedCounts["Connection_Image"] = 0;

    }

    public void CloseAddCanvas()
    {
        Add_New_FieldDevice_Canvas.SetActive(false);
        Update_FieldDevice_Canvas.SetActive(false);
        List_FieldDevice_Canvas.SetActive(true);
    }

    private void ResetAllInputFields()
    {
        Name_TextField.text = "";
        RatedPower_TextField.text = "";
        RatedCurrent_TextField.text = "";
        ActiveCurrent_TextField.text = "";
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
            if (field == "Connection_Image" && GlobalVariable.temp_Dictionary_ImageInformationModel.TryGetValue(textValue, out var imageInformationModel))
            {
                var imageInfoModel = new ImageInformationModel(imageInformationModel.Id, imageInformationModel.Name);
                if (!temp_Dictionary_ImageConnectionModel.ContainsKey(textValue))
                {
                    temp_Dictionary_ImageConnectionModel[textValue] = imageInfoModel;
                }
                else
                {
                    Destroy(temp_Item_Transform.gameObject);
                }
            }
        }
        Debug.Log("Text Value: " + textValue);
    }

    public void OpenListImageConnectionSelection() => OpenListSelection("Connection_Image", Connection_Image_Item_Prefab, Connection_Image_Parent_VerticalLayout_Group);

    public void OpenListSelection(string field, GameObject itemPrefab, GameObject parentGroup)
    {
        if (!initialize_FieldDevice_List_Option_Selection.Selection_Option_Canvas.activeSelf)
            initialize_FieldDevice_List_Option_Selection.Selection_Option_Canvas.SetActive(true);

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
        temp_Dictionary_ImageConnectionModel.Remove(item.GetComponentInChildren<TMP_Text>().text);
        Destroy(item);
    }

    public void CloseListSelection()
    {
        initialize_FieldDevice_List_Option_Selection.selection_List_Connection_Image_Panel.SetActive(false);
        initialize_FieldDevice_List_Option_Selection.Selection_Option_Canvas.SetActive(false);
    }

    public void CloseListSelectionFromBackButton()
    {
        ClearDeActiveChildren(Connection_Image_Parent_VerticalLayout_Group);
        temp_Item_Transform.gameObject.SetActive(false);
        initialize_FieldDevice_List_Option_Selection.selection_List_Connection_Image_Panel.SetActive(false);
        initialize_FieldDevice_List_Option_Selection.Selection_Option_Canvas.SetActive(false);
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
            "Connection_Image" => initialize_FieldDevice_List_Option_Selection.selection_List_Connection_Image_Panel,
            _ => throw new ArgumentException("Invalid field name")
        };
    }

    private void Update()
    {
        if (initialize_FieldDevice_List_Option_Selection.selection_List_Connection_Image_Panel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || (Gamepad.current?.buttonEast?.wasPressedThisFrame == true) || (Keyboard.current?.escapeKey.wasPressedThisFrame == true))
            {
                CloseListSelectionFromBackButton();
            }
        }
    }

    private void OpenErrorDialog(string title = "Tạo thiết bị trường mới thất bại", string message = "Đã có lỗi xảy ra khi tạo thiết bị trường mới. Vui lòng thử lại sau.")
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

    private void OpenSuccessDialog(FieldDeviceInformationModel model)
    {
        DialogTwoButton.SetActive(true);

        var backgroundTransform = DialogTwoButton.transform.Find("Background");
        var horizontalGroupTransform = backgroundTransform.Find("Horizontal_Group");

        backgroundTransform.Find("Dialog_Status_Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("images/UIimages/Success_Icon_For_Dialog");
        backgroundTransform.Find("Dialog_Content").GetComponent<TMP_Text>().text = $"Bạn đã thành công thêm thiết bị trường <b><color=#004C8A>{model.Name}</b></color> vào hệ thống";
        backgroundTransform.Find("Dialog_Title").GetComponent<TMP_Text>().text = "Thêm thiết bị trường mới thành công";

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
            scrollRect.verticalNormalizedPosition = 1;
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
        if (GlobalVariable.APIRequestType.Contains("POST_FieldDevice"))
        {

            Show_Toast.Instance.ShowToast("failure", "Thêm thiết bị trường mới thất bại");
            OpenErrorDialog();
        }
        StartCoroutine(Show_Toast.Instance.Set_Instance_Status_False(1f));
    }

    public void ShowSuccess()
    {
        if (GlobalVariable.APIRequestType.Contains("POST_FieldDevice"))
        {
            Show_Toast.Instance.ShowToast("success", "Thêm thiết bị trường mới thành công");
            OpenSuccessDialog(fieldDeviceInformationModel);
        }
        StartCoroutine(Show_Toast.Instance.Set_Instance_Status_False(1f));
    }

    public void DisplayList(List<FieldDeviceInformationModel> models) { }
    public void DisplayCreateResult(bool success) { }
    public void DisplayUpdateResult(bool success) { }
    public void DisplayDeleteResult(bool success) { }

    public void DisplayDetail(FieldDeviceInformationModel model) { }
}
