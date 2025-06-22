using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NavigationToPanels : MonoBehaviour
{
    [SerializeField] private GameObject initialScreen;
    [SerializeField] private TMP_Text generalModuleTitle;
    [SerializeField] private GameObject[] destinationScreens;
    [SerializeField] private List<Button> navButtons;
    // [SerializeField] private bool isFieldDevice = false;

    private Canvas parentCanvas;

    private void OnEnable()
    {
        // if (parentCanvas == null) // Chỉ tìm parentCanvas nếu chưa gán
        //     parentCanvas = GetComponentInParent<Canvas>();

        // if (initialScreen == null) // Gán initialScreen nếu chưa gán
        //     initialScreen = parentCanvas.transform.Find("Basic_Panel")?.gameObject;

        // if (generalModuleTitle == null && initialScreen != null) // Gán tiêu đề nếu chưa gán
        //     generalModuleTitle = initialScreen.transform.Find("Title")?.GetComponent<TMP_Text>();

        // StaticVariable.generalPanel = initialScreen;

        // if (generalModuleTitle != null)
        //     generalModuleTitle.text = GetModuleTitle(parentCanvas.gameObject.name);

        SetInitialState();

        for (int i = 0; i < destinationScreens.Length; i++)
        {
            int localIndex = i; // Bản sao cục bộ để tránh lỗi closure
            navButtons[i].onClick.AddListener(() => NavigateNewScreen(localIndex));
        }
    }

    private void OnDisable()
    {
        foreach (var button in navButtons)
        {
            button.onClick.RemoveAllListeners();
        }

        // StaticVariable.generalPanel = null;

    }

    private void Start()
    {

    }

    private void SetInitialState()
    {
        // if (initialScreen != null)
        //     initialScreen.SetActive(true);

        foreach (var screen in destinationScreens)
        {
            if (screen.activeSelf) // Chỉ thay đổi trạng thái khi cần thiết
                screen.SetActive(false);
        }
    }

    // private string GetModuleTitle(string fullName)
    // {
    //     if (isFieldDevice)
    //     {
    //         return generalModuleTitle.text;
    //     }
    //     else
    //     {
    //         return $"Module {fullName.Split('_')[0]}";
    //     }
    // }

    public void NavigateNewScreen(int index)
    {
        // yield return new WaitUntil(() => StaticVariable.ready_To_Update_UI);

        if (initialScreen != null)
            initialScreen.SetActive(false);

        for (int i = 0; i < destinationScreens.Length; i++)
        {
            if (destinationScreens[i].activeSelf != (i == index)) // Chỉ thay đổi trạng thái nếu cần
                destinationScreens[i].SetActive(i == index);
        }
    }

    public void NavigatePop()
    {
        StartCoroutine(WaitForASecond());
    }

    private IEnumerator WaitForASecond()
    {
        foreach (var screen in destinationScreens)
        {
            if (screen.activeSelf)
                screen.SetActive(false);
        }

        yield return new WaitForSeconds(0.2f);

        if (initialScreen != null)
            initialScreen.SetActive(true);
    }
}
