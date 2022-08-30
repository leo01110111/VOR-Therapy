using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GuideBall : MonoBehaviour
{
    [SerializeField] private Material PointerMaterial; //Color of sphere
    [SerializeField] private GameObject GuideSphere; //Sphere that guides user
    [SerializeField] private GameObject PlayerSphere; //User controller sphere
    [SerializeField] private GameObject DurationInput; //Slider Input
    [SerializeField] private GameObject AngleInput; //Slider Input
    [SerializeField] private GameObject SpeedInput; //Slider Input
    [SerializeField] private TextMeshProUGUI UpdateLabel;
    [SerializeField] private TextMeshProUGUI AngleLabel;
    [SerializeField] private TextMeshProUGUI DurationLabel;
    [SerializeField] private TextMeshProUGUI CenterLabel; //Shows degrees and warning
    [SerializeField] private HeadRotate headRotate;  //Class that tracks how much the head is turning.
    [SerializeField] private AudioSource PaceAudio; //Metronome sound
    [SerializeField] private AudioSource ErrorAudio; //Warning sound
    [SerializeField] private GameObject OptionInput; //Exercise type dropdown

    public float guideRadius;
    public float playerRadius;
    public float duration;

    public float playerHeight;

    private int count;
    private int turningAngle; //How much a person should turn on each side.
    private int choiceIndex;

    private float actualSpeed; //The speed that is being inputted into the guide sphere.
    public float guideRotation = 0f;
    private float initialTime = 0f;
    private float speed; //Speed value determined by slider, shouldn't changed in Update(). Use actualspeed instead

    //Switches for one time actions in loops and running with shows the state of program
    private bool resetSwitcher = true;
    private bool limitSwitcher = true;
    private bool running = false;
    private bool audioSwitcher = true; 


    void Update()
    {
        playerHeight = 1.6f;
        Debug.Log(headRotate.xRotation);
        if (running == true) {
            if (choiceIndex == 0)
            {
                if (resetSwitcher == true) // resets pos of spheres
                {
                    ResetPosition(GuideSphere, -2.098f, 1.6f, 0f);
                    ResetPosition(PlayerSphere, -2.098f, 1.6f, 0f);
                    resetSwitcher = false;
                }
                horizontalVOR(); //function that moves spheres accordingly
            }
            else if (choiceIndex == 1)
            {
                if (resetSwitcher == true) 
                {
                    ResetPosition(GuideSphere, -2.098f, 1.6f, 0f);
                    ResetPosition(PlayerSphere, -2.098f, 1.6f, 0f);
                    resetSwitcher = false;
                }
                verticalVOR();

            }

            if (duration <= (Time.time - initialTime)) //timer
            {
                actualSpeed = 0;
                guideRotation = 0;
                running = false;
                UpdateLabel.color = new Color(0, 255, 0, 255);
                UpdateLabel.text = "Exercise complete!";
            }
            else
            {
                UpdateLabel.color = new Color(1, 1, 1, 255);
                int seconds = (int)(duration - (Time.time - initialTime)) % 60;
                int minutes = (int)(duration - (Time.time - initialTime)) / 60;
                if (seconds % 10 == seconds)
                    UpdateLabel.text = minutes + ":" + "0" + seconds;
                else
                    UpdateLabel.text = minutes + ":" + seconds;
            }

        }
    }
    void horizontalVOR()
    {
        if (headRotate.yRotation > turningAngle || headRotate.yRotation < -turningAngle)
        {
            CenterLabel.color = new Color(255, 0, 0, 255);
            PointerMaterial.color = new Color(1, 0, 0, 1);
            CenterLabel.text = "Don't over rotate!";
            if (audioSwitcher == true)
            {
                ErrorAudio.Play();
                audioSwitcher = false;
            }
                
        }
        else
        {
            CenterLabel.color = new Color(255, 255, 255, 255);
            PointerMaterial.color = new Color(0, 1, 0, 1);
            CenterLabel.text = Math.Abs(headRotate.yRotation).ToString() + "°";
            audioSwitcher = true;
        }

        guideRotation += actualSpeed * Time.deltaTime; //angle change
        //pos of player and guide sphere in circular path
        PlayerSphere.transform.Translate(new Vector3(-playerRadius * (float)Math.Cos(headRotate.yRotation * (Math.PI / 180)) - PlayerSphere.transform.position.x, 0f, -playerRadius * (float)Math.Sin(headRotate.yRotation * (Math.PI / 180)) - PlayerSphere.transform.position.z));
        GuideSphere.transform.Translate(new Vector3(-guideRadius * (float)Math.Cos(guideRotation * (Math.PI / 180)) - GuideSphere.transform.position.x, 0f, -guideRadius * (float)Math.Sin(guideRotation * (Math.PI / 180)) - GuideSphere.transform.position.z));

        //changes direction of guide when reaches angle limit
        if (GuideSphere.transform.position.z > -guideRadius * (float)Math.Sin(-turningAngle * (Math.PI / 180)) && limitSwitcher == false)
        {
            PaceAudio.Play();
            actualSpeed = speed;
            count++;
            limitSwitcher = true;
        }
        else if (GuideSphere.transform.position.z < -guideRadius * (float)Math.Sin(turningAngle * (Math.PI / 180)) && limitSwitcher == true)
        {
            PaceAudio.Play();
            actualSpeed = -speed;
            count++;
            limitSwitcher = false;
        }
    }

    //pretty similar to horizontalVOR, just with a vertical path instead.
    void verticalVOR()
    {
        Debug.Log(headRotate.xRotation);

        if (headRotate.xRotation > turningAngle)
        {
            if(headRotate.xRotation < 180)
            {
                CenterLabel.color = new Color(255, 0, 0, 255);
                PointerMaterial.color = new Color(1, 0, 0, 1);
                CenterLabel.text = "Don't over rotate!";
                if (audioSwitcher == true)
                {
                    ErrorAudio.Play();
                    audioSwitcher = false;
                }
            }
        }
        else
        {
            CenterLabel.color = new Color(255, 255, 255, 255);
            PointerMaterial.color = new Color(0, 1, 0, 1);
            CenterLabel.text = Math.Abs(headRotate.xRotation).ToString() + "°";
            audioSwitcher = true;
        }

        if (360 - headRotate.xRotation > turningAngle)
        {
            if (headRotate.xRotation > 180)
            {
                CenterLabel.color = new Color(255, 0, 0, 255);
                PointerMaterial.color = new Color(1, 0, 0, 1);
                CenterLabel.text = "Don't over rotate!";
                if (audioSwitcher == true)
                {
                    ErrorAudio.Play();
                    audioSwitcher = false;
                }
            }

        }
        else
        {
            CenterLabel.color = new Color(255, 255, 255, 255);
            PointerMaterial.color = new Color(0, 1, 0, 1);
            CenterLabel.text = Math.Abs(360-headRotate.xRotation).ToString() + "°";
            audioSwitcher = true;
        }

        guideRotation += actualSpeed * Time.deltaTime;

        PlayerSphere.transform.Translate(new Vector3(-playerRadius * (float)Math.Cos(headRotate.xRotation * (Math.PI / 180)) - PlayerSphere.transform.position.x, -playerRadius * (float)Math.Sin(headRotate.xRotation * (Math.PI / 180)) - PlayerSphere.transform.position.y + playerHeight , 0f));
        GuideSphere.transform.Translate(new Vector3(-guideRadius * (float)Math.Cos(guideRotation * (Math.PI / 180)) - GuideSphere.transform.position.x, -guideRadius * (float)Math.Sin(guideRotation * (Math.PI / 180)) - GuideSphere.transform.position.y + playerHeight, 0f));

        if (GuideSphere.transform.position.y > -guideRadius * (float)Math.Sin(-turningAngle * (Math.PI / 180)) + playerHeight && limitSwitcher == false)
        {
            PaceAudio.Play();
            actualSpeed = speed;
            count++;
            limitSwitcher = true;
        }
        else if (GuideSphere.transform.position.y < -guideRadius * (float)Math.Sin(turningAngle * (Math.PI / 180)) + playerHeight && limitSwitcher == true)
        {
            PaceAudio.Play();
            actualSpeed = -speed;
            count++;
            limitSwitcher = false;
        }
    }

    void ResetPosition(GameObject obj, float x, float y, float z)
    {
        obj.transform.position = new Vector3(x, y, z);
    }

    //gets slider values and other settings as well as reset variables and states
    public void ExerciseStart()
    {
        initialTime = Time.time;
        limitSwitcher = true;
        count = 0;
        guideRotation = 0;
        duration = DurationInput.GetComponent<Slider>().value * 60;
        turningAngle = (int)AngleInput.GetComponent<Slider>().value;
        Debug.Log("Angle " + turningAngle);
        actualSpeed = SpeedInput.GetComponent<Slider>().value;
        Debug.Log("Speed " + actualSpeed);
        speed = actualSpeed;
        choiceIndex = OptionInput.GetComponent<TMP_Dropdown>().value;
        running = true;
        resetSwitcher = true;
    }

    public void ToggleRun()
    {
        running = !running;
    }

    public void LabelUpdate()
    {
        AngleLabel.text = "Turning Angle: " + AngleInput.GetComponent<Slider>().value + "°";
        float tmp = AngleInput.GetComponent<Slider>().value;
        DurationLabel.text = "Duration: " + DurationInput.GetComponent<Slider>().value + " minutes";
    }
}
