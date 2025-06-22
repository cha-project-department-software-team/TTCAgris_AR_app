using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EasyUI.Progress;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CreateImageSettingView : MonoBehaviour, IImageView
{
    public Initialize_Image_List_Option_Selection initialize_Image_List_Option_Selection;

    public WebCamPhotoCamera webCamPhotoCamera;
    public PickPhotoFromCamera pickPhotoFromCamera;
    public PickPhotoFromGallery pickPhotoFromGallery;

    [Header("Send Request")]
    [SerializeField] private ImagePresenter _presenter;
    public Button sendRequestButton;
    public TMP_Text imageName;

    [Header("Canvas")]
    public GameObject List_Image_Canvas;
    public GameObject Add_New_Image_Canvas;
    public GameObject ConfirmRequestCanvas;
    public GameObject PickPhotoFunctionCanvas;


    [Header("Sprites")]
    private Sprite selectedSprite;
    private Sprite normalSprite;

    private Sprite Colorful_Camera_Selection;
    private Sprite Colorful_Non_Image_Status_Icon;
    private Sprite Colorful_Gallery_Selection;

    private Sprite Grey_Camera_Selection;
    private Sprite Grey_Non_Image_Status_Icon;
    private Sprite Grey_Gallery_Selection;

    [Header("Images")]
    public Image photoIcon;
    public Image CameraIcon;
    public Image GalleryIcon;


    [Header("LayOut Group")]
    public ScrollRect scrollRect;
    public RectTransform viewPortTransform;
    public GameObject parent_Content_Vertical_Group;


    [Header("Back Buttons")]
    [SerializeField] private Button backButton;
    [SerializeField] private Button backButtonDeviceListSelection;
    [SerializeField] private Button backButtonJBListSelection;
    [SerializeField] private Button backButtonTSDListSelection;
    [SerializeField] private Button backButtonFieldDeviceListSelection;


    [Header("Selection Buttons")]
    public List<Button> ObjectSelectionButtons;
    public List<Button> TypeImageSelectionButtons;


    [Header("Selection ObjectNames")]
    public TMP_Text jbObjectName;
    public TMP_Text tsdObjectName;
    public TMP_Text fieldDeviceObjectName;
    public TMP_Text deviceObjectName;
    private List<string> objectNameTexts;
    public string finalObjectNameText = "";

    [Header("Selection TypeImages")]
    public TMP_Text ConnectionImageTypeText;
    public TMP_Text OutDoorImageTypeText;
    private List<string> typeImageTexts;
    public string finalTypeImageText = "";

    public GameObject DialogOneButton;
    public GameObject DialogTwoButton;


    public ImageInformationModel ImageInformationModel;

    private readonly Dictionary<string, DeviceInformationModel> temp_Dictionary_DeviceModel = new();
    private readonly Dictionary<string, JBInformationModel> temp_Dictionary_JBModel = new();
    private readonly Dictionary<string, JBInformationModel> temp_Dictionary_TSDModel = new();
    private readonly Dictionary<string, FieldDeviceInformationModel> temp_Dictionary_FieldDeviceModel = new();
    private Sprite successConfirmButtonSprite;

    private readonly Dictionary<string, GameObject> selectedGameObjects = new()
    {
        { "Devices",  null },
        { "JBs",  null },
        { "TSDs",  null },
        { "FieldDevices",  null }
    };

    private readonly Dictionary<string, int> selectedCounts = new()
    {
        { "Devices", 0 },
        { "JBs", 0 },
        { "TSDs", 0 },
        { "FieldDevices", 0 }
    };

    private bool isObjectSelected = false;
    private bool isTypeImageSelected = false;

    private int grapperId;
    public string fieldObjectOption;

    void OnEnable()
    {
        grapperId = GlobalVariable.GrapperId;

        PreloadSprites();

        AddButtonListeners(initialize_Image_List_Option_Selection.Device_List_Selection_Option_Content_Transform, "Devices");
        AddButtonListeners(initialize_Image_List_Option_Selection.JB_List_Selection_Option_Content_Transform, "JBs");
        AddButtonListeners(initialize_Image_List_Option_Selection.TSD_List_Selection_Option_Content_Transform, "TSDs");
        AddButtonListeners(initialize_Image_List_Option_Selection.FieldDevice_List_Selection_Option_Content_Transform, "FieldDevices");

        backButton.onClick.RemoveAllListeners();
        backButtonDeviceListSelection.onClick.RemoveAllListeners();
        backButtonJBListSelection.onClick.RemoveAllListeners();
        backButtonTSDListSelection.onClick.RemoveAllListeners();
        backButtonFieldDeviceListSelection.onClick.RemoveAllListeners();

        backButtonDeviceListSelection.onClick.AddListener(() => CloseListSelectionFromBackButton("Devices"));
        backButtonJBListSelection.onClick.AddListener(() => CloseListSelectionFromBackButton("JBs"));
        backButtonTSDListSelection.onClick.AddListener(() => CloseListSelectionFromBackButton("TSDs"));
        backButtonFieldDeviceListSelection.onClick.AddListener(() => CloseListSelectionFromBackButton("FieldDevices"));
        backButton.onClick.AddListener(CloseAddCanvas);

        PrepareButtonClicked();
        scrollRect.verticalNormalizedPosition = 1;

    }
    void Update()
    {
        SetImageColor();
    }
    void SetImageColor()
    {
        if (isObjectSelected && isTypeImageSelected)
        {
            if (photoIcon.sprite != Colorful_Non_Image_Status_Icon
            && CameraIcon.sprite != Colorful_Camera_Selection
            && GalleryIcon.sprite != Colorful_Gallery_Selection)

                photoIcon.sprite = Colorful_Non_Image_Status_Icon;
            CameraIcon.sprite = Colorful_Camera_Selection;
            GalleryIcon.sprite = Colorful_Gallery_Selection;

            CameraIcon.gameObject.GetComponent<Button>().interactable = true;
            GalleryIcon.gameObject.GetComponent<Button>().interactable = true;
        }
        else
        {
            if (photoIcon.sprite != Grey_Non_Image_Status_Icon
            && CameraIcon.sprite != Grey_Camera_Selection
            && GalleryIcon.sprite != Grey_Gallery_Selection)

                photoIcon.sprite = Grey_Non_Image_Status_Icon;
            CameraIcon.sprite = Grey_Camera_Selection;
            GalleryIcon.sprite = Grey_Gallery_Selection;

            CameraIcon.gameObject.GetComponent<Button>().interactable = false;
            GalleryIcon.gameObject.GetComponent<Button>().interactable = false;
        }
    }
    void Awake()
    {
        // var DeviceManager = FindObjectOfType<DeviceManager>();
        _presenter = new ImagePresenter(this, ManagerLocator.Instance.ImageManager._IImageService);

        objectNameTexts = new List<string>
        {
            jbObjectName.text,
            tsdObjectName.text,
            fieldDeviceObjectName.text,
            deviceObjectName.text
        };

        typeImageTexts = new List<string>
        {
            ConnectionImageTypeText.text,
            OutDoorImageTypeText.text
        };
        // DeviceManager._IDeviceService
    }

    private void PrepareButtonClicked()
    {
        ConfigureButtonListeners(ObjectSelectionButtons, OpenListSelection);
        ConfigureButtonListeners(TypeImageSelectionButtons, null);
    }

    private void ConfigureButtonListeners(List<Button> buttons, Action<string> openListAction)
    {
        foreach (var button in buttons)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                if (openListAction != null)
                {
                    openListAction($"{button.gameObject.name}s");
                    isObjectSelected = false;
                }
                else
                {
                    isTypeImageSelected = false;
                }
                HandleButtonClick(button, buttons, $"{button.gameObject.name}s");
            });

        }

    }

    private void HandleButtonClick(Button clickedButton, List<Button> buttonList, string field)
    {
        TMP_Text textValue = field switch
        {
            "Devices" => deviceObjectName,
            "JBs" => jbObjectName,
            "TSDs" => tsdObjectName,
            "FieldDevices" => fieldDeviceObjectName,
            "ConnectionImages" => ConnectionImageTypeText,
            "OutdoorImages" => OutDoorImageTypeText,
            _ => null
        };
        if (textValue == null)
        {
            Debug.LogError("Invalid Button textValue");
            return;
        }
        if (textValue == ConnectionImageTypeText || textValue == OutDoorImageTypeText)
        {
            isTypeImageSelected = true;

            if (textValue == ConnectionImageTypeText)
            {
                finalTypeImageText = "ConnectionImage"; //! Lấy được tên loại ảnh
            }

            else if (textValue == OutDoorImageTypeText)
            {
                finalTypeImageText = "OutdoorImage"; //! Lấy được tên loại ảnh
            }
        }

        UpdateButtonAppearance(clickedButton, true, textValue, field);

        foreach (var button in buttonList.Where(button => button != clickedButton))
        {
            var buttonText = button.GetComponentInChildren<TMP_Text>();
            UpdateButtonAppearance(button, false, buttonText, field);
        }
    }

    private void UpdateButtonAppearance(Button button, bool isSelected, TMP_Text textValue, string field)
    {
        var image = button.GetComponent<Image>();
        if (image == null || textValue == null) return;

        image.sprite = isSelected ? selectedSprite : normalSprite;
        textValue.enableVertexGradient = !isSelected;
        if (!isSelected)
        {
            textValue.colorGradient = new VertexGradient(
                new Color32(0x03, 0xB1, 0x5C, 0xFF),
                new Color32(0x03, 0xB1, 0x5C, 0xFF),
                new Color32(0x00, 0x83, 0xAD, 0xFF),
                new Color32(0x00, 0x83, 0xAD, 0xFF)
            );
        }
        else
        {
            textValue.color = Color.white;
            ResetButtonTextValue(field);
        }
    }
    private void ResetButtonTextValue(string field)
    {
        switch (field)
        {
            case "JBs":
                tsdObjectName.text = "TSD";
                fieldDeviceObjectName.text = "Thiết bị trường (Bơm, vít, băng tải...)";
                deviceObjectName.text = "Thiết bị cảm biến";
                break;
            case "TSDs":
                jbObjectName.text = "JB";
                fieldDeviceObjectName.text = "Thiết bị trường (Bơm, vít, băng tải...)";
                deviceObjectName.text = "Thiết bị cảm biến";
                break;
            case "FieldDevices":
                jbObjectName.text = "JB";
                tsdObjectName.text = "TSD";
                deviceObjectName.text = "Thiết bị cảm biến";
                break;
            case "Devices":
                jbObjectName.text = "JB";
                tsdObjectName.text = "TSD";
                fieldDeviceObjectName.text = "Thiết bị trường (Bơm, vít, băng tải...)";
                break;
        }
    }
    private void PreloadSprites()
    {
        selectedSprite = Resources.Load<Sprite>("images/UIimages/Image_Object_Selected");
        normalSprite = Resources.Load<Sprite>("images/UIimages/Image_Object_Selection");
        // interactiveSprite = Resources.Load<Sprite>("images/UIimages/Image_Object_Interactive");

        Colorful_Camera_Selection = Resources.Load<Sprite>("images/UIimages/Camera_Selection");
        Colorful_Non_Image_Status_Icon = Resources.Load<Sprite>("images/UIimages/Colorful_Non_Image_Status_Icon");
        Colorful_Gallery_Selection = Resources.Load<Sprite>("images/UIimages/Gallery_Selection");

        Grey_Camera_Selection = Resources.Load<Sprite>("images/UIimages/Grey_Camera_Selection");
        Grey_Non_Image_Status_Icon = Resources.Load<Sprite>("images/UIimages/Grey_Non_Image_Status_Icon");
        Grey_Gallery_Selection = Resources.Load<Sprite>("images/UIimages/Grey_Gallery_Selection");

        successConfirmButtonSprite = Resources.Load<Sprite>("images/UIimages/Success_Back_Button_Background");
        Debug.Log(successConfirmButtonSprite);
    }



    void OnDisable()
    {
        RenewView();
    }


    private void RenewView()
    {
        isObjectSelected = false;
        isTypeImageSelected = false;

        ResetImageIcons();
        ResetButtons(ObjectSelectionButtons, objectNameTexts);
        ResetButtons(TypeImageSelectionButtons, typeImageTexts);
    }

    private void ResetImageIcons()
    {
        photoIcon.sprite = Grey_Non_Image_Status_Icon;
        CameraIcon.sprite = Grey_Camera_Selection;
        GalleryIcon.sprite = Grey_Gallery_Selection;
    }

    private void ResetButtons(List<Button> buttons, List<string> texts)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            var button = buttons[i];
            var buttonImage = button.gameObject.GetComponent<Image>();
            var buttonTitle = button.gameObject.GetComponentInChildren<TMP_Text>();

            if (buttonImage != null) buttonImage.sprite = normalSprite;
            if (buttonTitle != null)
            {
                buttonTitle.enableVertexGradient = true;
                buttonTitle.colorGradient = new VertexGradient(
                    new Color32(0x03, 0xB1, 0x5C, 0xFF),
                    new Color32(0x03, 0xB1, 0x5C, 0xFF),
                    new Color32(0x00, 0x83, 0xAD, 0xFF),
                    new Color32(0x00, 0x83, 0xAD, 0xFF)
                );
                buttonTitle.text = texts[i];
            }
        }
    }

    public void CloseAddCanvas()
    {
        Add_New_Image_Canvas.SetActive(false);
        List_Image_Canvas.SetActive(true);
    }

    private void AddButtonListeners(Transform contentTransform, string field)
    {
        foreach (Transform option in contentTransform)
        {
            if (option.TryGetComponent<Button>(out var button))
            {
                var optionText = option.GetComponentInChildren<TMP_Text>()?.text;
                if (!string.IsNullOrEmpty(optionText))
                {
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(() => SelectOption(optionText, field));
                }
            }
        }
    }

    private void SelectOption(string optionTextValue, string field)
    {
        TMP_Text targetText = field switch
        {
            "Devices" => deviceObjectName,
            "JBs" => jbObjectName,
            "TSDs" => tsdObjectName,
            "FieldDevices" => fieldDeviceObjectName,
            _ => null
        };

        if (targetText != null)
        {
            targetText.text = optionTextValue;

            isObjectSelected = true;

            finalObjectNameText = optionTextValue; //! Lấy được tên đối tượng cần thêm ảnh: Ví dụ như JB5

            // Debug.Log($"finalObjectNameText: {finalObjectNameText}");
            ;
            CloseListSelection(field);
        }
        fieldObjectOption = field;
        Debug.Log($"fieldObjectOption: {fieldObjectOption}");
    }

    public void CloseListSelection(string field)
    {
        GetSelectionPanel(field)?.SetActive(false);
    }

    public void CloseListSelectionFromBackButton(string field)
    {
        CloseListSelection(field);
    }

    public void OpenListDeviceSelection() => OpenListSelection("Devices");
    public void OpenListJBSelection() => OpenListSelection("JBs");
    public void OpenListTSDSelection() => OpenListSelection("TSDs");
    public void OpenListFieldDeviceSelection() => OpenListSelection("FieldDevices");

    public void OpenListSelection(string field)
    {
        GetSelectionPanel(field).SetActive(true);
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
            "Devices" => initialize_Image_List_Option_Selection.selection_List_Device_Panel,
            "JBs" => initialize_Image_List_Option_Selection.selection_List_JB_Panel,
            "TSDs" => initialize_Image_List_Option_Selection.selection_List_TSD_Panel,
            "FieldDevices" => initialize_Image_List_Option_Selection.selection_List_FieldDevice_Panel,
            _ => throw new ArgumentException("Invalid field Name")
        };
    }

    private void OpenErrorDialog(string title = "Thêm hình ảnh mới thất bại", string message = "Đã có lỗi xảy ra khi Thêm hình ảnh mới. Vui lòng thử lại sau.")
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

    private void OpenSuccessDialog(ImageInformationModel model)
    {
        DialogTwoButton.SetActive(true);

        var backgroundTransform = DialogTwoButton.transform.Find("Background");
        var horizontalGroupTransform = backgroundTransform.Find("Horizontal_Group");

        backgroundTransform.Find("Dialog_Status_Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("images/UIimages/Success_Icon_For_Dialog");
        backgroundTransform.Find("Dialog_Content").GetComponent<TMP_Text>().text = $"Bạn đã thành công thêm hình ảnh <b><color=#004C8A>{model.Name}</b></color> vào hệ thống";
        backgroundTransform.Find("Dialog_Title").GetComponent<TMP_Text>().text = "thêm hình ảnh mới thành công";
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
            scrollRect.verticalNormalizedPosition = 1;
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
        if (GlobalVariable.APIRequestType.Contains("POST_Image"))
        {
            OpenErrorDialog();
        }
    }

    public void ShowSuccess()
    {
        if (GlobalVariable.APIRequestType.Contains("POST_Image"))
        {
            Show_Toast.Instance.ShowToast("success", "thêm hình ảnh mới thành công");
            OpenSuccessDialog(ImageInformationModel);
        }

        StartCoroutine(Show_Toast.Instance.Set_Instance_Status_False(1f));
    }

    public void DisplayList(List<ImageInformationModel> models) { }
    public void DisplayCreateResult(bool success) { }
    public void DisplayUpdateResult(bool success) { }
    public void DisplayDeleteResult(bool success) { }
    public void DisplayDetail(ImageInformationModel model) { }
}
