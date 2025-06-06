#define STANDARD_DETECTION
//#define WECHAT_DETECTION
// #undef STANDARD_DETECTION
#undef WECHAT_DETECTION

using OpenCVForUnity.Calib3dModule;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.ObjdetectModule;
using OpenCVForUnity.UnityUtils;
using OpenCVForUnity.UnityUtils.Helper;
using OpenCVForUnity.Wechat_qrcodeModule;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Debug = UnityEngine.Debug;

[RequireComponent(typeof(MultiSource2MatHelper))]
public class NewQRCodeDetectorMulti : MonoBehaviour
{
    [Header("Output")]
    //public RawImage resultPreview;

    [Space(10)]
    public float markerLength = 0.1f;

    [Space(10)]
    public float deltaTime = 0;
    public ARHelperMulti arHelper;
    public Manager manager;

    [Space(10)]
    Mat grayMat;
    Texture2D texture;
    QRCodeDetector detector;
    // WeChatQRCode wechatDetector;
    Mat points;
    List<string> decodedInfo;
    // string decodedInfo;
    List<Mat> straightQrcode;
    // Mat straightQrcode;
    MultiSource2MatHelper multiSource2MatHelper;
    Mat camMatrix;
    MatOfDouble distCoeffs;
    private DateTime lastFrameTime;
    private float detectionInterval = 0.5f; // Detect mỗi 0.5 giây
    private float lastDetectionTime = 0;
    private bool result;
    Mat rgbaMat;
    private List<Mat> pointSetList = new List<Mat>();
    public bool detectionAck { get; private set; } = true;
    public Coroutine coroutine { get; private set; }
    private bool isChangeOrientation = false;

    void Start()
    {
        StartCoroutine(StartDetect());
    }

    private void Awake()
    {
        multiSource2MatHelper ??= gameObject.GetComponent<MultiSource2MatHelper>();
        StartCoroutine(InitCoroutine());
    }
    private IEnumerator InitCoroutine()
    {
        if (isChangeOrientation == false)
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
            yield return new WaitForSeconds(0.05f);
            isChangeOrientation = true;
        }
    }

    private IEnumerator StartDetect()
    {
        yield return new WaitUntil(() => isChangeOrientation == true);

        Utils.setDebugMode(true);
        multiSource2MatHelper.outputColorFormat = Source2MatHelperColorFormat.RGBA;

        InitQrDetector();
        // InitWeChatDetector();
        lastFrameTime = DateTime.Now;
        multiSource2MatHelper.Initialize();
    }

    [Conditional("STANDARD_DETECTION")]
    private void InitQrDetector()
    {
        detector = new QRCodeDetector();
    }

    // [Conditional("WECHAT_DETECTION")]
    // private void InitWeChatDetector()
    // {
    //     wechatDetector = new WeChatQRCode();
    // }

    private IEnumerator DetectionCoroutine()
    {
        //Debug.Log("Coroutine started");
        var task = Task.Run(() =>
        {
            //WeChatDetection(); 
            StandardDetection();
        });
        yield return new WaitUntil(() => task.IsCompleted);
        coroutine = null;
    }

    public void OnSourceToMatHelperInitialized()
    {

        Mat rgbaMat = multiSource2MatHelper.GetMat();

        texture = new Texture2D(rgbaMat.cols(), rgbaMat.rows(), TextureFormat.RGBA32, false);
        Utils.matToTexture2D(rgbaMat, texture);

        gameObject.GetComponent<Renderer>().material.mainTexture = texture;

        // Adjust the scale of the game object to match the dimensions of the texture
        gameObject.transform.localScale = new Vector3(rgbaMat.cols(), rgbaMat.rows(), 1);

        float width = rgbaMat.width();
        float height = rgbaMat.height();

        float imageSizeScale = 1.0f;
        float widthScale = (float)Screen.width / width;
        float heightScale = (float)Screen.height / height;
        if (widthScale < heightScale)
        {
            Camera.main.orthographicSize = (width * (float)Screen.height / (float)Screen.width) / 2;
            imageSizeScale = (float)Screen.height / (float)Screen.width;
        }
        else
        {
            Camera.main.orthographicSize = height / 2;
        }

        // set camera parameters.
        double fx;
        double fy;
        double cx;
        double cy;

        int max_d = (int)Mathf.Max(width, height);
        fx = max_d;
        fy = max_d;
        cx = width / 2.0f;
        cy = height / 2.0f;

        camMatrix = new Mat(3, 3, CvType.CV_64FC1);
        camMatrix.put(0, 0, fx);
        camMatrix.put(0, 1, 0);
        camMatrix.put(0, 2, cx);
        camMatrix.put(1, 0, 0);
        camMatrix.put(1, 1, fy);
        camMatrix.put(1, 2, cy);
        camMatrix.put(2, 0, 0);
        camMatrix.put(2, 1, 0);
        camMatrix.put(2, 2, 1.0f);

        distCoeffs = new MatOfDouble(0, 0, 0, 0);

        Size imageSize = new Size(width * imageSizeScale, height * imageSizeScale);
        double apertureWidth = 0;
        double apertureHeight = 0;
        double[] fovx = new double[1];
        double[] fovy = new double[1];
        double[] focalLength = new double[1];
        Point principalPoint = new Point(0, 0);
        double[] aspectratio = new double[1];

        Calib3d.calibrationMatrixValues(camMatrix, imageSize, apertureWidth, apertureHeight, fovx, fovy, focalLength, principalPoint, aspectratio);

        // To convert the difference of the FOV value of the OpenCV and Unity. 
        double fovXScale = (2.0 * Mathf.Atan((float)(imageSize.width / (2.0 * fx)))) / (Mathf.Atan2((float)cx, (float)fx) + Mathf.Atan2((float)(imageSize.width - cx), (float)fx));
        double fovYScale = (2.0 * Mathf.Atan((float)(imageSize.height / (2.0 * fy)))) / (Mathf.Atan2((float)cy, (float)fy) + Mathf.Atan2((float)(imageSize.height - cy), (float)fy));

        grayMat = new Mat(rgbaMat.rows(), rgbaMat.cols(), CvType.CV_8UC1);

        points = new Mat();
        decodedInfo = new List<string>();
        straightQrcode = new List<Mat>();

#if !OPENCV_DONT_USE_WEBCAMTEXTURE_API
        // If the WebCam is front facing, flip the Mat horizontally. Required for successful detection.
        if (multiSource2MatHelper.source2MatHelper is WebCamTexture2MatHelper webCamHelper)
            webCamHelper.flipHorizontal = webCamHelper.IsFrontFacing();
#endif
        arHelper.SetCamMatrix(camMatrix);
        arHelper.SetDistCoeffs(distCoeffs);
        arHelper.Initialize(Screen.width, Screen.height, rgbaMat.width(), rgbaMat.height());
        StaticVariable.is_Custom_Camera = true;
    }

    public void OnSourceToMatHelperDisposed()
    {

        if (grayMat != null)
            grayMat.Dispose();

        if (texture != null)
        {
            Destroy(texture);
            texture = null;
        }

        if (points != null)
            points.Dispose();

        if (decodedInfo != null)
            decodedInfo.Clear();

        if (straightQrcode != null)
            foreach (var item in straightQrcode)
            {
                item.Dispose();
            }

        if (arHelper != null)
            arHelper.Dispose();
    }

    public void OnSourceToMatHelperErrorOccurred(Source2MatHelperErrorCode errorCode, string message)
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!multiSource2MatHelper.IsPlaying() || !multiSource2MatHelper.DidUpdateThisFrame())
            return;

        rgbaMat = multiSource2MatHelper.GetMat();
        Imgproc.cvtColor(rgbaMat, grayMat, Imgproc.COLOR_RGBA2GRAY);

        if (Time.time - lastDetectionTime >= detectionInterval && detectionAck)
        {
            lastDetectionTime = Time.time;
            coroutine = StartCoroutine(DetectionCoroutine());
            // DetectionCoroutine();
            detectionAck = false;
        }

        if (GlobalVariable.isCameraPaused)
            return;

        if (manager.isCanvasOpen)
        {
            if (manager.isCanvasOpen)
            {
                foreach (var marker in arHelper.markers.Values)
                {
                    marker.Dispose();
                }
                arHelper.markers.Clear();
            }
            Finalize(rgbaMat);
            return;
        }

        deltaTime = lastFrameTime == null ? Time.unscaledDeltaTime : (float)(DateTime.Now - lastFrameTime).TotalSeconds;
        lastFrameTime = DateTime.Now;

        if (coroutine == null && result)
        {
            int count = Math.Min(pointSetList.Count, decodedInfo.Count);
            for (int i = 0; i < count; i++)
            {
                if (string.IsNullOrEmpty(decodedInfo[i]) || pointSetList[i] == null)
                    continue;

                float[] points_arr = new float[8];
                pointSetList[i].get(0, 0, points_arr);

                using MatOfPoint3f objectPoints = new MatOfPoint3f(
                    new Point3(-markerLength / 2f, markerLength / 2f, 0),
                    new Point3(markerLength / 2f, markerLength / 2f, 0),
                    new Point3(markerLength / 2f, -markerLength / 2f, 0),
                    new Point3(-markerLength / 2f, -markerLength / 2f, 0)
                );

                var content = decodedInfo[i];
                if (!arHelper.markers.TryGetValue(content, out var marker))
                {
                    marker = new QrMarker();
                    arHelper.markers[content] = marker;
                }

                marker.ImagePoints = new Vector2[4]
                {
                    new(points_arr[0], points_arr[1]),
                    new(points_arr[2], points_arr[3]),
                    new(points_arr[4], points_arr[5]),
                    new(points_arr[6], points_arr[7])
                };
                marker.ObjectPoints = objectPoints.toVector3Array();
                marker.TimeFromLastUpdate = 0;
            }
        }

        if (coroutine == null)
        {
            detectionAck = true;
        }

        var keysToRemove = arHelper.markers
            .Where(item => item.Value.TimeFromLastUpdate > QrMarker.UpdateTimeLimit)
            .Select(item => item.Key)
            .ToList();

        foreach (var key in keysToRemove)
        {
            arHelper.markers[key].Dispose();
            arHelper.markers.Remove(key);
        }

        foreach (var marker in arHelper.markers.Values)
        {
            marker.AddTime(deltaTime);
        }

        Finalize(rgbaMat);
    }

    void Finalize(Mat rgbaMat)
    {
        Utils.matToTexture2D(rgbaMat, texture);
    }

    // [Conditional("WECHAT_DETECTION")]
    // private void WeChatDetection()
    // {

    //     foreach (var pointSet in pointSetList)
    //     {
    //         pointSet.Dispose();
    //     }
    //     decodedInfo = wechatDetector.detectAndDecode(grayMat, pointSetList);



    //     foreach (var pointSet in pointSetList)
    //     {
    //         float[] points_arr = new float[8];
    //         try
    //         {
    //             pointSet.get(0, 0, points_arr);
    //             var x1 = points_arr[0] - points_arr[4];
    //             var y1 = points_arr[1] - points_arr[5];
    //             var x2 = points_arr[2] - points_arr[6];
    //             var y2 = points_arr[3] - points_arr[7];
    //             if (x1 * y2 - x2 * y1 < 0)
    //             {
    //                 (points_arr[2], points_arr[6]) = (points_arr[6], points_arr[2]);
    //                 (points_arr[3], points_arr[7]) = (points_arr[7], points_arr[3]);
    //             }
    //             pointSet.put(0, 0, points_arr);
    //         }
    //         catch (Exception e)
    //         {
    //             Debug.LogError(e);
    //         }
    //     }
    //     result = (decodedInfo != null) && (decodedInfo.Count > 0);
    // }

    [Conditional("STANDARD_DETECTION")]
    private void StandardDetection()
    {
        result = detector.detectAndDecodeMulti(grayMat, decodedInfo, points, straightQrcode);
        if (!result) return;

        for (int i = 0; i < points.rows(); i++)
        {
            float[] points_arr = new float[8];
            points.get(i, 0, points_arr);

            if (pointSetList.Count <= i)
                pointSetList.Add(new Mat(4, 1, CvType.CV_32FC2));
            // else
            //     pointSetList[i].setTo(new Scalar(0)); // Reset existing Mat to avoid stale data

            pointSetList[i].put(0, 0, points_arr);

            if (decodedInfo.Count <= i)
                decodedInfo.Add("");
        }

        if (pointSetList.Count > points.rows())
            pointSetList.RemoveRange(points.rows(), pointSetList.Count - points.rows());

        if (decodedInfo.Count > points.rows())
            decodedInfo.RemoveRange(points.rows(), decodedInfo.Count - points.rows());
    }

    void OnDestroy()
    {
        multiSource2MatHelper.Dispose();

        if (detector != null)
            detector.Dispose();
    }
}