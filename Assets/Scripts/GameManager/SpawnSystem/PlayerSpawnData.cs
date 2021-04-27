using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Placed on the Player game object
public class PlayerSpawnData : MonoBehaviour
{
    public float SpawnTime;
    public float SpawnDelay;
    public bool isDead;
    public bool spawning;

    private SpawnManager spawnManager;
    //private PlayerData player;

    private void Awake()
    {
        spawnManager = GameObject.Find("GameManager").GetComponent<SpawnManager>();
    }

    private void Update()
    {
        //isDead = player.isDead;
        if (isDead)
        {
            waitForRespawn();
        }
    }

    public void waitForRespawn()
    {
        SpawnTime = SpawnDelay;
        spawning = false;
        //Show RespawnTimer HUD
        
        //Delay Respawn
        while (!spawning)
        {
            SpawnTime -= Time.deltaTime;
            if(SpawnTime <= 0){
                spawning = true;
            }
        }

        //Respawn player
        spawnManager.respawn(this.gameObject);
    }
}
