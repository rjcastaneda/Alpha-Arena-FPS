using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//To be placed on Spawner objects
public class Spawner : MonoBehaviour
{
    //Spawner Values
    public float timeForSpawn;
    public float delayTime; //Amount of time before spawner can spawn
    public float collRadius; //Area to check for players
    public bool readyForSpawn;
    public List<GameObject> players;
    public Vector3 position;
    public Transform spawnTransform;

    private SphereCollider spawnerCollider;
    private SpawnManager spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        spawnerCollider = this.gameObject.GetComponent<SphereCollider>();
        spawnManager = GameObject.Find("GameManager").GetComponent<SpawnManager>();
        players = new List<GameObject>();
        position = transform.position;
        spawnTransform = this.transform;
        spawnerCollider.radius = collRadius;
        readyForSpawn = true;
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfReady();
    }

    //function to check if any player is near the spawner.
    //Prevents unfair spawning
    public bool CheckIfNearby()
    {
        bool nearby = false;

        if(players.Count > 0) {
            nearby = true;
        }

        return nearby;
    }

    //Function to continously check timer since last spawn.
    public void CheckIfReady()
    {
        if (timeForSpawn <= 0)
        {
            readyForSpawn = true;
            return;
        }
        else if (!readyForSpawn && timeForSpawn >= 0)
        {
            timeForSpawn -= Time.deltaTime;
        }
    }

    //Function to reset spawner
    public void SpawnerReset()
    {
        readyForSpawn = false;
        timeForSpawn = delayTime;
    }

    //Triggers to keep count of the amount players near the spawner.
    private void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;
        if(obj.tag == "Player")
        {   
            if(!players.Contains(obj)){
                players.Add(obj);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject obj = other.gameObject;
        if (obj.tag == "Player")
        {
            if (players.Contains(obj)){
                players.Remove(obj);
            }
        }
    }

    //Keeps syncing spawner data to other players.
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(timeForSpawn);
            stream.SendNext(readyForSpawn);
        }
        else if (stream.IsReading)
        {
            timeForSpawn = (float)stream.ReceiveNext();
            readyForSpawn = (bool)stream.ReceiveNext();
        }
    }
}
