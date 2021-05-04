using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class MGTextHUD : MonoBehaviourPun
{
    public TextMeshProUGUI MGText;  
    private float timeToAppear = 2.5f;
    private float timeToDisappear;

    private void Start()
    {
        MGText = gameObject.GetComponent<TextMeshProUGUI>();
    }

    //Call to enable the text, which also sets the timer
    public void EnableText()
    {
        MGText.enabled = true;
        timeToDisappear = Time.time + timeToAppear;
    }

    //We check every frame if the timer has expired and the text should disappear
    void Update()
    {
        if (MGText.enabled && (Time.time >= timeToDisappear))
        {
            MGText.enabled = false;
        }
    }
}
