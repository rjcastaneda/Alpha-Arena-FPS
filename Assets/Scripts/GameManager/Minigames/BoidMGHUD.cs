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

    void Start()
    {
        boidHUD = transform.Find("PlayerHUD").transform.Find("BoidHUD").gameObject;
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
        maxScore.text = crntManager.score.ToString();
        timeLeft.text = crntManager.timer.ToString();
    }
}
