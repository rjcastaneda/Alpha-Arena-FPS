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

    private AIController boidAIController;
        
    private void Awake()
    {
        boidAIController = transform.Find("BoidController").GetComponent<AIController>();
    }


    private void OnTriggerExit(Collider other)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void Update()
    {
        
    }

    public void AddToScore()
    {
        score++;
    }

    void startMiniGame()
    {
        score = 0;
        maxScore = boidAIController.numBoids;
    }

    void checkTimer()
    {

    }

    void checkIfComplete()
    {

    }
}
