using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//Script placed on BoidMGPlatform. Handles information about boids minigame
public class BoidsMinigameManager : MonoBehaviourPunCallbacks
{
    public float timer;
    public float timeToSet;
    public float score;
    public float maxScore;
    public bool started;

    private GameObject player;
    private AIController boidAIController;
    private MiniGameManager miniGameManager;
    private MiniGameRoom thisRoom;
        
    private void Awake()
    {
        boidAIController = transform.Find("BoidController").GetComponent<AIController>();
        miniGameManager = GameObject.Find("Game Manager").GetComponent<MiniGameManager>();
        thisRoom = gameObject.GetComponent<MiniGameRoom>();
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            BoidMGHUD boidHud = other.gameObject.GetComponent<BoidMGHUD>();
            boidHud.enabled = false;
            boidHud.crntManager = null;
            player = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            BoidMGHUD boidHud = other.gameObject.GetComponent<BoidMGHUD>();
            boidHud.enabled = true;
            boidHud.crntManager = this;
            player = other.gameObject;
        }
    }

    private void Update()
    {
        CheckTimer();
        CheckComplete();
    }

    public void AddToScore()
    {
        score++;
    }

    void StartMiniGame()
    {
        score = 0;
        maxScore = boidAIController.numBoids;
        timer = timeToSet;
        started = true;
    }

    //function to decrement the timer and to check if the player won or lost the minigame.
    void CheckTimer()
    {
        if(timer > 0 && started){
            timer -= Time.fixedDeltaTime;
        }

        if(timer <= 0)
        {
            if(score == maxScore)
            {
                miniGameManager.WonGame(thisRoom.ID, player);
                started = false;
            }
            else if(score != maxScore)
            {
                miniGameManager.FailedGame(thisRoom.ID, player);
                started = false;
            }
        }
    }

    //Function to check if player completed game before timer.
    void CheckComplete()
    {
        if(score == maxScore){
            miniGameManager.WonGame(thisRoom.ID, player);
        }
    }

}
