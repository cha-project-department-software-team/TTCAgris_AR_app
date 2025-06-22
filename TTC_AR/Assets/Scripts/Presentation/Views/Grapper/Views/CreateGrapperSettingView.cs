using System;
using System.Collections.Generic;
using System.Linq;
using EasyUI.Progress;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateGrapperSettingView : MonoBehaviour, IGrapperView
{
    // public Initialize_Grapper_List_Option_Selection initialize_Grapper_List_Option_Selection;
    private GrapperPresenter _presenter;
    [SerializeField] private GrapperInformationModel GrapperInformationModel;

    [Header("Input Fields")]
    [SerializeField] private TMP_InputField Name_TextField;

    [Header("Buttons")]
    [SerializeField] private Button submitButton;
    [SerializeField] private Button backButton;

    public GameObject DialogOneButton;
    public GameObject DialogTwoButton;

    // void Awake()
    // {
    //     var GrapperManager = FindObjectOfType<GrapperManager>();
    //     _presenter = new GrapperPresenter(this, GrapperManager._IGrapperService);
    // }
    private Sprite successConfirmButtonSprite;
    private int companyId;

    void Awake()
    {
        _presenter = new GrapperPresenter(this, ManagerLocator.Instance.GrapperManager._IGrapperService);
    }

    void OnEnable()
    {
        companyId = GlobalVariable.companyId;
        successConfirmButtonSprite = Resources.Load<Sprite>("images/UIimages/Success_Back_Button_Background");
        Debug.Log(successConfirmButtonSprite);

        RenewView();        // 
        // AddButtonListeners(initialize_Grapper_List_Option_Selection.Module_List_Selection_Option_Content_Transform, "Modules");
        backButton.onClick.RemoveAllListeners();
        submitButton.onClick.RemoveAllListeners();
        submitButton.onClick.AddListener(OnSubmitButtonClick);
    }

    void OnDisable()
    {
        RenewView();
    }

    private void OnSubmitButtonClick()
    {

        if (string.IsNullOrEmpty(Name_TextField.text))
        {
            OpenErrorDialog("Vui lòng nhập mã Grapper");
            return;
        }
        if (GlobalVariable.temp_Dictionary_GrapperInformationModel.ContainsKey(Name_TextField.text))
        {
            OpenErrorDialog("Khu vực đã tồn tại", "Vui lòng nhập mã Khu vực khác");
            return;
        }
        GrapperInformationModel = new GrapperInformationModel(
         name: Name_TextField.text
             );
        if (GrapperInformationModel != null)
        {
            _presenter.CreateNewGrapper(companyId, GrapperInformationModel);
        }
    }

    private void RenewView()
    {
        // ClearActiveChildren(initialize_Grapper_List_Option_Selection.Module_List_Selection_Option_Content_Transform);
        ResetAllInputFields();
    }


    private void ResetAllInputFields()
    {
        Name_TextField.text = "";
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



    private void OpenErrorDialog(string title = "Tạo Grapper IO mới thất bại", string message = "Đã có lỗi xảy ra khi tạo Grapper IO mới. Vui lòng thử lại sau.")
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

    private void OpenSuccessDialog(GrapperInformationModel model)
    {
        DialogTwoButton.SetActive(true);

        var backgroundTransform = DialogTwoButton.transform.Find("Background");
        var horizontalGroupTransform = backgroundTransform.Find("Horizontal_Group");

        backgroundTransform.Find("Dialog_Status_Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("images/UIimages/Success_Icon_For_Dialog");
        backgroundTransform.Find("Dialog_Content").GetComponent<TMP_Text>().text = $"Bạn đã thành công thêm Grapper IO <b><color=#004C8A>{model.Name}</b></color> vào hệ thống";
        backgroundTransform.Find("Dialog_Title").GetComponent<TMP_Text>().text = "Thêm Grapper IO mới thành công";

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
        if (GlobalVariable.APIRequestType.Contains("POST_Grapper"))
        {
            OpenErrorDialog();
        }
    }

    public void ShowSuccess()
    {
        if (GlobalVariable.APIRequestType.Contains("POST_Grapper"))
        {
            Show_Toast.Instance.ShowToast("success", "Thêm Grapper IO mới thành công");
            OpenSuccessDialog(GrapperInformationModel);
        }

        StartCoroutine(Show_Toast.Instance.Set_Instance_Status_False(1f));
    }

    public void DisplayList(List<GrapperInformationModel> models) { }
    public void DisplayCreateResult(bool success) { }
    public void DisplayUpdateResult(bool success) { }
    public void DisplayDeleteResult(bool success) { }
    public void DisplayDetail(GrapperInformationModel model) { }
}
