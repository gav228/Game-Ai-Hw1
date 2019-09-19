using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the place to put all of the various steering behavior methods we're going
/// to be using. Probably best to put them all here, not in NPCController.
/// </summary>

public class SteeringBehavior : MonoBehaviour {

    // The agent at hand here, and whatever target it is dealing with
    public NPCController agent;
    public NPCController target;

    // Below are a bunch of variable declarations that will be used for the next few
    // assignments. Only a few of them are needed for the first assignment.

    // For pursue and evade functions
    public float maxPrediction;
    public float maxAcceleration;

    // For arrive function
    public float maxSpeed;
    public float targetRadiusL;
    public float slowRadiusL;
    public float timeToTarget;

    // For Face function
    public float maxRotation;
    public float maxAngularAcceleration;
    public float targetRadiusA;
    public float slowRadiusA;

    // For wander function
    public float wanderOffset;
    public float wanderRadius;
    public float wanderRate;
    private float wanderOrientation;

    // Holds the path to follow
    public GameObject[] Path;
    public int current = 0;

    protected void Start() {
        agent = GetComponent<NPCController>();
        timeToTarget = 3;
        maxPrediction = 1f;
        maxRotation = 1f;
        //wanderOrientation = agent.orientation;
    }

    public float mapToRange(float rotation)
    {
        float rotationRad = rotation * Mathf.Deg2Rad;
        /*if (rotationRad <= (-1 * Mathf.PI))
        {
            rotationRad += 2 * Mathf.PI; //RotationRad is negative and we want it to be positive
        }
        else if (rotationRad > Mathf.PI)
        {
            rotationRad += (-2 * Mathf.PI);
        }*/

        rotationRad = rotationRad - 2 * Mathf.PI * Mathf.Floor((rotationRad + Mathf.PI) / (2 * Mathf.PI));
        return rotationRad;
    }

    public Vector3 Seek()
    {
        return target.position - agent.position;
    }

    public Vector3 Flee()
    {
        return agent.position - target.position;
    }

    public Vector3 Arrive()
    {
        if (target.position[0] - agent.position[0] < 1 && target.position[2] - agent.position[2] < 1)
        {
            agent.velocity = new Vector3(0,0,0);
            return new Vector3(0, 0, 0);
        }
        return (target.position - agent.position) / timeToTarget;
    }

    public Vector3 Persue()
    {
        float prediction = 0;
        Vector3 predicted_position = target.position;

        // distanve to target
        Vector3 direction = target.position - agent.position;
        float distance = direction.sqrMagnitude;
        

        // find current speed
        float speed = agent.velocity.sqrMagnitude;

        if (speed <= (distance / maxPrediction)){
            prediction = maxPrediction;
        } else
        {
            prediction = distance / speed;
        }

        predicted_position += target.velocity * prediction;

        return predicted_position - agent.position;
    }

    public Vector3 Evade()
    {
        float prediction = 0;
        Vector3 predicted_position = target.position;

        // distanve to target
        Vector3 direction = target.position - agent.position;
        float distance = direction.sqrMagnitude;


        // find current speed
        float speed = agent.velocity.sqrMagnitude;

        if (speed <= (distance / maxPrediction))
        {
            prediction = maxPrediction;
        }
        else
        {
            prediction = distance / speed;
        }

        predicted_position += target.velocity * prediction;

        return agent.position - predicted_position;
    }
    public float Face()
    {
        Vector3 Direction = target.position - agent.position;

        if (Direction.magnitude == 0)
        {
            return 0f;
        }
        float newTargetOrientation = Vector3.Angle(Vector3.forward, Direction);

        float rotation = newTargetOrientation - agent.transform.eulerAngles.y;


        rotation = mapToRange(rotation);
        float rotationSize = Mathf.Abs(rotation);

        float targetRotation = maxRotation;
        targetRotation *= rotation / rotationSize;

        return targetRotation - agent.rotation;
    }

    public float Align()
    {
        float rotation = Vector3.Angle(Vector3.forward, target.position) - agent.transform.eulerAngles.y;
        print(Vector3.Angle(Vector3.forward, target.position));

        rotation = mapToRange(rotation);
        float rotationSize = Mathf.Abs(rotation);

        float targetRotation = maxRotation;
        targetRotation *= rotation / rotationSize;
        
        return targetRotation - agent.rotation;
 
    }




}
