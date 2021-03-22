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
                //actionsOut.DiscreteActions.Array[i] = 1;
                actionsOut.ContinuousActions.Array[i] = 1.0f;
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            for (int i = 0; i < actionsOut.ContinuousActions.Array.Length; i++)
            {
                //actionsOut.DiscreteActions.Array[i] = -1;
                actionsOut.ContinuousActions.Array[i] = -1f;
            }
        }
        else
        {
            for (int i = 0; i < actionsOut.ContinuousActions.Array.Length; i++)
            {
                //actionsOut.DiscreteActions.Array[i] = 0;
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
            print("done");
            EndEpisodeAndRespawn(0.1f);
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //print("collecting");
        foreach(Rigidbody2D rb in bodys)
        {
            //print("rot " + Mathf.Sin(rb.rotation * Mathf.Deg2Rad) + " " + Mathf.Cos(rb.rotation * Mathf.Deg2Rad)+ " " + (rb.position - body.position));
            sensor.AddObservation(Mathf.Sin(rb.rotation * Mathf.Deg2Rad));
            sensor.AddObservation(Mathf.Cos(rb.rotation * Mathf.Deg2Rad));
            if (rb != body)
            {
                sensor.AddObservation(rb.position - body.position);
            }
        }
        foreach (HingeJoint2D hinge in joints)
        {
            //print("angle " + hinge.jointAngle);
            float middle = (hinge.limits.min + hinge.limits.max) / 2f;
            float normalisedAngle = (hinge.jointAngle - middle) / (hinge.limits.max - middle);
            sensor.AddObservation(hinge);
        }
        /*sensor.AddObservation(body.rotation);
        sensor.AddObservation(RescaleValue(body.position.y, 0, 50, true));
        sensor.AddObservation(RescaleValue(body.position.y, 0, 5, true));

        //print("speed");
        Vector3 speed = body.InverseTransformDirection(body.GetComponent<Rigidbody>().velocity) / 10f;
        sensor.AddObservation(RescaleValue(speed.x, 0, 1, true));
        sensor.AddObservation(RescaleValue(speed.y, 0, 1, true));
        sensor.AddObservation(RescaleValue(speed.z, 0, 1, true));

        //print("angularSpeed");
        Vector3 angularSpeed = body.GetComponent<Rigidbody>().angularVelocity;
        sensor.AddObservation(RescaleValue(angularSpeed.x, 0, 5, true));
        sensor.AddObservation(RescaleValue(angularSpeed.y, 0, 5, true));
        sensor.AddObservation(RescaleValue(angularSpeed.z, 0, 5, true));*/
    }

    public override void OnEpisodeBegin()
    {
        joints = new List<HingeJoint2D>();
        bodys = new List<Rigidbody2D>();
        print("starting");
        foreach (Transform trans in segments)
        {
            //print("adding");
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
