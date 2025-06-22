using System;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.UnityUtils;
using UnityEngine;

// CoreModule: các kiểu dữ liệu cơ bản như Mat, Point, Scalar, v.v.
// ImgprocModule: các hàm xử lý ảnh như chuyển sang grayscale.
// ObjdetectModule: chứa QRCodeDetector.
// UnityUtils: hỗ trợ chuyển đổi giữa Texture2D của Unity và Mat của OpenCV.


//! Class QrMarker trong script được thiết kế để quản lý thông tin và trạng thái của
//! một mã QR được phát hiện trong ứng dụng thực tế tăng cường (AR).
//! Lớp này đóng vai trò như một nơi lưu trữ dữ liệu liên quan đến mã QR (như nội dung, vị trí, ma trận AR)
//! và cung cấp các phương thức để cập nhật và xử lý vị trí của mã QR trong không gian 3D.
public class QrMarker : IDisposable
{
    public static double UpdateTimeConstant = 1.0; // Hằng số thời gian cập nhật
    public static double smoothThreshold = 1.0; // Ngưỡng mượt mà
    public bool firstDetection = true; // Biến kiểm tra xem đây có phải là lần phát hiện đầu tiên hay không
    public string Content; // Nội dung của mã QR
    public float UpdateTimeFactor = 0.5f; // Hệ số thời gian cập nhật
    public Vector2[] ImagePoints; // Điểm ảnh trong hình ảnh
    public Vector2[] ImagePointsFiltered; // Điểm ảnh đã được lọc
    public Vector3[] ObjectPoints; // Điểm đối tượng trong không gian 3D
    public Matrix4x4 ArMatrix; // Ma trận AR biểu diễn vị trí và góc quay của mã QR
    public GameObject arButton; // Đối tượng GameObject tương ứng với mã QR
    public Mat Corner; // Ma trận chứa các điểm góc của mã QR
    public double TimeFromLastUpdate = 0; // Thời gian từ lần cập nhật cuối, được sử dụng để tính toán thời gian cập nhật
    public static float UpdateTimeLimit = 1.5f; // Giới hạn thời gian cập nhật
    public bool MatrixFilterEnabled = true; // Biến kiểm tra xem có bật bộ lọc ma trận hay không
    public Mat tvec; // Ma trận tvec chứa thông tin về vị trí
    public Mat rvec; // Ma trận rvec chứa thông tin về góc quay
    public Vector3 qrPosition; // Vị trí của mã QR trong không gian 3D

    public QrMarker(string content, Vector2[] imagePoints, Vector3[] objectPoints, Matrix4x4 arMatrix, GameObject arButton, Mat corner)
    {
        Content = content;
        ImagePoints = imagePoints;
        ObjectPoints = objectPoints;
        ArMatrix = arMatrix;
        this.arButton = arButton;
        Corner = corner;
    }

    public QrMarker(float updateFactor, string content, Vector2[] imagePoints, Vector3[] objectPoints, Matrix4x4 arMatrix, GameObject arButton, Mat corner, bool matrixFilterEnabled)
    {
        UpdateTimeConstant = updateFactor;
        Content = content;
        ImagePoints = imagePoints;
        ObjectPoints = objectPoints;
        ArMatrix = arMatrix;
        this.arButton = arButton;
        Corner = corner;
        MatrixFilterEnabled = matrixFilterEnabled;
    }

    public QrMarker()
    {
    }

    //? Phương thức này được sử dụng để thêm thời gian vào biến TimeFromLastUpdate.
    //? Điều này có thể được sử dụng để theo dõi thời gian đã trôi qua kể từ lần cập nhật cuối cùng.
    public void AddTime(float deltaTime)
    {
        TimeFromLastUpdate += deltaTime;
    }

    //? Cập nhật vị trí/góc quay của mã QR
    //? Phương thức này được sử dụng để cập nhật ma trận AR của mã QR.
    //? Nếu bộ lọc ma trận được bật và thời gian từ lần cập nhật cuối lớn hơn ngưỡng mượt mà,
    //? Parameter matrix từ ARHelperMulti.CalculateARMatrix
    public void UpdateArMatrix(Matrix4x4 matrix)
    {
        firstDetection = false;
        if (!MatrixFilterEnabled || TimeFromLastUpdate > smoothThreshold || firstDetection)
        {
            //? nếu tắt bộ lọc , vượt ngưỡng mượt mà hoặc là lần phát hiện đầu tiên 
            ArMatrix = matrix * Matrix4x4.identity;
            //? thì gán ma trận AR bằng ma trận mới
            //? * identity là ma trận đơn vị, tức là không thay đổi gì cả
            UpdateTransform();
            return;
        }
        var oldPos = ArMatrix.GetPosition();
        //? GetPosition là một phương thức để lấy vị trí của ma trận AR

        var newPos = matrix.GetPosition();

        var oldRotation = ArMatrix.rotation;
        //? rotation là thuộc tính để lấy góc quay của ma trận AR

        var newRotation = matrix.rotation;

        var updateRatio = (float)(TimeFromLastUpdate / UpdateTimeConstant);

        //? updateRatio là tỷ lệ thời gian cập nhật,
        //? được tính bằng cách chia thời gian từ lần cập nhật cuối cho hằng số thời gian cập nhật

        var finalPos = Vector3.Lerp(oldPos, newPos, updateRatio);
        //? Lerp là một hàm nội suy tuyến tính giữa hai điểm, để làm muợt mà vị trí di chuyển từ oldPos đến newPos

        var finalRotation = Quaternion.Slerp(oldRotation, newRotation, updateRatio);
        //? Slerp là một hàm nội suy trên mặt cầu giữa hai quaternion,
        //? để làm muợt mà góc quay từ oldRotation đến newRotation

        ArMatrix.SetTRS(finalPos, finalRotation, new Vector3(1, 1, 1));
        //? SetTRS là một phương thức để thiết lập ma trận AR bằng cách sử dụng vị trí, góc quay và tỷ lệ
        //? T => Translation (dịch chuyển)
        //? R => Rotation (quay)
        //? S => Scale (tỷ lệ)
        //? Đầu tiên scale → sau đó xoay → cuối cùng là dịch chuyển.
        //! => Ma trận mới ArMatrix được cập nhật làm mượt → sử dụng cho arButton.
        UpdateTransform();
    }

    private void UpdateTransform()
    {
        if (arButton == null)
        {
            return;
        }
        ARUtils.SetTransformFromMatrix(arButton.transform, ref ArMatrix);
        //? SetTransformFromMatrix là một phương thức để thiết lập vị trí và góc quay của GameObject (AR Button) 
        //? Theo ma trận AR
        qrPosition = arButton.transform.position;

    }



    public void Dispose()
    //? Giải phóng tài nguyên
    //? Phương thức này được sử dụng để giải phóng tài nguyên mà lớp QrMarker đang sử dụng.
    {
        Corner?.Dispose();
        rvec?.Dispose();
        tvec?.Dispose();
    }
}
// }