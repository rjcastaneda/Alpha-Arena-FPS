using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MiniGameRoom : MonoBehaviourPunCallbacks
{
    public string MGName;
    public int ID;
    public bool isOccupied = false;

    public void StartMiniGame(GameObject Player)
    {
        if (MGName == "Boids")
        {
            gameObject.GetComponent<BoidsMinigameManager>().StartMiniGame(Player);
        }
    }

    //Pun2 synchronization
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(isOccupied);
        }
        else if (stream.IsReading)
        {
            isOccupied = (bool)stream.ReceiveNext();
        }
    }
}
