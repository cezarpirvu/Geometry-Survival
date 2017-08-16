using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameButton : MonoBehaviour
{

    // the gameobject for the input field of the button
    private GameObject inputField;
    // the gameobject for the map size value
    private GameObject nameField;
    // check if the button has been pressed
    private bool pressed = false;

    // Use this for initialization
    void Start()
    {
        // add listener to the button
        Button nameButton = GetComponent<Button>();
        nameButton.onClick.AddListener(onClick);

        // initially disable the input field
        enableInputField(false);

        // initially the map size for the player is set to 1
        if (transform.childCount != 0)
        {
            foreach (Transform child in gameObject.transform)
            {
                if (child.name.Equals("InputField"))
                {
                    if (child.transform.childCount != 0)
                    {
                        foreach (Transform children in child.transform)
                        {
                            if (children.name.Equals("Text"))
                            {
                                nameField = children.gameObject;
                            }
                        }
                    }
                }
            }
        }
    }

    // get the player's name
    public string getPlayerName()
    {
        return nameField.GetComponent<Text>().text;
    }

    // set the name for the player
    void setMapSize(string value)
    {
        nameField.GetComponent<Text>().text = value.ToString();
    }

    // enable or disable the input field of the "Name" button
    public void enableInputField(bool check)
    {
        if (transform.childCount != 0)
        {
            foreach (Transform child in gameObject.transform)
            {
                if (child.name.Equals("InputField"))
                {
                    inputField = child.gameObject;
                    // initially hide the inputfield and it's text
                    inputField.GetComponent<Image>().enabled = check;
                    inputField.GetComponent<InputField>().enabled = check;

                    // disable the children of the input field
                    if (child.transform.childCount != 0)
                    {
                        foreach (Transform children in child.transform)
                        {
                            if (children.name.Equals("Placeholder"))
                            {
                                children.GetComponent<Text>().enabled = check;
                            }
                            else
                            {
                                if (children.name.Equals("Text"))
                                {
                                    children.GetComponent<Text>().enabled = check;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    // the listener of the button
    void onClick()
    {
        if (!pressed)
        {
            enableInputField(true);
        }
        else
        {
            enableInputField(false);
        }

        pressed = !pressed;
    }

    // set if the button has been pressed or nots
    public void buttonPressed(bool check)
    {
        pressed = !check;
    }
}
