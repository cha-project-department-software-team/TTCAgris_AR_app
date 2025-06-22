using System;
using System.Collections.Generic;
using EasyUI.Progress;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateModuleSpecificationSettingView : MonoBehaviour, IModuleSpecificationView
{
    private ModuleSpecificationPresenter _presenter;

    // Input Fields
    [Header("Input Fields")]
    [SerializeField] private TMP_InputField ModuleSpecificationCode_TextField;
    [SerializeField] private TMP_InputField Type_TextField;
    [SerializeField] private TMP_InputField NumOfIo_TextField;
    [SerializeField] private TMP_InputField SignalType_TextField;
    [SerializeField] private TMP_InputField CompatibleTBUs_TextField;
    [SerializeField] private TMP_InputField OperatingVoltage_TextField;

    [SerializeField] private TMP_InputField OperatingCurrent_TextField;
    [SerializeField] private TMP_InputField FlexbusCurrent_TextField;
    [SerializeField] private TMP_InputField Alarm_TextField;
    [SerializeField] private TMP_InputField Note_TextField;
    [SerializeField] private TMP_InputField PdfManual_TextField;

    [Header("Buttons")]
    [SerializeField] private Button submitButton;
    [SerializeField] private Button backButton;
    public GameObject DialogOneButton;
    public GameObject DialogTwoButton;

    [Header("Canvas")]
    public GameObject List_ModuleSpecification_Canvas;
    public GameObject Add_New_ModuleSpecification_Canvas;
    public GameObject Update_ModuleSpecification_Canvas;

    public ScrollRect ScrollView;
    private ModuleSpecificationModel _ModuleSpecificationModel;
    private int companyId;

    private Sprite successConfirmButtonSprite;

    void Awake()
    {
        _presenter = new ModuleSpecificationPresenter(this,
         ManagerLocator.Instance.ModuleSpecificationManager._IModuleSpecificationService);
    }

    void OnEnable()
    {
        successConfirmButtonSprite = Resources.Load<Sprite>("images/UIimages/Success_Back_Button_Background");
        Debug.Log(successConfirmButtonSprite);
        ReNewUI(); 
        companyId = GlobalVariable.companyId;
    }
    private void ReNewUI()
    {
        ResetAllInputFields();

        submitButton.onClick.RemoveAllListeners();
        backButton.onClick.RemoveAllListeners();

        backButton.onClick.AddListener(CloseAddCanvas);
        submitButton.onClick.AddListener(OnSubmitButtonClick);
    }
    private void OnSubmitButtonClick()
    {

        if (string.IsNullOrEmpty(ModuleSpecificationCode_TextField.text))
        {
            OpenErrorDialog("Tạo loại Module mới thất bại", "Vui lòng nhập mã loại Module");
            return;
        }
        if (GlobalVariable.temp_Dictionary_DeviceInformationModel.ContainsKey(ModuleSpecificationCode_TextField.text))
        {
            OpenErrorDialog("Tạo loại Module mới thất bại", "Loại Module này đã tồn tại");
            return;
        }
        _ModuleSpecificationModel = new ModuleSpecificationModel(
            code: ModuleSpecificationCode_TextField.text,
            type: string.IsNullOrEmpty(Type_TextField.text) ? "Chưa cập nhật" : Type_TextField.text,
            numOfIO: string.IsNullOrEmpty(NumOfIo_TextField.text) ? "Chưa cập nhật" : NumOfIo_TextField.text,
            signalType: string.IsNullOrEmpty(SignalType_TextField.text) ? "Chưa cập nhật" : SignalType_TextField.text,
            compatibleTBUs: string.IsNullOrEmpty(CompatibleTBUs_TextField.text) ? "Chưa cập nhật" : CompatibleTBUs_TextField.text,
            operatingVoltage: string.IsNullOrEmpty(OperatingVoltage_TextField.text) ? "Chưa cập nhật" : OperatingVoltage_TextField.text,
            operatingCurrent: string.IsNullOrEmpty(OperatingCurrent_TextField.text) ? "Chưa cập nhật" : OperatingCurrent_TextField.text,
            flexbusCurrent: string.IsNullOrEmpty(FlexbusCurrent_TextField.text) ? "Chưa cập nhật" : FlexbusCurrent_TextField.text,
            alarm: string.IsNullOrEmpty(Alarm_TextField.text) ? "Chưa cập nhật" : Alarm_TextField.text,
            note: string.IsNullOrEmpty(Note_TextField.text) ? "Chưa cập nhật" : Note_TextField.text,
            pdfManual: string.IsNullOrEmpty(PdfManual_TextField.text) ? "Chưa cập nhật" : PdfManual_TextField.text
          );
        _presenter.CreateNewModuleSpecification(
            companyId, _ModuleSpecificationModel
       );
    }

    void OnDisable()
    {
        backButton.onClick.RemoveAllListeners();
        submitButton.onClick.RemoveAllListeners();
    }
    public void CloseAddCanvas()
    {
        if (Add_New_ModuleSpecification_Canvas.activeSelf) Add_New_ModuleSpecification_Canvas.SetActive(false);
        if (Update_ModuleSpecification_Canvas.activeSelf) Update_ModuleSpecification_Canvas.SetActive(false);
        if (!List_ModuleSpecification_Canvas.activeSelf) List_ModuleSpecification_Canvas.SetActive(true);
    }

    private void OpenErrorDialog(string title = "Tạo loại Module mới thất bại", string content = "Đã có lỗi xảy ra khi tạo loại Module mới. Vui lòng thử lại sau.")
    {
        DialogOneButton.SetActive(true);
        var backButton = DialogOneButton.transform.Find("Background/Back_Button").GetComponent<Button>();
        backButton.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/UIimages/Error_Back_Button_Background");
        var dialog_Icon = DialogOneButton.transform.Find("Background/Dialog_Status_Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("images/UIimages/Error_Icon_For_Dialog");
        var dialog_Content = DialogOneButton.transform.Find("Background/Dialog_Content").GetComponent<TMP_Text>().text = content;
        var dialog_Title = DialogOneButton.transform.Find("Background/Dialog_Title").GetComponent<TMP_Text>().text = title;
        backButton.onClick.RemoveAllListeners();
        backButton.onClick.AddListener(() =>
        {
            DialogOneButton.SetActive(false);
        });
    }
    private void OpenSuccessDialog(ModuleSpecificationModel model)
    {
        DialogTwoButton.SetActive(true);

        var backgroundTransform = DialogTwoButton.transform.Find("Background");
        var horizontalGroupTransform = backgroundTransform.Find("Horizontal_Group");

        backgroundTransform.Find("Dialog_Status_Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("images/UIimages/Success_Icon_For_Dialog");
        backgroundTransform.Find("Dialog_Content").GetComponent<TMP_Text>().text = $"Bạn đã thành công thêm loại Module <color=#004C8A><b>{model.Code}</b></color> vào hệ thống";
        backgroundTransform.Find("Dialog_Title").GetComponent<TMP_Text>().text = "Thêm loại Module mới thành công";

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

        backButton.onClick.AddListener(() =>
               {
                   ReNewUI();
                   DialogTwoButton.SetActive(false);
                   CloseAddCanvas();
               });

        confirmButton.onClick.AddListener(() =>
        {
            ReNewUI();
            DialogTwoButton.SetActive(false);
            ScrollView.verticalNormalizedPosition = 1;
        });


    }
    private void ResetAllInputFields()
    {
        ModuleSpecificationCode_TextField.text = "";
        Type_TextField.text = "";
        NumOfIo_TextField.text = "";
        SignalType_TextField.text = "";
        CompatibleTBUs_TextField.text = "";
        OperatingVoltage_TextField.text = "";
        OperatingCurrent_TextField.text = "";
        FlexbusCurrent_TextField.text = "";
        Alarm_TextField.text = "";
        Note_TextField.text = "";
        PdfManual_TextField.text = "";
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

    public void ShowLoading(string title) => ShowProgressBar(title, "Dữ liệu đang được tải...");
    public void HideLoading() => HideProgressBar();
    public void ShowError(string message)
    {
        if (GlobalVariable.APIRequestType.Contains("POST_ModuleSpecification"))
        {
            OpenErrorDialog();
        }
        StartCoroutine(Show_Toast.Instance.Set_Instance_Status_False(1f));
    }

    public void ShowSuccess(string message)
    {

        if (GlobalVariable.APIRequestType.Contains("POST_ModuleSpecification"))
        {

            Show_Toast.Instance.ShowToast("success", message);
            OpenSuccessDialog(_ModuleSpecificationModel);
        }
        StartCoroutine(Show_Toast.Instance.Set_Instance_Status_False(1f));
    }


    public void DisplayList(List<ModuleSpecificationModel> models) { }
    public void DisplayCreateResult(bool success) { }
    public void DisplayUpdateResult(bool success) { }
    public void DisplayDeleteResult(bool success) { }
    public void DisplayDetail(ModuleSpecificationModel model) { }
}