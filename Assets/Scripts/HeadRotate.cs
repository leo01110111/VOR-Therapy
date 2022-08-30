using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;
//using System.Numerics;

public class HeadRotate : MonoBehaviour
{
    public float yRotation;
    public float xRotation;
    public GameObject CameraOffset;
    public GameObject MainCamera;
    
    void Update()
    {
        yRotation = (float)Math.Round(270 - MainCamera.transform.rotation.eulerAngles.y);
        xRotation = (float)Math.Round(MainCamera.transform.rotation.eulerAngles.x);
    }
    public void Reset()
    {

        CameraOffset.transform.Rotate(new Vector3(0, -MainCamera.transform.localEulerAngles.y, 0));
        CameraOffset.transform.Translate(new Vector3(-MainCamera.transform.localPosition.x, 0, -MainCamera.transform.localPosition.z));
        yRotation = 0f;
        yRotation = 0f;

    }
}
