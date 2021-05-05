using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//Script placed on BoidMGPlatform. Handles information about boids minigame
public class BoidsMinigameManager : MonoBehaviourPunCallbacks
{
    public float timer;
    public float timeToSet = 10f;
    public float score;
    public float maxScore;
    public bool started = false;

    [SerializeField] private GameObject playerGO;
    [SerializeField] private PhotonView playerPV;
    [SerializeField] private AIController boidAIController;
    [SerializeField] private MiniGameManager miniGameManager;
    [SerializeField] private MiniGameRoom thisRoom;
        
    private void Start()
    {
        boidAIController = transform.Find("BoidController").GetComponent<AIController>();
        miniGameManager = GameObject.Find("GameManager").GetComponent<MiniGameManager>();
        thisRoom = gameObject.GetComponent<MiniGameRoom>();
    }

    private void Update()
    {
        if(started)
        {
            CheckTimer();
            CheckComplete();
        }
    }

    public void AddToScore()
    {
        if(playerPV.IsMine)
        {
            score++;
        }
    }

    public void StartMiniGame(GameObject joinedPlayer)
    {
        playerGO = joinedPlayer;
        playerPV = playerGO.GetComponent<PhotonView>();
        if(playerPV.IsMine)
        {
            BoidMGHUD boidHud = playerGO.GetComponent<BoidMGHUD>();
            boidHud.enabled = true;
            boidHud.crntManager = this;
            boidAIController.RenableBoids();
            score = 0;
            maxScore = boidAIController.numBoids;
            timer = timeToSet;
            started = true;
        }
    }

    //function to decrement the timer and to check if the player won or lost the minigame.
    void CheckTimer()
    {
        if(timer > 0 && started){
            timer -= Time.deltaTime;
        }

        if(timer <= 0)
        {
            if(score == maxScore)
            {
                if (playerPV.IsMine)
                {
                    boidAIController.DisabledBoids();
                    BoidMGHUD boidHud = playerGO.GetComponent<BoidMGHUD>();
                    boidHud.enabled = false;
                    boidHud.crntManager = null;
                    miniGameManager.WonGame(thisRoom.ID, playerGO);
                }
                
                started = false;
            }
            else if(score != maxScore)
            {
                if (playerPV.IsMine)
                {
                    boidAIController.DisabledBoids();
                    BoidMGHUD boidHud = playerGO.GetComponent<BoidMGHUD>();
                    boidHud.enabled = false;
                    boidHud.crntManager = null;
                    miniGameManager.FailedGame(thisRoom.ID, playerGO);
                }
                
                started = false;
            }
        }
    }

    //Function to check if player completed game before timer.
    void CheckComplete()
    {
        if(score == maxScore){
            if (playerPV.IsMine)
            {
                boidAIController.enabled = false;
                BoidMGHUD boidHud = playerGO.GetComponent<BoidMGHUD>();
                boidHud.enabled = false;
                boidHud.crntManager = null;
                miniGameManager.WonGame(thisRoom.ID, playerGO);
            }

            started = false;
        }
    }

}
