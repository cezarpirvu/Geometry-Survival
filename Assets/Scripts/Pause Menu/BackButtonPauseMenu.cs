using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackButtonPauseMenu : MonoBehaviour
{

    //the pause game canvas
    private PauseMenu pauseMenu;

    // Use this for initialization
    void Start()
    {
        // the back button
        Button backButton = GetComponent<Button>();
        // add listener to the back button
        backButton.onClick.AddListener(onClick);

        // reference the pause game menu
        pauseMenu = (PauseMenu)FindObjectOfType(typeof(PauseMenu));
    }

    // if the back button has been pressed
    void onClick()
    {
        // return to the pause menu
        pauseMenu.GetComponent<Canvas>().sortingOrder = 1;
        pauseMenu.GetComponent<CanvasGroup>().alpha = 1;
        pauseMenu.GetComponent<CanvasGroup>().interactable = true;

        // disable the options menu
        OptionsMenuPauseMenu optionsMenu = (OptionsMenuPauseMenu)FindObjectOfType(typeof(OptionsMenuPauseMenu));
        optionsMenu.enableOptionsMenu(false);
    }
}
