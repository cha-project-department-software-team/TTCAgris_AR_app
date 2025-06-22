using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EasyUI.Progress;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateUIListJBPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private Button closeButton;
    [SerializeField] private GameObject generalPanel;
    [SerializeField] private GameObject listJBPanel;
    [SerializeField] private GameObject JBDetailPanel;
    [SerializeField] private Transform content;
    [SerializeField] private GameObject jbPrefab;
    [SerializeField] private List<JBInformationModel> jbInformationList = new List<JBInformationModel>();
    [SerializeField] private List<GameObject> instantiatedJBObjects = new List<GameObject>();
    private Dictionary<string, JBInformationModel> jbInformationDictionary = new Dictionary<string, JBInformationModel>();
    void Awake()
    {

    }

    private void OnEnable()
    {
        if (GlobalVariable.temp_Dictionary_JBInformationModel.Any())
        {
            jbInformationDictionary = GlobalVariable.temp_Dictionary_JBInformationModel;
        }
        else
        {
            jbInformationDictionary = new Dictionary<string, JBInformationModel>();
        }
        if (GlobalVariable.temp_ListJBInformationModel_FromModule != null)
        {
            jbInformationList = GlobalVariable.temp_ListJBInformationModel_FromModule;
        }
        RefreshJBListPanel();
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(() => CloseListJBPanel());
        StartCoroutine(UpdateListPanelUI());
    }
    private void RefreshJBListPanel()
    {
        foreach (Transform child in content)
        {
            if (child.gameObject != jbPrefab)
            {
                Destroy(child.gameObject);
            }
        }
    }
    private void CloseListJBPanel()
    {
        generalPanel.SetActive(true);
        listJBPanel.SetActive(false);
    }

    private void OnDisable()
    {
        closeButton.onClick.RemoveAllListeners();
        instantiatedJBObjects.Clear();
        StopAllCoroutines();
    }
    private IEnumerator UpdateListPanelUI()
    {
        instantiatedJBObjects.Clear();
        yield return null;
        jbPrefab.SetActive(true);
        if (jbInformationList.Any())
        {
            Debug.Log("jbInformationList: " + jbInformationList.Count);
            Debug.Log("jbInformationDictionary: " + jbInformationDictionary.Count);

            foreach (var model in jbInformationList)
            {
                var newJBItem = Instantiate(jbPrefab, content);
                newJBItem.SetActive(true);
                var jbInfor = newJBItem.GetComponent<JBInfor>();
                jbInfor.value.text = model.Name;
                if (jbInformationDictionary.TryGetValue(model.Name, out JBInformationModel temp_model))
                {

                    jbInfor.Location.text = temp_model.Location;
                }
                else
                {
                    jbInfor.Location.text = "Được ghi chú trong sơ đồ";
                }
                //  jbInfor.Location.text = model.Location;
                jbInfor.button.onClick.RemoveAllListeners();
                jbInfor.button.onClick.AddListener(() =>
                {
                    GlobalVariable.navigate_from_List_Devices = false;
                    GlobalVariable.navigate_from_list_JBs = true;
                    OpenJBDetailPanel(model);
                });
                instantiatedJBObjects.Add(newJBItem);
            }
        }
        else
        {
            var newJBItem = Instantiate(jbPrefab, content);
            instantiatedJBObjects.Add(newJBItem);
            newJBItem.SetActive(true);
            newJBItem.GetComponent<JBInfor>().HandleEmptyList();
            var Horizontal_JB_TSD = newJBItem.GetComponent<HorizontalLayoutGroup>();
            Horizontal_JB_TSD.childAlignment = TextAnchor.MiddleCenter;
        }
        jbPrefab.SetActive(false);
    }
    private void OpenJBDetailPanel(JBInformationModel model)
    {
        GlobalVariable.JBId = model.Id;
        GlobalVariable.navigate_from_list_JBs = true;
        GlobalVariable.navigate_from_List_Devices = false;
        GlobalVariable.navigate_from_JB_TSD_Detail = true;
        listJBPanel.SetActive(false);
        JBDetailPanel.SetActive(true);
    }




}
