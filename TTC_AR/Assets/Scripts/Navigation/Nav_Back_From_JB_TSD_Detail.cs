using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


public class Nav_Back_From_JB_TSD_Detail : MonoBehaviour
{

    [SerializeField]
    public Canvas main_Canvas;
    public GameObject list_Devices;
    public GameObject jb_TSD_General;
    public GameObject jb_TSD_Detail;
    void Start()
    {
        main_Canvas ??= GetComponentInParent<Canvas>();
        list_Devices ??= main_Canvas.gameObject.transform.Find("List_Devices").gameObject;
        jb_TSD_General ??= main_Canvas.gameObject.transform.Find("JB_TSD_General_Panel").gameObject;
        jb_TSD_Detail ??= main_Canvas.gameObject.transform.Find("Detail_JB_TSD").gameObject;
    }

    void Update()
    {
    }

    public void NavigatePop()
    {
        if (GlobalVariable.navigate_from_list_JBs)
        {
            jb_TSD_Detail.SetActive(false);
            jb_TSD_General.SetActive(true);
            GlobalVariable.navigate_from_list_JBs = false;
        }
        if (GlobalVariable.navigate_from_List_Devices)
        {
            jb_TSD_Detail.SetActive(false);
            list_Devices.SetActive(true);
            GlobalVariable.navigate_from_List_Devices = false;
        }

    }
}
