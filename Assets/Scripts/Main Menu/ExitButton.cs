using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        // the exit button
        Button exitButton = GetComponent<Button>();
        // set listener on the click button
        exitButton.onClick.AddListener(onClick);
    }

    // listener method for the exit button that closes the application
    void onClick()
    {
        Application.Quit();
    }
}
