using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//Placed on the Game Manager object
public class SpawnManager : MonoBehaviourPunCallbacks
{
    [SerializeField]private List<Spawner> spawnerList;
    private GameObject spawnerHolder;

    // Start is called before the first frame update
    void Start()
    {
        //Initialize Variables
        spawnerHolder = GameObject.Find("Spawners");

        //Initialize Lists
        spawnerList = new List<Spawner>();
        foreach(Transform child in spawnerHolder.transform)
        {
            spawnerList.Add(child.gameObject.GetComponent<Spawner>());
        }

    }

    //Function to respawn the player gameobject;
    public Transform GetSpawnPoint()
    {
        const int OFFSET = 1;
        int spawnNum = Random.Range(0, spawnerList.Count - OFFSET);
        bool foundSpawn = false;
        Transform spawnPos = null;

        //While loop to keep searching for an appropriate spawn point;
        while(!foundSpawn) {
           Spawner spawner = spawnerList[spawnNum];
           
           //We check if spawners are ready and if there are nearby players
           if(spawner.readyForSpawn /*&& spawner.CheckIfNearby()*/) {
               spawnPos = spawner.spawnTransform;
               spawner.SpawnerReset();
               foundSpawn = true; 
           }
           else {
                spawnNum = Random.Range(0, spawnerList.Count - OFFSET);
           }
        }

        return spawnPos;
    }
}
