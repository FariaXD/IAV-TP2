using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public Checkpoint[] checkpoints;
    public CheckeredFlag checkeredFlag;
    public int currentCheckPoint = 0;
    public int numOfCheckPoints;
    public bool canEndLap = false;
    void Start()
    {
        GameObject checkpointParent = GameObject.FindGameObjectWithTag("Checkpoints");
        checkpoints = checkpointParent.GetComponentsInChildren<Checkpoint>();
        checkeredFlag = checkpointParent.GetComponentInChildren<CheckeredFlag>();
        numOfCheckPoints = checkpoints.Length;
    }

    public bool CheckpointPassed(Checkpoint passed){
        if(!canEndLap && passed == checkpoints[currentCheckPoint]){
            currentCheckPoint++;
            if(currentCheckPoint == numOfCheckPoints)
                canEndLap = true;
            return true;
        }
        return false;
    }

    public Transform GetCurrentCheckpointPos(){
        return checkpoints[currentCheckPoint].gameObject.transform;
    }

    public Transform GetCheckeredFlagPos(){
        return checkeredFlag.gameObject.transform;
    }

    public void Reset(){
        canEndLap = false;
        currentCheckPoint = 0;
    }


}
