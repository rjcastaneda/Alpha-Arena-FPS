using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHud : MonoBehaviour
{
    public Text timeText;
    public float timeRemaining = 90;
    public bool timerIsRunning = false;
   
    void Start()
    {
        timerIsRunning = true;       
    }

    // Update is called once per frame
    void Update()
    {
        if(timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                Debug.Log("Time has run out!");
                timerIsRunning = false;
                timeRemaining = 0;
                Timeover();
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

    }

    void Timeover()
    {
        PhotonNetwork.LoadLevel(0);
        //load the Main menu screen
    }
}
