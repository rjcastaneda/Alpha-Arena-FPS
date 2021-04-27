using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//Placed on the Game Manager object
public class SpawnManager : MonoBehaviourPunCallbacks
{
    public int numSpawns;
    [SerializeField]
    private List<Spawner> spawnerList;
    private List<Photon.Realtime.Player> players;

    private GameObject spawnerHolder;
    SpawnManager Instance;

    // Start is called before the first frame update
    void Start()
    {
        //Applying singleton 
        Instance = this;

        //Initialize Variables
        spawnerHolder = GameObject.Find("Spawners");

        //Initialize Lists
        spawnerList = new List<Spawner>();
        foreach(Transform child in spawnerHolder.transform)
        {
            spawnerList.Add(child.gameObject.GetComponent<Spawner>());
        }

        players = new List<Photon.Realtime.Player>();
        foreach(Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            players.Add(player);
        }
    }

    //Function to respawn the player gameobject;
    public void respawn(GameObject player)
    {
        const int OFFSET = 1;
        int spawnNum = Random.Range(0, spawnerList.Count - OFFSET);
        bool foundSpawn = false;
        //PlayerData pData = player.getComponent<PlayerData>();
        //pData.reset() basically reset health and stuff.

        //While loop to keep searching for an appropriate spawn point;
        while(!foundSpawn) {
           Spawner spawner = spawnerList[spawnNum];
           
           //We check if spawners are ready and if there are nearby players
           if(spawner.readyForSpawn && spawner.CheckIfNearby()) {
               player.transform.position = spawner.position;
               spawner.SpawnerReset();
               foundSpawn = true; 
           }
           else {
                spawnNum = Random.Range(0, spawnerList.Count - OFFSET);
           }
        }
    }
}
