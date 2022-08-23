using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GuideBall : MonoBehaviour
{
    [SerializeField] private Material PointerMaterial;
    [SerializeField] private GameObject GuideSphere;
    [SerializeField] private GameObject PlayerSphere;
    [SerializeField] private GameObject Pointer;
    [SerializeField] private GameObject Stick;
    [SerializeField] private GameObject DurationInput;
    [SerializeField] private GameObject AngleInput;
    [SerializeField] private GameObject SpeedInput;
    [SerializeField] private TextMeshProUGUI UpdateLabel;
    [SerializeField] private TextMeshProUGUI AngleLabel;
    [SerializeField] private TextMeshProUGUI DurationLabel;
    [SerializeField] private TextMeshProUGUI CenterLabel;

    public float guideRadius;
    public float playerRadius;

    public HeadRotate headRotate; //Class that tracks how much the head is turning.

   //public TextMeshProUGUI SpeedLabel;

    private int count;
    private int turningAngle; //How much a person should turn on each side.

    private float actualSpeed; //The speed that is being inputted into the guide sphere.
    private float guideRotation = 0;
    private float initialTime = 0;
    private float duration;
    private float speed; 

    private bool switcher = true;
    private bool running = false;


    void Update()
    {
        //Debug.Log("Reps Done: " + count/2);
        //Debug.Log("Speed: " + speed);
        Debug.Log("Time Left: " + (int)(duration - (Time.time - initialTime)));
        if (headRotate.yRotation > turningAngle || headRotate.yRotation < -turningAngle)
        {
            CenterLabel.color = new Color(255, 0, 0, 255);
            PointerMaterial.color = new Color(1, 0, 0, 1);
            CenterLabel.text = "Don't over rotate!";
        }
        else
        {
            CenterLabel.color = new Color(255, 255, 255, 255);
            PointerMaterial.color = new Color(0, 1, 0, 1);
            CenterLabel.text = Math.Abs(headRotate.yRotation).ToString() + "°";
        }

        guideRotation += actualSpeed * (Time.time - initialTime) * Time.deltaTime;

        Pointer.transform.Rotate(new Vector3(0,(float)((-headRotate.yRotation)-(Pointer.transform.eulerAngles.y-360)), 0), Space.World); //Head Pointer
        PlayerSphere.transform.Translate(new Vector3(-playerRadius * (float)Math.Cos(headRotate.yRotation * (Math.PI / 180)) - PlayerSphere.transform.position.x, 0f, -playerRadius * (float)Math.Sin(headRotate.yRotation * (Math.PI / 180)) - PlayerSphere.transform.position.z));
        GuideSphere.transform.Translate(new Vector3(-guideRadius * (float)Math.Cos(guideRotation * (Math.PI / 180)) - GuideSphere.transform.position.x, 0f, -guideRadius * (float)Math.Sin(guideRotation * (Math.PI / 180)) - GuideSphere.transform.position.z));

        if (GuideSphere.transform.position.z > -guideRadius * (float)Math.Sin(-turningAngle * (Math.PI / 180)) && running == true && switcher == false)
        {
            actualSpeed = speed;
            count++;
            switcher = true;
        }
        else if (GuideSphere.transform.position.z < -guideRadius * (float)Math.Sin(turningAngle * (Math.PI / 180)) && running == true && switcher == true)
        {
            actualSpeed = -speed;
            count++;
            switcher = false;
        }

        if (duration <= (Time.time - initialTime))
        {
            actualSpeed = 0;
            guideRotation = 0;
            running = false;
            UpdateLabel.color = new Color(0, 255, 0, 255);
            UpdateLabel.text = "Exercise complete!";
        }
        else
        {
            UpdateLabel.color = new Color(0, 0, 0, 255);
            int seconds = (int)(duration - (Time.time - initialTime)) % 60;
            int minutes = (int)(duration - (Time.time - initialTime)) / 60;
            UpdateLabel.text = minutes + ":" + seconds;
        }
    }

    public void ExerciseStart()
    {
        initialTime = Time.time;
        switcher = true;
        count = 0;
        guideRotation = 0;
        duration = DurationInput.GetComponent<Slider>().value * 60;
        turningAngle = (int)AngleInput.GetComponent<Slider>().value;
        Debug.Log("Angle " + turningAngle);
        actualSpeed = SpeedInput.GetComponent<Slider>().value;
        Debug.Log("Speed " + actualSpeed);
        speed = actualSpeed;
        running = true;
    }

    public void LabelUpdate()
    {
        AngleLabel.text = "Turning Angle: " + AngleInput.GetComponent<Slider>().value + "°";
        float tmp = AngleInput.GetComponent<Slider>().value;
        DurationLabel.text = "Duration: " + DurationInput.GetComponent<Slider>().value + " minutes";
       // SpeedLabel.text = "Speed: " + SpeedInput.GetComponent<Slider>().value * 60/(4*tmp) + " reps per minute";
    }
}
