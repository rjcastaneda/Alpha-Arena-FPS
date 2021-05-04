using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//Master script that handles Room information and has functions, placed on game manager
public class MiniGameManager : MonoBehaviourPunCallbacks
{
    [SerializeField]private List<GameObject> MGRooms;
    [SerializeField]private List<MiniGameRoom> MGRoomScript;
    private Transform MGRoomContainer;

    private BuffsManager buffsManager;
    private SpawnManager spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        MGRoomContainer = GameObject.Find("MiniGameRooms").transform;
        spawnManager = GameObject.Find("GameManager").GetComponent<SpawnManager>();
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
        Transform spawn = spawnManager.GetSpawnPoint();

        //Exit MiniGameRoom
        for (int x = 0; x < MGRoomScript.Count; x++)
        {
            if (MGRoomScript[x].ID == RoomID)
            {
                MGRoomScript[x].isOccupied = false;
                break;
            }
        }

        Player.transform.position = spawn.position;
    }

    public void FailedGame(int RoomID, GameObject Player)
    {
        Player.transform.Find("GameFailed").GetComponent<MGTextHUD>().EnableText();
        ExitRoom(RoomID, Player);
    }

    public void WonGame(int RoomID, GameObject Player)
    {
        Player.transform.Find("GameWon").GetComponent<MGTextHUD>().EnableText();
        //Add buff to player
        ExitRoom(RoomID, Player);
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
