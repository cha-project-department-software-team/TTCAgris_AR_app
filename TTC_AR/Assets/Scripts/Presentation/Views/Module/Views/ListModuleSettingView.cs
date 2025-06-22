using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EasyUI.Progress;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ListModuleSettingView : MonoBehaviour, IModuleView
{
    [Header("Canvas")]
    public GameObject List_Module_Canvas;
    public GameObject Add_New_Module_Canvas;
    public GameObject Update_Module_Canvas;

    public GameObject Module_Item_Prefab;
    public GameObject Parent_Vertical_Layout_Group;
    public ScrollRect scrollView;
    private List<GameObject> listModuleItems = new List<GameObject>();

    public GameObject DialogOneButton;
    public GameObject DialogTwoButton;
    private ModulePresenter _presenter;
    private Sprite warningConfirmButtonSprite;
    private int grapperId;
    private GameObject _moduleItem;
    void Awake()
    {
        _presenter = new ModulePresenter(this,
        ManagerLocator.Instance.ModuleManager._IModuleService);
    }
    void OnEnable()
    {
        grapperId = GlobalVariable.GrapperId;

        warningConfirmButtonSprite = Resources.Load<Sprite>("images/UIimages/Warning_Back_Button_Background");
        Debug.Log(warningConfirmButtonSprite);
        LoadListModule();
    }
    void OnDisable()
    {
    }

    private void RefreshList()
    {
        Module_Item_Prefab.SetActive(true);
        foreach (var item in listModuleItems)
        {
            if (item != Module_Item_Prefab)
                Destroy(item);
        }
        listModuleItems.Clear();
    }

    public void LoadListModule()
    {
        RefreshList();
        _presenter.LoadListModule(grapperId);
    }
    public void DisplayList(List<ModuleInformationModel> models)
    {
        if (models.Any())
        {
            foreach (var model in models)
            {
                // int ModuleIndex = models.IndexOf(model);
                // Debug.Log(ModuleIndex);
                var newModuleItem = Instantiate(Module_Item_Prefab, Parent_Vertical_Layout_Group.transform);
                newModuleItem.SetActive(true);
                Transform newModuleItemTransform = newModuleItem.transform;
                Transform newModuleItemPreviewInforGroup = newModuleItemTransform.GetChild(0);
                newModuleItemPreviewInforGroup.Find("Preview_Module_Name").GetComponent<TMP_Text>().text = model.Name;
                Transform newModuleItemPreviewButtonGroup = newModuleItemTransform.GetChild(1);
                listModuleItems.Add(newModuleItem);
                newModuleItemPreviewButtonGroup.Find("Group/Edit_Button").GetComponent<Button>().onClick.AddListener(() => EditModuleItem(model.Id));
                newModuleItemPreviewButtonGroup.Find("Group/Delete_Button").GetComponent<Button>().onClick.AddListener(() => DeleModuleItem(newModuleItem, model));
            }
        }
        else
        {
            Debug.Log("No Mccs found");
        }
        Module_Item_Prefab.SetActive(false);
        scrollView.verticalNormalizedPosition = 1f;
    }

    private void EditModuleItem(int id)
    {
        GlobalVariable.moduleId = id;
        OpenUpdateCanvas();
    }
    private void DeleModuleItem(GameObject ModuleItem, ModuleInformationModel model)
    {
        OpenDeleteWarningDialog(ModuleItem, model);
    }

    public void OpenAddNewCanvas()
    {
        Add_New_Module_Canvas.SetActive(true);
        List_Module_Canvas.SetActive(false);
        Update_Module_Canvas.SetActive(false);
    }
    private void OpenUpdateCanvas()
    {
        List_Module_Canvas.SetActive(false);
        Add_New_Module_Canvas.SetActive(false);
        Update_Module_Canvas.SetActive(true);
    }

    private void OpenDeleteWarningDialog(GameObject ModuleItem, ModuleInformationModel model)
    {
        DialogTwoButton.SetActive(true);

        var backgroundTransform = DialogTwoButton.transform.Find("Background");

        var Horizontal_Group = DialogTwoButton.transform.Find("Background/Horizontal_Group").gameObject.transform;

        var dialog_Content = DialogTwoButton.transform.Find("Background/Dialog_Content").GetComponent<TMP_Text>().text = $"Bạn có chắc chắn muốn xóa Module IO <color=#ED1C24><b>{model.Name}</b></color> khỏi hệ thống? Hãy kiểm tra kĩ trước khi nhấn nút \"xác nhận\" phía dưới";

        var dialog_Title = DialogTwoButton.transform.Find("Background/Dialog_Title").GetComponent<TMP_Text>().text = "Xóa Module IO khỏi hệ thống?";

        backgroundTransform.Find("Dialog_Status_Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("images/UIimages/Warning_Icon_For_Dialog");

        var confirmButton = Horizontal_Group.Find("Confirm_Button").GetComponent<Button>();
        var backButton = Horizontal_Group.Find("Back_Button").GetComponent<Button>();

        var confirmButtonSprite = confirmButton.GetComponent<Image>();

        confirmButtonSprite.sprite = warningConfirmButtonSprite;

        var confirmButtonText = confirmButton.GetComponentInChildren<TMP_Text>();
        var backButtonText = backButton.GetComponentInChildren<TMP_Text>();

        // var colors = confirmButton.colors;
        // colors.normalColor = new Color32(92, 237, 115, 255); // #5CED73 in RGB
        // confirmButton.colors = colors;

        confirmButtonText.text = "Xác nhận";
        backButtonText.text = "Trở lại";

        confirmButton.onClick.RemoveAllListeners();

        backButton.onClick.RemoveAllListeners();

        confirmButton.onClick.AddListener(() =>
        {
            _moduleItem = ModuleItem;
            _presenter.DeleteModule(model.Id);
            DialogTwoButton.SetActive(false);
        });
        backButton.onClick.AddListener(() =>
        {
            DialogTwoButton.SetActive(false);
        });
    }
    private void OpenErrorDialog(string title = "Xóa Module IO thất bại", string message = "Đã có lỗi xảy ra khi xóa Module IO khỏi hệ thống. Vui lòng thử lại sau")
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
        if (GlobalVariable.APIRequestType.Contains("GET_Module_List"))
        {
            OpenErrorDialog(title: "Tải danh sách thất bại", message: " Đã có lỗi xảy ra khi tải danh sách Module IO. Vui lòng thử lại sau");
        }
        if (GlobalVariable.APIRequestType.Contains("DELETE_Module"))
        {
            OpenErrorDialog(title: "Xóa Module IO thất bại", message: "Đã có lỗi xảy ra khi xóa Module IO khỏi hệ thống. Vui lòng thử lại sau");
        }
    }
    public void ShowSuccess(string message)
    {
        if (GlobalVariable.APIRequestType.Contains("GET_Module_List"))
        {
            Show_Toast.Instance.ShowToast("success", message: "Tải danh sách thành công");
        }
        if (GlobalVariable.APIRequestType.Contains("DELETE_Module"))
        {
            listModuleItems.Remove(_moduleItem);
            Destroy(_moduleItem.gameObject);
            Show_Toast.Instance.ShowToast("success", message: "Xóa Module IO thành công");

        }
        StartCoroutine(Show_Toast.Instance.Set_Instance_Status_False());
    }
    // Không dùng trong ListView
    public void DisplayDetail(ModuleInformationModel model) { }
    public void DisplayCreateResult(bool success) { }
    public void DisplayUpdateResult(bool success) { }
    public void DisplayDeleteResult(bool success) { }
}