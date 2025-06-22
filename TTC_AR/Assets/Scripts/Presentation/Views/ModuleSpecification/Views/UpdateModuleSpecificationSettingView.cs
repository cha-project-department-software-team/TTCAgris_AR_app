using System.Collections.Generic;
using EasyUI.Progress;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateModuleSpecificationSettingView : MonoBehaviour, IModuleSpecificationView
{
    private ModuleSpecificationPresenter _presenter;

    // Input Fields
    [Header("Input Fields")]
    [SerializeField] private List<TMP_InputField> moduleSpecificationTextFieldValues;
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
    public GameObject Add__ModuleSpecification_Canvas;
    public GameObject Update_ModuleSpecification_Canvas;
    public ScrollRect ScrollView;
    private ModuleSpecificationModel _ModuleSpecificationModel;
    private int moduleSpecificationId;

    void Awake()
    {
        _presenter = new ModuleSpecificationPresenter(this, ManagerLocator.Instance.ModuleSpecificationManager._IModuleSpecificationService);
    }



    void OnEnable()
    {
        moduleSpecificationId = GlobalVariable.moduleSpecificationId;
        ModuleSpecificationCode_TextField.interactable = false;

        submitButton.onClick.RemoveAllListeners();
        backButton.onClick.RemoveAllListeners();

        backButton.onClick.AddListener(CloseUpdateCanvas);
        submitButton.onClick.AddListener(OnSubmitButtonClick);

        PreloadDetailById();

    }
    private void OnSubmitButtonClick()
    {

        if (string.IsNullOrEmpty(ModuleSpecificationCode_TextField.text))
        {
            OpenErrorDialog("loại Module thất bại", "Vui lòng nhập mã loại Module");
            return;
        }
        if (GlobalVariable.temp_Dictionary_DeviceInformationModel.ContainsKey(ModuleSpecificationCode_TextField.text))
        {
            OpenErrorDialog("loại Module thất bại", "Mã loại Module này đã tồn tại");
            return;
        }
        _ModuleSpecificationModel = new ModuleSpecificationModel(
            code: ModuleSpecificationCode_TextField.text,
            type: Type_TextField.text,
            numOfIO: NumOfIo_TextField.text,
            signalType: SignalType_TextField.text,
            compatibleTBUs: CompatibleTBUs_TextField.text,
            operatingVoltage: OperatingVoltage_TextField.text,
            operatingCurrent: OperatingCurrent_TextField.text,
            flexbusCurrent: FlexbusCurrent_TextField.text,
            alarm: Alarm_TextField.text,
            note: Note_TextField.text,
            pdfManual: PdfManual_TextField.text
          );
        _presenter.UpdateModuleSpecification(
                 moduleSpecificationId, _ModuleSpecificationModel
            );
    }

    public void PreloadDetailById()
    {
        _presenter.LoadDetailById(moduleSpecificationId);
    }

    void OnDisable()
    {
        backButton.onClick.RemoveAllListeners();
        submitButton.onClick.RemoveAllListeners();
    }
    public void CloseUpdateCanvas()
    {
        if (Add__ModuleSpecification_Canvas.activeSelf) Add__ModuleSpecification_Canvas.SetActive(false);
        if (Update_ModuleSpecification_Canvas.activeSelf) Update_ModuleSpecification_Canvas.SetActive(false);
        if (!List_ModuleSpecification_Canvas.activeSelf) List_ModuleSpecification_Canvas.SetActive(true);
    }
    public void DisplayDetail(ModuleSpecificationModel model)
    {
        SetInitialInputFields(model);
    }
    private void OpenErrorDialog(string title = "Cập nhật loại Module thất bại", string content = "Đã có lỗi xảy ra khi cập nhật loại Module này. Vui lòng thử lại sau.")
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
        DialogOneButton.SetActive(true);
        var backButton = DialogOneButton.transform.Find("Background/Back_Button").GetComponent<Button>();
        backButton.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/UIimages/Success_Back_Button_Background");
        var dialog_Icon = DialogOneButton.transform.Find("Background/Dialog_Status_Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("images/UIimages/Success_Icon_For_Dialog");
        var dialog_Content = DialogOneButton.transform.Find("Background/Dialog_Content").GetComponent<TMP_Text>().text = $"Bạn đã thành công cập nhật loại Module <b><color =#004C8A>{model.Code}</b></color>"; ;
        var dialog_Title = DialogOneButton.transform.Find("Background/Dialog_Title").GetComponent<TMP_Text>().text = "Cập nhật loại Module thành công";
        backButton.onClick.RemoveAllListeners();
        backButton.onClick.AddListener(() =>
        {
            DialogOneButton.SetActive(false);
            ResetAllInputFields();
            CloseUpdateCanvas();
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
    private void SetInitialInputFields(ModuleSpecificationModel model)
    {
        ModuleSpecificationCode_TextField.text = model.Code;
        Type_TextField.text = string.IsNullOrEmpty(model.Type) ? "Chưa cập nhật" : model.Type;
        NumOfIo_TextField.text = string.IsNullOrEmpty(model.NumOfIO) ? "Chưa cập nhật" : model.NumOfIO;
        SignalType_TextField.text = string.IsNullOrEmpty(model.SignalType) ? "Chưa cập nhật" : model.SignalType;
        CompatibleTBUs_TextField.text = string.IsNullOrEmpty(model.CompatibleTBUs) ? "Chưa cập nhật" : model.CompatibleTBUs;
        OperatingVoltage_TextField.text = string.IsNullOrEmpty(model.OperatingVoltage) ? "Chưa cập nhật" : model.OperatingVoltage;
        OperatingCurrent_TextField.text = string.IsNullOrEmpty(model.OperatingCurrent) ? "Chưa cập nhật" : model.OperatingCurrent;
        FlexbusCurrent_TextField.text = string.IsNullOrEmpty(model.FlexbusCurrent) ? "Chưa cập nhật" : model.FlexbusCurrent;
        Alarm_TextField.text = string.IsNullOrEmpty(model.Alarm) ? "Chưa cập nhật" : model.Alarm;
        Note_TextField.text = string.IsNullOrEmpty(model.Note) ? "Chưa cập nhật" : model.Note;
        PdfManual_TextField.text = string.IsNullOrEmpty(model.PdfManual) ? "Chưa cập nhật" : model.PdfManual;
        ModuleSpecificationCode_TextField.interactable = false;

        foreach (var textField in moduleSpecificationTextFieldValues)
        {
            if (textField.text == "Chưa cập nhật" || string.IsNullOrEmpty(textField.text))
            {
                textField.textComponent.color = Color.red;
                textField.textComponent.fontStyle = FontStyles.Bold;
            }
            else
            {
                textField.textComponent.color = Color.black;
                textField.textComponent.fontStyle = FontStyles.Normal;
            }
        }
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
        if (GlobalVariable.APIRequestType.Contains("PUT_ModuleSpecification"))
        {
            OpenErrorDialog(title: "Cập nhật loại Module thất bại", content: message);
        }
        if (GlobalVariable.APIRequestType.Contains("GET_ModuleSpecification"))
        {
            OpenErrorDialog(title: "Tải dữ liệu thất bại", content: message);
        }
    }
    public void ShowSuccess(string message)
    {

        if (GlobalVariable.APIRequestType.Contains("PUT_ModuleSpecification"))
        {

            Show_Toast.Instance.ShowToast("success", message);
            OpenSuccessDialog(_ModuleSpecificationModel);
        }

        if (GlobalVariable.APIRequestType.Contains("GET_ModuleSpecification"))
        {
            Show_Toast.Instance.ShowToast("success", message);
        }
        StartCoroutine(Show_Toast.Instance.Set_Instance_Status_False(1f));
    }

    public void DisplayList(List<ModuleSpecificationModel> models) { }
    public void DisplayUpdateResult(bool success) { }
    public void DisplayDeleteResult(bool success) { }
    public void DisplayCreateResult(bool success)
    {
    }
}