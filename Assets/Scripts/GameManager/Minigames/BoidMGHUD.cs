using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//Script that is placed on player.
public class BoidMGHUD : MonoBehaviourPun
{
    private GameObject boidHUD;

    private void OnEnable()
    {
        boidHUD.SetActive(true);
    }

    private void OnDisable()
    {
        boidHUD.SetActive(false);
    }

    void Start()
    {
        boidHUD = transform.Find("PlayerHUD").transform.Find("BoidHUD").gameObject;
    }
}
