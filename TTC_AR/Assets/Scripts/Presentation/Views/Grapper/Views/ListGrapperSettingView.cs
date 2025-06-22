using System.Collections.Generic;
using System.Linq;
using EasyUI.Progress;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ListGrapperSettingView : MonoBehaviour, IGrapperView
{
    [Header("Canvas")]
    public GameObject List_Grapper_Canvas;
    public GameObject Add_New_Grapper_Canvas;
    public GameObject Update_Grapper_Canvas;

    public GameObject Grapper_Item_Prefab;
    public GameObject Parent_Vertical_Layout_Group;
    public ScrollRect scrollView;
    private List<GameObject> listGrapperItems = new List<GameObject>();

    public GameObject DialogOneButton;
    public GameObject DialogTwoButton;
    private GrapperPresenter _presenter;
    private Sprite warningConfirmButtonSprite;
    private int companyId;
    private GameObject GrapperItem;

    void Awake()
    {

        _presenter = new GrapperPresenter(this,
        ManagerLocator.Instance.GrapperManager._IGrapperService);

    }
    void OnEnable()
    {
        companyId = GlobalVariable.companyId;
        warningConfirmButtonSprite = Resources.Load<Sprite>("images/UIimages/Warning_Back_Button_Background");
        Debug.Log(warningConfirmButtonSprite);
        LoadListGrapper();
    }
    void OnDisable()
    {
    }

    private void RefreshList()
    {
        Grapper_Item_Prefab.SetActive(true);
        foreach (var item in listGrapperItems)
        {
            if (item != Grapper_Item_Prefab)
                Destroy(item);
        }
        listGrapperItems.Clear();
    }

    public void LoadListGrapper()
    {
        RefreshList();
        _presenter.LoadListGrapper(companyId);
    }
    public void DisplayList(List<GrapperInformationModel> models)
    {
        if (models.Any())
        {
            foreach (var model in models)
            {
                int GrapperIndex = models.IndexOf(model);
                Debug.Log(GrapperIndex);
                var newGrapperItem = Instantiate(Grapper_Item_Prefab, Parent_Vertical_Layout_Group.transform);
                Transform newGrapperItemTransform = newGrapperItem.transform;
                Transform newGrapperItemPreviewInforGroup = newGrapperItemTransform.GetChild(0);
                newGrapperItemPreviewInforGroup.Find("Preview_Grapper_Name").GetComponent<TMP_Text>().text = model.Name;
                Transform newGrapperItemPreviewButtonGroup = newGrapperItemTransform.GetChild(1);
                listGrapperItems.Add(newGrapperItem);
                newGrapperItemPreviewButtonGroup.Find("Group/Edit_Button").GetComponent<Button>().onClick.AddListener(() => EditGrapperItem(model.Id));
                newGrapperItemPreviewButtonGroup.Find("Group/Delete_Button").GetComponent<Button>().onClick.AddListener(() => DeleGrapperItem(newGrapperItem, model));
            }
        }
        else
        {
            Debug.Log("No Grappers found");
        }
        Grapper_Item_Prefab.SetActive(false);
    }

    private void EditGrapperItem(int id)
    {
        GlobalVariable.GrapperId = id;
        OpenUpdateCanvas();
    }
    private void DeleGrapperItem(GameObject GrapperItem, GrapperInformationModel model)
    {
        OpenDeleteWarningDialog(GrapperItem, model);
    }

    public void OpenAddNewCanvas()
    {
        Add_New_Grapper_Canvas.SetActive(true);
        List_Grapper_Canvas.SetActive(false);
        Update_Grapper_Canvas.SetActive(false);
    }
    private void OpenUpdateCanvas()
    {
        List_Grapper_Canvas.SetActive(false);
        Add_New_Grapper_Canvas.SetActive(false);
        Update_Grapper_Canvas.SetActive(true);
    }

    private void OpenDeleteWarningDialog(GameObject GrapperItem, GrapperInformationModel model)
    {
        DialogTwoButton.SetActive(true);

        var backgroundTransform = DialogTwoButton.transform.Find("Background");

        var Horizontal_Group = DialogTwoButton.transform.Find("Background/Horizontal_Group").gameObject.transform;

        var dialog_Content = DialogTwoButton.transform.Find("Background/Dialog_Content").GetComponent<TMP_Text>().text = $"Bạn có chắc chắn muốn xóa thông tin Grapper IO <b><color=#ED1C24>{model.Name}</b></color> khỏi hệ thống? Hãy kiểm tra kĩ trước khi nhấn nút xác nhận phía dưới";

        var dialog_Title = DialogTwoButton.transform.Find("Background/Dialog_Title").GetComponent<TMP_Text>().text = "Xóa Grapper IO khỏi hệ thống?";

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
            Debug.Log(model.Id);
            this.GrapperItem = GrapperItem;
            _presenter.DeleteGrapper(model.Id);
            DialogTwoButton.SetActive(false);
        });
        backButton.onClick.AddListener(() =>
        {
            DialogTwoButton.SetActive(false);
        });
    }
    private void OpenErrorDialog(string title = "Xóa Grapper IO thất bại", string message = "Đã có lỗi xảy ra khi xóa Grapper IO khỏi hệ thống. Vui lòng thử lại sau")
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
        if (GlobalVariable.APIRequestType.Contains("GET_Grapper_List"))
        {
            OpenErrorDialog(title: "Tải danh sách thất bại", message: "Đã có lỗi xảy ra khi tải danh sách. Vui lòng thử lại sau");
        }
        if (GlobalVariable.APIRequestType.Contains("DELETE_Grapper"))
        {
            OpenErrorDialog();
        }
    }
    public void ShowSuccess()
    {
        if (GlobalVariable.APIRequestType.Contains("GET_Grapper_List"))
        {
            Show_Toast.Instance.ShowToast("success", "Tải danh sách thành công");
        }
        if (GlobalVariable.APIRequestType.Contains("DELETE_Grapper"))
        {
            listGrapperItems.Remove(GrapperItem);
            Destroy(GrapperItem);
            Show_Toast.Instance.ShowToast("success", "Xóa Grapper IO thành công");
        }

        StartCoroutine(Show_Toast.Instance.Set_Instance_Status_False());
    }

    // Không dùng trong ListView
    public void DisplayDetail(GrapperInformationModel model) { }
    public void DisplayCreateResult(bool success) { }
    public void DisplayUpdateResult(bool success) { }
    public void DisplayDeleteResult(bool success) { }
}