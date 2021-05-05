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

    [Header("Objects")]
    [SerializeField ]private Attractor attractor;
    [SerializeField] private BoidsMinigameManager boidsMiniGameManager;

    //Insantiate boids then disable
    private void Start()
    {
        boids = new List<Boid>();
        boidPool = new List<GameObject>();
        numSpawned = 0;
        InstantiateBoid();
    }

    [PunRPC]
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

    [PunRPC]
    public void DisabledBoids()
    {
        foreach (Boid boid in boids)
        {
            boid.enabled = false;
        }

        foreach (GameObject boid in boidPool)
        {
            boid.transform.position = transform.position;
            boid.transform.rotation = transform.rotation;
            boid.SetActive(false);
        }
    }

    //Function to spawn boids into the scene.
    [PunRPC]
    public void InstantiateBoid()
    {
        while(numSpawned < numBoids)
        {
            GameObject boidGO = Instantiate(boidPreFab, transform.position, transform.rotation);
            Boid boid = boidGO.GetComponent<Boid>();
            boid.transform.SetParent(boidAnchor);
            boid.parent = boidAnchor;
            boid.AICont = this;
            boid.attractor = attractor;
            boid.boidsMiniGameManager = boidsMiniGameManager;
            boidGO.GetComponent<Neighborhood>().AICont = this;
            boids.Add(boid);
            boidPool.Add(boidGO);
            boidGO.SetActive(false);
            numSpawned++;
        }
    }
}

