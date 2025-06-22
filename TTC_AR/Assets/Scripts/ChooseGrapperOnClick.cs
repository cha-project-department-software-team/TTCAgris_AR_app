// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.SceneManagement;
// using UnityEngine.UI;

// public class ChooseGrapperOnClick : MonoBehaviour
// {
//     public string grapperName;
//     public Button onClickButton;
//     public Get_All_Data_By_Grapper_For_Searching getAllDataByGrapperForSearching;
//     public EventPublisher eventPublisher;

//     private void Awake()
//     {
//         onClickButton ??= gameObject.GetComponent<Button>();
//         eventPublisher ??= FindObjectOfType<EventPublisher>();
//     }

//     private void Start()
//     {
//         onClickButton.onClick.AddListener(StartHandleButtonClick);
//     }

//     private void StartHandleButtonClick()
//     {
//         if (SceneManager.GetActiveScene().name == MyEnum.MenuScene.GetDescription() && grapperName != "GrapperA")
//         {

//         }
//         //! else StartCoroutine(HandleButtonClick());
//         // Vô hiệu hóa nút để không thể click lại trong quá trình xử lý
//     }

//     // private IEnumerator HandleButtonClick()
//     // {// Vô hiệu hóa nút để không thể click lại trong quá trình xử lý
//     //     APIManager.Instance.GrapperId = APIManager.Instance.Dic_GrapperBasicNonListRackModels[grapperName];
//     //     Debug.Log($"Grapper ID: {APIManager.Instance.GrapperId}");
//     //     getAllDataByGrapperForSearching.grapperId = APIManager.Instance.GrapperId;
//     //     GlobalVariable.GrapperId = APIManager.Instance.GrapperId;
//     //     yield return SelectGrapperOnClick();
//     //     Debug.Log("Button Clicked!" + GlobalVariable.GrapperId);
//     //     eventPublisher.TriggerEvent_ButtonClicked();
//     //     yield return new WaitForSeconds(2f);
//     //     if (!onClickButton.interactable) onClickButton.interactable = true;
//     // }

//     private IEnumerator SelectGrapperOnClick()
//     {
//         // Grapper_Basic_Model grapper = GlobalVariable.temp_ListGrapperBasicModels.Find(grap => grap.Name == grapperName);
//         //     if (grapper != null)
//         //     {
//         //         GlobalVariable.temp_GrapperBasicModel = grapper;
//         //         Filter_List_Rack_And_Module_From_Grapper(grapper.Id, GlobalVariable.temp_ListGrapperBasicModels);
//         //     }
//         //     else
//         //     {
//         //         Debug.LogError($"Grapper not found: {grapperName}");
//         //     }
//         yield return new WaitForEndOfFrame();

//     }

//     private void OnDisable()
//     {
//         onClickButton.onClick.RemoveAllListeners();
//     }

//     // private void Filter_List_Rack_And_Module_From_Grapper(int grapperId, List<Grapper_Basic_Model> grapper_Basic_Models)
//     // {
//     //     var grapper_Basic_Model = grapper_Basic_Models.Find(grapper => grapper.Id == grapperId);
//     //     if (grapper_Basic_Model == null)
//     //     {
//     //         Debug.LogError($"Grapper with ID {grapperId} not found.");
//     //         return;
//     //     }

//     //     var tempRackList = new List<Rack_Non_List_Module_Model>(grapper_Basic_Model.List_Rack_Basic_Model);
//     //     var tempModuleList = new List<ModuleBasicNonRackModel>();

//     //     foreach (var rack in grapper_Basic_Model.List_Rack_Basic_Model)
//     //     {
//     //         tempModuleList.AddRange(rack.List_ModuleBasicNonRackModel);
//     //     }

//     //     GlobalVariable.temp_ListRackBasicModels = tempRackList;
//     //     GlobalVariable.temp_ListModuleBasicNonRackModels = tempModuleList;

//     //     Debug.Log($"Filter_List_Rack_From_Grapper: {GlobalVariable.temp_ListRackBasicModels.Count} \nFilter_List_Module_From_Grapper: {GlobalVariable.temp_ListModuleBasicNonRackModels.Count}");
//     // }
// }