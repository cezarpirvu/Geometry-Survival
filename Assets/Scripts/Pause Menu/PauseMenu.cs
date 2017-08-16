using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        // the pause menu canvas
        Canvas pauseMenu = GetComponent<Canvas>();
        CanvasGroup canvasGroup = pauseMenu.GetComponent<CanvasGroup>();

        // hide the pause menu until "escape" is pressed
        pauseMenu.sortingOrder = 0;
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
    }
}
