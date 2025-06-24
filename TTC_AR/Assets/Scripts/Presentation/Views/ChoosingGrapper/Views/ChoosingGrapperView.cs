using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyUI.Progress;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChoosingGrapperView : MonoBehaviour, IChoosingGrapper
{

    [Header("UI References")]

    private ChoosingGrapperPresenter _presenter;
    public List<Button> grapperButtons;
    private List<GrapperInformationModel> grapperInformationModels;
    private void Awake()
    {
        _presenter = new ChoosingGrapperPresenter(this, ManagerLocator.Instance.GrapperManager._IGrapperService);
    }
    private void OnEnable()
    {
        _presenter.GetGrappersInfor();
    }
    private void Start()
    {

    }
    public void Display()
    {
        if (!GlobalVariable.temp_ListGrapperInformationModel.Any())
        {
            grapperInformationModels = new List<GrapperInformationModel>();
        }
        else
        {
            grapperInformationModels = GlobalVariable.temp_ListGrapperInformationModel;
        }

        for (int i = 0; i < grapperButtons.Count; i++)
        {
            int index = i;
            if (
                !grapperInformationModels[index].List_RackBasicModel.Any() &&
                !grapperInformationModels[index].ListDeviceInformationModel.Any() &&
                !grapperInformationModels[index].ListJBInformationModel.Any() &&
                !grapperInformationModels[index].ListMccInformationModel.Any() &&
                !grapperInformationModels[index].ListFieldDeviceInformationModel.Any()
                )
            {
                grapperButtons[index].interactable = false;
            }
            else
            {
                grapperButtons[index].interactable = true;
            }
        }
    }


    private void ShowProgressBar(string title, string details)
    {
        Progress.Show(title, ProgressColor.Blue, true);
        Progress.SetDetailsText(details);
    }


    public void ShowLoading(string title) => ShowProgressBar(title, "Đang tải dữ liệu...");
    public void HideLoading() => StartCoroutine(HideProgressBar());
    public void ShowError(string message)
    {
        Show_Toast.Instance.ShowToast("failure", message);
        StartCoroutine(Show_Toast.Instance.Set_Instance_Status_False());
    }
    public void ShowSuccess()
    {
        Show_Toast.Instance.ShowToast("success", "Tải dữ liệu thành công!");
        StartCoroutine(Show_Toast.Instance.Set_Instance_Status_False());
    }

    private IEnumerator HideProgressBar()
    {
        yield return new WaitForSeconds(0.5f);
        Progress.Hide();
    }


}