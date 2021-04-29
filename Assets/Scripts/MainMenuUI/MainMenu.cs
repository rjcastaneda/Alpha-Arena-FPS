using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   
    public Button ExitButton;
    public Button StartGameButton;
    public Button HowToPlayButton;
    private GameObject MainMenuCanvas;
    private GameObject HowToPlayCanvas;
    private GameObject StartGameCanvas;


    // Start is called before the first frame update
    void Start()
    {
        MainMenuCanvas = GameObject.Find("MainMenu");
        HowToPlayCanvas = GameObject.Find("HowToPlayMenu");
        StartGameCanvas = GameObject.Find("StartGameMenu");
        ExitButton = MainMenuCanvas.transform.Find("ExitGameButton").gameObject.GetComponent<Button>();
        StartGameButton = MainMenuCanvas.transform.Find("StartButton").gameObject.GetComponent<Button>();
        HowToPlayButton = MainMenuCanvas.transform.Find("HowToPlayButton").gameObject.GetComponent<Button>();

        //Add functions to buttons
        ExitButton.onClick.AddListener(Exit);
        StartGameButton.onClick.AddListener(StartGame);
        HowToPlayButton.onClick.AddListener(HowToPlay);

        //Set Defaults
        HowToPlayCanvas.SetActive(false);
        StartGameCanvas.SetActive(false);
    }

    //Exit button functions
    void Exit() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    //Start Game Function
    void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //MainMenuCanvas.SetActive(false);
        //StartGameCanvas.SetActive(true);
    }

    //How To Play Button function
    void HowToPlay()
    {
        MainMenuCanvas.SetActive(false);
        HowToPlayCanvas.SetActive(true);
    }
}
