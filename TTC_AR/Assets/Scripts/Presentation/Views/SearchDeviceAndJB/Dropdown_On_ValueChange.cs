using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EasyUI.Progress;
using System.Collections;

public class Dropdown_On_ValueChange : MonoBehaviour
{
    public SearchDeviceAndJBView searchableDropDownView;

    public GameObject prefab_Infor;
    public Open_Detail_Image open_Detail_Image;
    // public EventPublisher eventPublisher;
    public GameObject Device_Information_Group;
    public GameObject List_JB_Group;
    public GameObject JBPrefab;

    [SerializeField] private RectTransform contentTransform;
    [SerializeField] private TMP_Text code_Value_Text, function_Value_Text, range_Value_Text, unit_Value_Text, io_Value_Text;
    [SerializeField] private Image JB_Location_Image_Prefab, JB_Connection_Wiring_Image_Prefab;
    [SerializeField] private GameObject JB_Connection_Group, bottom_App_Bar;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Dictionary<string, Sprite> spriteCache = new();
    [SerializeField] private List<Image> instantiatedImages = new();
    [SerializeField] private Transform deviceInfo;

    private Dictionary<string, DeviceInformationModel> deviceDictionary;
    private Dictionary<string, JBInformationModel> jBDictionary;
    private string _jbName = "";

    private void Awake()
    {
        searchableDropDownView ??= GameObject.Find("Searchable").GetComponent<SearchDeviceAndJBView>();
        InitUIElements();
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    private void Start()
    {
        StartCoroutine(LoadData());
    }
    void OnDestroy()
    {
        ResetResources();
        StopAllCoroutines();
    }

    private IEnumerator LoadData()
    {
        yield return new WaitUntil(
            () => GlobalVariable_Search_Devices.temp_ListDeviceInformationModel.Any() &&
            GlobalVariable_Search_Devices.temp_ListJBInformationModel.Any()
        );

        try
        {
            Prepare_Device_Dictionary_For_Searching();
            Prepare_JB_Dictionary_For_Searching();
            searchableDropDownView.Initialize();
            searchableDropDownView.SetInitialTextFieldValue();
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message + " " + e.StackTrace + " " + e.Source + " " + e.InnerException);
        }
    }

    private void InitUIElements()
    {
        scrollRect ??= prefab_Infor.GetComponent<ScrollRect>();
        var content = prefab_Infor.transform.Find("Content");
        deviceInfo = content.Find("Device_information");
        code_Value_Text ??= deviceInfo.Find("Code_group/Code_value").GetComponent<TMP_Text>();
        function_Value_Text ??= deviceInfo.Find("Function_group/Function_value").GetComponent<TMP_Text>();
        range_Value_Text ??= deviceInfo.Find("Range_group/Range_value").GetComponent<TMP_Text>();
        io_Value_Text ??= deviceInfo.Find("IO_group/IO_value").GetComponent<TMP_Text>();
        // var jbConnectionGroup = content.Find("JB_Connection_group/JB_Connection_text_group");
        JB_Connection_Group ??= content.Find("JB_Connection_group").gameObject;
        JB_Location_Image_Prefab ??= JB_Connection_Group.transform.Find("image_BackGround/JB_Location_Image").GetComponent<Image>();
        JB_Connection_Wiring_Image_Prefab ??= JB_Connection_Group.transform.Find("JB_Connection_Wiring").GetComponent<Image>();
    }

    private void Prepare_Device_Dictionary_For_Searching()
    {
        var tempListDevice = GlobalVariable_Search_Devices.temp_ListDeviceInformationModel;
        deviceDictionary = tempListDevice
            .GroupBy(device => device.Code.ToLower())
            .ToDictionary(g => g.Key, g => g.First());

        foreach (var device in tempListDevice)
        {
            var functionKey = device.Function.ToLower();
            if (!deviceDictionary.ContainsKey(functionKey))
            {
                deviceDictionary.Add(functionKey, device);
            }
        }
    }

    private void Prepare_JB_Dictionary_For_Searching()
    {
        var tempListJB = GlobalVariable_Search_Devices.temp_ListJBInformationModel;

        jBDictionary = tempListJB
            .GroupBy(jb => jb.Name.ToLower())
            .ToDictionary(g => g.Key, g => g.First());
    }

    private void OnInputValueChanged(string input)
    {
        ClearWiringGroupAndCache();
        if (List_JB_Group.activeSelf)
        {
            List_JB_Group.SetActive(false);
        }
        if (Device_Information_Group.activeSelf)
        {
            Device_Information_Group.SetActive(false);
        }

        switch (searchableDropDownView.filter_Type)
        {
            case "Device":
                if (deviceDictionary.TryGetValue(input.ToLower(), out var device))
                {
                    List_JB_Group.SetActive(true);
                    Device_Information_Group.SetActive(true);
                    UpdateDeviceInformation(device);
                }
                break;
            case "JB/TSD":
                Device_Information_Group.SetActive(false);

                List_JB_Group.SetActive(true);
                JBPrefab.SetActive(true);
                if (jBDictionary.TryGetValue(input.ToLower(), out var jB))
                {
                    UpdateJBInformation(jB);
                }
                break;
        }
    }

    private void ClearWiringGroupAndCache()
    {
        foreach (Transform child in JB_Connection_Group.transform)
        {
            if (child.gameObject != JB_Connection_Wiring_Image_Prefab && child.gameObject.name.Contains("(Clone)"))
            {
                Destroy(child.gameObject);
            }
        }
        foreach (Transform child in List_JB_Group.transform)
        {
            if (child.gameObject != JB_Connection_Wiring_Image_Prefab && child.gameObject.name.Contains("(Clone)"))
            {
                Destroy(child.gameObject);
            }
        }
    }

    private async void UpdateDeviceInformation(DeviceInformationModel device)
    {
        if (!deviceInfo.gameObject.activeSelf) deviceInfo.gameObject.SetActive(true);
        code_Value_Text.text = device.Code;
        function_Value_Text.text = device.Function;
        unit_Value_Text.text = device.Unit;
        range_Value_Text.text = device.Range;
        io_Value_Text.text = device.IOAddress;

        if (device.JBInformationModels.Any())
        {
            ShowProgressBar("Đang tải hình ảnh...", "...");

            if (device.JBInformationModels.Count == 1)
            {
                if (!JBPrefab.activeSelf) JBPrefab.SetActive(true);
                var JBName = JBPrefab.transform.Find("JB_Connection_text_group/JB_Connection_value").GetComponent<TMP_Text>();
                var JBLocation = JBPrefab.transform.Find("JB_Connection_text_group/JB_Connection_location").GetComponent<TMP_Text>();
                _jbName = device.JBInformationModels[0].Name;
                var jbInformationModel = GlobalVariable_Search_Devices.temp_Dictionary_JBInformationModel.TryGetValue(_jbName, out var jbInfo) ? jbInfo : null;
                JBName.text = jbInformationModel.Name;
                JBLocation.text = jbInformationModel.Location;
                Debug.Log($"JB Name: {_jbName}, JB Location: {jbInformationModel.Location}");
                if (!string.IsNullOrEmpty(_jbName))
                {
                    Debug.Log("Run LoadDeviceSprites");
                    await LoadDeviceSprites(
                          list_Additional_Images: device.AdditionalConnectionImages,
                           jbInformationModel: jbInformationModel,
                           LocationImage: JB_Location_Image_Prefab,
                           ConnectionImage: JB_Connection_Wiring_Image_Prefab,
                           JB_List_Connection_Group: JB_Connection_Group.transform);
                    Canvas.ForceUpdateCanvases();
                    LayoutRebuilder.ForceRebuildLayoutImmediate(contentTransform);
                }
            }
            else
            {
                if (!JBPrefab.activeSelf) JBPrefab.SetActive(true);
                foreach (var jb in device.JBInformationModels)
                {
                    int jbIndex = device.JBInformationModels.IndexOf(jb);
                    var newJB = Instantiate(JBPrefab, List_JB_Group.transform);
                    var JBName = newJB.transform.Find("JB_Connection_text_group/JB_Connection_value").GetComponent<TMP_Text>();
                    var JBLocation = newJB.transform.Find("JB_Connection_text_group/JB_Connection_location").GetComponent<TMP_Text>();
                    _jbName = jb.Name;
                    var jbInformationModel = GlobalVariable_Search_Devices.temp_Dictionary_JBInformationModel.TryGetValue(_jbName, out var jbInfo) ? jbInfo : null;
                    JBName.text = jbInformationModel.Name;
                    JBLocation.text = jbInformationModel.Location;
                    Debug.Log($"JB Name: {_jbName}, JB Location: {jbInformationModel.Location}");
                    if (!string.IsNullOrEmpty(_jbName))
                    {
                        Image JB_Location_Image = newJB.transform.Find("JB_Location_Image").GetComponent<Image>();
                        Image JB_Connection_Wiring_Image = newJB.transform.Find("JB_Connection_Wiring").GetComponent<Image>();
                        await LoadDeviceSprites(
                             list_Additional_Images: device.AdditionalConnectionImages,
                              jbInformationModel: jbInformationModel,
                             LocationImage: JB_Location_Image,
                             ConnectionImage: JB_Connection_Wiring_Image,
                             JB_List_Connection_Group: newJB.transform);
                    }
                }
                Canvas.ForceUpdateCanvases();
                LayoutRebuilder.ForceRebuildLayoutImmediate(contentTransform);
                JBPrefab.SetActive(false);
            }
            HideProgressBar();
        }
        else
        {
            List_JB_Group.SetActive(false);
        }
    }

    private async void UpdateJBInformation(JBInformationModel jB)
    {
        ShowProgressBar("Đang tải hình ảnh...", "...");
        if (deviceInfo.gameObject.activeSelf) deviceInfo.gameObject.SetActive(false);
        var JBName = JBPrefab.transform.Find("JB_Connection_text_group/JB_Connection_value").GetComponent<TMP_Text>();
        var JBLocation = JBPrefab.transform.Find("JB_Connection_text_group/JB_Connection_location").GetComponent<TMP_Text>();
        _jbName = jB.Name;
        GlobalVariable_Search_Devices.jbName = _jbName;
        var jbInformationModel = GlobalVariable_Search_Devices.temp_Dictionary_JBInformationModel.TryGetValue(_jbName, out var jbInfo) ? jbInfo : null;
        JBName.text = $"{jbInformationModel.Name}:";
        JBLocation.text = jbInformationModel.Location;
        Debug.Log($"JB Name: {_jbName}, JB Location: {jbInformationModel.Location}");
        if (!string.IsNullOrEmpty(_jbName))
        {
            await LoadDeviceSprites(
             list_Additional_Images: null,
             jbInformationModel: jbInformationModel,
             LocationImage: JB_Location_Image_Prefab,
             ConnectionImage: JB_Connection_Wiring_Image_Prefab,
             JB_List_Connection_Group: JB_Connection_Group.transform);
        }
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(List_JB_Group.transform as RectTransform);
        HideProgressBar();
    }

    private async Task LoadDeviceSprites(List<ImageInformationModel> list_Additional_Images, JBInformationModel jbInformationModel, Image LocationImage, Image ConnectionImage, Transform JB_List_Connection_Group)
    {
        if (jbInformationModel == null || jbInformationModel.ListConnectionImages == null)
        {
            Debug.LogError("jbInformationModel hoặc ListConnectionImages bị null!");
            return;
        }

        var total_List_Connection_Images = new List<ImageInformationModel>(jbInformationModel.ListConnectionImages);

        if (list_Additional_Images != null && list_Additional_Images.Count > 0)
        {
            total_List_Connection_Images.AddRange(list_Additional_Images);
        }
        Debug.Log("Run LoadDeviceSprites: ApplySpriteJBImages");
        await ApplySpriteJBImages(
          outdoorImage: jbInformationModel.OutdoorImage,
          listConnectionImages: total_List_Connection_Images,
          JB_Location_Image: LocationImage,
          JB_Connection_Wiring_Image: ConnectionImage,
          JB_List_Connection_Group: JB_List_Connection_Group);
        if (scrollRect != null)
        {
            scrollRect.verticalNormalizedPosition = 1f;
        }
        else
        {
            Debug.LogWarning("scrollRect bị null, không thể thay đổi vị trí roll!");
        }
    }

    private async Task ApplySpriteJBImages(ImageInformationModel outdoorImage, List<ImageInformationModel> listConnectionImages, Image JB_Location_Image, Image JB_Connection_Wiring_Image, Transform JB_List_Connection_Group)
    {
        var tasks = new List<Task>();

        if (outdoorImage != null)
        {
            if (!string.IsNullOrEmpty(outdoorImage.Name))
            {
                tasks.Add(searchableDropDownView._presenter.LoadImageAsync(outdoorImage.Name, JB_Location_Image));
                var buttonComponent = JB_Location_Image.gameObject.GetComponent<Button>();
                AddButtonListener(buttonComponent, () => open_Detail_Image.Open_Detail_Canvas(JB_Location_Image));

            }
        }
        else
        {   //! Được ghi chú trên sơ đồ
            var buttonComponent = JB_Location_Image.gameObject.GetComponent<Button>();
            AddButtonListener(buttonComponent, () => open_Detail_Image.Open_Detail_Canvas(JB_Location_Image));
            //! Nhớ đổi thành tên ảnh thay vì Url
            tasks.Add(searchableDropDownView._presenter.LoadImageAsync("JB_Location_Noted.png", JB_Location_Image));

        }

        if (listConnectionImages.Any())
        {
            foreach (var image in listConnectionImages)
            {
                var newImageObject = Instantiate(JB_Connection_Wiring_Image.gameObject, JB_List_Connection_Group);
                var imageComponent = newImageObject.GetComponent<Image>();
                var buttonComponent = newImageObject.GetComponent<Button>();

                AddButtonListener(buttonComponent, () => open_Detail_Image.Open_Detail_Canvas(imageComponent));
                tasks.Add(searchableDropDownView._presenter.LoadImageAsync(image.Name, newImageObject.GetComponent<Image>()));

            }
        }
        await Task.WhenAll(tasks);

        JB_Connection_Wiring_Image.gameObject.SetActive(false);

        ResizeImages(JB_Location_Image, JB_List_Connection_Group);
    }

    private void AddButtonListener(Button button, Action onClickAction)
    {

        if (button == null)
        {
            return;
        }

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            onClickAction?.Invoke();
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

    private void ResizeImages(Image locationImage, Transform JB_List_Connection_Group)
    {
        StartCoroutine(Resize_GameObject_Function.Set_NativeSize_For_GameObject(locationImage));

        foreach (var connectionImage in JB_List_Connection_Group.GetComponentsInChildren<Image>())
        {
            if (connectionImage.gameObject.activeSelf && connectionImage.name.Contains("(Clone)") || connectionImage.name.Contains("Location"))
                StartCoroutine(Resize_GameObject_Function.Set_NativeSize_For_GameObject(connectionImage));
        }
    }

    // private void SetSprite(Image imageComponent, string jb_name)
    // {
    //     if (!spriteCache.TryGetValue(jb_name, out var jbSprite))
    //     {
    //         spriteCache.TryGetValue("JB_TSD_Location_Note", out jbSprite);
    //     }
    //     imageComponent.sprite = jbSprite;
    //     imageComponent.gameObject.GetComponent<Button>().onClick.AddListener(() => open_Detail_Image.Open_Detail_Canvas(imageComponent));
    //     StartCoroutine(Resize_GameObject_Function.Set_NativeSize_For_GameObject(imageComponent));
    // }

    // private void CreateAndSetSprite(string jb_name)
    // {
    //     var jbLocationImage = Instantiate(JB_Location_Image_Prefab, JB_Connection_Group.transform);
    //     jbLocationImage.transform.SetSiblingIndex(JB_Location_Image_Prefab.transform.GetSiblingIndex() + 1);
    //     jbLocationImage.gameObject.SetActive(true);
    //     SetSprite(jbLocationImage, jb_name);
    //     instantiatedImages.Add(jbLocationImage);
    // }

    // private void ClearInstantiatedImages()
    // {
    //     foreach (var img in instantiatedImages)
    //     {
    //         if (img != null)
    //         {
    //             Destroy(img.gameObject);
    //         }
    //     }
    //     instantiatedImages.Clear();
    // }

    private void ResetResources()
    {
        ClearWiringGroupAndCache();
        GlobalVariable_Search_Devices.temp_ListDeviceInformationModel.Clear();
        GlobalVariable_Search_Devices.temp_Dictionary_JBInformationModel.Clear();
        GlobalVariable_Search_Devices.temp_ListJBInformationModel.Clear();
        GlobalVariable_Search_Devices.temp_List_Device_For_Fitler.Clear();
        GlobalVariable_Search_Devices.temp_List_JB_For_Fitler.Clear();
    }

    private void HandleOrientationChange(ScreenOrientation newOrientation)
    {
        // if (newOrientation == ScreenOrientation.Portrait)
        // {
        //     bottom_App_Bar.SetActive(true);
        // }
        // else if (newOrientation == ScreenOrientation.LandscapeLeft || newOrientation == LandscapeRight)
        // {
        //     bottom_App_Bar.SetActive(false);
        // }
    }
}