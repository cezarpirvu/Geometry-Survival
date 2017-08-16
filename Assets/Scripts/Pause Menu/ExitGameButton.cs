using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ExitGameButton : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        // the exit game button
        Button exitButton = GetComponent<Button>();
        // add listener to the exit game button
        exitButton.onClick.AddListener(onClick);
    }
	
	// exit the game if this button has been pressed
    void onClick ()
    {
        Application.Quit();
    }
}
