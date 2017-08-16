using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsButtonPauseMenu : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        // add listener to the button
        Button optionsButton = GetComponent<Button>();
        optionsButton.onClick.AddListener(onClick);
    }

    // check if the button has been pressed
    void onClick()
    {
        OptionsMenuPauseMenu menu = (OptionsMenuPauseMenu)FindObjectOfType(typeof(OptionsMenuPauseMenu));
        menu.enableOptionsMenu(true);
    }
}
