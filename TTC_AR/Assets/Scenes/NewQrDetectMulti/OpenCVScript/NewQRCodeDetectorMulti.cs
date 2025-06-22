#define STANDARD_DETECTION
#define WECHAT_DETECTION
#undef STANDARD_DETECTION
//#undef WECHAT_DETECTION
//! 
//? Nhận hình ảnh từ camera (qua MultiSource2MatHelper).
//? Phát hiện nhiều mã QR trong ảnh (qua QRCodeDetector hoặc WeChatQRCode) => Chọn QCodeDetector.
//? Trích xuất thông tin QR code + tọa độ các góc.
//? Tính toán pose (vị trí + hướng) của mã QR trong không gian 3D.
//? Cập nhật các GameObject tương ứng trong không gian AR (thông qua ARHelperMulti).
//!

using OpenCVForUnity.Calib3dModule;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.ObjdetectModule;
using OpenCVForUnity.UnityUtils;
using OpenCVForUnity.UnityUtils.Helper;
using OpenCVForUnity.Wechat_qrcodeModule;
using OpenCVForUnityExample;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

/// <summary>
/// QRCodeDetector Example
/// An example of detecting QRCode using the QRCodeDetector class.
/// https://github.com/opencv/opencv/blob/master/samples/cpp/qrcode.cpp
/// </summary>
[RequireComponent(typeof(MultiSource2MatHelper))]
public class NewQRCodeDetectorMulti : MonoBehaviour
{
    [Header("Output")]
    /// <summary>
    /// The RawImage for previewing the result.
    /// </summary>
    public RawImage resultPreview; //! Chưa dùng
    [Space(10)]
    /// <summary>
    /// The length of the markers' side. Normally, unit is meters.
    /// </summary>
    public float markerLength = 0.06f; //! Mã QR 6cm

    [Space(10)]
    public float deltaTime = 0;
    /// <summary>
    /// deltaTime is the time elapsed since the last frame.
    /// </summary>

    /// <summary>
    /// The enable low pass filter toggle.
    /// </summary>
    public Toggle enableLowPassFilterToggle; //! Chưa dùng

    /// <summary>
    /// ARHelper
    /// </summary>
    public ARHelperMulti arHelper;
    public Manager manager;

    [Space(10)]

    /// <summary>
    /// The gray mat.
    /// </summary>
    Mat grayMat; //? grayMat là một ma trận OpenCV chứa ảnh xám (grayscale) của ảnh đầu vào.

    /// <summary>
    /// The texture.
    /// </summary>
    Texture2D texture; //? texture là một đối tượng Texture2D của Unity, được sử dụng để hiển thị ảnh

    /// <summary>
    /// The QRCode detector.
    /// </summary>
    QRCodeDetector detector; //! detector là một đối tượng QRCodeDetector của OpenCV, được sử dụng để phát hiện mã QR

    /// <summary>
    /// The WeChat QR Code Detector
    /// </summary>
    WeChatQRCode wechatDetector; //? wechatDetector là một đối tượng WeChatQRCode của OpenCV, được sử dụng để phát hiện mã QR của WeChat

    /// <summary>
    /// The points.
    /// </summary>
    Mat points;
    //? Points là một ma trận OpenCV chứa các điểm của mã QR, 
    //? Các điểm này là tọa độ của các góc của mã QR trong ảnh đầu vào.
    //? Ví dụ về các tọa độ như sau: (x1, y1), (x2, y2), (x3, y3), (x4, y4)

    /// <summary>
    /// The decoded info
    /// </summary>
    List<string> decodedInfos; //? decodedInfos là một danh sách chứa thông tin đã giải mã từ mã QR.

    /// <summary>
    /// The straight qrcode
    /// </summary>
    List<Mat> straightQrcode;
    //? straightQrcode là một danh sách chứa các ma trận OpenCV đại diện cho mã QR đã được
    //? làm thẳng (straightened) từ ảnh đầu vào. Làm thẳng ở đây nghĩa là biến đổi hình ảnh sao cho mã QR trở thành một hình chữ nhật phẳng, dễ đọc hơn.
    //? Ví dụ về các ma trận như sau: Mat(2, 2, CV_32FC2)
    //? CV_32FC2 là kiểu dữ liệu của ma trận, trong đó 32 là kích thước của mỗi phần tử (float),

    /// <summary>
    /// The multi source to mat helper.
    /// </summary>
    MultiSource2MatHelper multiSource2MatHelper;
    //! MultiSource2MatHelper là một đối tượng giúp chuyển đổi dữ liệu từ nhiều nguồn (như camera, video) thành ma trận OpenCV.
    //! Ma trận OpenCV này bao gồm các thông tin về ảnh như kích thước, định dạng màu sắc,.. của ảnh đầu vào.
    /// <summary>
    /// The cameraparam matrix.
    /// </summary>
    Mat camMatrix;
    //? CamMatrix là một ma trận OpenCV chứa thông tin về ma trận camera,
    //? Ma trận camera này bao gồm các thông số như: tiêu cự (focal length), điểm chính (principal point) và tỷ lệ khung hình (aspect ratio).

    /// <summary>
    /// The distortion coeffs.
    /// </summary>
    MatOfDouble distCoeffs;
    //? DistCoeffs là một ma trận OpenCV chứa các hệ số biến dạng (distortion coefficients) của camera,
    //? Các hệ số này được sử dụng để chỉnh sửa (correct) các biến dạng trong ảnh do ống kính camera gây ra.
    /// <summary>
    /// The FPS monitor.
    /// </summary>
    FpsMonitor fpsMonitor;
    //? FpsMonitor là một đối tượng dùng để theo dõi và hiển thị tốc độ khung hình (FPS) của ứng dụng.
    //? FPS là số lượng khung hình (frames) được hiển thị trong một giây,

    /// <summary>
    /// Last frame time
    /// </summary>
    private DateTime lastFrameTime;
    //? lastFrameTime là thời gian của khung hình trước đó, được sử dụng để tính toán thời gian giữa các khung hình.

    /// <summary>
    /// Detection result
    /// </summary>
    private bool result;
    //? result là một biến boolean cho biết liệu việc phát hiện mã QR có thành công hay không.

    /// <summary>
    /// List of detected point sets
    /// </summary>
    private List<Mat> pointSetList = new List<Mat>();
    //? pointSetList là một danh sách chứa các ma trận OpenCV đại diện cho các tập hợp điểm đã phát hiện từ mã QR.

    public bool detectionAck { get; private set; } = true;
    //? detectionAck là một biến boolean cho biết liệu việc phát hiện mã QR có được xác nhận hay không.
    public Coroutine coroutine { get; private set; }
    //? coroutine là một biến kiểu Coroutine, được sử dụng để quản lý các tác vụ bất đồng bộ trong Unity.


    private void Awake()
    {
        multiSource2MatHelper ??= GetComponent<MultiSource2MatHelper>();
        // StartCoroutine(InitCoroutine());
    }
    // private IEnumerator InitCoroutine()
    // {
    //     if (isChangeOrientation == false)
    //     {
    //         Screen.orientation = ScreenOrientation.LandscapeLeft;
    //         yield return new WaitForSeconds(0.05f);
    //         isChangeOrientation = true;
    //     }
    // }

    void Start()
    {
        StartCoroutine(StartDetect());
    }

    void Update()
    {

        if (!multiSource2MatHelper.IsPlaying() || !multiSource2MatHelper.DidUpdateThisFrame())
        {
            return;
        }

        Mat rgbaMat = multiSource2MatHelper.GetMat(); //! Liên tục truy xuất hình ảnh từ camera

        Imgproc.cvtColor(rgbaMat, grayMat, Imgproc.COLOR_RGBA2GRAY); //! Sau đó chuyển sang dạng grayMat
        //Imgproc.medianBlur(grayMat, grayMat, 5);
        //Imgproc.cvtColor(grayMat,rgbaMat, Imgproc.COLOR_GRAY2RGBA);


        //Time increment for each QrMarker
        if (lastFrameTime == null) //! Check xem có phải là Frame đầu tiên không
        {
            deltaTime = Time.unscaledDeltaTime; //! Nếu là Frame đầu tiên thì đặt deltaTime bằng deltaTime của hệ thống
            lastFrameTime = DateTime.Now;
        }
        else  //! Nếu đã là các Frame sau
        {
            var delta = DateTime.Now - lastFrameTime; //! Đặt deltaTime bằng thời gian trôi qua kể từ khung hình trước đó
            lastFrameTime = DateTime.Now;
            deltaTime = (float)(delta.TotalMilliseconds / 1000);
        }

        if (detectionAck) //! Kiểm tra xem cờ phát hiện mã QR có bật không?
        {
            if (manager.enableQRCodeDetection && coroutine == null)
            {
                coroutine = StartCoroutine(DetectionCoroutine()); //! Bật việc phát hiện mã QR
                detectionAck = false; //! Đặt detectionAck thành false để không cho phép phát hiện mã QR trong khi đang xử lý
            }

        }

        //! Nếu tính năng nhận diện mã ngưng chạy và trước khi dừng đã có nhận diện được mã QR 
        if (coroutine == null && result)
        {   //! Do có nhận diện được mã QR nên là pointSetList.Count >0 và decodedInfos.Count>0
            for (int i = 0; i < Math.Min(pointSetList.Count, decodedInfos.Count); i++)
            {
                //! Bộ lọc bỏ qua các mã không hợp lệ (ko truy xuất đc content và điểm góc)
                if (string.IsNullOrEmpty(decodedInfos[i])) continue;
                if (pointSetList[i] == null) continue;

                float[] points_arr = new float[8];
                // var test = new Mat(2, 2, CvType.CV_32FC2);
                //! Đọc điểm ảnh (tọa độ pixel trong ảnh 2D)
                pointSetList[i].get(0, 0, points_arr);

                //!Tạo danh sách các điểm 3D tương ứng (tọa độ thực tế trong thế giới AR) với Z=0
                using MatOfPoint3f objectPoints = new MatOfPoint3f(
                    new Point3(-markerLength / 2f, markerLength / 2f, 0),
                    new Point3(markerLength / 2f, markerLength / 2f, 0),
                    new Point3(markerLength / 2f, -markerLength / 2f, 0),
                    new Point3(-markerLength / 2f, -markerLength / 2f, 0)
                    );


                //! Lấy thông tin giải mã của mã QR để lưu vào arHelper.markers
                var content = decodedInfos[i];


                //! Tạo hoặc cập nhật marker
                if (!arHelper.markers.ContainsKey(content))
                {
                    arHelper.markers.Add(content, new QrMarker());
                }


                //TODO Cập nhật điểm đối tượng và điểm ảnh của điểm đánh dấu 
                //TODO và Reset thời gian cập nhật lần cuối của điểm đánh dấu về 0 

                var marker = arHelper.markers[decodedInfos[i]];
                //! các điểm góc trong ảnh (2D).
                marker.ImagePoints = new Vector2[4]
                {
                                        new(points_arr[0], points_arr[1]),
                                        new(points_arr[2], points_arr[3]),
                                        new(points_arr[4], points_arr[5]),
                                        new(points_arr[6], points_arr[7])
                };

                //!các điểm thực trong không gian 3D.
                marker.ObjectPoints = objectPoints.toVector3Array();

                marker.TimeFromLastUpdate = 0;
            }
        }
        // else
        // {
        //     var delta = DateTime.Now - lastFrame;
        //     if (delta.TotalMilliseconds > QrMarker.UpdateTimeLimit)
        //     {
        //         Imgproc.putText(rgbaMat, "Decoding failed.", new Point(5, rgbaMat.rows() - 10), Imgproc.FONT_HERSHEY_SIMPLEX, 0.7, new Scalar(255, 255, 255, 255), 2, Imgproc.LINE_AA, false);
        //     }
        // }

        if (coroutine == null)
        {
            detectionAck = true;
        }

        var keysToRemove = new List<string>();

        //! Nếu các Marker đã nhận diện trước đó (giải mã thành công, đã lưu vào arHelper.markers), 
        //! nhưng lâu quá chưa nhận diện lại được, thì sẽ giải phóng dung lượng

        foreach (var item in arHelper.markers)
        {
            var key = item.Key;
            var marker = item.Value;
            if (marker.TimeFromLastUpdate > QrMarker.UpdateTimeLimit)
                if (marker.TimeFromLastUpdate > QrMarker.UpdateTimeLimit)
                {
                    marker.Dispose();
                    keysToRemove.Add(key);
                }
        }

        foreach (var key in keysToRemove)
        {
            arHelper.markers[key].Dispose();
            arHelper.markers.Remove(key);
        }

        foreach (var item in arHelper.markers)
        {
            item.Value.AddTime(deltaTime: deltaTime);
        }

        Finalize(rgbaMat);
    }


    private IEnumerator StartDetect()
    {
        // yield return new WaitUntil(
        //     () => Screen.orientation == ScreenOrientation.LandscapeLeft && multiSource2MatHelper != null
        //     );

        yield return new WaitUntil(
                    () => multiSource2MatHelper != null
                    );

        Utils.setDebugMode(true);
        multiSource2MatHelper.outputColorFormat = Source2MatHelperColorFormat.RGBA;
        //? Định dạng màu đầu ra của MultiSource2MatHelper được đặt là RGBA, tức là mỗi pixel sẽ có 4 kênh màu: đỏ (R), xanh lá cây (G), xanh dương (B) và alpha (A).

        InitQrDetector();
        //? Gọi phương thức InitQrDetector() để khởi tạo đối tượng QRCodeDetector.
        InitWeChatDetector();
        //? Gọi phương thức InitWeChatDetector() để khởi tạo đối tượng WeChatQRCode.

        //var shapes = arHelper.arGameObjectOrigin.GetComponentsInChildren<MeshRenderer>();
        //foreach (var shape in shapes)
        //{
        //    shape.gameObject.SetActive(false);
        //    Debug.Log($"Disabled {shape.gameObject.name}");
        //}

        lastFrameTime = DateTime.Now;
        //? Gán giá trị thời gian hiện tại cho lastFrameTime, để theo dõi thời gian giữa các khung hình.

        multiSource2MatHelper.Initialize();
        //? Gọi phương thức Initialize() của MultiSource2MatHelper
        //? để khởi tạo các tham số cần thiết cho việc chuyển đổi dữ liệu từ nguồn đầu vào thành ma trận OpenCV.
        //? Khi multiSource2MatHelper được khởi tạo, hàm OnSourceToMatHelperInitialized() sẽ được gọi tự động,
        //? do hàm OnSourceToMatHelperInitializedOnSourceToMatHelperInitialized() được gán vào UnityEvent "OnInitialized" của MultiSource2MatHelper.
    }

    [Conditional("STANDARD_DETECTION")]
    private void InitQrDetector()
    {
        detector = new QRCodeDetector();
    }

    [Conditional("WECHAT_DETECTION")]
    private void InitWeChatDetector()
    {
        wechatDetector = new WeChatQRCode();
    }


    IEnumerator DetectionCoroutine()
    {
        //Debug.Log("Coroutine started");
        var task = Task.Run(() => { WeChatDetection(); StandardDetection(); });

        //! Đợi run xong hết tất cả các line trong hai hàm WeChatDetection(); StandardDetection() 
        //! thì task.IsCompleted = true
        //! đồng nghĩa với việc "Dừng tính năng quét mã"

        yield return new WaitUntil(() => task.IsCompleted);
        coroutine = null;
        //? ==> Cũng là set Result = TRUE
        //Debug.Log("Coroutine completed");
    }

    /// <summary>
    /// Raises the source to mat helper initialized event.
    /// </summary>

    // phương thức này được gọi khi helper đã được khởi tạo

    public void OnSourceToMatHelperInitialized()
    {

        Mat rgbaMat = multiSource2MatHelper.GetMat();
        //? Lấy ma trận rgbaMat từ MultiSource2MatHelper,
        //? rgbaMat là một ma trận OpenCV chứa ảnh đầu vào từ camera,
        //? Ma trận này có định dạng RGBA, tức là mỗi pixel sẽ có 4 kênh màu: đỏ (R), xanh lá cây (G), xanh dương (B) và alpha (A).

        texture = new Texture2D(rgbaMat.cols(), rgbaMat.rows(), TextureFormat.RGBA32, false);
        //? Tạo một đối tượng Texture2D mới với kích thước bằng kích thước của ma trận rgbaMat
        //? Đối tượng này sẽ được sử dụng để hiển thị ảnh trong Unity.
        //?  Ảnh này là  ảnh đầu vào từ camera, được chuyển đổi từ định dạng RGBA sang định dạng Texture2D.

        Utils.matToTexture2D(rgbaMat, texture);
        //? Chuyển đổi ma trận rgbaMat thành texture bằng phương thức matToTexture2D của Utils.

        //resultPreview.texture = texture;`
        //resultPreview.GetComponent<AspectRatioFitter>().aspectRatio = (float)texture.width / texture.height;

        // Set the Texture2D as the main texture of the Renderer component attached to the game object
        gameObject.GetComponent<Renderer>().material.mainTexture = texture;
        //? Renderer này là MeshRenderer => Logo 
        //? Gán texture làm màn hình camera lên material.mainTexture 

        // Adjust the scale of the game object to match the dimensions of the texture
        gameObject.transform.localScale = new Vector3(rgbaMat.cols(), rgbaMat.rows(), 1);


        Debug.Log(
        "Screen.width " + Screen.width
        + " Screen.height " + Screen.height
        + " Screen.orientation " + Screen.orientation);

        Debug.Log("OnSourceToMatHelperInitialized " + rgbaMat.cols() + " " + rgbaMat.rows());

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

        if (fpsMonitor != null)
        {
            fpsMonitor.Add("width", rgbaMat.width().ToString());
            fpsMonitor.Add("height", rgbaMat.height().ToString());
            fpsMonitor.Add("orientation", Screen.orientation.ToString());
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
        decodedInfos = new List<string>();
        straightQrcode = new List<Mat>();


#if !OPENCV_DONT_USE_WEBCAMTEXTURE_API
        // If the WebCam is front facing, flip the Mat horizontally. Required for successful detection.
        if (multiSource2MatHelper.source2MatHelper is WebCamTexture2MatHelper webCamHelper)
            webCamHelper.flipHorizontal = webCamHelper.IsFrontFacing();
#endif
        arHelper.SetCamMatrix(camMatrix);
        arHelper.SetDistCoeffs(distCoeffs);

        arHelper.Initialize(
            screenWidth: Screen.width,
            screenHeight: Screen.height,
            imageWidth: rgbaMat.width(),
            imageHeight: rgbaMat.height()
            );

        //! arHelper.Initialize được gọi để khởi tạo các tham số cần thiết cho việc xử lý ảnh trong không gian AR.
        //! Phương thức này sẽ thiết lập các thông số camera, kích thước ảnh và các tham số khác cần thiết cho việc phát hiện mã QR trong không gian AR.
    }

    /// <summary>
    /// Raises the source to mat helper disposed event.
    /// </summary>
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

        if (decodedInfos != null)
            decodedInfos.Clear();

        if (straightQrcode != null)
            foreach (var item in straightQrcode)
            {
                item.Dispose();
            }

        if (arHelper != null)
            arHelper.Dispose();
    }

    /// <summary>
    /// Raises the source to mat helper error occurred event.
    /// </summary>
    /// <param name="errorCode">Error code.</param>
    /// <param name="message">Message.</param>
    public void OnSourceToMatHelperErrorOccurred(Source2MatHelperErrorCode errorCode, string message)
    {
        Debug.Log("OnSourceToMatHelperErrorOccurred " + errorCode + ":" + message);

        if (fpsMonitor != null)
        {
            fpsMonitor.consoleText = "ErrorCode: " + errorCode + ":" + message;
        }
    }

    // Update is called once per frame

    void Finalize(Mat rgbaMat)
    {
        Utils.matToTexture2D(rgbaMat, texture);
    }


    //! Nếu tại thời điểm run hàm WeChatDetection() mà màn hình camera có quay tới các mã QR thì result sẽ true nếu giải mã thành công
    [Conditional("WECHAT_DETECTION")]
    private void WeChatDetection()
    {
        foreach (var pointSet in pointSetList)
        {
            pointSet.Dispose();
        }

        decodedInfos = wechatDetector.detectAndDecode(grayMat, pointSetList); //! Vưà detect vừa giải mã
                                                                              //? Phát hiện mã QR trong ảnh xám grayMat
                                                                              //? giải mã nội dung QR
                                                                              //! Trong pointSetList, mỗi phần tử của là một Mat chứa tọa độ 4 góc của 1 mã QR?
                                                                              //! ==> pointSetList.Count = decodedInfos.Count
                                                                              //! ==> decodedInfos.Count = số lượng mã QR trong hình ảnh GrayMat
                                                                              //! Kiểm tra thứ tự điểm góc và sửa lại nếu cần nếu mã QR bị ngược
        foreach (var pointSet in pointSetList)
        {
            float[] points_arr = new float[8];
            try
            {
                pointSet.get(0, 0, points_arr);
                var x1 = points_arr[0] - points_arr[4];
                var y1 = points_arr[1] - points_arr[5];
                var x2 = points_arr[2] - points_arr[6];
                var y2 = points_arr[3] - points_arr[7];
                if (x1 * y2 - x2 * y1 < 0)
                {
                    (points_arr[2], points_arr[6]) = (points_arr[6], points_arr[2]);
                    (points_arr[3], points_arr[7]) = (points_arr[7], points_arr[3]);
                }
                pointSet.put(0, 0, points_arr);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        result = (decodedInfos != null) && (decodedInfos.Count > 0); //! Có nhận diện được không (có lấy được content trong mã QR ko)
    }

    [Conditional("STANDARD_DETECTION")]
    private void StandardDetection()
    {
        result = detector.detectAndDecodeMulti(grayMat, decodedInfos, points, straightQrcode);
        if (result)
        {
            for (int i = 0; i < points.rows(); i++)
            {
                float[] points_arr = new float[8];
                points.get(i, 0, points_arr);
                if (pointSetList.Count <= i)
                {
                    pointSetList.Add(new Mat((4, 1), CvType.CV_32FC2));
                }
                pointSetList[i].put(0, 0, points_arr);
                if (decodedInfos.Count <= i)
                {
                    decodedInfos.Add("");
                }
            }
        }

        if (pointSetList.Count > points.rows())
            pointSetList.RemoveRange(points.rows(), pointSetList.Count - points.rows());

        if (decodedInfos.Count > points.rows())
            decodedInfos.RemoveRange(points.rows(), decodedInfos.Count - points.rows());
    }

    void OnDestroy()
    {
        multiSource2MatHelper.Dispose();

        if (detector != null)
            detector.Dispose();
    }
}