using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//Script placed on player prefab
//Holds values of the player, and performs functiosn related to those values
public class PlayerData : MonoBehaviourPunCallbacks
{
    //Player values
    public float maxHealth;
    public float health;
    public List<string> buffs;
    public bool isDead;
    
    //Player Spawn Data
    public float SpawnTime;
    public float SpawnDelay;
    public bool spawning;

    //Player
    private Vector3 playerPos;
    private SpawnManager spawnManager;

    private void Awake()
    {
        playerPos = this.transform.position;
        isDead = false;
        health = maxHealth;
        spawnManager = GameObject.Find("GameManager").GetComponent<SpawnManager>();
    }

    //Function to wait for respawn;
    public void waitForRespawn()
    {
        SpawnTime = SpawnDelay;
        spawning = false;
        //Show RespawnTimer HUD

        //Delay Respawn
        while (!spawning)
        {
            SpawnTime -= Time.deltaTime;
            if (SpawnTime <= 0)
            {
                spawning = true;
            }
        }
    }

    //Keeps syncing data to players
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(health);
        }
        else if (stream.IsReading)
        {
            health = (float)stream.ReceiveNext();
        }
    }
}
