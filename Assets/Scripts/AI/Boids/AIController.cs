using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.IO;

public class AIController : MonoBehaviourPunCallbacks
{
    static public List<Boid> boids;
    static public List<GameObject> boidPool;

    [Header("Spawn Settings")]
    public GameObject boidPreFab;
    public Transform boidAnchor;
    public int numBoids;
    public int numSpawned;
    public float spawnRadius = 100f;
    public float spawnDelay = 0.1f;

    [Header("Boid Settings")]
    public float velocity = 50f;
    public float neighborDist = 30f;
    public float collDist = 4f;
    public float collAvoid = 2f;
    public float velMatching = .25f;
    public float flockCentering = .2f;
    public float attractPull = 5f;
    public float attractPush = 2f;
    public float attractPushDist = 1f;


    private void Awake()
    {
        boids = new List<Boid>();
        numSpawned = 0;
        InstantiateBoid();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        RenableBoids();
    }

    public void RenableBoids()
    {
        foreach (Boid boid in boids)
        {
            boid.enabled = true;
        }

        foreach (GameObject boid in boidPool)
        {
            boid.transform.position = transform.position;
            boid.transform.rotation = transform.rotation;
            boid.SetActive(true);
        }
        
        
    }

    //Function to spawn boids into the scene.
    public void InstantiateBoid()
    {
        GameObject boidGO = PhotonNetwork.InstantiateRoomObject(Path.Combine("Prefabs","Boid"), transform.position, transform.rotation);
        Boid boid = boidGO.GetComponent<Boid>();
        boid.transform.SetParent(boidAnchor);
        boids.Add(boid);
        boidGO.SetActive(false);
        numSpawned++;
        while(numSpawned < numBoids)
        {
            Invoke("InstantiateBoid", spawnDelay);
        }
    }
}

