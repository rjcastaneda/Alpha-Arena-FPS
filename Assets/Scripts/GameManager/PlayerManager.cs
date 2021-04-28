using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

//Scripte placed onto GameManager as child object
public class PlayerManager : MonoBehaviourPunCallbacks
{
    PhotonView PV;
    SpawnManager spawnManager;
    GameObject player;
    PlayerData playerData;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
        spawnManager = GameObject.Find("GameManager").GetComponent<SpawnManager>();
    }

    private void Start()
    {
        if(PV.IsMine){
            CreatePlayer();
        }
    }

    void CreatePlayer()
    {
        Transform spawnpoint = spawnManager.GetSpawnPoint();
        player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), spawnpoint.position, spawnpoint.rotation, 0, new object[] { PV.ViewID });
        playerData = player.GetComponent<PlayerData>();
    }

    public void PlayerDeath()
    {
        playerData.waitForRespawn();
        PhotonNetwork.Destroy(player);
        CreatePlayer();
    }
}
