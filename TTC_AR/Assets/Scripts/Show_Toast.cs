using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;

public class Show_Toast : MonoBehaviour
{
    // Singleton Instance
    public static Show_Toast Instance { get; set; }

    public Transform toastParent;
    public GameObject toastPrefab;
    public string toastStatus = "loading";
    public Transform existingToast;
    public Image toastBackground;
    public TMP_Text toastText;
    // Tìm tất cả các GameObject trong scene
    public GameObject[] allObjects;
    // Preload Sprites for better performance
    [SerializeField] private string toastMessageInitial = "";
    [SerializeField] private bool showToastInitial = false;
    private Sprite loadingSprite;
    private Sprite successSprite;
    private Sprite failureSprite;


    private void Awake()
    {

    }
    void OnEnable()
    {
        allObjects = FindObjectsOfType<GameObject>();
        toastParent ??= GetComponent<Canvas>().transform;

        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            Debug.LogWarning("Multiple instances of Show_Toast found. Destroying duplicate.");
        }
        else
        {
            Instance = this;
            // Debug.Log("Show_Toast");
            PreloadSprites();
        }
        if (showToastInitial)
        {
            if (GlobalVariable.recentScene == MyEnum.SelectionsScene.GetDescription() && GlobalVariable.previousScene != MyEnum.LoginScene.GetDescription())
            {
                return;
            }
            else
            {
                Set_Instance_Status_True();
                ShowToast(toastStatus, toastMessageInitial);
                StartCoroutine(Set_Instance_Status_False());
                showToastInitial = false;
            }
        }
    }

    void OnDisable()
    {
        Instance = null;
    }

    void Start()
    {
    }

    private void PreloadSprites()
    {
        loadingSprite = Resources.Load<Sprite>("images/UIimages/loading_notification");
        successSprite = Resources.Load<Sprite>("images/UIimages/success_notification");
        failureSprite = Resources.Load<Sprite>("images/UIimages/error_notification");
    }

    public void ShowToast(string toastStatus, string message)
    {
        if (existingToast == null)
        {
            existingToast = Instantiate(toastPrefab, toastParent).transform;
            var layoutToast = existingToast.transform.GetChild(0);

            toastText = layoutToast.GetComponentInChildren<TMP_Text>();

            toastBackground = layoutToast.GetComponentInChildren<Image>();

            existingToast.gameObject.SetActive(true);
        }
        else
        {
            existingToast.gameObject.SetActive(true);
        }

        if (toastText != null && toastBackground != null)
        {
            toastText.text = message;
            switch (toastStatus.ToLower())
            {
                case "loading":
                    toastText.color = new Color32(0x00, 0x96, 0xFF, 0xFF);
                    toastBackground.sprite = loadingSprite;
                    break;
                case "success":
                    toastText.color = new Color32(0x27, 0xC8, 0x6F, 0xFF);
                    toastBackground.sprite = successSprite;
                    break;
                case "failure":
                    toastText.color = new Color32(0xFF, 0x49, 0x39, 0xFF);
                    toastBackground.sprite = failureSprite;
                    break;
                default:
                    Debug.LogWarning("Unknown toast status: " + toastStatus);
                    break;
            }
        }

    }
    private void SetInstanceStatus(bool status)
    {

        if (status == false)
        {
            if (existingToast == null)
            {
                // Debug.LogError("existingToast is null. Make sure it is assigned properly.");
                return;
            }
            existingToast.gameObject.SetActive(status);
        }

        foreach (GameObject obj in allObjects)
        {
            if (obj != null && obj.name == "LeanTouch")
            {
                obj.SetActive(!status);
                // Debug.Log($"{(status ? "Deactivated" : "Activated")}: {obj.name}");
            }
        }
    }


    public void Set_Instance_Status_True()
    {
        SetInstanceStatus(true);
    }

    public IEnumerator Set_Instance_Status_False(float time = 1f)
    {
        yield return new WaitForSeconds(time);
        SetInstanceStatus(false);
    }
}

