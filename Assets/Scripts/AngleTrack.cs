using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class AngleTrack : MonoBehaviour
{
    public TextMeshProUGUI LocText;

    void Start()
    {
        
    }

    void Update()
    {
        LocText.text = "Position: " + transform.position.ToString() + "\n Rotation: " + transform.rotation.eulerAngles.ToString();
    }
}
