using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//Script placed on BoidMGPlatform.
public class BoidsMinigameManager : MonoBehaviourPunCallbacks
{
    public float timer;
    public float timeToSet;
    public float score;
    public float maxScore;
    public bool started;
    private AIController boidAIController;
    private MiniGameManager miniGameManager;
        
    private void Awake()
    {
        boidAIController = transform.Find("BoidController").GetComponent<AIController>();
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            BoidMGHUD boidHud = other.gameObject.GetComponent<BoidMGHUD>();
            boidHud.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            BoidMGHUD boidHud = other.gameObject.GetComponent<BoidMGHUD>();
            boidHud.enabled = false;
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

    void CheckTimer()
    {
        if(timer > 0 && started){
            timer -= Time.fixedDeltaTime;
        }

        if(timer <= 0)
        {
            if(score == maxScore)
            {
                miniGameManager.WonGame();
                started = false;
            }
            else if(score != maxScore)
            {
                miniGameManager.FailedGame();
                started = false;
            }
        }
    }

    void CheckComplete()
    {
        if(score == maxScore)
        {
            miniGameManager.WonGame();
        }
    }

}
