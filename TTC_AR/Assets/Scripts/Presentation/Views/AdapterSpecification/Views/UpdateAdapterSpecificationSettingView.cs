using System.Collections.Generic;
using EasyUI.Progress;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateAdapterSpecificationSettingView : MonoBehaviour, IAdapterSpecificationView
{
    private AdapterSpecificationPresenter _presenter;

    // Input Fields
    [Header("Input Fields")]
    [SerializeField] private TMP_InputField AdapterSpecificationCode_TextField;
    [SerializeField] private TMP_InputField Type_TextField;
    [SerializeField] private TMP_InputField Communication_TextField;
    [SerializeField] private TMP_InputField NumOfAdapterAllowed_TextField;
    [SerializeField] private TMP_InputField CommSpeed_TextField;
    [SerializeField] private TMP_InputField InputSupply_TextField;

    [SerializeField] private TMP_InputField OutputSupply_TextField;
    [SerializeField] private TMP_InputField InrushCurrent_TextField;
    [SerializeField] private TMP_InputField Alarm_TextField;
    [SerializeField] private TMP_InputField Note_TextField;
    [SerializeField] private TMP_InputField PDFManual_TextField;

    [Header("Buttons")]
    [SerializeField] private Button submitButton;
    [SerializeField] private Button backButton;
    public GameObject DialogOneButton;
    public GameObject DialogTwoButton;

    [Header("Canvas")]
    public GameObject List_AdapterSpecification_Canvas;
    public GameObject Add__AdapterSpecification_Canvas;
    public GameObject Update_AdapterSpecification_Canvas;

    public ScrollRect ScrollView;

    private AdapterSpecificationModel _adapterSpecificationModel;

    void Awake()
    {
        // var DeviceManager = FindObjectOfType<DeviceManager>();
        _presenter = new AdapterSpecificationPresenter(this, ManagerLocator.Instance.AdapterSpecificationManager._IAdapterSpecificationService);
        // DeviceManager._IDeviceService
    }
    void OnEnable()
    {
        backButton.onClick.RemoveAllListeners();
        submitButton.onClick.RemoveAllListeners();

        backButton.onClick.AddListener(CloseUpdateCanvas);
        _presenter.LoadDetailById(GlobalVariable.adapterSpecificationId);
        submitButton.onClick.AddListener(OnSubmitButtonClick);
    }


    private void OnSubmitButtonClick()
    {

        if (string.IsNullOrEmpty(AdapterSpecificationCode_TextField.text))
        {
            OpenErrorDialog("Tạo loại Adapter mới thất bại", "Vui lòng nhập mã loại Adapter");
            return;
        }
        if (GlobalVariable.temp_Dictionary_DeviceInformationModel.ContainsKey(AdapterSpecificationCode_TextField.text))
        {
            OpenErrorDialog("Tạo loại Adapter mới thất bại", "Mã loại Adapter này đã tồn tại");
            return;
        }
        _adapterSpecificationModel = new AdapterSpecificationModel(
                 AdapterSpecificationCode_TextField.text,
                 Type_TextField.text,
                 Communication_TextField.text,
                 NumOfAdapterAllowed_TextField.text,
                 CommSpeed_TextField.text,
                 InputSupply_TextField.text,
                 OutputSupply_TextField.text,
                 InrushCurrent_TextField.text,
                 Alarm_TextField.text,
                 Note_TextField.text,
                 PDFManual_TextField.text
             );
        _presenter.UpdateAdapterSpecification(
                 GlobalVariable.adapterSpecificationId, _adapterSpecificationModel
            );

    }


    public void PreloadDetailById()
    {
        _presenter.LoadDetailById(GlobalVariable.adapterSpecificationId);
    }

    void OnDisable()
    {
        backButton.onClick.RemoveAllListeners();
        submitButton.onClick.RemoveAllListeners();
        StopAllCoroutines();
    }
    public void CloseUpdateCanvas()
    {
        if (Add__AdapterSpecification_Canvas.activeSelf) Add__AdapterSpecification_Canvas.SetActive(false);
        if (Update_AdapterSpecification_Canvas.activeSelf) Update_AdapterSpecification_Canvas.SetActive(false);
        if (!List_AdapterSpecification_Canvas.activeSelf) List_AdapterSpecification_Canvas.SetActive(true);
    }
    public void DisplayDetail(AdapterSpecificationModel model)
    {
        SetInitialInputFields(model);
    }
    private void OpenErrorDialog(string title = "Tạo loại Adapter mới thất bại", string message = "Đã có lỗi xảy ra khi tạo loại Adapter mới. Vui lòng thử lại sau.")
    {
        DialogOneButton.SetActive(true);
        var backButton = DialogOneButton.transform.Find("Background/Back_Button").GetComponent<Button>();
        backButton.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/UIimages/Error_Back_Button_Background");
        var dialog_Icon = DialogOneButton.transform.Find("Background/Dialog_Status_Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("images/UIimages/Error_Icon_For_Dialog");
        var dialog_Content = DialogOneButton.transform.Find("Background/Dialog_Content").GetComponent<TMP_Text>().text = message;
        var dialog_Title = DialogOneButton.transform.Find("Background/Dialog_Title").GetComponent<TMP_Text>().text = title;
        backButton.onClick.RemoveAllListeners();
        backButton.onClick.AddListener(() =>
        {
            DialogOneButton.SetActive(false);
        });
    }


    private void OpenSuccessDialog(AdapterSpecificationModel model)
    {
        DialogOneButton.SetActive(true);
        var backButton = DialogOneButton.transform.Find("Background/Back_Button").GetComponent<Button>();
        backButton.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/UIimages/Success_Back_Button_Background");
        var dialog_Icon = DialogOneButton.transform.Find("Background/Dialog_Status_Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("images/UIimages/Success_Icon_For_Dialog");
        var dialog_Content = DialogOneButton.transform.Find("Background/Dialog_Content").GetComponent<TMP_Text>().text = $"Bạn đã thành công cập nhật loại Adapter <b><color =#004C8A>{model.Code}</b></color>"; ;
        var dialog_Title = DialogOneButton.transform.Find("Background/Dialog_Title").GetComponent<TMP_Text>().text = "Cập nhật loại Adapter thành công";
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
        AdapterSpecificationCode_TextField.text = "";
        Type_TextField.text = "";
        Communication_TextField.text = "";
        NumOfAdapterAllowed_TextField.text = "";
        CommSpeed_TextField.text = "";
        InputSupply_TextField.text = "";
        OutputSupply_TextField.text = "";
        InrushCurrent_TextField.text = "";
        Alarm_TextField.text = "";
        Note_TextField.text = "";
        PDFManual_TextField.text = "";
    }
    private void SetInitialInputFields(AdapterSpecificationModel model)
    {
        AdapterSpecificationCode_TextField.text = model.Code;
        Type_TextField.text = model.Type;
        Communication_TextField.text = model.Communication;
        NumOfAdapterAllowed_TextField.text = model.NumOfModulesAllowed;
        CommSpeed_TextField.text = model.CommSpeed;
        InputSupply_TextField.text = model.InputSupply;
        OutputSupply_TextField.text = model.OutputSupply;
        InrushCurrent_TextField.text = model.InrushCurrent;
        Alarm_TextField.text = model.Alarm;
        Note_TextField.text = model.Note;
        PDFManual_TextField.text = model.PdfManual;
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
        if (GlobalVariable.APIRequestType.Contains("PUT_AdapterSpecification"))
        {
            OpenErrorDialog();
        }
        if (GlobalVariable.APIRequestType.Contains("GET_AdapterSpecification"))
        {
            OpenErrorDialog(title: "Tải dữ liệu thất bại", message: "Đã có lỗi xảy ra khi tải dữ liệu loại Adapter. Vui lòng thử lại sau");
        }
    }
    public void ShowSuccess(string message)
    {

        if (GlobalVariable.APIRequestType.Contains("PUT_AdapterSpecification"))
        {
            Show_Toast.Instance.ShowToast("success", "Cập nhật dữ liệu thành công");
            OpenSuccessDialog(_adapterSpecificationModel);
        }
        if (GlobalVariable.APIRequestType.Contains("GET_AdapterSpecification"))
        {
            Show_Toast.Instance.ShowToast("success", "Tải dữ liệu thành công");
        }
        StartCoroutine(Show_Toast.Instance.Set_Instance_Status_False(1f));
    }




    public void DisplayList(List<AdapterSpecificationModel> models) { }
    public void DisplayUpdateResult(bool success) { }
    public void DisplayDeleteResult(bool success) { }
    public void DisplayCreateResult(bool success)
    {
    }
}