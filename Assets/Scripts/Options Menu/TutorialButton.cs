using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialButton : MonoBehaviour
{

    // check if the button has been pressed
    private bool pressed = false;

    // Use this for initialization
    void Start()
    {
        // add listener to the button
        Button nameButton = GetComponent<Button>();
        nameButton.onClick.AddListener(onClick);

        enableTutorial(false);
    }

    // Update is called once per frame
    void Update()
    {
        // if the user presses the mouse button then the panel is hidden
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(1))
        {
            enableTutorial(false);
            buttonPressed(true);
        }
    }

    // enable or disable the tutorial window
    public void enableTutorial(bool check)
    {
        if (transform.childCount != 0)
        {
            foreach(Transform child in transform)
            {
                if (child.name.Equals("Panel"))
                {
                    child.GetComponent<Image>().enabled = check;
                    child.transform.GetChild(0).GetComponent<Text>().enabled = check;
                }
            }
        }
    }

    // the listener of the button
    void onClick()
    {
        if (!pressed)
        {
            enableTutorial(true);
        }
        else
        {
            enableTutorial(false);
        }

        pressed = !pressed;
    }

    // set if the button has been pressed or nots
    public void buttonPressed(bool check)
    {
        pressed = !check;
    }
}
