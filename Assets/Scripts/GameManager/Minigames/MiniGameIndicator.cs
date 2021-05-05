using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

//Script placed on player, script is extension of Obelisk functionality
public class MiniGameIndicator : MonoBehaviourPun
{
    public Obelisk crntObelisk;
    private GameObject MGIndicator;
    private PhotonView PV;

    private void OnEnable()
    {
        MGIndicator.SetActive(true);
    }

    private void OnDisable()
    {
        MGIndicator.SetActive(false);
    }

    private void Awake()
    {
        MGIndicator = transform.Find("PlayerHUD").transform.Find("MGIndicator").gameObject;
        PV = gameObject.GetComponent<PhotonView>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            if(PV.IsMine)
            {
                crntObelisk.EnterRoom(this.gameObject);
            }
        }
    }
}
