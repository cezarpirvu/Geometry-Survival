using UnityEngine;
using UnityEngine.UI;

public class OptionsButton : MonoBehaviour
{

    // the main and options menu
    private MainMenu mainMenu;
    private OptionsMenu optionsMenu;

    // use this for initialization
    void Start()
    {
        // the options button
        Button options = GetComponent<Button>();
        //set listener on the click button
        options.onClick.AddListener(onClick);

        // reference to the main and options menu
        mainMenu = (MainMenu)FindObjectOfType(typeof(MainMenu));
        optionsMenu = (OptionsMenu)FindObjectOfType(typeof(OptionsMenu));
    }

    // listener method for the options button that switches to the options menu
    void onClick()
    {
        // send the main menu to the background and disable interactions
        mainMenu.GetComponent<CanvasGroup>().alpha = 0;
        mainMenu.GetComponent<CanvasGroup>().interactable = false;
        mainMenu.GetComponent<Canvas>().sortingOrder = 0;

        // send the options menu to the foreground and enable interactions
        optionsMenu.GetComponent<CanvasGroup>().alpha = 1;
        optionsMenu.GetComponent<CanvasGroup>().interactable = true;
        optionsMenu.GetComponent<Canvas>().sortingOrder = 1;
    }
}