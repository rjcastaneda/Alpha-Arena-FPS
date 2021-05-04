using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MiniGameManager : MonoBehaviourPunCallbacks
{
    [SerializeField]private List<GameObject> MGRooms;
    [SerializeField]private List<MiniGameRoom> MGRoomScript;
    private Transform MGRoomContainer;

    private BuffsManager buffsManager;

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


    public void EnterRoom(int RoomID, GameObject Player)
    {
        int idx = 0;
        for(int x = 0; x < MGRoomScript.Count; x++)
        {
            if(MGRoomScript[x].ID == RoomID)
            {
                idx = x;
                break;
            }
        }

        Transform spawnpoint = MGRooms[idx].transform.Find("PlayerSpawn").transform;
        Player.transform.position = spawnpoint.position;
        MGRoomScript[idx].isOccupied = true;
    }

    public void ExitRoom(int RoomID, GameObject Player)
    {
        //Get spawnpoint
        //Spawn player there
    }

    public void FailedGame()
    {

    }

    public void WonGame()
    {

    }

    public int FindAvailableRoom()
    {
        foreach(MiniGameRoom room in MGRoomScript)
        {
            if(!room.isOccupied){
                return room.ID;
            }
        }

        return -1;
    }
    
}
