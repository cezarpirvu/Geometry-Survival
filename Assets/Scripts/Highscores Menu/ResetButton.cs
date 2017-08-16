using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResetButton : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        // the reset button
        Button resetButton = GetComponent<Button>();
        resetButton.onClick.AddListener(onClick);
	}
	
	// when the button is pressed, the leaderboard is reset
    void onClick ()
    {
        // destroy the leaderboard with all the entries
        GameObject leaderboard = GameObject.FindGameObjectWithTag("Leaderboard");
        Destroy(leaderboard);
        
        // reset the values in the database
        MainMenu mainMenu = MainMenu.instance;
        mainMenu.resetHighscores();
    }
}
