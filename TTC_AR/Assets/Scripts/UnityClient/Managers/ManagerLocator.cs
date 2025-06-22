using UnityEngine;

public class ManagerLocator : MonoBehaviour
{
    //! Singleton
    private static ManagerLocator _instance;
    public static ManagerLocator Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject managerLocatorObject = new GameObject("ManagerLocator");
                _instance = managerLocatorObject.AddComponent<ManagerLocator>();
                DontDestroyOnLoad(managerLocatorObject);
            }
            return _instance;
        }
    }

    //! Các Manager cần quản lý
    public CompanyManager CompanyManager { get; private set; }
    public GrapperManager GrapperManager { get; private set; }
    public RackManager RackManager { get; private set; }
    public ModuleManager ModuleManager { get; private set; }
    public DeviceManager DeviceManager { get; private set; }
    public JBManager JBManager { get; private set; }
    public MccManager MccManager { get; private set; }
    public FieldDeviceManager FieldDeviceManager { get; private set; }
    public ModuleSpecificationManager ModuleSpecificationManager { get; private set; }
    public AdapterSpecificationManager AdapterSpecificationManager { get; private set; }
    public ImageManager ImageManager { get; private set; }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            CompanyManager = gameObject.AddComponent<CompanyManager>();
            GrapperManager = gameObject.AddComponent<GrapperManager>();
            RackManager = gameObject.AddComponent<RackManager>();
            ModuleManager = gameObject.AddComponent<ModuleManager>();
            JBManager = gameObject.AddComponent<JBManager>();
            DeviceManager = gameObject.AddComponent<DeviceManager>();
            MccManager = gameObject.AddComponent<MccManager>();
            FieldDeviceManager = gameObject.AddComponent<FieldDeviceManager>();
            ModuleSpecificationManager = gameObject.AddComponent<ModuleSpecificationManager>();
            AdapterSpecificationManager = gameObject.AddComponent<AdapterSpecificationManager>();
            ImageManager = gameObject.AddComponent<ImageManager>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {

    }

}
