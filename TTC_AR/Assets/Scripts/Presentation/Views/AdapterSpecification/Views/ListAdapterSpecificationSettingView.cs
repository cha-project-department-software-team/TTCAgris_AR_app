using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyUI.Progress;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ListAdapterSpecificationSettingView : MonoBehaviour, IAdapterSpecificationView
{
    [Header("Canvas")]
    public GameObject List_AdapterSpecification_Canvas;
    public GameObject Add_New_AdapterSpecification_Canvas;
    public GameObject Update_AdapterSpecification_Canvas;

    public GameObject AdapterSpecification_Item_Prefab;
    public GameObject Parent_Vertical_Layout_Group;
    public ScrollRect scrollView;
    private List<GameObject> listAdapterSpecificationItems = new List<GameObject>();


    public GameObject DialogOneButton;
    public GameObject DialogTwoButton;
    private AdapterSpecificationPresenter _presenter;
    private Sprite warningConfirmButtonSprite;
    private int companyId;
    private GameObject AdapterSpecificationItem;

    void Awake()
    {
        // var DeviceManager = FindObjectOfType<DeviceManager>();
        _presenter = new AdapterSpecificationPresenter(this, ManagerLocator.Instance.AdapterSpecificationManager._IAdapterSpecificationService);
        // DeviceManager._IDeviceService
    }

    void OnEnable()
    {
        companyId = GlobalVariable.companyId;
        warningConfirmButtonSprite = Resources.Load<Sprite>("images/UIimages/Warning_Back_Button_Background");
        Debug.Log(warningConfirmButtonSprite);
        LoadListAdapterSpecification();
    }
    void OnDisable()
    {
        StopAllCoroutines();
    }

    private void RefreshList()
    {
        AdapterSpecification_Item_Prefab.SetActive(true);
        foreach (var item in listAdapterSpecificationItems)
        {
            if (item != AdapterSpecification_Item_Prefab)
                Destroy(item);
        }
        listAdapterSpecificationItems.Clear();
    }

    public void LoadListAdapterSpecification()
    {
        RefreshList();
        _presenter.LoadListAdapterSpecification(companyId);

    }
    public void DisplayList(List<AdapterSpecificationModel> models)
    {
        if (models.Any())
        {
            foreach (var model in models)
            {
                int AdapterSpecificationIndex = models.IndexOf(model);
                Debug.Log(AdapterSpecificationIndex);
                var newAdapterSpecificationItem = Instantiate(AdapterSpecification_Item_Prefab, Parent_Vertical_Layout_Group.transform);
                Transform newAdapterSpecificationItemTransform = newAdapterSpecificationItem.transform;
                Transform newAdapterSpecificationItemPreviewInforGroup = newAdapterSpecificationItemTransform.GetChild(0);
                newAdapterSpecificationItemPreviewInforGroup.Find("Preview_AdapterSpecification_Code").GetComponent<TMP_Text>().text = model.Code;
                Transform newAdapterSpecificationItemPreviewButtonGroup = newAdapterSpecificationItemTransform.GetChild(1);
                listAdapterSpecificationItems.Add(newAdapterSpecificationItem);
                newAdapterSpecificationItemPreviewButtonGroup.Find("Group/Edit_Button").GetComponent<Button>().onClick.AddListener(() => EditAdapterSpecificationItem(model.Id));
                newAdapterSpecificationItemPreviewButtonGroup.Find("Group/Delete_Button").GetComponent<Button>().onClick.AddListener(() => DeleAdapterSpecificationItem(newAdapterSpecificationItem, model));
            }
        }
        else
        {
            Debug.Log("No AdapterSpecifications found");
        }
        AdapterSpecification_Item_Prefab.SetActive(false);

    }

    private void EditAdapterSpecificationItem(int id)
    {

        GlobalVariable.adapterSpecificationId = id;

        OpenUpdateCanvas();
    }
    private void DeleAdapterSpecificationItem(GameObject AdapterSpecificationItem, AdapterSpecificationModel model)

    {
        OpenDeleteWarningDialog(AdapterSpecificationItem, model);
    }

    public void OpenAddNewCanvas()
    {
        Add_New_AdapterSpecification_Canvas.SetActive(true);
        List_AdapterSpecification_Canvas.SetActive(false);
        Update_AdapterSpecification_Canvas.SetActive(false);
    }
    private void OpenUpdateCanvas()
    {
        Update_AdapterSpecification_Canvas.SetActive(true);
        List_AdapterSpecification_Canvas.SetActive(false);
        Add_New_AdapterSpecification_Canvas.SetActive(false);
    }

    private void OpenDeleteWarningDialog(GameObject AdapterSpecificationItem, AdapterSpecificationModel model)
    {
        DialogTwoButton.SetActive(true);

        var backgroundTransform = DialogTwoButton.transform.Find("Background");

        var Horizontal_Group = DialogTwoButton.transform.Find("Background/Horizontal_Group").gameObject.transform;

        var dialog_Content = DialogTwoButton.transform.Find("Background/Dialog_Content").GetComponent<TMP_Text>().text = $"Bạn có chắc chắn muốn xóa thông tin loại Adapter <b><color =#004C8A>{model.Code}</b></color> khỏi hệ thống? Hãy kiểm tra kĩ trước khi nhấn nút xác nhận phía dưới";

        var dialog_Title = DialogTwoButton.transform.Find("Background/Dialog_Title").GetComponent<TMP_Text>().text = "Xóa loại Adapter khỏi hệ thống?";

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
            this.AdapterSpecificationItem = AdapterSpecificationItem;
            _presenter.DeleteAdapterSpecification(model.Id);
            DialogTwoButton.SetActive(false);

        });
        backButton.onClick.AddListener(() =>
        {
            DialogTwoButton.SetActive(false);
        });
    }


    private void OpenErrorDialog(string title = "Tạo loại Adapter mới thất bại", string content = "Đã có lỗi xảy ra khi tạo loại Adapter mới. Vui lòng thử lại sau.")
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
        if (GlobalVariable.APIRequestType.Contains("GET_AdapterSpecification_List"))
        {
            OpenErrorDialog(title: "Tải danh sách loại Adapter thất bại", content: "Đã có lỗi xảy ra khi tải danh sách loại Adapter. Vui lòng thử lại sau");
        }
        if (GlobalVariable.APIRequestType.Contains("DELETE_AdapterSpecification"))
        {
            OpenErrorDialog();
        }

    }
    public void ShowSuccess(string message)
    {

        if (GlobalVariable.APIRequestType.Contains("GET_AdapterSpecification_List"))
        {
            Show_Toast.Instance.ShowToast("success", "Tải danh sách thành công");
        }
        if (GlobalVariable.APIRequestType.Contains("DELETE_AdapterSpecification"))
        {
            listAdapterSpecificationItems.Remove(AdapterSpecificationItem);
            Destroy(AdapterSpecificationItem);
            Show_Toast.Instance.ShowToast("success", "Xóa loại Adapter thành công");
        }

        StartCoroutine(Show_Toast.Instance.Set_Instance_Status_False(1f));
    }

    // Không dùng trong ListView
    public void DisplayDetail(AdapterSpecificationModel model) { }
    public void DisplayCreateResult(bool success) { }
    public void DisplayUpdateResult(bool success) { }
    public void DisplayDeleteResult(bool success) { }
}