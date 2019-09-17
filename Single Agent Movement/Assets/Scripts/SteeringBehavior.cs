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
        //wanderOrientation = agent.orientation;
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
        return target.position;
    }

    public Vector3 Evade()
    {
        return agent.position;
    }

    
}
