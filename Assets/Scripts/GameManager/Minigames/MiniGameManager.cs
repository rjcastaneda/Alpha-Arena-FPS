using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MiniGameManager : MonoBehaviourPunCallbacks
{
    private List<GameObject> MGRooms;
    private List<MiniGameRoom> MGRoomScript;
    private Transform MGRoomContainer;


    // Start is called before the first frame update
    void Start()
    {
        MGRoomContainer = GameObject.Find("MiniGameRooms").transform;
        foreach (Transform child in MGRoomContainer)
        {
            MGRooms.Add(child.gameObject);
            MGRoomScript.Add(child.gameObject.GetComponent<MiniGameRoom>());
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
