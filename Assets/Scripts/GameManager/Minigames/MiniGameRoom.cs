using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MiniGameRoom : MonoBehaviourPunCallbacks
{
    public bool isOccupied;

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
