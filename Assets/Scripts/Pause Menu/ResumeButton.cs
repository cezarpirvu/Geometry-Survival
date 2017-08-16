using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResumeButton : MonoBehaviour
{

    //the pause game canvas
    private PauseMenu pauseMenu;
    private MapSettings game;

    // Use this for initialization
    void Start()
    {
        // the resume button
        Button resumeButton = GetComponent<Button>();
        // add listener to the resume button
        resumeButton.onClick.AddListener(onClick);

        // reference the pause game menu
        pauseMenu = (PauseMenu)FindObjectOfType(typeof(PauseMenu));

        game = MapSettings.instance;
    }

    // if the resume button has been pressed
    void onClick()
    {
        // return to the game
        pauseMenu.GetComponent<Canvas>().sortingOrder = 0;
        pauseMenu.GetComponent<CanvasGroup>().alpha = 0;

        // unfreeze the game only if the player can still continue
        Player player = (Player)FindObjectOfType(typeof(Player));
        if (player.getPlayerLife() > 0)
        {
            // unfreeze the game
            Time.timeScale = 1;

            // unpause
            game.setEscape(false);
        }

        game.setPause(false);
    }
}
