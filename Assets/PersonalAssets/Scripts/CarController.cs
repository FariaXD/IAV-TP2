using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System;


public class CarController : Agent
{
    public WheelCollider[] wheels;
    public GameObject centerOfMass;
    public float motorTorque = 200f;
    public float breakingForce = 200f;
    public float turnRadius = 6;
    public float steeringMax = 45;
    public float downForceValue = 50f;
    private float currentAccelaration = 0f;
    private float currentBreakForce = 0f;
    public Rigidbody currentRigidBody;
    public float KPH;
    private CheckpointManager chkManager;
    private GameObject startingPos;
    private Quaternion startingRotate;

    void ResetSimulation(){
        transform.localPosition = startingPos.transform.position;
        transform.localRotation = startingRotate;
        chkManager.Reset();
    }

    public override void OnEpisodeBegin()
    {
        ResetSimulation();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        if(chkManager.canEndLap){
            Vector3 dirToPos = (chkManager.GetCheckeredFlagPos().position - transform.position).normalized;
            sensor.AddObservation(dirToPos.x);
            sensor.AddObservation(dirToPos.z);
        }
        else{
            Vector3 dirToPos = (chkManager.GetCurrentCheckpointPos().position - transform.position).normalized;
            sensor.AddObservation(dirToPos.x);
            sensor.AddObservation(dirToPos.z);
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float horizontal = actions.ContinuousActions[0];
        float vertical = (actions.ContinuousActions[1] < 0) ? 0 : actions.ContinuousActions[1];
        bool brakePressed = actions.ContinuousActions[2] == 1;

        ForwardVehicle(vertical);
        SteerVehicle(horizontal);
        BrakeVehicle(brakePressed);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxis("Horizontal");
        continuousActions[1] = (Input.GetAxis("Vertical") < 0)?0:Input.GetAxis("Vertical");
        continuousActions[2] = Input.GetKey(KeyCode.Space) ? 1 : 0;
    }

    private void OnTriggerEnter(Collider collision) {
        if (collision.gameObject.TryGetComponent<Checkpoint>(out Checkpoint checkpoint))
        {
            bool validCheckPoint = chkManager.CheckpointPassed(checkpoint);
            if(validCheckPoint) AddReward(1f);
            else AddReward(-1f);
        }

        if(collision.gameObject.TryGetComponent<CheckeredFlag>(out CheckeredFlag checkeredFlag)){
            if(chkManager.canEndLap){
                AddReward(1f);
                EndEpisode();
            }
            else AddReward(-1f);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.TryGetComponent<Ground>(out Ground ground))
        {
            AddReward(-1f);
            EndEpisode();
        }
    }



    void Awake()
    {
        chkManager = GetComponent<CheckpointManager>();
        startingPos = GameObject.FindGameObjectWithTag("StartingPos");
        currentRigidBody.centerOfMass = centerOfMass.transform.localPosition;
        startingRotate = transform.rotation;
    }

    void FixedUpdate()
    {
        AddDownForce();
        AntiRoll(wheels[0], wheels[1]);
        AntiRoll(wheels[2], wheels[3]);
        KPH = Mathf.Round(currentRigidBody.velocity.magnitude * 3.6f); //Calculates KPH
    }

    void ForwardVehicle(float throttle)
    {
        currentAccelaration = (motorTorque / 2) * throttle;
        for (int i = 2; i < wheels.Length; i++)
            wheels[i].motorTorque = currentAccelaration;
        AddReward(throttle/MaxStep);
    }

    void SteerVehicle(float steer)
    {
        //ackerman steering formula
        //steerAngle - Mathf. Rad2Deg * Mathf.Atan (2.55f / (radius + (1.5f / 2))) * horizontalInput;
        float horizontal = steer;
        if (horizontal > 0)
        {
            //rear tracks size is set to 1.5f wheel base has been set to 2.55f
            wheels[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (turnRadius + (1.5f / 2))) * horizontal;
            wheels[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (turnRadius - (1.5f / 2))) * horizontal;
        }
        else if (horizontal < 0)
        {
            wheels[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (turnRadius - (1.5f / 2))) * horizontal;
            wheels[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (turnRadius + (1.5f / 2))) * horizontal;
        }
        else
        {
            wheels[0].steerAngle = 0;
            wheels[1].steerAngle = 0;
        }
    }

    // Stabilize car turning, so it doesnt flip over easily (Simulates Real Stabilizer Bars)
    void AntiRoll(WheelCollider wheelL, WheelCollider wheelR)
    {
        WheelCollider WheelL = wheelL;
        WheelCollider WheelR = wheelR;
        float AntiRoll = 5.0f;
        WheelHit hit;
        float travelL = 1.0f;
        float travelR = 1.0f;

        bool groundedL = WheelL.GetGroundHit(out hit);
        if (groundedL)
            travelL = (-WheelL.transform.InverseTransformPoint(hit.point).y - WheelL.radius) / WheelL.suspensionDistance;

        var groundedR = WheelR.GetGroundHit(out hit);
        if (groundedR)
            travelR = (-WheelR.transform.InverseTransformPoint(hit.point).y - WheelR.radius) / WheelR.suspensionDistance;

        var antiRollForce = (travelL - travelR) * AntiRoll;

        if (groundedL)
            currentRigidBody.AddForceAtPosition(WheelL.transform.up * -antiRollForce,
                   WheelL.transform.position);
        if (groundedR)
            currentRigidBody.AddForceAtPosition(WheelR.transform.up * antiRollForce,
                   WheelR.transform.position);

    }

    void BrakeVehicle(bool brake)
    {
        if (brake)
            currentBreakForce = breakingForce;
        else
            currentBreakForce = 0f;

        for (int i = 0; i < wheels.Length; i++)
            wheels[i].brakeTorque = currentBreakForce;
    }

    void AddDownForce()
    {
        currentRigidBody.AddForce(-transform.up * downForceValue * currentRigidBody.velocity.magnitude);
    }
}
