using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

//Script that is placed on player.
public class BoidMGHUD : MonoBehaviourPun
{

    public GameObject boidHUD;
    public BoidsMinigameManager crntManager;
    public TextMeshProUGUI crntScore;
    public TextMeshProUGUI maxScore;
    public TextMeshProUGUI timeLeft;

    private void Update()
    {
        CheckText();
    }

    private void OnEnable()
    {
        boidHUD.SetActive(true);
    }

    private void OnDisable()
    {
        boidHUD.SetActive(false);
    }

    void CheckText()
    {
        crntScore.text = crntManager.score.ToString();
        maxScore.text = crntManager.maxScore.ToString();
        timeLeft.text = Mathf.RoundToInt(crntManager.timer).ToString();
    }
}
