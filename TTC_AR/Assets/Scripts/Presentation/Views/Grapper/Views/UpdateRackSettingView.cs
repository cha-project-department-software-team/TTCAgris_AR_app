// using System;
// using System.Collections.Generic;
// using System.Linq;
// using EasyUI.Progress;
// using TMPro;
// using UnityEngine;
// using UnityEngine.UI;

// public class UpdateRackSettingView : MonoBehaviour, IRackView
// {
//     public Initialize_Rack_List_Option_Selection initialize_Rack_List_Option_Selection;
//     private RackPresenter _presenter;

//     [SerializeField] private RackInformationModel RackInformationModel;

//     [Header("Input Fields")]
//     [SerializeField] private TMP_InputField Name_TextField;

//     [Header("Basic")]
//     public ScrollRect scrollRect;
//     public RectTransform viewPortTransform;
//     public GameObject parent_Content_Vertical_Group;
//     private Transform temp_Item_Transform;

//     [Header("LayOutGroup")]
//     public GameObject List_Modules_Parent_Grid_Layout_Group;

//     [Header("Prefab")]
//     public GameObject Module_Item_Prefab;


//     [Header("Buttons")]
//     [SerializeField] private Button submitButton;
//     [SerializeField] private Button backButton;

//     [SerializeField] private Button backButtonModuleListSelection;




//     public GameObject DialogOneButton;
//     public GameObject DialogTwoButton;

//     [Header("Canvas")]
//     public GameObject List_Rack_Canvas;
//     public GameObject Add_New_Rack_Canvas;
//     public GameObject Update_Rack_Canvas;

//     private readonly Dictionary<string, ModuleInformationModel> temp_Dictionary_ModuleModel = new();

//     private readonly Dictionary<string, List<GameObject>> selectedGameObjects = new()
//     {
//         { "Modules", new List<GameObject>() }

//     };

//     private readonly Dictionary<string, int> selectedCounts = new()
//     {

//         { "Modules", 0 },

//     };

//     // void Awake()
//     // {
//     //     var RackManager = FindObjectOfType<RackManager>();
//     //     _presenter = new RackPresenter(this, RackManager._IRackService);
//     // }
//     void Awake()
//     {
//         // var ModuleManager = FindObjectOfType<ModuleManager>();
//         _presenter = new RackPresenter(this, ManagerLocator.Instance.RackManager._IRackService);
//         // ModuleManager._IModuleService
//     }

//     void OnEnable()
//     {
//         ResetAllInputFields();

//         AddButtonListeners(initialize_Rack_List_Option_Selection.Module_List_Selection_Option_Content_Transform, "Modules");

//         backButton.onClick.RemoveAllListeners();
//         submitButton.onClick.RemoveAllListeners();

//         backButtonModuleListSelection.onClick.RemoveAllListeners();
//         backButtonModuleListSelection.onClick.AddListener(() => CloseListSelectionFromBackButton("Modules"));

//         backButton.onClick.AddListener(CloseAddCanvas);
//         submitButton.onClick.AddListener(OnSubmitButtonClick);

//         scrollRect.verticalNormalizedPosition = 1;

//         _presenter.LoadDetailById(GlobalVariable.rackId);

//     }

//     void OnDisable()
//     {
//         RenewView();
//     }

//     private void OnSubmitButtonClick()
//     {
//         RackInformationModel = new RackInformationModel(
//             name: Name_TextField.text,
//             listModuleInformationModel: temp_Dictionary_ModuleModel.Values.Any() ? temp_Dictionary_ModuleModel.Values.ToList() : new List<ModuleInformationModel>()
//             );

//         if (string.IsNullOrEmpty(Name_TextField.text))
//         {
//             OpenErrorDialog("Vui lòng nhập mã Rack");
//             return;
//         }
//         else
//         {
//             // Debug.Log("Rack Name: " + RackInformationModel.Name);
//             // Debug.Log("Rack Name: " + RackInformationModel.Rack.Name);
//             // Debug.Log("Module Count: " + RackInformationModel.ListModuleInformationModel.Count);
//             // Debug.Log("JB Count: " + RackInformationModel.ListJBInformationModel.Count);
//             // Debug.Log("RackSpecification Code: " + RackInformationModel.RackSpecificationModel.Code);
//             // Debug.Log("AdapterSpecification Code: " + RackInformationModel.AdapterSpecificationModel.Code);

//             _presenter.UpdateRack(GlobalVariable.rackId, RackInformationModel);
//         }
//     }
//     public void loadDetailById()
//     {
//         RenewView();

//         _presenter.LoadDetailById(GlobalVariable.rackId);
//     }
//     private void RenewView()
//     {


//         ClearActiveChildren(List_Modules_Parent_Grid_Layout_Group);

//         ResetAllInputFields();


//         temp_Dictionary_ModuleModel.Clear();
//         selectedGameObjects["Modules"].Clear();
//         selectedCounts["Modules"] = 0;

//     }

//     public void CloseAddCanvas()
//     {
//         Add_New_Rack_Canvas.SetActive(false);
//         Update_Rack_Canvas.SetActive(false);
//         List_Rack_Canvas.SetActive(true);
//     }

//     private void ResetAllInputFields()
//     {
//         Name_TextField.text = "";
//     }

//     private void AddButtonListeners(Transform contentTransform, string field)
//     {
//         foreach (Transform option in contentTransform)
//         {
//             AddButtonListener(option, () => SelectOption(option.gameObject.GetComponentInChildren<TMP_Text>().text, field));
//         }
//     }

//     private void AddButtonListener(Transform option, UnityEngine.Events.UnityAction action)
//     {
//         var button = option.gameObject.GetComponent<Button>();
//         if (button != null)
//         {
//             button.onClick.RemoveAllListeners();
//             button.onClick.AddListener(action);
//         }
//     }

//     private void SelectOption(string optionTextValue, string field)
//     {
//         SetItemTextValue(temp_Item_Transform, optionTextValue, field);
//         CloseListSelection(field);

//         selectedCounts[field]++;
//         if (!selectedGameObjects[field].Contains(temp_Item_Transform.gameObject))
//         {
//             selectedGameObjects[field].Add(temp_Item_Transform.gameObject);
//         }
//         else
//         {
//             Destroy(temp_Item_Transform.gameObject);
//         }
//     }

//     private void SetItemTextValue(Transform temp_Item_Transform, string textValue, string field)
//     {
//         var itemText = temp_Item_Transform.GetComponentInChildren<TMP_Text>();

//         if (itemText == null) return;

//         itemText.text = textValue;

//         switch (field)
//         {

//             case "Modules":
//                 if (GlobalVariable.temp_Dictionary_ModuleInformationModel.TryGetValue(textValue, out var ModuleInfo))
//                 {
//                     if (!temp_Dictionary_ModuleModel.ContainsKey(textValue))
//                     {
//                         temp_Dictionary_ModuleModel[textValue] = new ModuleInformationModel(ModuleInfo.Id, ModuleInfo.Name);
//                     }
//                     else
//                     {
//                         Destroy(temp_Item_Transform.gameObject);
//                     }
//                 }
//                 break;
//         }
//     }
//     public void OpenListModuleSelection() => OpenListSelection("Modules", Module_Item_Prefab, List_Modules_Parent_Grid_Layout_Group);

//     public void OpenListSelection(string field, GameObject itemPrefab, GameObject parentGroup)
//     {
//         // if (!initialize_Rack_List_Option_Selection.Selection_Option_Canvas.activeSelf)
//         //     initialize_Rack_List_Option_Selection.Selection_Option_Canvas.SetActive(true);

//         var newItem = Instantiate(itemPrefab, parentGroup.transform);
//         temp_Item_Transform = newItem.transform;
//         var button = newItem.GetComponentInChildren<Button>();
//         button.onClick.AddListener(() => DeselectItem(newItem, field));
//         GetSelectionPanel(field).SetActive(true);
//     }

//     private void DeselectItem(GameObject item, string field)
//     {
//         selectedCounts[field]--;
//         selectedGameObjects[field].Remove(item);
//         temp_Dictionary_ModuleModel.Remove(item.GetComponentInChildren<TMP_Text>().text);
//         Destroy(item);


//     }

//     public void CloseListSelection(string field)
//     {
//         switch (field)
//         {

//             case "Modules":
//                 initialize_Rack_List_Option_Selection.selection_List_Module_Panel.SetActive(false);
//                 break;

//         }
//         // initialize_Rack_List_Option_Selection.Selection_Option_Canvas.SetActive(false);
//     }

//     public void CloseListSelectionFromBackButton(string field)
//     {
//         temp_Item_Transform.gameObject.SetActive(false);
//         switch (field)
//         {

//             case "Modules":
//                 initialize_Rack_List_Option_Selection.selection_List_Module_Panel.SetActive(false);
//                 break;

//         }
//         // initialize_Rack_List_Option_Selection.Selection_Option_Canvas.SetActive(false);
//     }

//     private void ClearDeActiveChildren(GameObject parentGroup)
//     {
//         foreach (Transform child in parentGroup.transform)
//         {
//             if (!child.gameObject.activeSelf && parentGroup.transform.childCount > 1)
//                 Destroy(child.gameObject);
//         }
//     }

//     private void ClearActiveChildren(GameObject parentGroup)
//     {
//         foreach (Transform child in parentGroup.transform)
//         {
//             Destroy(child.gameObject);
//         }
//     }

//     private GameObject GetSelectionPanel(string field)
//     {
//         return field switch
//         {
//             "Modules" => initialize_Rack_List_Option_Selection.selection_List_Module_Panel,

//             _ => throw new ArgumentException("Invalid field Name")
//         };
//     }

//     private void OpenErrorDialog(string title = "Cập nhật Rack IO thất bại", string message = "Đã có lỗi xảy ra khi cập nhật Rack IO. Vui lòng thử lại sau.")
//     {
//         DialogOneButton.SetActive(true);
//         var backButton = DialogOneButton.transform.Find("Background/Back_Button").GetComponent<Button>();
//         backButton.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/UIimages/Error_Back_Button_Background");
//         DialogOneButton.transform.Find("Background/Dialog_Status_Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("images/UIimages/Error_Icon_For_Dialog");
//         DialogOneButton.transform.Find("Background/Dialog_Content").GetComponent<TMP_Text>().text = message;
//         DialogOneButton.transform.Find("Background/Dialog_Title").GetComponent<TMP_Text>().text = title;

//         backButton.onClick.RemoveAllListeners();
//         backButton.onClick.AddListener(() =>
//         {
//             DialogOneButton.SetActive(false);
//         });
//     }

//     private void OpenSuccessDialog(string title = "Cập nhật Rack IO thành công", string message = "Bạn đã cập nhật Rack IO thành công: ")
//     {
//         DialogOneButton.SetActive(true);

//         var backButton = DialogOneButton.transform.Find("Background/Back_Button").GetComponent<Button>();

//         backButton.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/UIimages/Success_Back_Button_Background");

//         DialogOneButton.transform.Find("Background/Dialog_Status_Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("images/UIimages/Success_Icon_For_Dialog");

//         DialogOneButton.transform.Find("Background/Dialog_Content").GetComponent<TMP_Text>().text = message + $"<b><color=#004C8A>{RackInformationModel.Name}</b></color>";

//         DialogOneButton.transform.Find("Background/Dialog_Title").GetComponent<TMP_Text>().text = title;

//         backButton.onClick.RemoveAllListeners();

//         backButton.onClick.AddListener(() =>
//         {
//             DialogOneButton.SetActive(false);
//             _presenter.LoadDetailById(GlobalVariable.rackId);
//             scrollRect.verticalNormalizedPosition = 1;
//         }
//        );
//     }


//     private void ShowProgressBar(string title, string details)
//     {
//         Progress.Show(title, ProgressColor.Blue, true);
//         Progress.SetDetailsText(details);
//     }

//     private void HideProgressBar()
//     {
//         Progress.Hide();
//     }

//     public void ShowLoading(string title) => ShowProgressBar(title, "Đang tải dữ liệu...");
//     public void HideLoading() => HideProgressBar();

//     public void ShowError(string message)
//     {
//         if (GlobalVariable.APIRequestType.Contains("PUT_Rack"))
//         {
//             OpenErrorDialog();
//         }
//     }

//     public void ShowSuccess()
//     {
//         if (GlobalVariable.APIRequestType.Contains("PUT_Rack"))
//         {
//             Show_Toast.Instance.ShowToast("success", "Cập nhật Rack IO thành công");
//             OpenSuccessDialog();
//         }

//         StartCoroutine(Show_Toast.Instance.Set_Instance_Status_False(1f));
//     }

//     public void DisplayList(List<RackInformationModel> models) { }
//     public void DisplayCreateResult(bool success) { }
//     public void DisplayUpdateResult(bool success) { }
//     public void DisplayDeleteResult(bool success) { }
//     private void SetInitialInputFields(RackInformationModel model)
//     {
//         Name_TextField.text = model.Name;
//     }
//     public void DisplayDetail(RackInformationModel model)
//     {
//         SetInitialInputFields(model);

//         PopulateItems(model.ListModuleInformationModel?.Select(item => item.Name).ToList(), Module_Item_Prefab, List_Modules_Parent_Grid_Layout_Group, "Modules");
//         AddButtonListeners(initialize_Rack_List_Option_Selection.Module_List_Selection_Option_Content_Transform, "Modules");
//     }

//     private void PopulateItems(List<string> listItems, GameObject itemPrefab, GameObject parentLayoutGroup, string field)
//     {
//         if (listItems == null || listItems.Count == 0) return;

//         var parentTransform = parentLayoutGroup.transform;
//         foreach (var item in listItems)
//         {
//             var newItem = Instantiate(itemPrefab, parentTransform);
//             SetItemTextValue(newItem.transform, item, field);
//             AddButtonListener(newItem.transform.Find("Deselect_Button"), () => DeselectItem(newItem, field));
//             selectedGameObjects[field].Add(newItem);
//         }
//     }

// }





