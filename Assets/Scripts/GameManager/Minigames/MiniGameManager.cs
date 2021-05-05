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
        Debug.Log(spawnpoint.position);

        //Move player position
        CharacterController CC = Player.GetComponent<CharacterController>();
        CC.enabled = false;
        Player.transform.position = spawnpoint.position;
        CC.enabled = true;

        MGRoomScript[idx].isOccupied = true;
        MGRoomScript[idx].StartMiniGame(Player);
    }

    public void ExitRoom(int RoomID, GameObject Player)
    {
        Transform spawn = spawnManager.GetSpawnPoint();
        Debug.Log(spawn.position);

        //Exit MiniGameRoom
        for (int x = 0; x < MGRoomScript.Count; x++)
        {
            if (MGRoomScript[x].ID == RoomID)
            {
                MGRoomScript[x].isOccupied = false;
                break;
            }
        }

        //Move player position
        CharacterController CC = Player.GetComponent<CharacterController>();
        CC.enabled = false;
        Player.transform.position = spawn.position;
        CC.enabled = true;
    }

    public void FailedGame(int RoomID, GameObject Player)
    {
        Player.transform.Find("PlayerHUD").transform.Find("GameFailed").GetComponent<MGTextHUD>().EnableText();
        ExitRoom(RoomID, Player);
    }

    public void WonGame(int RoomID, GameObject Player)
    {
        Player.transform.Find("PlayerHUD").transform.Find("GameWon").GetComponent<MGTextHUD>().EnableText();
        Player.GetComponent<PlayerData>().health = 125;
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
