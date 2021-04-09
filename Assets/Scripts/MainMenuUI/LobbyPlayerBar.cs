using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LobbyPlayerBar : MonoBehaviour
{
    public int BarID;
    public string PlayerName;

    private TextMeshProUGUI BarText;

    private void Start()
    {
        BarText = this.gameObject.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        changePlayerName("Empty");
    }

    public void changePlayerName(string name)
    {
        BarText.text = name;
    }
}
