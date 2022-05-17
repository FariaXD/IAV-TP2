using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Transform[] poles;

    private void Awake() {
        poles = GetComponentsInChildren<Transform>();
    }
}
