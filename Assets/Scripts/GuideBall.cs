using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using TMPro;
using System.Reflection;

public class GuideBall : MonoBehaviour
{
    public float turningAngle = 20f; //How much a person should turn on each side.
    public int speed; //Number between 1 and 3. 1 = slow, 2 = medium, 3 = fast.
    public HeadRotate headRotate; //class that tracks how much the head is turning.
    public TextMeshProUGUI Test;
    public GameObject GuideSphere;
    public GameObject Pointer;
    public GameObject Stick;
    private Renderer render; 



    float radius = -1f;
    void Awake()
    {
        render = Stick.GetComponent<Renderer>();
    }

    void Update()
    {
        //Test.text = Pointer.transform.eulerAngles.y.ToString();
        if (headRotate.yRotation > turningAngle || headRotate.yRotation < -turningAngle)
        {
            Test.text = "Don't over rotate!";
            render.material.SetColor("_Color", new Color(0, 0, 0));
        }
        else
        {
            Test.text = "";
            render.material.SetColor("_Color", new Color(1, 1, 1));
        }
        Pointer.transform.Rotate(new Vector3(0,(float)((-headRotate.yRotation)-(Pointer.transform.eulerAngles.y-360)), 0), Space.World); //Head Pointer
        GuideSphere.transform.Translate(new Vector3(radius *(float)Math.Cos(headRotate.yRotation * (Math.PI / 180)) - GuideSphere.transform.position.x,0f,radius*(float)Math.Sin(headRotate.yRotation * (Math.PI / 180)) - GuideSphere.transform.position.z));
    }
}
