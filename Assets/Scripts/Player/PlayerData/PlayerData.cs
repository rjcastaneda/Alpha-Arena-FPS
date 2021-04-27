using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//Script placed on player prefab
//Holds values of the player, and performs functiosn related to those values
public class PlayerData : MonoBehaviourPunCallbacks
{
    public float maxHealth;
    public float health;
    public List<string> buffs;
    public bool isDead;
    private Vector3 playerPos;

    private void Awake()
    {
        playerPos = this.transform.position;
        isDead = false;
        health = maxHealth;
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
