using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class StickAgent : Unity.MLAgents.Agent
{

    public List<Transform> segments;
    public Rigidbody2D body;

    private List<HingeJoint2D> joints;
    private List<Rigidbody2D> bodys;
    private float distance;
    private ArenaScript arena;

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        if (Input.GetKey(KeyCode.W))
        {
            for (int i = 0; i < actionsOut.ContinuousActions.Array.Length; i++)
            {
                actionsOut.ContinuousActions.Array[i] = 1.0f;
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            for (int i = 0; i < actionsOut.ContinuousActions.Array.Length; i++)
            {
                actionsOut.ContinuousActions.Array[i] = -1f;
            }
        }
        else
        {
            for (int i = 0; i < actionsOut.ContinuousActions.Array.Length; i++)
            {
                actionsOut.ContinuousActions.Array[i] = 0f;
            }
        }
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {

        float newDistance = GetDistance();
        //print("reward " + (newDistance - distance));
        AddReward((newDistance - distance)*0.1f);
        distance = newDistance;

        for(int i= 0;i<joints.Count;i++)
        {
            HingeJoint2D hinge = joints[i];
            float middle = (hinge.limits.min + hinge.limits.max) / 2f;
            float normalisedAngle = (hinge.jointAngle - middle) / (hinge.limits.max - middle);
            //float difference = Mathf.Clamp(actionBuffers.ContinuousActions[i], -1, 1) - normalisedAngle;
            float difference = actionBuffers.ContinuousActions[i] - normalisedAngle;
            //print("normalised angle " + normalisedAngle);

            JointMotor2D motor = hinge.motor;
            motor.motorSpeed = Mathf.Clamp(difference * 1000f,-200,200);
            hinge.motor = motor;
        }

        //print("steps " + StepCount + " " + MaxStep);
        if (StepCount > MaxStep - 1)
        {
            EndEpisodeAndRespawn(0.1f);
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //print("collecting");
        foreach(Rigidbody2D rb in bodys)
        {
            //print("rot " + Mathf.Sin(rb.rotation * Mathf.Deg2Rad) + " " + Mathf.Cos(rb.rotation * Mathf.Deg2Rad)+ " " + ((rb.position - body.position)/2.73f));
            sensor.AddObservation(Mathf.Sin(rb.rotation * Mathf.Deg2Rad));
            sensor.AddObservation(Mathf.Cos(rb.rotation * Mathf.Deg2Rad));
            //print("ang vel " + rb.angularVelocity);
            sensor.AddObservation(RescaleValue(rb.angularVelocity, 0, 360, true));

            //print("lin vel " + rb.velocity);
            //sensor.AddObservation(RescaleValue(rb.velocity.x, 0, 10, true));
            //sensor.AddObservation(RescaleValue(rb.velocity.y, 0, 10, true));



            if (rb != body)
            {
                sensor.AddObservation((rb.position - body.position)/2.73f);
                //Vector2 relativeVelocity = rb.velocity - body.velocity;
                //print("rel lin vel " + relativeVelocity);
                //sensor.AddObservation(RescaleValue(relativeVelocity.x, 0, 8, true));
                //sensor.AddObservation(RescaleValue(relativeVelocity.y, 0, 8, true));
            }
        }
        foreach (HingeJoint2D hinge in joints)
        {
            float middle = (hinge.limits.min + hinge.limits.max) / 2f;
            float normalisedAngle = (hinge.jointAngle - middle) / (hinge.limits.max - middle);
            sensor.AddObservation(normalisedAngle);
            //print("angle " + normalisedAngle);
            //print("joint speed " + RescaleValue(hinge.jointSpeed, 0, 200, false));
            sensor.AddObservation(RescaleValue(hinge.jointSpeed, 0, 200, false));
            //print("reaction " + hinge.reactionForce);
            //sensor.AddObservation(RescaleValue(hinge.reactionForce.x, 0, 100, true));
            //sensor.AddObservation(RescaleValue(hinge.reactionForce.y, 0, 100, true));
            //print("reaction2 " + hinge.reactionTorque);
            //sensor.AddObservation(RescaleValue(hinge.reactionTorque, 0, 100, true));
        }
    }

    public override void OnEpisodeBegin()
    {
        joints = new List<HingeJoint2D>();
        bodys = new List<Rigidbody2D>();
        foreach (Transform trans in segments)
        {
            joints.Add(trans.GetComponent<HingeJoint2D>());
            bodys.Add(trans.GetComponent<Rigidbody2D>());
        }
        arena = transform.parent.GetComponent<ArenaScript>();
        distance = GetDistance();
    }

    // minValue can be the middle value if you want to rescale from -1 to 1
    private float RescaleValue(float value, float minValue, float maxValue, bool useAtan)
    {
        float val = (value - minValue) / (maxValue - minValue);
        //print(val);
        if (useAtan)
        {
            return Mathf.Atan(val) / (Mathf.PI / 2f);
        }
        return val;
    }

    private float GetDistance()
    {
        return body.position.x;
    }

    public void EndEpisodeAndRespawn(float reward)
    {
        AddReward(reward);
        EndEpisode();
        arena.EndEpisodeAndRespawn(gameObject);
    }
}
