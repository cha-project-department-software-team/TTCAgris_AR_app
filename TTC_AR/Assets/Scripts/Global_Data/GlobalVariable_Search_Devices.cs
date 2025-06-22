using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;

public class GlobalVariable_Search_Devices : MonoBehaviour
{
   public static DeviceInformationModel device;
   public static string jbName;
   public static string moduleName;
   public static List<DeviceInformationModel> all_Device_Models;

   //! Lưu ý chỉ lấy những thiết bị cần xem, không phải toàn bộ thiết bị kết nối đến 1 module (chỉ nằm trong 2 file Excel - các cảm biến )
   public static List<DeviceInformationModel> temp_ListDeviceInformationModel = new List<DeviceInformationModel>();
   public static Dictionary<string, JBInformationModel> temp_Dictionary_JBInformationModel = new Dictionary<string, JBInformationModel>();
   public static List<JBInformationModel> temp_ListJBInformationModel = new List<JBInformationModel>();
   public static List<string> temp_List_Device_For_Fitler = new List<string>(); // just Function and Code
   public static List<string> temp_List_JB_For_Fitler = new List<string>(); // just Function and Code


}