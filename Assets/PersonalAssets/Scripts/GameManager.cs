using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool racing;
    public GameObject[] cars = new GameObject[14];
    public Transform[] startingPos;

    
    private void Awake() {
        GameObject[] numberOfCars = GameObject.FindGameObjectsWithTag("NumberOfCar");
        int numToAssign = 1;
        foreach(GameObject num in numberOfCars){
            num.GetComponent<TMPro.TextMeshProUGUI>().text = numToAssign.ToString();
            numToAssign++;
        }
        startingPos = GameObject.FindGameObjectWithTag("Grid").GetComponentsInChildren<Transform>();
        if (racing)
        {
            for (int i = 0; i < cars.Length; i++)
            {
                cars[i].transform.localPosition = startingPos[i + 1].position;
                
            }
        }
    }

    private void Start(){
       
    }
}
