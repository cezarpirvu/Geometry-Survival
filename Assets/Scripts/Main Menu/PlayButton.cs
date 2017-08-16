using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour {

    // the theme of the main menu
    private AudioSource clip;

    // the panel name canvas
    private GameObject panelCanvas;

    // Use this for initialization
    void Start ()
    {
        // the play button
        Button playButton = GetComponent<Button>();
        // set listener on the click button
        playButton.onClick.AddListener(onClick);

        // reference to the main menu theme
        MainMenu menu = (MainMenu)FindObjectOfType(typeof(MainMenu));
        clip = menu.GetComponent<AudioSource>();

        //reference to the panel name canvas
        panelCanvas = GameObject.FindGameObjectWithTag("Panel Name Canvas");
    }

    // Update is called once per frame
    void Update ()
    {
        // check if the panel is visible
        if (panelCanvas.GetComponent<Canvas>().enabled == true)
        {
            // if the user presses the mouse button then the panel is hidden
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(1))
            {
                panelCanvas.GetComponent<Canvas>().enabled = false;
            }
        }
    }

    // listener method for the play button that loads other scene
    void onClick ()
    {
        MainMenu menu = (MainMenu)FindObjectOfType(typeof(MainMenu));
        // check if a user name has been introduced
        if (string.Compare(menu.getPlayerName(), "") == 0)
        {
            panelCanvas.GetComponent<Canvas>().enabled = true;
        } else
        {
            // mute the main menu theme and load the game
            clip.mute = true;
            SceneManager.LoadScene("Game", LoadSceneMode.Single);
        }
    }
}