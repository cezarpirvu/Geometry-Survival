using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HighscoresButton : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        // the highscore button
        Button highscoresButton = GetComponent<Button>();
        // add listener to the button
        highscoresButton.onClick.AddListener(onClick);
	}
	
    // when the button is pressed go to the "Highscores Menu"
    void onClick ()
    {
        // initialize the main menu canvas
        MainMenu mainMenu = (MainMenu)FindObjectOfType(typeof(MainMenu));
        Canvas mainMenuCanvas = mainMenu.GetComponent<Canvas>();
        CanvasGroup mainMenuCanvasGroup = mainMenuCanvas.GetComponent<CanvasGroup>();

        // hide the main menu
        mainMenuCanvas.sortingOrder = 0;
        mainMenuCanvasGroup.alpha = 0;
        mainMenuCanvasGroup.interactable = false;

        // initialize the highscores menu canvas
        HighscoresMenu highscoresMenu = HighscoresMenu.instance;
        Canvas highscoresMenuCanvas = highscoresMenu.GetComponent<Canvas>();
        CanvasGroup highscoresMenuCanvasGroup = highscoresMenuCanvas.GetComponent<CanvasGroup>();

        // show the highscores menu
        highscoresMenuCanvas.sortingOrder = 1;
        highscoresMenuCanvasGroup.alpha = 1;
        highscoresMenuCanvasGroup.interactable = true;
    }
}
