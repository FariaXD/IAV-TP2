using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TrackManager : MonoBehaviour
{

    public String wallTag;
    public String grassTag;
    public int outOfBoundsLayer;
    private void Awake() {
        AddCorrectTagsToTrack();
    }

    void AddCorrectTagsToTrack()
    {
        GameObject[] walls = FindGameObjectsWithName("Collider-Fences");
        GameObject[] grasses = FindGameObjectsWithName("Collider-Grass");
        foreach (GameObject wall in walls){
            wall.tag = wallTag;
            wall.layer = outOfBoundsLayer;
            wall.AddComponent<OutofBounds>();
        }

        foreach (GameObject grass in grasses)
        {
            grass.tag = grassTag;
            grass.layer = outOfBoundsLayer;
            grass.AddComponent<Grass>();
        }
           
    }

    GameObject[] FindGameObjectsWithName(string name)
    {
        GameObject[] gameObjects = GameObject.FindObjectsOfType<GameObject>();
        GameObject[] arr = new GameObject[gameObjects.Length];
        int FluentNumber = 0;
        for (int i = 0; i < gameObjects.Length; i++)
        {
            if (gameObjects[i].name == name)
            {
                arr[FluentNumber] = gameObjects[i];
                FluentNumber++;
            }
        }
        Array.Resize(ref arr, FluentNumber);
        return arr;
    }
}
