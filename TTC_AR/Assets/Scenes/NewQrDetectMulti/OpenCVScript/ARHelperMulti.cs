using OpenCVForUnity.Calib3dModule;
using OpenCVForUnity.CoreModule;

// #if UNITY_EDITOR
using OpenCVForUnity.UnityUtils.Helper.Editor;
// #endif
// using OpenCVForUnityExample;
using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;
using TMPro;

namespace OpenCVForUnity.UnityUtils.Helper
{
    public class ARHelperMulti : MonoBehaviour
    {
        [TooltipAttribute("Target AR GameObject")]
        public GameObject arGameObjectOrigin;
        [TooltipAttribute("Target AR Camera")]
        public Camera arCamera;
        public enum UpdateTarget
        {
            ARGameObject,
            ARCamera
        }

        [Space(10)]

        [TooltipAttribute("Specifies the GameObject whose Transform is to be updated.")]
        public UpdateTarget updateTarget;

        [Space(10)]

        [TooltipAttribute("If true, the CalculateARMatrix() method is automatically called in the LateUpdate() method; if false, you must call it yourself.")]
        public bool calculateARMatrixInLateUpdate = true;

        [TooltipAttribute("If true, the UpdateTransform() method is automatically called in the LateUpdate() method; if false, you must call it yourself.")]
        public bool updateTransformInLateUpdate = true;


        [Header("Camera Parameters")]


        [SerializeField, FormerlySerializedAs("screenWidth"), TooltipAttribute("Set the width of screen.")]
        // protected int _screenWidth = 640;
        protected int _screenWidth = 770;
        public virtual int screenWidth
        {
            get { return _screenWidth; }
            set
            {
                int _value = Mathf.Clamp(value, 0, int.MaxValue);
                if (_screenWidth != _value)
                {
                    _screenWidth = _value;
                    if (hasInitDone) Initialize();
                }
            }
        }


        [SerializeField, FormerlySerializedAs("screenHeight"), TooltipAttribute("Set the height of screen.")]
        // protected int _screenHeight = 480;
        protected int _screenHeight = 480;
        public virtual int screenHeight
        {
            get { return _screenHeight; }
            set
            {
                int _value = Mathf.Clamp(value, 0, int.MaxValue);
                if (_screenHeight != _value)
                {
                    _screenHeight = _value;
                    if (hasInitDone) Initialize();
                }
            }
        }


        [SerializeField, FormerlySerializedAs("imageWidth"), TooltipAttribute("Set the width of image.")]
        protected int _imageWidth = 770;
        public virtual int imageWidth
        {
            get { return _imageWidth; }
            set
            {
                int _value = Mathf.Clamp(value, 0, int.MaxValue);
                if (_imageWidth != _value)
                {
                    _imageWidth = _value;
                    if (hasInitDone) Initialize();
                }
            }
        }


        [SerializeField, FormerlySerializedAs("imageHeight"), TooltipAttribute("Set the height of image.")]
        protected int _imageHeight = 480;
        public virtual int imageHeight
        {
            get { return _imageHeight; }
            set
            {
                int _value = Mathf.Clamp(value, 0, int.MaxValue);
                if (_imageHeight != _value)
                {
                    _imageHeight = _value;
                    if (hasInitDone) Initialize();
                }
            }
        }


        [SerializeField, FormerlySerializedAs("camMatrixValues"), Tooltip("Specifies the initial value of camMatrix used for camera calibration. If the number of elements in this array is less than 9, it is automatically calculated from the Screen's Width and Height and the Image's Width and Height.")]
#if UNITY_EDITOR
        [LabeledArray("f_x", "01", "c_x", "10", "f_y", "c_y", "20", "21", "22")]
#endif
        protected double[] _camMatrixValues = null;
        public virtual double[] camMatrixValues
        {
            get { return _camMatrixValues; }
            set
            {
                _camMatrixValues = value;
                _camMatrixValues = ValidateArraySize(_camMatrixValues, CAMMATRIXVALUES_MAX_SIZE);
                if (hasInitDone) Initialize();
            }
        }


        [SerializeField, FormerlySerializedAs("distCoeffsValues"), Tooltip("Specifies the initial value of distCoeffs used for camera calibration. If the number of elements in this array is less than 5, all elements are set to 0.")]
#if UNITY_EDITOR
        [LabeledArray("k_1", "k_2", "k_3", "p_1", "p_2", "k_4", "k_5", "k_6", "s_1", "s_2", "s_3", "s_4", "τ_x", "τ_y")]
#endif
        protected double[] _distCoeffsValues = null;
        public virtual double[] distCoeffsValues
        {
            get { return _distCoeffsValues; }
            set
            {
                _distCoeffsValues = value;
                _distCoeffsValues = ValidateArraySize(_distCoeffsValues, DISTCOEFFSVALUES_MAX_SIZE);
                if (hasInitDone) Initialize();
            }
        }


        [Header("2D Points")]

        [TooltipAttribute("Specify the imagePoints argument to the Calib3d.solvePnP() method.")]
        public Vector2[] imagePoints;

        [Header("3D Points")]

        [TooltipAttribute("Enable this flag if the object point is a left-hand coordinate system (Unity).")]
        public bool leftHandedCoordinates = false;

        [TooltipAttribute("Specify the objectPoints argument to the Calib3d.solvePnP() method.")]
        public Vector3[] objectPoints;

        [Header("LowPassFilter")]

        [TooltipAttribute("When enabled, LowPassFilter suppresses noise.")]

        public bool useLowPassFilter = false;
        [TooltipAttribute("Position parameter of LowPassFilter")]

        [Range(0.0f, 10.0f)]
        public float positionLowPassParam = 4f;

        [TooltipAttribute("Rotation parameter of LowPassFilter")]

        [Range(0.0f, 10.0f)]
        public float rotationLowPassParam = 2f;


        [Header("Apply axis inversion to ARMatrix")]

        [TooltipAttribute("Apply X-axis inversion to ARMatrix.")]
        public bool applyXaxisInversionToARMatrix = false;

        [TooltipAttribute("Apply Y-axis inversion to ARMatrix.")]
        public bool applyYaxisInversionToARMatrix = false;

        [TooltipAttribute("Apply Z-axis inversion to ARMatrix.")]
        public bool applyZaxisInversionToARMatrix = false;
        public Dictionary<string, QrMarker> markers = new Dictionary<string, QrMarker>();
        public GameObject[] arGameObjects;
        protected bool hasInitDone = false;
        private PoseData oldPoseData;
        private Mat camMatrix;
        private MatOfDouble distCoeffs;
        private Matrix4x4 invertXMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(-1, 1, 1));
        private Matrix4x4 invertYMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1, -1, 1));
        private Matrix4x4 invertZMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1, 1, -1));
        private Matrix4x4 opencvCameraProjectionMatrix;
        private Matrix4x4 transformMatrix;
        private Matrix4x4 arMatrix;
        private Mat rvec;
        private Mat tvec;
        private bool isARGameObjectInARCameraViewport = false;
        private const int CAMMATRIXVALUES_MIN_SIZE = 9;
        private const int CAMMATRIXVALUES_MAX_SIZE = 9;
        private const int DISTCOEFFSVALUES_MIN_SIZE = 5;
        private const int DISTCOEFFSVALUES_MAX_SIZE = 14;
        private Vector3 arGameObjectDefaultLocalPosition;
        private Quaternion arGameObjectDefaultLocalRotation;
        private Vector3 arGameObjectDefaultLocalScale;
        private Vector3 arCameraDefaultLocalPosition;
        private Quaternion arCameraDefaultLocalRotation;
        private Vector3 arCameraDefaultLocalScale;

        private float arCameraDefaultFieldOfView;

        private void Start()
        {
            // Deactivate the arGameObjectOrigin
            arGameObjectOrigin.SetActive(false);
            // Debug.Log("arGameObjectOrigin deactivated");
        }

        protected virtual void OnValidate()
        {
            //Debug.Log("OnValidate");

            _screenWidth = Mathf.Clamp(_screenWidth, 0, int.MaxValue);
            _screenHeight = Mathf.Clamp(_screenHeight, 0, int.MaxValue);
            _imageWidth = Mathf.Clamp(_imageWidth, 0, int.MaxValue);
            _imageHeight = Mathf.Clamp(_imageHeight, 0, int.MaxValue);

            _camMatrixValues = ValidateArraySize(_camMatrixValues, CAMMATRIXVALUES_MAX_SIZE);
            _distCoeffsValues = ValidateArraySize(_distCoeffsValues, DISTCOEFFSVALUES_MAX_SIZE);

            if (hasInitDone) Initialize();
        }

        private double[] ValidateArraySize(double[] array, int maxSize)
        {
            if (array != null && array.Length > maxSize)
            {
                // Debug.LogWarning($"The size of the array exceeds the limit of {maxSize}. It will be truncated.");
                System.Array.Resize(ref array, maxSize);
            }
            return array;
        }

        void LateUpdate()
        {
            //Debug.Log("LateUpdate");

            if (!hasInitDone) return;

            if (!updateTransformInLateUpdate)
            {
                return;
            }

            foreach (var arGameObject in arGameObjects)
            {
                arGameObject.SetActive(false);
            }
            int index = 0;
            foreach (var item in markers)
            {
                var marker = item.Value;
                var key = item.Key;
                if (index >= arGameObjects.Length)
                {
                    break;
                }
                var arGameObject = arGameObjects[index];
                arGameObject.SetActive(true);
                var textMeshList = arGameObject.GetComponentsInChildren<TextMeshPro>(includeInactive: true);
                foreach (var mesh in textMeshList)
                {
                    mesh.text = $"{key}";
                }
                CalculateARMatrix(ref marker);
                UpdateTransform(arGameObject);
                marker.GameObject = arGameObject;
                marker.UpdateArMatrix(arMatrix);
                index++;
            }
        }

        protected virtual void OnDestroy()
        {
            if (hasInitDone)
                Dispose();
        }

        public virtual void Initialize()
        {
            //Debug.Log("Initialize");

            _Initialize();
        }

        public virtual void Initialize(int screenWidth, int screenHeight, int imageWidth, int imageHeight, double[] camMatrixValues = null, double[] distCoeffsValues = null, Vector2[] imagePoints = null, Vector3[] objectPoints = null)
        {
            //Debug.Log("Initialize");

            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
            _imageWidth = imageWidth;
            _imageHeight = imageHeight;

            if (camMatrixValues != null)
                _camMatrixValues = ValidateArraySize(camMatrixValues, 9);
            if (distCoeffsValues != null)
                _distCoeffsValues = ValidateArraySize(distCoeffsValues, 14);

            if (imagePoints != null)
                this.imagePoints = imagePoints;

            if (objectPoints != null)
                this.objectPoints = objectPoints;

            _Initialize();
        }

        public virtual void Dispose()
        {
            //Debug.Log("Dispose");

            ResetUpdateTargetTransform();

            hasInitDone = false;

            if (camMatrix != null)
            {
                camMatrix.Dispose();
                camMatrix = null;
            }

            if (distCoeffs != null)
            {
                distCoeffs.Dispose();
                distCoeffs = null;
            }

            if (rvec != null)
            {
                rvec.Dispose();
                rvec = null;
            }

            if (tvec != null)
            {
                tvec.Dispose();
                tvec = null;
            }

            if (markers != null)
            {
                foreach (var marker in markers)
                {
                    marker.Value?.Dispose();
                }
                markers.Clear();
            }
        }

        public virtual bool IsInitialized()
        {
            return hasInitDone;
        }

        private void _Initialize()
        {
            //Debug.Log("_Initialize");

            if (hasInitDone)
            {
                Dispose();
            }

            if (arGameObjectOrigin == null)
                return;

            if (arCamera == null)
                return;

            // Save initial values for ARGameObject and ARCamera.
            arGameObjectDefaultLocalPosition = arGameObjectOrigin.transform.localPosition;
            arGameObjectDefaultLocalRotation = arGameObjectOrigin.transform.localRotation;
            arGameObjectDefaultLocalScale = arGameObjectOrigin.transform.localScale;

            arCameraDefaultLocalPosition = arCamera.transform.localPosition;
            arCameraDefaultLocalRotation = arCamera.transform.localRotation;
            arCameraDefaultLocalScale = arCamera.transform.localScale;
            arCameraDefaultFieldOfView = arCamera.fieldOfView;

            var canvas = GameObject.FindGameObjectWithTag("3D Canvas");
            // Initialize the array of gameObjects;
            arGameObjects = new GameObject[4];
            for (int i = 0; i < arGameObjects.Length; i++)
            {
                arGameObjects[i] = GameObject.Instantiate(arGameObjectOrigin);
                arGameObjects[i].SetActive(false);
                // Debug.LogWarning(arGameObjects[i].layer);
                //arGameObjects[i].layer = 5; //UI
                if (canvas != null)
                {
                    //arGameObjects[i].transform.SetParent(canvas.transform);
                }
                else
                {
                    Debug.LogWarning("Cannot find 3D Canvas");
                }
            }
            float width = _imageWidth;
            float height = _imageHeight;
            float imageSizeScale = 1.0f;
            float widthScale = (float)_screenWidth / width;
            float heightScale = (float)_screenHeight / height;
            if (widthScale < heightScale)
            {
                imageSizeScale = (float)_screenHeight / (float)_screenWidth;
            }

            if (_camMatrixValues.Length < CAMMATRIXVALUES_MIN_SIZE)
            {
                _camMatrixValues = new double[CAMMATRIXVALUES_MIN_SIZE];

                //set cameraparam
                int max_d = (int)Mathf.Max(width, height);
                double _fx = max_d;
                double _fy = max_d;
                double _cx = width / 2.0f;
                double _cy = height / 2.0f;

                _camMatrixValues[0] = _fx;
                _camMatrixValues[1] = 0;
                _camMatrixValues[2] = _cx;
                _camMatrixValues[3] = 0;
                _camMatrixValues[4] = _fy;
                _camMatrixValues[5] = _cy;
                _camMatrixValues[6] = 0;
                _camMatrixValues[7] = 0;
                _camMatrixValues[8] = 1.0f;
            }
            camMatrix = new Mat(3, 3, CvType.CV_64FC1);
            MatUtils.copyToMat<double>(_camMatrixValues, camMatrix);
            if (_distCoeffsValues.Length < DISTCOEFFSVALUES_MIN_SIZE)
            {
                _distCoeffsValues = new double[DISTCOEFFSVALUES_MIN_SIZE];
            }
            distCoeffs = new MatOfDouble(_distCoeffsValues);

            //calibration camera
            Size imageSize = new Size(width * imageSizeScale, height * imageSizeScale);
            double apertureWidth = 0;
            double apertureHeight = 0;
            double[] fovx = new double[1];
            double[] fovy = new double[1];
            double[] focalLength = new double[1];
            Point principalPoint = new Point(0, 0);
            double[] aspectratio = new double[1];

            Calib3d.calibrationMatrixValues(camMatrix, imageSize, apertureWidth, apertureHeight, fovx, fovy, focalLength, principalPoint, aspectratio);

            double fx = _camMatrixValues[0];
            double fy = _camMatrixValues[4];
            double cx = _camMatrixValues[2];
            double cy = _camMatrixValues[5];
            double fovXScale = (2.0 * Mathf.Atan((float)(imageSize.width / (2.0 * fx)))) / (Mathf.Atan2((float)cx, (float)fx) + Mathf.Atan2((float)(imageSize.width - cx), (float)fx));
            double fovYScale = (2.0 * Mathf.Atan((float)(imageSize.height / (2.0 * fy)))) / (Mathf.Atan2((float)cy, (float)fy) + Mathf.Atan2((float)(imageSize.height - cy), (float)fy));

            if (arCamera != null)
            {
                //Adjust Unity Camera FOV
                if (widthScale < heightScale)
                {
                    arCamera.fieldOfView = (float)(fovx[0] * fovXScale);
                }
                else
                {
                    arCamera.fieldOfView = (float)(fovy[0] * fovYScale);
                }
            }

            oldPoseData = new PoseData();
            transformMatrix = Matrix4x4.identity;
            arMatrix = Matrix4x4.identity;

            Matrix4x4 openGLCameraProjectionMatrix = ARUtils.CalculateProjectionMatrixFromCameraMatrixValues((float)fx, (float)fy, (float)cx, (float)cy, width, height, arCamera.nearClipPlane, arCamera.farClipPlane);
            Matrix4x4 zaxisInvertionMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1, 1, -1));
            opencvCameraProjectionMatrix = openGLCameraProjectionMatrix * zaxisInvertionMatrix;

            isARGameObjectInARCameraViewport = false;

            hasInitDone = true;
        }

        public virtual void CalculateARMatrix(ref QrMarker marker)
        {
            if (!hasInitDone) return;

            if (arGameObjectOrigin == null)
                return;
            if (arCamera == null)
                return;

            if (camMatrix == null)
                return;
            if (distCoeffs == null)
                return;

            if (marker.ImagePoints == null)
                return;
            if (marker.ObjectPoints == null)
                return;

            if (marker.ImagePoints.Length != marker.ObjectPoints.Length)
                return;
            if (marker.ImagePoints.Length != 4)
                return;
            if (marker.ObjectPoints.Length != 4)
                return;

            Vector2[] imagePoints = marker.ImagePoints;
            Vector3[] objectPoints;
            if (leftHandedCoordinates)
            {
                objectPoints = new Vector3[marker.ObjectPoints.Length];
                for (int i = 0; i < marker.ObjectPoints.Length; i++)
                    objectPoints[i] = new Vector3(marker.ObjectPoints[i].x, -marker.ObjectPoints[i].y, marker.ObjectPoints[i].z);
            }
            else
            {
                objectPoints = marker.ObjectPoints;
            }

            using (MatOfPoint2f m_markerCorners2d = new MatOfPoint2f(imagePoints))
            using (MatOfPoint3f m_markerCorners3d = new MatOfPoint3f(objectPoints))
            {

                // First call to solvePnP
                if (marker.rvec == null || marker.tvec == null)
                {
                    marker.rvec = new Mat(3, 1, CvType.CV_64FC1);
                    marker.tvec = new Mat(3, 1, CvType.CV_64FC1);
                    //Calib3d.solvePnP(m_markerCorners3d, m_markerCorners2d, camMatrix, distCoeffs, rvec, tvec);
                    Calib3d.solvePnP(m_markerCorners3d, m_markerCorners2d, camMatrix, distCoeffs, marker.rvec, marker.tvec, false, Calib3d.SOLVEPNP_UPNP);
                }


                isARGameObjectInARCameraViewport = false;
                double[] tvecValue = new double[3];
                marker.tvec.get(0, 0, tvecValue);
                Vector4 pos = opencvCameraProjectionMatrix * new Vector4((float)tvecValue[0], (float)tvecValue[1], (float)tvecValue[2], 1.0f);
                if (pos.w != 0)
                {
                    float x = pos.x / pos.w, y = pos.y / pos.w, z = pos.z / pos.w;
                    if (x >= -1 && x <= 1 && y >= -1 && y <= 1 && z >= 0 && z <= 1)
                    {
                        isARGameObjectInARCameraViewport = true;
                    }
                }


                if (double.IsNaN(tvecValue[2]) || !isARGameObjectInARCameraViewport)
                {
                    Calib3d.solvePnP(m_markerCorners3d, m_markerCorners2d, camMatrix, distCoeffs, marker.rvec, marker.tvec, false, Calib3d.SOLVEPNP_UPNP);
                }
                else
                {
                    Calib3d.solvePnP(m_markerCorners3d, m_markerCorners2d, camMatrix, distCoeffs, marker.rvec, marker.tvec, false, Calib3d.SOLVEPNP_IPPE_SQUARE);
                }


                if (isARGameObjectInARCameraViewport)
                {
                    // Convert to unity pose data.
                    double[] rvecArr = new double[3];
                    marker.rvec.get(0, 0, rvecArr);
                    double[] tvecArr = new double[3];
                    marker.tvec.get(0, 0, tvecArr);
                    PoseData poseData = ARUtils.ConvertRvecTvecToPoseData(rvecArr, tvecArr);

                    oldPoseData = poseData;

                    transformMatrix = ARUtils.ConvertPoseDataToMatrix(ref oldPoseData, true);
                }
            }

            arMatrix = transformMatrix;

            if (applyXaxisInversionToARMatrix) arMatrix = arMatrix * invertXMatrix;
            if (applyYaxisInversionToARMatrix) arMatrix = arMatrix * invertYMatrix;
            if (applyZaxisInversionToARMatrix) arMatrix = arMatrix * invertZMatrix;
        }

        public virtual void UpdateTransform(GameObject gameObject)
        {
            if (!hasInitDone) return;

            if (updateTarget == UpdateTarget.ARCamera)
            {
                //arMatrix = arGameObjectOrigin.transform.localToWorldMatrix * arMatrix.inverse;
                //ARUtils.SetTransformFromMatrix(arCamera.transform, ref arMatrix);
            }
            else
            {
                arMatrix = arCamera.transform.localToWorldMatrix * arMatrix;
                //ARUtils.SetTransformFromMatrix(gameObject.transform, ref arMatrix);
            }
        }

        public virtual void ResetUpdateTargetTransform()
        {
            if (!hasInitDone) return;

            if (IsObjectDestroyed(arGameObjectOrigin)) return;
            if (IsObjectDestroyed(arCamera)) return;

            arGameObjectOrigin.transform.localPosition = arGameObjectDefaultLocalPosition;
            arGameObjectOrigin.transform.localRotation = arGameObjectDefaultLocalRotation;
            arGameObjectOrigin.transform.localScale = arGameObjectDefaultLocalScale;

            arCamera.transform.localPosition = arCameraDefaultLocalPosition;
            arCamera.transform.localRotation = arCameraDefaultLocalRotation;
            arCamera.transform.localScale = arCameraDefaultLocalScale;
            arCamera.fieldOfView = arCameraDefaultFieldOfView;
        }

        private bool IsObjectDestroyed(UnityEngine.Object obj)
        {
            return obj == null;
        }

        public virtual void SetCamMatrix(Mat camMatrix)
        {
            if (camMatrix != null && !camMatrix.empty())
            {
                _camMatrixValues = new double[camMatrix.total()];
                camMatrix.get(0, 0, _camMatrixValues);
                _camMatrixValues = ValidateArraySize(_camMatrixValues, 9);

                if (hasInitDone) Initialize();
            }

        }

        public virtual void SetDistCoeffs(MatOfDouble distCoeffs)
        {
            if (distCoeffs != null && !distCoeffs.empty())
            {
                _distCoeffsValues = new double[distCoeffs.total()];
                distCoeffs.get(0, 0, _distCoeffsValues);
                _distCoeffsValues = ValidateArraySize(distCoeffsValues, 14);

                if (hasInitDone) Initialize();
            }
        }
    }
}
