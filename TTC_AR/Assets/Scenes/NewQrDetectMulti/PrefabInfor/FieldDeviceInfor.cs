using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FieldDeviceInfor : MonoBehaviour
{
    public TMP_Text fieldDeviceName;
    public Button button;

    public void SetFieldDeviceName(string name)
    {
        fieldDeviceName.text = name;
    }
    
}
