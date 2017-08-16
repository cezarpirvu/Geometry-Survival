using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour {

    // Use this for initialization
    void Start ()
    {
        // the main menu button
        Button mainMenuButton = GetComponent<Button>();
        // add listener to the main menu button
        mainMenuButton.onClick.AddListener(onCLick);
    }
	
    // return to the main menu if the button is pressed
	void onCLick ()
    {
        // check if the music is enabled/disabled from the options menu
        MusicButton mainMenuMusic = MusicButton.instance;
        MainMenu mainMenu = MainMenu.instance;
        MusicButtonPauseMenu musicButton = (MusicButtonPauseMenu)FindObjectOfType(typeof(MusicButtonPauseMenu));

        // if the music is enabled/disabled, inform the main menu
        if (musicButton.getMusicButtonText().Equals("Music: Off"))
        {
            mainMenu.setMute(true);
            mainMenuMusic.setMusic(false);
        }
        else
        {
            mainMenu.setMute(false);
            mainMenuMusic.setMusic(true);
        }

        // retrieve the score of the current player from the score HUD
        PlayerScore score = PlayerScore.instance;
        int currentScore = score.getScore();
        // set the score of the current player in the main menu for the highscores
        mainMenu.setScore(currentScore);

        // unfreeze the game
        Time.timeScale = 1;
        // go back to the main menu
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }
}
