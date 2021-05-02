using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    [Header("Boid Velocity Vars")]
    public Rigidbody boidRB;
    public Vector3 boidPos;
    public Vector3 velocity;
    public Vector3 velAttract;
    public Vector3 delta;

    [Header("Boid Movement Vars")]
    public Vector3 velAvoid;
    public Vector3 closePos;
    public Vector3 velAlign;
    public Vector3 velCenter;
    public float FDT;

    [Header("Boid State")]
    public bool attracted;
    public string leader;

    private AIController AICont;
    private Attractor attractor;
    private Neighborhood NBHD;

    private void Awake()
    {
        boidRB = this.gameObject.GetComponent<Rigidbody>();
        AICont = GameObject.Find("AIController").GetComponent<AIController>();
        attractor = GameObject.Find("Attractor").GetComponent<Attractor>();
        NBHD = this.gameObject.GetComponent<Neighborhood>();
        boidPos = Random.onUnitSphere * AICont.spawnRadius;
        velocity = Random.onUnitSphere * AICont.spawnRadius;
        boidRB.velocity = velocity;
        LookAhead();
    }

    private void FixedUpdate()
    {
        velocity = boidRB.velocity;
        boidPos = this.gameObject.transform.position;

        //Calculations for Boid properties.
        CalcAvoidance();
        CalcVelMatch();
        CalcCentering();
        CalcAttraction();
        
        //Movement by changing the velocity
        FDT = Time.fixedDeltaTime;
        if (velAvoid != Vector3.zero){
            velocity = Vector3.Lerp(velocity, velAvoid, AICont.collAvoid * FDT);
        }
        else {

            if(velAlign != Vector3.zero){
                velocity = Vector3.Lerp(velocity, velAlign, AICont.velMatching * FDT);
            }

            if(velCenter != Vector3.zero){
                velocity = Vector3.Lerp(velocity, velAlign, AICont.flockCentering * FDT);
            }

            if(velAttract != Vector3.zero)
            {
                if (attracted) {
                    velocity = Vector3.Lerp(velocity, velAttract, AICont.attractPull * FDT);
                }
                else {
                    velocity = Vector3.Lerp(velAttract, -velAttract, AICont.attractPush * FDT);
                }
            }
            
        }

        //Apply the Velocity
        velocity = velocity.normalized * AICont.velocity;
        boidRB.velocity = velocity;
        LookAhead();
    }

    //Function for Collision Avoidance Calculations
    void CalcAvoidance()
    {
        velAvoid = Vector3.zero;
        closePos = NBHD.AvgClosePos();

        if (closePos != Vector3.zero)
        {
            velAvoid = boidPos - closePos;
            velAvoid.Normalize();
            velAvoid *= AICont.velocity;
        }
    }

    //Function for Velocity Matching Calculations
    void CalcVelMatch()
    {

        velAlign = NBHD.AvgVel();
        if (velAlign != Vector3.zero)
        {
            velAlign.Normalize();
            velAlign *= AICont.velocity;
        }
    }

    //Function for Boid Centering Calculations
    void CalcCentering()
    {
        
        velCenter = NBHD.AvgPosition();
        if (velCenter != Vector3.zero)
        {
            velCenter -= transform.position;
            velCenter.Normalize();
            velCenter *= AICont.velocity;
        }
    }

    //function for Attraction Calculations
    void CalcAttraction()
    {
        
        delta = attractor.position - boidPos;
        attracted = (delta.magnitude > AICont.attractPushDist);
        velAttract = delta.normalized * AICont.velocity;
    }

    //Function for boid to look at direction of their velocity.
    void LookAhead(){
        transform.LookAt(boidPos + boidRB.velocity);
    }
}
