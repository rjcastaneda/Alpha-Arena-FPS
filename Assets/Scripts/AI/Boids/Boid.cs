using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Boid : MonoBehaviourPun
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

    public Transform parent;
    
    public AIController AICont;
    public Attractor attractor;
    public Neighborhood NBHD;
    public BoidsMinigameManager boidsMiniGameManager;

    private void Awake()
    {
        boidRB = this.gameObject.GetComponent<Rigidbody>();
        NBHD = this.gameObject.GetComponent<Neighborhood>();
    }

    [PunRPC]
    private void OnEnable()
    {
        boidPos = Random.onUnitSphere * AICont.spawnRadius;
        velocity = Random.onUnitSphere * AICont.spawnRadius;
        boidRB.velocity = velocity;
        LookAhead();
    }

    [PunRPC]
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

    [PunRPC]
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

    [PunRPC]
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

    [PunRPC]
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

    [PunRPC]
    //function for Attraction Calculations
    void CalcAttraction()
    {
        
        delta = attractor.position - boidPos;
        attracted = (delta.magnitude > AICont.attractPushDist);
        velAttract = delta.normalized * AICont.velocity;
    }

    [PunRPC]
    //Function for boid to look at direction of their velocity.
    void LookAhead(){
        transform.LookAt(boidPos + boidRB.velocity);
    }

    [PunRPC]
    public void die()
    {
        boidsMiniGameManager.AddToScore();
        this.gameObject.SetActive(false);
        this.enabled = false;
    }
}
