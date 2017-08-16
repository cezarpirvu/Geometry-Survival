using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BackButtonHighscores : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        Button backButton = GetComponent<Button>();
        // add listener to the button
        backButton.onClick.AddListener(onClick);
	}

    // when the back button is pressed, return to the main menu
    void onClick ()
    {
        // disable the highscores menu canvas
        HighscoresMenu highscores = (HighscoresMenu)FindObjectOfType(typeof(HighscoresMenu));
        Canvas highscoresMenu = highscores.GetComponent<Canvas>();
        CanvasGroup highscoresMenuCanvas = highscoresMenu.GetComponent<CanvasGroup>();

        highscoresMenu.sortingOrder = 0;
        highscoresMenuCanvas.alpha = 0;
        highscoresMenuCanvas.interactable = false;

        // enable the main menu canvas
        MainMenu menu = MainMenu.instance;
        Canvas mainMenu = menu.GetComponent<Canvas>();
        CanvasGroup mainMenuCanvas = mainMenu.GetComponent<CanvasGroup>();

        mainMenu.sortingOrder = 1;
        mainMenuCanvas.alpha = 1;
        mainMenuCanvas.interactable = true;
    }
}
