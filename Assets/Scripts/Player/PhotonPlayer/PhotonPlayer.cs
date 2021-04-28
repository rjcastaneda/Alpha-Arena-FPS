using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//Script placed on player
//Handles
public class PhotonPlayer : MonoBehaviourPunCallbacks
{
    PhotonView PV;
    PlayerData playerData;
    PlayerManager playerManager;

    public void Awake()
    {
        PV = this.gameObject.GetComponent<PhotonView>();
        playerData = this.gameObject.GetComponent<PlayerData>();
        //playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
    }

    //Damage taken runs on shooter's view but data is sent to all
    public void TakeDamage(float damage)
    {
        PV.RPC("RPC_TakeDamage", RpcTarget.All, damage);
    }

    //Damage taken that runs on the victim's computer
    [PunRPC]
    void RPC_TakeDamage(float damage)
    {
        if (!PV.IsMine) { return; }
        playerData.health -= damage;

        if(playerData.health <= 0)
        {
            playerManager.PlayerDeath();
        }
    }
}
