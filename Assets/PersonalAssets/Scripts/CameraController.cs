using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera frontCamera, backCamera, worldCamera;
    // Start is called before the first frame update
    void Start()
    {
        frontCamera.enabled = false;
        backCamera.enabled = false;
        worldCamera.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            frontCamera.enabled = true;
            backCamera.enabled = false;
            worldCamera.enabled = false;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            frontCamera.enabled = false;
            backCamera.enabled = true;
            worldCamera.enabled = false;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            frontCamera.enabled = false;
            backCamera.enabled = false;
            worldCamera.enabled = true;
        }

    }
}
