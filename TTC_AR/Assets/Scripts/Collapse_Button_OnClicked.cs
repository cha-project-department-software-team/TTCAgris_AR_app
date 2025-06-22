using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Collapse_Button_OnClicked : MonoBehaviour
{
    public List<RectTransform> optionGameObjects; // Main buttons
    public List<GameObject> collapseButtons;  // Arrow icons
    public List<GameObject> functionButtons; // Setting icons
    private List<bool> collapsedStatus; // Button expansion status
    private List<RectTransform> arrowIcons; // Arrow icons
    private List<Vector3> arrowInitialEulerAngles;  // Initial arrow positions

    void Start()
    {
        int count = optionGameObjects.Count;

        // Initialize lists with capacity to avoid resizing
        collapsedStatus = new List<bool>(new bool[count]);
        arrowIcons = new List<RectTransform>(count);
        arrowInitialEulerAngles = new List<Vector3>(count);

        for (int i = 0; i < count; i++)
        {
            // Cache arrow icon RectTransform
            RectTransform arrowIcon = collapseButtons[i].transform.Find("Arrow_Icon").GetComponent<RectTransform>();
            arrowIcons.Add(arrowIcon);
            arrowInitialEulerAngles.Add(arrowIcon.localEulerAngles);

            // Cache buttons and add listeners
            Button collapseButton = collapseButtons[i].GetComponent<Button>();
            Button functionButton = functionButtons[i].GetComponent<Button>();

            int index = i; // Avoid closure issue
            collapseButton.onClick.AddListener(() => CollapseBtnOnClicked(index));
            functionButton.onClick.AddListener(() => FunctionBtnOnClicked(index));
        }
    }

    void OnDestroy()
    {
        // Remove all listeners to avoid memory leaks
        foreach (var collapseButton in collapseButtons)
        {
            collapseButton.GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }

    public void CollapseBtnOnClicked(int buttonIndex)
    {
        bool isCollapsed = !collapsedStatus[buttonIndex];
        collapsedStatus[buttonIndex] = isCollapsed;

        // Update arrow rotation and function button visibility
        arrowIcons[buttonIndex].localEulerAngles = new Vector3(0, 0, isCollapsed ? 180 : 0);
        functionButtons[buttonIndex].SetActive(isCollapsed);

        Debug.Log($"Button is collapsed: {buttonIndex} : {isCollapsed}");
    }

    public void FunctionBtnOnClicked(int buttonIndex)
    {
        Debug.Log($"Function button is clicked: {buttonIndex}");
    }
}
