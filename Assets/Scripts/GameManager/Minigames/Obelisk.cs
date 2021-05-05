using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Obelisk : MonoBehaviourPunCallbacks
{
    public float timeDelay = 30;
    public float cooldown;
    public bool ready;

    private MiniGameManager miniGameManager;

    private void Start()
    {
        ready = false;
        cooldown = timeDelay;
        miniGameManager = GameObject.Find("GameManager").GetComponent<MiniGameManager>();
    }

    public void EnterRoom(GameObject Player)
    {
        int Room = miniGameManager.FindAvailableRoom();
        if(Room != -1)
        {
            cooldown = timeDelay;
            ready = false;
            MiniGameIndicator MGOther = Player.gameObject.GetComponent<MiniGameIndicator>();
            MGOther.enabled = false;
            MGOther.crntObelisk = null;
            miniGameManager.EnterRoom(Room, Player);
        }
        
    }

    void checkCooldown()
    {   
        if(!ready)
        {
            cooldown -= Time.fixedDeltaTime;
            if (cooldown <= 0) {
                ready = true;
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        checkCooldown();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            PhotonView PV = other.gameObject.GetComponent<PhotonView>();
            if(ready && PV.IsMine){
               MiniGameIndicator MGOther = other.gameObject.GetComponent<MiniGameIndicator>();
               MGOther.enabled = true;
               MGOther.crntObelisk = this;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PhotonView PV = other.gameObject.GetComponent<PhotonView>();
        if (other.tag == "Player" && PV.IsMine)
        {
            MiniGameIndicator MGOther = other.gameObject.GetComponent<MiniGameIndicator>();
            MGOther.enabled = false;
            MGOther.crntObelisk = null;
        }
    }

    //Pun2 synchronization
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(cooldown);
            stream.SendNext(ready);
        }
        else if (stream.IsReading)
        {
            cooldown = (float)stream.ReceiveNext();
            ready = (bool)stream.ReceiveNext();
        }
    }
}
