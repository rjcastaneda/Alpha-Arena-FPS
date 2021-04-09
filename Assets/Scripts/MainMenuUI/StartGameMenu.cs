using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGameMenu : MonoBehaviour
{
    public Button backButton;
    private GameObject MainMenuCanvas;

    // Start is called before the first frame update
    void Start()
    {
        backButton = this.gameObject.transform.Find("BackButton").GetComponent<Button>();
        backButton.onClick.AddListener(Back);
        MainMenuCanvas = GameObject.Find("MainMenu");
    }

    void Back()
    {
        this.gameObject.SetActive(false);
        MainMenuCanvas.SetActive(true);
    }
}
