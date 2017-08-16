using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BackButton : MonoBehaviour {

    // the main and options menu and the player name canvas
    private MainMenu mainMenu;
    private OptionsMenu optionsMenu;

    // the initial welcome message
    private string initialMessage = "Welcome!";

    // Use this for initialization
    void Start ()
    {
        // the back button
        Button back = GetComponent<Button>();
        // set listener on the click button
        back.onClick.AddListener(onClick);

        // reffer to the main and options menu
        mainMenu = (MainMenu)FindObjectOfType(typeof(MainMenu));
        optionsMenu = (OptionsMenu)FindObjectOfType(typeof(OptionsMenu));
    }

    // listener method for the options button that switches to the options menu
    void onClick ()
    {
        // send the options menu to the background and disable interactions
        optionsMenu.GetComponent<CanvasGroup>().alpha = 0;
        optionsMenu.GetComponent<CanvasGroup>().interactable = false;
        optionsMenu.GetComponent<Canvas>().sortingOrder = 0;

        // send the main menu to the foreground and enable interactions
        mainMenu.GetComponent<CanvasGroup>().alpha = 1;
        mainMenu.GetComponent<CanvasGroup>().interactable = true;
        mainMenu.GetComponent<Canvas>().sortingOrder = 1;

        // change the welcome message from the main menu by adding the current player name
        changeMessage();

        // hide the input field of the map size button
        MapSizeButton mapButton = (MapSizeButton)FindObjectOfType(typeof(MapSizeButton));
        mapButton.enableInputField(false);
        mapButton.buttonPressed(true);
        // hide the input field of the name button
        NameButton nameButton = (NameButton)FindObjectOfType(typeof(NameButton));
        nameButton.enableInputField(false);
        nameButton.buttonPressed(true);
        // hide the tutorial
        TutorialButton tutorialButton = (TutorialButton)FindObjectOfType(typeof(TutorialButton));
        tutorialButton.enableTutorial(false);
        tutorialButton.buttonPressed(true);
    }

    // add the name of the current player to the welcome message from the main menu
    void changeMessage ()
    {
        // reference for the player name in the input field and the one stored in the main menu
        NameButton playerName = (NameButton)FindObjectOfType(typeof(NameButton));
        MainMenu menu = MainMenu.instance;

        // check if the player's name in the main menu only if it is null and the input field is not null
        if (menu.getPlayerName().Equals("") && !playerName.getPlayerName().Equals(""))
        {
            // check if the player's current name is different from the new one and change it if so
            if (!menu.getPlayerName().Equals(playerName.getPlayerName()))
            {
                // store the current player's name in the main menu
                menu.setPlayerName(playerName.getPlayerName());
                // find the "Welcome Message" gameobject
                GameObject welcome = GameObject.FindGameObjectWithTag("Welcome Message");
                string text = welcome.GetComponent<Text>().text;

                // if there is already a name in the welcome message, replace it with the current player's name
                if (initialMessage.Length == text.Length)
                {
                    welcome.GetComponent<Text>().text = text.Substring(0, text.Length - 1) + ", " + playerName.getPlayerName() + "!";
                }
                else
                {
                    welcome.GetComponent<Text>().text = initialMessage.Substring(0, initialMessage.Length - 1) + ", " + playerName.getPlayerName() + "!";
                }

            }
        }
        else
        {
            // if there is a name stored in the main menu, overwrite it with the current player's name
            if (!menu.getPlayerName().Equals(playerName.getPlayerName()) && !playerName.getPlayerName().Equals(""))
            {
                // store the current player's name in the main menu
                menu.setPlayerName(playerName.getPlayerName());
                // find the "Welcome Message" gameobject
                GameObject welcome = GameObject.FindGameObjectWithTag("Welcome Message");
                string text = welcome.GetComponent<Text>().text;

                // if there is already a name in the welcome message, replace it with the current player's name
                if (initialMessage.Length == text.Length)
                {
                    welcome.GetComponent<Text>().text = text.Substring(0, text.Length - 1) + ", " + playerName.getPlayerName() + "!";
                }
                else
                {
                    welcome.GetComponent<Text>().text = initialMessage.Substring(0, initialMessage.Length - 1) + ", " + playerName.getPlayerName() + "!";
                }
            }
        }
    }
}
