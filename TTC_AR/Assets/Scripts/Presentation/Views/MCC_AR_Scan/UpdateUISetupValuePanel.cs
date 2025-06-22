using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class UpdateUISetupValuePanel : MonoBehaviour
{
    [SerializeField] private List<TMP_Text> setup_Value_Texts;
    [SerializeField] private MccInformationModel MCCInformationModel;
    [SerializeField] private FieldDeviceInformationModel FieldDeviceInformationModel;
    [SerializeField] private GameObject generalPanel;
    [SerializeField] private GameObject setupValuePanel;
    [SerializeField] private Button closeButton;


    private void OnEnable()
    {
        MCCInformationModel = GlobalVariable.temp_MCCInformationModel;
        FieldDeviceInformationModel = GlobalVariable.temp_FieldDeviceInformationModel;
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(() => CloseSetupValuePanel());
        Initialize();
    }
    private void CloseSetupValuePanel()
    {
        generalPanel.SetActive(true);
        setupValuePanel.SetActive(false);
    }
    private void Initialize()
    {
        StartCoroutine(Update_UI());
    }
    private IEnumerator Update_UI()
    {
        yield return null; //

        string[] values = {
            FieldDeviceInformationModel.Name,
            MCCInformationModel.CabinetCode,
            MCCInformationModel.Brand,
            FieldDeviceInformationModel.RatedPower,
            FieldDeviceInformationModel.RatedCurrent,
            FieldDeviceInformationModel.ActiveCurrent,
            FieldDeviceInformationModel.Note,
        };

        int count = Mathf.Min(setup_Value_Texts.Count, values.Length);
        for (int i = 0; i < count; i++)
        {
            TMP_Text textElement = setup_Value_Texts[i];
            string value = values[i];

            if (string.IsNullOrEmpty(value) || value == "Chưa cập nhật")
            {
                textElement.text = "Chưa cập nhật";
                textElement.color = Color.red;
                textElement.fontStyle = FontStyles.Bold;
            }
            else
            {
                textElement.text = value;
                textElement.color = Color.black;
                textElement.fontStyle = FontStyles.Normal;
            }
            yield return null; // Yield to avoid blocking the main thread
        }
    }

    private void OnDisable()
    {
        closeButton.onClick.RemoveAllListeners();
        StopAllCoroutines();
    }
}