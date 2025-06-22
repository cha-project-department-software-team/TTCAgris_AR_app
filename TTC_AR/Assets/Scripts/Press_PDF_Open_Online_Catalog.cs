using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Press_PDF_Open_Online_Catalog : MonoBehaviour
{
    public List<Button> listButton = new List<Button>();
    public List<string> urls = new List<string>();
    public bool open_module_adapter_Catalog = false;
    private string adapterSpecificationCode;
    private string moduleSpecificationCode;
    public Dictionary<string, string> online_Adapter_Catalog_Url = new Dictionary<string, string>();
    public Dictionary<string, string> online_Module_Catalog_Url = new Dictionary<string, string>();

    void OnEnable()
    {
        online_Adapter_Catalog_Url.Clear();
        online_Module_Catalog_Url.Clear();
        // adapterSpecificationCode = GlobalVariable.temp_AdapterSpecificationModel.Code;
        // moduleSpecificationCode = GlobalVariable.temp_ModuleSpecificationModel.Code;

        // if (open_module_adapter_Catalog)
        // {
        //     listButton.Add(GameObject.Find("Module_Button_Detail_Datasheet_View").GetComponent<Button>());
        //     listButton.Add(GameObject.Find("Adapter_Button_Detail_Datasheet_View").GetComponent<Button>());
        //     listButton[0].onClick.AddListener(() => Open_Module_Adapter_Online_Catalog(true, false));
        //     listButton[1].onClick.AddListener(() => Open_Module_Adapter_Online_Catalog(false, true));
        // }
        // else
        // {
        //     foreach (var button in listButton)
        //     {
        //         int index = listButton.IndexOf(button);
        //         button.onClick.AddListener(() => Open_url(index));
        //     }
        // }
    }

    private void Open_Module_Adapter_Online_Catalog(bool open_module_Catalog, bool open_adapter_Catalog)
    {
        if (open_module_Catalog && !open_adapter_Catalog)
        {
            Application.OpenURL(online_Module_Catalog_Url[moduleSpecificationCode]);
        }
        else if (!open_module_Catalog && open_adapter_Catalog)
        {
            Application.OpenURL(online_Adapter_Catalog_Url[adapterSpecificationCode]);
        }
    }
    public void Open_url(string code)
    {
        if (online_Adapter_Catalog_Url.TryGetValue(code, out string adapterUrl))
        {
            Application.OpenURL(adapterUrl);

        }
        if (online_Module_Catalog_Url.TryGetValue(code, out string moduleUrl))
        {
            Application.OpenURL(moduleUrl);
        }

    }

    void OnDisable()
    {
        listButton.RemoveAll(button => button != null);
    }
    void Start()
    {

    }
}
