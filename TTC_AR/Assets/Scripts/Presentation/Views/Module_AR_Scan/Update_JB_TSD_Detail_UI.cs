using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using EasyUI.Progress;
using System.Threading.Tasks;
using System.Linq;

public class Update_JB_TSD_Detail_UI : MonoBehaviour, IJBView
{
    [SerializeField] private GameObject jB_TSD_Detail_Panel;
    [SerializeField] private TMP_Text jbNameValue;
    [SerializeField] private ScrollRect scroll_Area;
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject jb_Infor_Prefab;
    [SerializeField] private TMP_Text jb_location_value;
    [SerializeField] private Image jb_location_imagePrefab;
    [SerializeField] private Image jb_connection_imagePrefab;
    [SerializeField] private GameObject emptySpace;
    private List<GameObject> instantiatedImages = new List<GameObject>();
    private JBPresenter _presenter;
    private JBInformationModel _jbInformationModel;
    private List<ImageInformationModel> list_Additional_Connection_Images = new List<ImageInformationModel>();

    void Awake()
    {
        _presenter = new JBPresenter(this, ManagerLocator.Instance.JBManager._IJBService);
    }

    private void OnEnable()
    {
        Initialize();
        if (GlobalVariable.temp_List_AdditionalImages != null && GlobalVariable.temp_List_AdditionalImages.Any())
        {
            list_Additional_Connection_Images = GlobalVariable.temp_List_AdditionalImages;
        }
        else
        {
            list_Additional_Connection_Images = new List<ImageInformationModel>();
        }
        _presenter.LoadDetailById(GlobalVariable.JBId);

    }
    public void DisplayDetail(JBInformationModel model)
    {
        if (model != null)
        {
            _jbInformationModel = model;
            GlobalVariable.temp_JBInformationModel = _jbInformationModel;
            GlobalVariable.JBId = _jbInformationModel.Id;
        }
        UpdateJBTextInfor(_jbInformationModel);
        UpdateUIImages(_jbInformationModel);
    }

    private void OnDisable()
    {
        ClearInstantiatedImageObjects();
        GlobalVariable.temp_List_AdditionalImages.Clear();
        list_Additional_Connection_Images.Clear();
    }

    private void Initialize()
    {
        jbNameValue ??= jB_TSD_Detail_Panel.transform.Find("Horizontal_JB_TSD_jbNameValue/JB_TSD_jbNameValue").GetComponent<TMP_Text>();
        content ??= jB_TSD_Detail_Panel.transform.Find("Scroll_Area/Content").gameObject;
        jb_Infor_Prefab ??= content.transform.Find("JB_Infor").gameObject;
        jb_location_value ??= jb_Infor_Prefab.transform.Find("Text_JB_location_group/Location_Value").GetComponent<TMP_Text>();
        jb_location_imagePrefab ??= jb_Infor_Prefab.transform.Find("JB_location_imagePrefab").GetComponent<Image>();
        jb_connection_imagePrefab ??= content.transform.Find("JB_connection_imagePrefab").GetComponent<Image>();
    }

    private void UpdateJBTextInfor(JBInformationModel model)
    {
        if (!string.IsNullOrEmpty(model.Name))
        {
            jbNameValue.text = model.Name;
        }
        else
        {
            jbNameValue.text = "Chưa cập nhật";
            jbNameValue.color = Color.red;
            jbNameValue.fontStyle = FontStyles.Bold;
        }
        if (!string.IsNullOrEmpty(model.Location))
        {
            jb_location_value.text = model.Location;

        }
        else
        {
            jb_location_value.text = "Được ghi chú trong sơ đồ";
            jb_location_value.color = Color.red;
            jb_location_value.fontStyle = FontStyles.Bold;
        }

    }

    private async void UpdateUIImages(JBInformationModel model)
    {
        ShowLoading("Đang tải hình ảnh...");
        try
        {
            ClearInstantiatedImageObjects();
            jb_location_imagePrefab.gameObject.SetActive(true);
            var tasks = new List<Task>();
            // Handle location image
            if (model.OutdoorImage != null && !string.IsNullOrEmpty(model.OutdoorImage.Name))
            {
                tasks.Add(LoadImage.Instance.LoadImageFromUrlAsync(model.OutdoorImage.Name, jb_location_imagePrefab));
            }
            else
            {
                tasks.Add(LoadImage.Instance.LoadImageFromUrlAsync("JB_Location_Noted.png", jb_location_imagePrefab));
            }

            // Prepare connection images
            jb_connection_imagePrefab.gameObject.SetActive(true); // Hide prefab template
            var allConnectionImages = new List<ImageInformationModel>();

            if (model.ListConnectionImages != null && model.ListConnectionImages.Any())
                allConnectionImages.AddRange(model.ListConnectionImages);

            if (list_Additional_Connection_Images != null && list_Additional_Connection_Images.Any())
                allConnectionImages.AddRange(list_Additional_Connection_Images);

            foreach (var imageModel in allConnectionImages)
            {
                if (imageModel == null || string.IsNullOrEmpty(imageModel.Name)) continue;

                var newConnectionImage = Instantiate(jb_connection_imagePrefab, content.transform);

                if (!newConnectionImage.gameObject.activeSelf) newConnectionImage.gameObject.SetActive(true);

                instantiatedImages.Add(newConnectionImage.gameObject);

                tasks.Add(LoadImage.Instance.LoadImageFromUrlAsync(imageModel.Name, newConnectionImage));

            }

            await Task.WhenAll(tasks);

            // Resize images
            if (jb_location_imagePrefab.gameObject.activeSelf)
                StartCoroutine(Resize_GameObject_Function.Set_NativeSize_For_GameObject(jb_location_imagePrefab));

            foreach (var imageObj in instantiatedImages)
            {
                var img = imageObj.GetComponent<Image>();
                if (img != null)
                    StartCoroutine(Resize_GameObject_Function.Set_NativeSize_For_GameObject(img));
            }
            emptySpace.transform.SetAsLastSibling();
            scroll_Area.verticalNormalizedPosition = 1f;
            jb_connection_imagePrefab.gameObject.SetActive(false);

        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error loading images: {ex.Message}");
            ShowError("Lỗi tải hình ảnh");
        }
        finally
        {
            HideLoading();
        }
    }

    private void ClearInstantiatedImageObjects()
    {
        foreach (var imageObject in instantiatedImages)
        {
            Destroy(imageObject);
        }
        instantiatedImages.Clear();
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

    public void ShowSuccess()
    {
        if (GlobalVariable.APIRequestType.Contains("GET_JB"))
        {
            Show_Toast.Instance.ShowToast("success", "Tải dữ liệu thành công");
        }
        StartCoroutine(Show_Toast.Instance.Set_Instance_Status_False(1f));
    }
    public void ShowError(string message)
    {
        if (GlobalVariable.APIRequestType.Contains("GET_JB"))
        {
            Show_Toast.Instance.ShowToast("failure", "Tải dữ liệu thất bại");
        }
        StartCoroutine(Show_Toast.Instance.Set_Instance_Status_False(1f));
    }

    public void DisplayList(List<JBInformationModel> models)
    {
    }

    public void DisplayCreateResult(bool success)
    {

    }
    public void DisplayUpdateResult(bool success)
    {

    }

    public void DisplayDeleteResult(bool success)
    {

    }
}