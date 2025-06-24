using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyUI.Progress;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginView : MonoBehaviour, ILoginView
{

    [Header("UI References")]
    [SerializeField] private TMP_InputField userNameField;
    [SerializeField] private TMP_InputField passwordField;
    [SerializeField] private Button loginButton;
    [SerializeField] private string targetSceneName;
    private string userName = "";
    private string password = "";
    private readonly Dictionary<string, string> staffAccounts = new()
    {
        {"ttc", "123456"},
        {"admin", "123456"},
        {"nhut", "123456"},
        {"my", "123456"}
    };
    public LoginPresenter _presenter;

    void Awake()
    {
        StopAllCoroutines();

        StartCoroutine(Init());

        Screen.orientation = ScreenOrientation.Portrait;

        PopulateFieldsIfLoggedIn();
    }
    private IEnumerator Init()
    {
        yield return
            new WaitUntil(() => ManagerLocator.Instance.CompanyManager != null &&
            ManagerLocator.Instance.GrapperManager != null);
        _presenter = new LoginPresenter(this,
            ManagerLocator.Instance.CompanyManager._ICompanyService,
            ManagerLocator.Instance.GrapperManager._IGrapperService
            , ManagerLocator.Instance.JBManager._IJBService,
             ManagerLocator.Instance.ImageManager._IImageService
            , ManagerLocator.Instance.ModuleManager._IModuleService,
            ManagerLocator.Instance.MccManager._IMccService,
            ManagerLocator.Instance.FieldDeviceManager._IFieldDeviceService,
            ManagerLocator.Instance.DeviceManager._IDeviceService,
            ManagerLocator.Instance.ModuleSpecificationManager._IModuleSpecificationService,
            ManagerLocator.Instance.AdapterSpecificationManager._IAdapterSpecificationService,
            ManagerLocator.Instance.RackManager._IRackService
             );
    }

    private void OnEnable()
    {
        GlobalVariable.APIRequestType.Clear();
        loginButton.onClick.RemoveAllListeners();
    }

    private void Start()
    {
        loginButton.onClick.RemoveAllListeners();
        loginButton.onClick.AddListener(HandleLogin);
    }

    private void Update()
    {
        HandleExitInput();
    }

    private void PopulateFieldsIfLoggedIn()
    {
        if (GlobalVariable.loginSuccess &&
            !string.IsNullOrWhiteSpace(GlobalVariable.accountModel.userName) &&
            !string.IsNullOrWhiteSpace(GlobalVariable.accountModel.password))
        {
            userNameField.text = GlobalVariable.accountModel.userName;
            passwordField.text = GlobalVariable.accountModel.password;
        }
    }

    private void HandleExitInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape) ||
            Gamepad.current?.buttonEast?.wasPressedThisFrame == true ||
            Keyboard.current?.escapeKey.wasPressedThisFrame == true)
        {
            Application.Quit();
        }
    }

    private void HandleLogin()
    {
        userName = userNameField.text.Trim();
        password = passwordField.text.Trim();

        if (staffAccounts.TryGetValue(userName.ToLower(), out string correctPassword) &&
            password == correctPassword)
        {
            LoginProcess();
        }
        else
        {
            StartCoroutine(ShowLoginFailure("Tên đăng nhập hoặc mật khẩu không chính xác!"));
        }
    }


    private void LoginProcess()
    {
        _presenter.LoginProcess();
    }

    private void ShowProgressBar(string title, string details)
    {
        Progress.Show(title, ProgressColor.Blue, true);
        Progress.SetDetailsText(details);
    }


    public void ShowLoading(string title) => ShowProgressBar(title, "Đang đăng nhập...");
    public void HideLoading() => StartCoroutine(HideProgressBar());
    public void ShowError(string message)
    {
        StartCoroutine(ShowLoginFailure(message));
    }
    public void ShowSuccess()
    {
        StartCoroutine(NavigateToScene(targetSceneName));
    }
    public IEnumerator NavigateToScene(string sceneName)
    {
        SetLoginSuccessData();
        yield return new WaitForSeconds(0.5f);
        // HideProgressBar();
        yield return SceneManager.LoadSceneAsync(sceneName);
    }


    private IEnumerator ShowLoginFailure(string message)
    {
        Show_Toast.Instance.ShowToast("failure", message);
        GlobalVariable.loginSuccess = false;
        yield return new WaitForSeconds(0.25f);
        yield return Show_Toast.Instance.Set_Instance_Status_False();
        // HideProgressBar();
    }
    private void SetLoginSuccessData()
    {
        GlobalVariable.accountModel.userName = userName;
        GlobalVariable.accountModel.password = password;
        GlobalVariable.ready_To_Nav_New_Scene = true;
        GlobalVariable.recentScene = targetSceneName;
        GlobalVariable.previousScene = MyEnum.LoginScene.GetDescription();
        GlobalVariable.loginSuccess = true;
    }

    private IEnumerator HideProgressBar()
    {
        yield return new WaitForSeconds(0.5f);
        Progress.Hide();
    }


}