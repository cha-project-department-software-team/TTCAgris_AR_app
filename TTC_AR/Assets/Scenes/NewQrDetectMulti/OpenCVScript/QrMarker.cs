using System;
using System.Collections;
using System.Collections.Generic;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.UnityUtils;
using Unity.VisualScripting;
using UnityEngine;

public class QrMarker : IDisposable
{
    public static double UpdateTimeConstant = 2.0;
    public static double smoothThreshold = 1.0;
    public bool firstDetection = true;
    public string Content;
    public Vector2[] ImagePoints;
    public Vector3[] ObjectPoints;
    public Matrix4x4 ArMatrix;
    public GameObject GameObject;
    public Mat Corner;
    public double TimeFromLastUpdate = 0;
    public static float UpdateTimeLimit = 2.0f;
    public bool MatrixFilterEnabled = true;
    public Mat tvec;
    public Mat rvec;
    public Vector3 qrPosition;

    public QrMarker(string content, Vector2[] imagePoints, Vector3[] objectPoints, Matrix4x4 arMatrix, GameObject gameObject, Mat corner)
    {
        Content = content;
        ImagePoints = imagePoints;
        ObjectPoints = objectPoints;
        ArMatrix = arMatrix;
        GameObject = gameObject;
        Corner = corner;
    }

    public QrMarker(float updateFactor, string content, Vector2[] imagePoints, Vector3[] objectPoints, Matrix4x4 arMatrix, GameObject gameObject, Mat corner, bool matrixFilterEnabled)
    {
        UpdateTimeConstant = updateFactor;
        Content = content;
        ImagePoints = imagePoints;
        ObjectPoints = objectPoints;
        ArMatrix = arMatrix;
        GameObject = gameObject;
        Corner = corner;
        MatrixFilterEnabled = matrixFilterEnabled;
    }

    public QrMarker()
    {
    }

    public void AddTime(float deltaTime)
    {
        TimeFromLastUpdate += deltaTime;
    }

    public void UpdateArMatrix(Matrix4x4 matrix)
    {
        firstDetection = false;
        if (!MatrixFilterEnabled || TimeFromLastUpdate > smoothThreshold || firstDetection)
        {
            ArMatrix = matrix * Matrix4x4.identity;
            UpdateTransform();
            return;
        }
        var oldPos = ArMatrix.GetPosition();
        var newPos = matrix.GetPosition();
        var oldRotation = ArMatrix.rotation;
        var newRotation = matrix.rotation;
        var updateRatio = (float)(TimeFromLastUpdate / UpdateTimeConstant);
        var finalPosition = Vector3.Lerp(oldPos, newPos, updateRatio);
        var finalRotation = Quaternion.Slerp(oldRotation, newRotation, updateRatio);
        ArMatrix.SetTRS(finalPosition, finalRotation, new Vector3(1, 1, 1));
        UpdateTransform();
    }

    private void UpdateTransform()
    {
        if (GameObject == null)
        {
            return;
        }
        ARUtils.SetTransformFromMatrix(GameObject.transform, ref ArMatrix);
        qrPosition = GameObject.transform.position;
    }

    public void Dispose()
    {
        Corner?.Dispose();
        rvec?.Dispose();
        tvec?.Dispose();
    }
}
// }
