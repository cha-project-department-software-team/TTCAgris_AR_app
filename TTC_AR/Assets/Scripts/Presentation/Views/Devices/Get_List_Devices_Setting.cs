// using System;
// using System.Collections.Generic;
// using TMPro;
// using UnityEngine;
// using UnityEngine.UI;

// public class Get_List_Devices_Setting : MonoBehaviour
// {
//     public GameObject List_Device_Canvas;
//     public GameObject Add_New_Device_Canvas;
//     public GameObject Update_Device_Canvas;
//     public GameObject DialogCanvas;
//     public GameObject Device_Item_Prefab;
//     public GameObject Parent_Vertical_Layout_Group;
//     public ScrollRect scrollView;
//     public GameObject DialogOneButton;
//     public GameObject DialogTwoButton;
//     // private IDeviceUseCase _DeviceUseCase;
//     private List<string> ListDeviceCode = new List<string>();
//     private List<GameObject> listDeviceItems = new List<GameObject>();

//     private void Awake()
//     {   // Khởi tạo dependency injection đơn giản
//         // IDeviceRepository repository = new DeviceRepository();
//         // _DeviceUseCase = new DeviceUseCase(repository);
//     }
//     private void Start()
//     {
//         if (GlobalVariable.list_DeviceCode != null && GlobalVariable.list_DeviceCode.Count > 0)
//         {
//             ListDeviceCode = GlobalVariable.list_DeviceCode;
//             Initialize(ListDeviceCode);
//         }
//         scrollView.normalizedPosition = new Vector2(0, 1);
//     }

//     private void Initialize(List<string> listDeviceCode)
//     {
//         foreach (var DeviceCode in listDeviceCode)
//         {
//             int DeviceIndex = ListDeviceCode.IndexOf(DeviceCode);

//             Debug.Log(DeviceIndex);

//             GameObject newDeviceItem = Instantiate(Device_Item_Prefab, Parent_Vertical_Layout_Group.transform);

//             Transform newDeviceItemTransform = newDeviceItem.transform;

//             Transform newDeviceItemPreviewInforGroup = newDeviceItemTransform.GetChild(0);

//             Transform newDeviceItemPreviewButtonGroup = newDeviceItemTransform.GetChild(1);

//             newDeviceItemPreviewInforGroup.Find("Preview_Device_Code").GetComponent<TMP_Text>().text = DeviceCode;

//             newDeviceItemPreviewInforGroup.Find("Preview_Device_GrapLocation").GetComponent<TMP_Text>().text = "GrapperA";

//             //  var Preview_Device_IOAddress = GlobalVariable.temp_Dictionary_DeviceIOAddress[DeviceCode];

//             // newDeviceItemPreviewInforGroup.Find("Preview_Device_IOAddress").GetComponent<TMP_Text>().text = Preview_Device_IOAddress;

//             newDeviceItemPreviewInforGroup.Find("Preview_Device_IOAddress").GetComponent<TMP_Text>().text = "A4.0.I";

//             listDeviceItems.Add(newDeviceItem);

//             newDeviceItemPreviewButtonGroup.Find("Group/Edit_Button").GetComponent<Button>().onClick.AddListener(EditDeviceItem);

//             newDeviceItemPreviewButtonGroup.Find("Group/Delete_Button").GetComponent<Button>().onClick.AddListener(() => DeleDeviceItem(newDeviceItem, DeviceCode));

//         }
//         Device_Item_Prefab.SetActive(false);
//     }
//     private void EditDeviceItem()
//     {
//         OpenUpdateCanvas();
//     }
//     private void DeleDeviceItem(GameObject DeviceItem, string DeviceCode)
//     {
//         OpenDeleteWarningPanel(DeviceItem, DeviceCode);
//     }

//     public void OpenAddNewCanvas()
//     {
//         Add_New_Device_Canvas.SetActive(true);
//         List_Device_Canvas.SetActive(false);
//         Update_Device_Canvas.SetActive(false);
//     }
//     private void OpenUpdateCanvas()
//     {
//         Update_Device_Canvas.SetActive(true);
//         List_Device_Canvas.SetActive(false);
//         Add_New_Device_Canvas.SetActive(false);
//     }
//     public void BackToListDevice()
//     {
//         List_Device_Canvas.SetActive(true);
//         Update_Device_Canvas.SetActive(false);
//         Add_New_Device_Canvas.SetActive(false);
//     }

//     private void OpenDeleteWarningPanel(GameObject DeviceItem, string DeviceCode)
//     {
//         DialogTwoButton.SetActive(true);
//         var Horizontal_Group = DialogTwoButton.transform.Find("Background/Horizontal_Group").gameObject.transform;
//         var dialog_Content = DialogTwoButton.transform.Find("Background/Dialog_Content").GetComponent<TMP_Text>().text =
//         $"Bạn có chắc chắn muốn thiết bị cảm biến <color=#FF0000><b>{DeviceCode}</b></color> khỏi hệ thống? Hãy kiểm tra kĩ trước khi nhấn nút xác nhận phía dưới";

//         var dialog_Title = DialogTwoButton.transform.Find("Background/Dialog_Title").GetComponent<TMP_Text>().text = "Xóa thiết bị cảm biến khỏi hệ thống?";

//         var confirmButton = Horizontal_Group.transform.Find("Confirm_Button").GetComponent<Button>();

//         var backButton = Horizontal_Group.transform.Find("Back_Button").GetComponent<Button>();

//         //var DeviceId = GlobalVariable.temp_ListDeviceInformationModelFromModule.Find(x => x.Code == DeviceCode).Id;

//         confirmButton.onClick.RemoveAllListeners();

//         backButton.onClick.RemoveAllListeners();

//         confirmButton.onClick.AddListener(() =>
//         {
//             Destroy(DeviceItem);

//             listDeviceItems.Remove(DeviceItem);

//             //OnSubmitDeleteDevice(DeviceId);

//             DialogTwoButton.SetActive(false);

//         });

//         backButton.onClick.AddListener(() =>
//         {
//             DialogTwoButton.SetActive(false);
//         });
//     }
//     // private async void OnSubmitDeleteDevice(int DeviceId)
//     // {
//     //     try
//     //     {
//     //         bool success = await _DeviceUseCase.DeleteDeviceModel(DeviceId);
//     //         if (success)
//     //         {
//     //             Debug.Log("Delete Device success");
//     //         }
//     //         else
//     //         {
//     //             Debug.Log("Delete Device failed");
//     //         }
//     //     }
//     //     catch (Exception ex)
//     //     {
//     //         Debug.LogError($"Error: {ex.Message}");
//     //     }
//     // }
// }
