using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EasyUI.Progress;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class ListJBSettingView : MonoBehaviour, IJBView
{
    [Header("Canvas")]
    public GameObject List_JB_Canvas;
    public GameObject Add_New_JB_Canvas;
    public GameObject Update_JB_Canvas;

    public GameObject JB_Item_Prefab;
    public GameObject Parent_Vertical_Layout_Group;
    public ScrollRect scrollView;
    private List<GameObject> listJBItems = new List<GameObject>();

    public GameObject DialogOneButton;
    public GameObject DialogTwoButton;
    private JBPresenter _presenter;
    private int grapperId;
    private GameObject _JBItem;
    private Sprite warningConfirmButtonSprite;

    void Awake()
    {
        _presenter = new JBPresenter(this, ManagerLocator.Instance.JBManager._IJBService);
    }
    void OnEnable()
    {
        grapperId = GlobalVariable.GrapperId;

        warningConfirmButtonSprite = Resources.Load<Sprite>("images/UIimages/Warning_Back_Button_Background");
        Debug.Log(warningConfirmButtonSprite);
        LoadListJB();
    }
    void OnDisable()
    {
    }

    private void RefreshList()
    {
        JB_Item_Prefab.SetActive(true);
        foreach (var item in listJBItems)
        {
            if (item != JB_Item_Prefab)
                Destroy(item);
        }
        listJBItems.Clear();
    }

    public void LoadListJB()
    {
        RefreshList();
        _presenter.LoadListJBGeneral(grapperId);
    }
    public void DisplayList(List<JBInformationModel> models)
    {
        if (models.Any())
        {
            JB_Item_Prefab.SetActive(true);
            foreach (var model in models)
            {
                // int JBIndex = models.IndexOf(model);
                // Debug.Log(JBIndex);

                var newJBItem = Instantiate(JB_Item_Prefab, Parent_Vertical_Layout_Group.transform);
                newJBItem.SetActive(true);
                Transform newJBItemTransform = newJBItem.transform;
                Transform newJBItemPreviewInforGroup = newJBItemTransform.GetChild(0);
                newJBItemPreviewInforGroup.Find("Preview_JB_Name").GetComponent<TMP_Text>().text = model.Name;
                Transform newJBItemPreviewButtonGroup = newJBItemTransform.GetChild(1);
                listJBItems.Add(newJBItem);
                var editButton = newJBItemPreviewButtonGroup.Find("Group/Edit_Button").GetComponent<Button>();
                var deleteButton = newJBItemPreviewButtonGroup.Find("Group/Delete_Button").GetComponent<Button>();
                editButton.onClick.RemoveAllListeners();
                deleteButton.onClick.RemoveAllListeners();
                editButton.onClick.AddListener(() => EditJBItem(model.Id));
                deleteButton.onClick.AddListener(() => DeleJBItem(newJBItem, model));
            }
        }
        else
        {
            Debug.Log("No JBs found");
        }
        JB_Item_Prefab.SetActive(false);
        scrollView.verticalNormalizedPosition = 1f;
    }

    private void EditJBItem(int id)
    {
        GlobalVariable.JBId = id;
        OpenUpdateCanvas();
    }
    private void DeleJBItem(GameObject JBItem, JBInformationModel model)
    {
        OpenDeleteWarningDialog(JBItem, model);
    }

    public void OpenAddNewCanvas()
    {
        Add_New_JB_Canvas.SetActive(true);
        List_JB_Canvas.SetActive(false);
        Update_JB_Canvas.SetActive(false);
    }
    private void OpenUpdateCanvas()
    {
        List_JB_Canvas.SetActive(false);
        Add_New_JB_Canvas.SetActive(false);
        Update_JB_Canvas.SetActive(true);
    }

    private void OpenDeleteWarningDialog(GameObject JBItem, JBInformationModel model)
    {
        DialogTwoButton.SetActive(true);

        var backgroundTransform = DialogTwoButton.transform.Find("Background");

        var Horizontal_Group = DialogTwoButton.transform.Find("Background/Horizontal_Group").gameObject.transform;

        var dialog_Content = DialogTwoButton.transform.Find("Background/Dialog_Content").GetComponent<TMP_Text>().text = $"Bạn có chắc chắn muốn xóa tủ: <color=#ED1C24><b>{model.Name}</b></color> khỏi hệ thống? Hãy kiểm tra kĩ trước khi nhấn nút \"xác nhận\" phía dưới";

        var dialog_Title = DialogTwoButton.transform.Find("Background/Dialog_Title").GetComponent<TMP_Text>().text = "Xóa tủ JB/TSD khỏi hệ thống?";

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
            _JBItem = JBItem;
            _presenter.DeleteJB(model.Id);
            DialogTwoButton.SetActive(false);
        });
        backButton.onClick.AddListener(() =>
        {
            DialogTwoButton.SetActive(false);
        });
    }
    private void OpenErrorDialog(string title = "Xóa tủ JB/TSD thất bại", string message = "Đã có lỗi xảy ra khi xóa tủ JB/TSD khỏi hệ thống. Vui lòng thử lại sau")
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
        if (GlobalVariable.APIRequestType.Contains("GET_JB_List_General"))
        {
            OpenErrorDialog(title: "Tải danh sách thất bại", message: message);
        }
        if (GlobalVariable.APIRequestType.Contains("DELETE_JB"))
        {
            OpenErrorDialog();
        }
    }
    public void ShowSuccess()
    {
        if (GlobalVariable.APIRequestType.Contains("GET_JB_List_General"))
        {
            Show_Toast.Instance.ShowToast("success", "Tải danh sách thành công");
        }
        if (GlobalVariable.APIRequestType.Contains("DELETE_JB"))
        {
            listJBItems.Remove(_JBItem);
            Destroy(_JBItem);
            Show_Toast.Instance.ShowToast("success", "Xóa tủ JB/TSD thành công");
        }

        StartCoroutine(Show_Toast.Instance.Set_Instance_Status_False());
    }

    // Không dùng trong ListView
    public void DisplayDetail(JBInformationModel model) { }
    public void DisplayCreateResult(bool success) { }
    public void DisplayUpdateResult(bool success) { }
    public void DisplayDeleteResult(bool success) { }
}