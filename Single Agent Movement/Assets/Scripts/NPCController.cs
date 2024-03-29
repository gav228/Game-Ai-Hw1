﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCController : MonoBehaviour {
    // Store variables for objects
    private SteeringBehavior ai;    // Put all the brains for steering in its own module
    private Rigidbody rb;           // You'll need this for dynamic steering

    // For speed 
    public Vector3 position;        // local pointer to the RigidBody's Location vector
    public Vector3 velocity;        // Will be needed for dynamic steering

    // For rotation
    public float orientation;       // scalar float for agent's current orientation
    public float rotation;          // Will be needed for dynamic steering

    public float maxSpeed;          // what it says

    public int mapState;            // use this to control which "phase" the demo is in

    private Vector3 linear;         // The resilts of the kinematic steering requested
    private float angular;          // The resilts of the kinematic steering requested

    public Text label;              // Used to displaying text nearby the agent as it moves around
    LineRenderer line;              // Used to draw circles and other things

    private void Start() {
        ai = GetComponent<SteeringBehavior>();
        rb = GetComponent<Rigidbody>();
        line = GetComponent<LineRenderer>();
        position = rb.position;
        orientation = transform.eulerAngles.y;
        maxSpeed = 7;
        linear = new Vector3(0,0,0);
        angular = 0;
    }

    /// <summary>
    /// Depending on the phase the demo is in, have the agent do the appropriate steering.
    /// 
    /// </summary>
    void FixedUpdate() {
        switch (mapState) {
            case 0:
                linear = ai.Seek();
                if (label)
                {
                    // replace "First algorithm" with the name of the actual algorithm you're demoing
                    // do this for each phase
                    label.text = name.Replace("(Clone)", "") + "\nAlgorithm: Seek algorithm";
                }
                break;
            case 1:
                linear = ai.Flee();
                if (label) {
                    // replace "First algorithm" with the name of the actual algorithm you're demoing
                    // do this for each phase
                    label.text = name.Replace("(Clone)","") + "\nAlgorithm: Flee algorithm"; 
                }
                // linear = ai.Pursue();   // For example
                // angular = ai.Face();    // For example

                // linear = ai.whatever();  -- replace with the desired calls
                // angular = ai.whatever();
                break;

            case 2:
                linear = ai.Arrive();
                if (label) {
                    label.text = name.Replace("(Clone)", "") + "\nAlgorithm: Arrive algorithm";
                }

                // linear = ai.whatever();  -- replace with the desired calls
                // angular = ai.whatever();
                break;

            case 3:
                linear = ai.Persue();
                if (label) {
                    label.text = name.Replace("(Clone)", "") + "\nAlgorithm: Pursue algorithm";
                }

                // linear = ai.whatever();  -- replace with the desired calls
                // angular = ai.whatever();
                break;

            case 4:
                linear = ai.Evade();
                if (label) {
                    label.text = name.Replace("(Clone)", "") + "\nAlgorithm: Evade algorithm";
                }

                // linear = ai.whatever();  -- replace with the desired calls
                // angular = ai.whatever();
                break;
            case 5:
                angular = ai.Align();
                if (label) {
                    label.text = name.Replace("(Clone)", "") + "\nAlgorithm: Align algorithm";
                }

                // linear = ai.whatever();  -- replace with the desired calls
                // angular = ai.whatever();
                break;
            case 6:
                linear = ai.Arrive();
                angular = ai.Align();
                if (label)
                {
                    label.text = name.Replace("(Clone)", "") + "\nAlgorithm: Align and Arrive";
                }
                break;
            case 7:
                angular = ai.Face();
                if (label)
                {
                    label.text = name.Replace("(Clone)", "") + "\nAlgorithm: Face";
                }
                break;
            case 8:
                angular = ai.Face();
                linear = ai.Arrive();
                if (label)
                {
                    label.text = name.Replace("(Clone)", "") + "\nAlgorithm: Face and Arrive";
                }
                break;
            // ADD CASES AS NEEDED
            case 20:
                return;
        }
        UpdateMovement(linear, angular, Time.deltaTime);
        DrawCircle(linear, 1);
        if (label) {
            label.transform.position = Camera.main.WorldToScreenPoint(this.transform.position);
        }
        
    }

    /// <summary>
    /// UpdateMovement is used to apply the steering behavior output to the agent itself.
    /// It also brings together the linear and acceleration elements so that the composite
    /// result gets applied correctly.
    /// </summary>
    /// <param name="steeringlin"></param>
    /// <param name="steeringang"></param>
    /// <param name="time"></param>
    private void UpdateMovement(Vector3 steeringlin, float steeringang, float time) {
        // Update the orientation, velocity and rotation
        orientation += rotation * time;
        velocity += steeringlin * time;
        rotation += steeringang * time;

        if (velocity.magnitude > maxSpeed) {
            velocity.Normalize();
            velocity *= maxSpeed;
        }

        rb.AddForce(velocity - rb.velocity, ForceMode.VelocityChange);
        position = rb.position;
        rb.MoveRotation(Quaternion.Euler(new Vector3(0, Mathf.Rad2Deg * orientation, 0)));
    }

    // <summary>
    // The next two methods are used to draw circles in various places as part of demoing the
    // algorithms.

    /// <summary>
    /// Draws a circle with passed-in radius around the center point of the NPC itself.
    /// </summary>
    /// <param name="radius">Desired radius of the concentric circle</param>
    public void DrawConcentricCircle(float radius) {
        line.positionCount = 51;
        line.useWorldSpace = false;
        float x;
        float z;
        float angle = 20f;

        for (int i = 0; i < 51; i++) {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            z = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

            line.SetPosition(i, new Vector3(x, 0, z));
            angle += (360f / 51);
        }
    }

    /// <summary>
    /// Draws a circle with passed-in radius and arbitrary position relative to center of
    /// the NPC.
    /// </summary>
    /// <param name="position">position relative to the center point of the NPC</param>
    /// <param name="radius">>Desired radius of the circle</param>
    public void DrawCircle(Vector3 position, float radius) {
        line.positionCount = 51;
        line.useWorldSpace = false;
        float x;
        float z;
        float angle = 20f;

        for (int i = 0; i < 51; i++) {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            z = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

            line.SetPosition(i, new Vector3(x, 1, z)+position);
            angle += (360f / 51);
        }
    }

    /// <summary>
    /// This is used to help erase the prevously drawn line or circle
    /// </summary>
    public void DestroyPoints() {
        if (line) {
            line.positionCount = 0;
        }
    }
}
