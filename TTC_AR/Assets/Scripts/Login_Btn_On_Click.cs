// // using System.Collections;
// // using System.Collections.Generic;
// // using UnityEngine;
// // using UnityEngine.InputSystem;
// // using UnityEngine.SceneManagement;
// // using UnityEngine.UI;
// // using TMPro;
// // using System.Threading.Tasks;
// // using UnityEngine.XR;
// // using System.Runtime.InteropServices;


// // public class Login_Btn_On_Click : MonoBehaviour
// // {
// //     [SerializeField] private TMP_InputField userNameField;
// //     [SerializeField] private TMP_InputField passwordField;
// //     [SerializeField] private Button loginButton;
// //     [SerializeField] private string targetSceneName;


// //     private string userName = "";
// //     private string password = "";

// //     private readonly Dictionary<string, string> staffAccounts = new Dictionary<string, string>
// //     {
// //         {"ttc", "123456"},
// //         {"admin", "123456"},
// //         {"nhut", "123456"},
// //         {"my", "123456"}
// //     };

// //     private void Awake()
// //     {
// //         SetScreenOrientation();
// //         PopulateFieldsIfLoggedIn();


// //     }
// //     private void Start()
// //     {
// //         loginButton.onClick.AddListener(HandleLogin);
// //     }
// //     private void Update()
// //     {
// //         CheckForExitInput();
// //     }

// //     private void SetScreenOrientation()
// //     {
// //         if (Screen.orientation != ScreenOrientation.Portrait)
// //         {
// //             Screen.orientation = ScreenOrientation.Portrait;
// //         }
// //     }

// //     private void PopulateFieldsIfLoggedIn()
// //     {
// //         if (GlobalVariable.loginSuccess && !string.IsNullOrWhiteSpace(GlobalVariable.accountModel.userName))
// //         {
// //             userNameField.text = GlobalVariable.accountModel.userName;
// //             passwordField.text = GlobalVariable.accountModel.password;
// //         }
// //     }


// //     private void CheckForExitInput()
// //     {
// //         if (Input.GetKeyDown(KeyCode.Escape) ||
// //             (Gamepad.current != null && Gamepad.current.buttonEast != null && Gamepad.current.buttonEast.wasPressedThisFrame) ||
// //             (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame))
// //         {

// //             Application.Quit();
// //         }
// //     }



// //     private void HandleLogin()
// //     {
// //         // FieldDeviceManager fieldDeviceManager = FindObjectOfType<FieldDeviceManager>();
// //         // fieldDeviceManager.GetFieldDeviceById(1);
// //         // CompanyManager CompanyManager = FindObjectOfType<CompanyManager>();
// //         // GlobalVariable.APIRequestType = "GET_JB";



// //         // jBManager.CreateNewJB(
// //         //     1,
// //         //     new JBRequestDto(
// //         //         name: "JBName49",
// //         //         location: "",
// //         //         deviceBasicDtos: null,
// //         //         moduleBasicDtos: null,
// //         //         outdoorImageBasicDto: null,
// //         //         connectionImageBasicDtos: null
// //         //     )
// //         // );
// //         // jBManager.GetJBList(1);
// //         // jBManager.CreateNewJB(
// //         //     1,
// //         //     new JBRequestDto(
// //         //         name: "JBName49",
// //         //         location: null,
// //         //         deviceBasicDtos: null,
// //         //         moduleBasicDtos: null,
// //         //         outdoorImageBasicDto: null,
// //         //         connectionImageBasicDtos: null
// //         //     )
// //         // );



// //         //!  DeviceManager DeviceManager = FindObjectOfType<DeviceManager>();
// //         //?DeviceManager.GetDeviceById(1);
// //         //?DeviceManager.GetDeviceList(1);
// //         // DeviceManager.DeleteDevice(1);
// //         // DeviceManager.UpdateDevice(

// //         //     new DeviceRequestDto(
// //         //         code: "DeviceCode",
// //         //         function: "DeviceFunction",
// //         //         range: "hoho",
// //         //         unit: "hello",
// //         //         ioAddress: "alibabababa",
// //         //         moduleBasicDto: null,
// //         //         jbBasicDto: null,
// //         //         additionalImageBasicDtos: null
// //         //     )
// //         // );


// //         //! ModuleSpecificationManager ModuleSpecificationManager = FindObjectOfType<ModuleSpecificationManager>();
// //         // ModuleSpecificationManager.DeleteModuleSpecification(2);
// //         //ModuleSpecificationManager.GetModuleSpecificationById(2);
// //         //ModuleSpecificationManager.GetModuleSpecificationList(1);
// //         //    ModuleSpecificationManager.UpdateModuleSpecification(
// //         //              new ModuleSpecificationRequestDto(
// //         //                   code: "Hello world",
// //         //                   type: "hello",
// //         //                   numOfIO: "kitty",
// //         //                   signalType: "ModuleSpecificationSignsdadsaasalType",
// //         //                   compatibleTBUs: "ModuleSpecificationdsadasdasCompatibleTBUs",
// //         //                   operatingVoltage: "ModuleSpecificationsdadsadasdOperatingVoltage",
// //         //                   operatingCurrent: "ModuleSpecificatiodsadsanOperatingCurrent",
// //         //                   flexbusCurrent: "ModuleSpecificationFlexsdadsadbusCurrent",
// //         //                    alarm: "",
// //         //                   note: "",
// //         //                   pdfManual: ""
// //         //              )
// //         //          );
// //         //! AdapterSpecificationManager AdapterSpecificationManager = FindObjectOfType<AdapterSpecificationManager>();
// //         //? AdapterSpecificationManager.GetAdapterSpecificationList(1);
// //         //?AdapterSpecificationManager.GetAdapterSpecificationById("12345");
// //         //? AdapterSpecificationManager.UpdateAdapterSpecification(
// //         //     new AdapterSpecificationRequestDto(
// //         //         code: "adaadadada",
// //         //         type: "Hello World",
// //         //         communication: "AdapterSpecificationCommunication",
// //         //         numOfModulesAllowed: "AdapterSpecificationNumOfModulesAllowed",
// //         //         commSpeed: "",
// //         //         inputSupply: "AdapterSpecificationInputSupply",
// //         //         outputSupply: "AdapterSpecificationOutputSupply",
// //         //         inrushCurrent: "AdapterSpecificationInrushCurrent",
// //         //         alarm: "AdapterSpecificationAlarm",
// //         //         note: "",
// //         //         pdfManual: "AdapterSpecificationPdfManual"
// //         //     )
// //         // );
// //         //? AdapterSpecificationManager.CreateNewAdapterSpecification(
// //         //     1,
// //         //     new AdapterSpecificationRequestDto(
// //         //     code: "AdapterSpecificationName",
// //         //     type: "AdapterSpecificationType",
// //         //     communication: "AdapterSpecificationCommunication",
// //         //     numOfModulesAllowed: "AdapterSpecificationNumOfModulesAllowed",
// //         //     commSpeed: "AdapterSpecificationCommSpeed",
// //         //     inputSupply: "AdapterSpecificationInputSupply",
// //         //     outputSupply: "AdapterSpecificationOutputSupply",
// //         //     inrushCurrent: "AdapterSpecificationInrushCurrent",
// //         //     alarm: "AdapterSpecificationAlarm",
// //         //     note: "AdapterSpecificationNote",
// //         //     pdfManual: "AdapterSpecificationPdfManual"
// //         //     )
// //         // );


// //         // FieldDeviceManager FieldDeviceManager = FindObjectOfType<FieldDeviceManager>();
// //         // FieldDeviceManager._IFieldDeviceService.GetListFieldDeviceAsync("1");
// //         GlobalVariable.APIRequestType.Clear();
// //         GlobalVariable.APIRequestType.Add("GET_JB_List_General");
// //         GlobalVariable.APIRequestType.Add("GET_Device_List_General");
// //         GlobalVariable.APIRequestType.Add("GET_Module_List");


// //         JBManager jBManager = FindObjectOfType<JBManager>();
// //         ImageManager imageManager = FindObjectOfType<ImageManager>();
// //         ModuleManager moduleManager = FindObjectOfType<ModuleManager>();
// //         DeviceManager deviceManager = FindObjectOfType<DeviceManager>();

// //         jBManager.GetListJBGeneral("1");
// //         imageManager._IImageService.GetListImageAsync("1");
// //         moduleManager._IModuleService.GetListModuleAsync("1");
// //         deviceManager._IDeviceService.GetListDeviceGeneralAsync("1");


// //         GlobalVariable.APIRequestType.Clear();


// //         userName = userNameField.text;
// //         password = passwordField.text;

// //         if (staffAccounts.TryGetValue(userName.ToLower(), out string foundPassword) && foundPassword == password)
// //         {
// //             StartCoroutine(HandleLoginSuccess());
// //         }
// //         else
// //         {
// //             StartCoroutine(HandleLoginFailure());
// //         }
// //     }

// //     private IEnumerator HandleLoginSuccess()
// //     {
// //         UpdateGlobalVariables();
// //         yield return LoadSceneAsync(targetSceneName);
// //     }

// //     private void UpdateGlobalVariables()
// //     {
// //         GlobalVariable.accountModel.userName = userName;
// //         GlobalVariable.accountModel.password = password;
// //         GlobalVariable.recentScene = targetSceneName;
// //         GlobalVariable.previousScene = MyEnum.LoginScene.GetDescription();
// //         GlobalVariable.loginSuccess = true;
// //     }

// //     private IEnumerator HandleLoginFailure()
// //     {
// //         
// //         Show_Toast.Instance.ShowToast("failure", "Sai tên đăng nhập hoặc mật khẩu!");
// //         yield return new WaitForSeconds(0.5f);
// //         Debug.Log("Sai tên đăng nhập hoặc mật khẩu!");
// //         GlobalVariable.loginSuccess = false;
// //         yield return Show_Toast.Instance.Set_Instance_Status_False();
// //     }

// //     private IEnumerator LoadSceneAsync(string sceneName)
// //     {
// //         //  
// //         Show_Toast.Instance.ShowToast("loading", "Đang đăng nhập...");

// //         GlobalVariable.ready_To_Nav_New_Scene = true;

// //         yield return new WaitUntil(() =>
// //             GlobalVariable.temp_ListDeviceInformationModel.Count > 0 &&
// //             GlobalVariable.temp_ListImageInformationModel.Count > 0 &&
// //             GlobalVariable.temp_ListModuleInformationModel.Count > 0
// //         );

// //         yield return Show_Toast.Instance.Set_Instance_Status_False();

// //         SceneManager.LoadSceneAsync(sceneName);
// //     }
// // }

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.InputSystem;
// using UnityEngine.SceneManagement;
// using UnityEngine.UI;
// using TMPro;
// using EasyUI.Progress;
// using System.Threading.Tasks;

// public class Login_Btn_On_Click : MonoBehaviour
// {
//     [Header("UI References")]
//     [SerializeField] private TMP_InputField userNameField;
//     [SerializeField] private TMP_InputField passwordField;
//     [SerializeField] private Button loginButton;
//     [SerializeField] private string targetSceneName;

//     private string userName = "";
//     private string password = "";

//     private readonly Dictionary<string, string> staffAccounts = new()
//     {
//         {"ttc", "123456"},
//         {"admin", "123456"},
//         {"nhut", "123456"},
//         {"my", "123456"}
//     };

//     private void Awake()
//     {
//         Screen.orientation = ScreenOrientation.Portrait;
//         PopulateFieldsIfLoggedIn();
//     }

//     private void OnEnable()
//     {
//         GlobalVariable.APIRequestType.Clear();
//         StopAllCoroutines();
//         loginButton.onClick.RemoveAllListeners();
//     }

//     private void Start()
//     {
//         loginButton.onClick.AddListener(HandleLogin);
//     }

//     private void Update()
//     {
//         HandleExitInput();
//     }

//     private void PopulateFieldsIfLoggedIn()
//     {
//         if (GlobalVariable.loginSuccess &&
//             !string.IsNullOrWhiteSpace(GlobalVariable.accountModel.userName) &&
//             !string.IsNullOrWhiteSpace(GlobalVariable.accountModel.password))
//         {
//             userNameField.text = GlobalVariable.accountModel.userName;
//             passwordField.text = GlobalVariable.accountModel.password;
//         }
//     }

//     private void HandleExitInput()
//     {
//         if (Input.GetKeyDown(KeyCode.Escape) ||
//             Gamepad.current?.buttonEast?.wasPressedThisFrame == true ||
//             Keyboard.current?.escapeKey.wasPressedThisFrame == true)
//         {
//             Application.Quit();
//         }
//     }

//     private void HandleLogin()
//     {
//         userName = userNameField.text.Trim();
//         password = passwordField.text.Trim();

//         if (staffAccounts.TryGetValue(userName.ToLower(), out string correctPassword) &&
//             password == correctPassword)
//         {
//             StartCoroutine(LoginProcess());
//         }
//         else
//         {
//             StartCoroutine(ShowLoginFailure("Tên đăng nhập hoặc mật khẩu không chính xác!"));
//         }
//     }

//     private IEnumerator LoginProcess()
//     {
//         ShowProgressBar("Đang đăng nhập...");

//         //!
//         GlobalVariable.APIRequestType.Add("GET_Company");
//         GlobalVariable.APIRequestType.Add("GET_Grapper_List");

//         var task1 = ManagerLocator.Instance.CompanyManager._ICompanyService.GetCompanyByIdAsync(1);
//         var task2 = ManagerLocator.Instance.GrapperManager._IGrapperService.GetListGrapperAsync(1);


//         yield return new WaitUntil(() => Task.WhenAll(task1, task2).IsCompleted);

//         GlobalVariable.APIRequestType.Remove("GET_Company");
//         GlobalVariable.APIRequestType.Remove("GET_Grapper_List");

//         if (task1.IsFaulted || task2.IsFaulted)
//         {
//             HideProgressBar();
//             yield return ShowLoginFailure("Tải dữ liệu thất bại");
//             yield break;
//         }
//         //!

//         SetLoginSuccessData();
//         yield return new WaitForSeconds(0.5f);
//         HideProgressBar();
//         yield return SceneManager.LoadSceneAsync(targetSceneName);
//     }

//     private IEnumerator ShowLoginFailure(string message)
//     {
//         Show_Toast.Instance.ShowToast("failure", message);
//         GlobalVariable.loginSuccess = false;
//         yield return new WaitForSeconds(0.25f);
//         yield return Show_Toast.Instance.Set_Instance_Status_False();
//         HideProgressBar();
//     }
//     private void SetLoginSuccessData()
//     {
//         GlobalVariable.accountModel.userName = userName;
//         GlobalVariable.accountModel.password = password;
//         GlobalVariable.ready_To_Nav_New_Scene = true;
//         GlobalVariable.recentScene = targetSceneName;
//         GlobalVariable.previousScene = MyEnum.LoginScene.GetDescription();
//         GlobalVariable.loginSuccess = true;
//     }

//     private void ShowProgressBar(string title)
//     {
//         Progress.Show(title, ProgressColor.Blue, true);
//         // Progress.SetDetailsText(details);
//     }

//     private void HideProgressBar()
//     {
//         Progress.Hide();
//     }
// }
