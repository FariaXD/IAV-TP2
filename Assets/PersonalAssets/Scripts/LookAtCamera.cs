using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
   public GameObject lookAt;

    void Start(){
        lookAt = GameObject.FindGameObjectWithTag("LookAt");
    }
    void Update()
    {
        transform.LookAt(transform.position + lookAt.transform.rotation * Vector3.forward, lookAt.transform.rotation * Vector3.up);

    }
}
