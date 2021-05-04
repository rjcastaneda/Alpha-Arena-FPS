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

    public void enterMinigame()
    {
        cooldown = timeDelay;
        ready = false;
    }

    private void Start()
    {
        miniGameManager = GameObject.Find("GameManager").GetComponent<MiniGameManager>();

        ready = false;
        cooldown = timeDelay;
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
            if(ready)
            {
                other.gameObject.transform.Find("PlayerHUD");
                //Enable option that when you press e you enter arena;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
              other.gameObject.transform.Find("PlayerHUD");
              //disable Hud once you exit area
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
