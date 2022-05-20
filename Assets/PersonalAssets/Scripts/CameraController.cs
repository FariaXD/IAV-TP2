using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera frontCamera, backCamera, worldCamera;
    private Camera currentCamera;
    private GameObject lookAt;
    private GameObject numberOfCar;
    // Start is called before the first frame update
    void Start()
    {
        lookAt = GameObject.FindGameObjectWithTag("LookAt");
        numberOfCar = GameObject.FindGameObjectWithTag("NumberOfCar");
        frontCamera.enabled = false;
        backCamera.enabled = false;
        worldCamera.enabled = true;
        currentCamera = worldCamera;
        lookAt.transform.position = currentCamera.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            frontCamera.enabled = true;
            backCamera.enabled = false;
            worldCamera.enabled = false;
            currentCamera = frontCamera;
            lookAt.transform.position = currentCamera.gameObject.transform.position;
            numberOfCar.SetActive(false);

        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            frontCamera.enabled = false;
            backCamera.enabled = true;
            worldCamera.enabled = false;
            currentCamera = backCamera;
            lookAt.transform.position = currentCamera.gameObject.transform.position;
            numberOfCar.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            frontCamera.enabled = false;
            backCamera.enabled = false;
            worldCamera.enabled = true;
            currentCamera = worldCamera;
            lookAt.transform.position = currentCamera.gameObject.transform.position;
            numberOfCar.SetActive(true);
        }


    }
}
