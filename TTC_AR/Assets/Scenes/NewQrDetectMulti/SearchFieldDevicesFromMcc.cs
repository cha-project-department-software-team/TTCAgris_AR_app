using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;
using UnityEngine.UI;

public class SearchFieldDevicesFromMcc : MonoBehaviour
{
    [SerializeField] private List<TMP_Text> mccInformation = new List<TMP_Text>();
    [SerializeField] private GameObject nav_Field_Device_Button_Prefab;
    [SerializeField] private Transform parentTransform;
    [SerializeField] private GameObject generalPanel;
    // [SerializeField] private GameObject gameObject_Empty;
    [SerializeField] private ScrollRect scroll_Area;
    private Dictionary<string, GameObject> dic_mccInformationModel_Button = new Dictionary<string, GameObject>();
    private List<FieldDeviceInformationModel> listFieldDeviceFromMcc;


    private void OnEnable()
    {
        StartCoroutine(UpdateUI());
        nav_Field_Device_Button_Prefab.SetActive(true);
        scroll_Area.verticalNormalizedPosition = 0f;
    }

    private IEnumerator UpdateUI()
    {
        DestroyAllInstancesExceptPrefab(dic_mccInformationModel_Button.Values.ToList());

        yield return new WaitUntil(() => gameObject.activeSelf && StaticVariable.ready_To_Update_MCC_UI);

        if (StaticVariable.temp_MccInformationModel == null)
        {
            Debug.Log("MccInformationModel is null");
            yield break;
        }
        else if (StaticVariable.temp_MccInformationModel.Brand == null)
        {
            Debug.Log("Brand is null");
            yield break;
        }

        mccInformation[0].text = StaticVariable.temp_MccInformationModel.Brand;
        mccInformation[1].text = StaticVariable.temp_MccInformationModel.Note;

        listFieldDeviceFromMcc = StaticVariable.temp_ListFieldDeviceModelFromMCC;

        if (listFieldDeviceFromMcc.Any())
        {
            foreach (var fieldDevice in listFieldDeviceFromMcc)
            {
                GameObject nav_Field_Device_Button = Instantiate(nav_Field_Device_Button_Prefab, parentTransform);
                dic_mccInformationModel_Button.Add(fieldDevice.Name, nav_Field_Device_Button);

                var fieldDeviceInfor = nav_Field_Device_Button.GetComponent<FieldDeviceInfor>();
                fieldDeviceInfor.SetFieldDeviceName(fieldDevice.Name);
                fieldDeviceInfor.button.onClick.AddListener(() => NavigateGeneralPanel(fieldDevice));
            }
            nav_Field_Device_Button_Prefab.SetActive(false);
            // gameObject_Empty.transform.SetAsLastSibling();
            scroll_Area.verticalNormalizedPosition = 1f;
        }
        // else
        // {
        //     Debug.Log("No field device found");
        // }
    }

    private void OnDisable()
    {
        DestroyAllInstancesExceptPrefab(dic_mccInformationModel_Button.Values.ToList());
        dic_mccInformationModel_Button.Clear();
    }

    private void DestroyAllInstancesExceptPrefab(List<GameObject> listInstances)
    {
        foreach (var instance in listInstances)
        {
            Destroy(instance);
        }
    }

    public async void NavigateGeneralPanel(FieldDeviceInformationModel model)
    {
        await GetFieldDeviceInformation.Instance.GetFieldDevice(model);
        // StaticVariable.temp_FieldDeviceInformationModel = model;

        gameObject.SetActive(false);
        generalPanel.SetActive(true);
    }
}
