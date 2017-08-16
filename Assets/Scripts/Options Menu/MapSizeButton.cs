using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSizeButton : MonoBehaviour
{

    // instantiate the class so that it can be reffered to from other scripts
    public static MapSizeButton instance;

    // the gameobject for the input field of the button
    private GameObject inputField;
    // the gameobject for the map size value
    private GameObject size;
    // initial size of the map
    private int mapSize = 1;
    //current size of the map
    private int currentMapSize;
    // check if the button has been pressed
    private bool pressed = false;

    // when the script instance is awaken, store the instance class (used for references in scripts)
    void Awake()
    {
        instance = this;
    }
    // Use this for initialization
    void Start()
    {
        // add listener to the button
        Button mapSizeButton = GetComponent<Button>();
        mapSizeButton.onClick.AddListener(onClick);

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
                                size = children.gameObject;
                                children.GetComponent<Text>().text = mapSize.ToString();
                            }
                        }
                    }
                }
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        currentMapSize = getMap();
    }

    // get the current size of the map
    public int getMapSize()
    {
        return currentMapSize;
    }

    // get the map size
    public int getMap()
    {
        if (!size.GetComponent<Text>().text.Equals(""))
        {
            return int.Parse(size.GetComponent<Text>().text);
        }
        else
        {
            return 1;
        }
    }

    // set the map size for the game
    void setMapSize(int value)
    {
        if (value == 0)
        {
            size.GetComponent<Text>().text = "1";
        }
        else
        {
            size.GetComponent<Text>().text = value.ToString();
        }
    }

    // enable or disable the input field of the "Map Size" button
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
