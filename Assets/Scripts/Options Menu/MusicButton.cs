using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MusicButton : MonoBehaviour {

     // instantiate the class so that it can be reffered to from other scripts
    public static MusicButton instance;

    // the music text corresponding to the button
    private GameObject text;

    // values fo the text field
    public static string defaultValue = "Music: On";

    // when the script instance is awaken, store the instance class (used for references in scripts)
    void Awake ()
    {
        instance = this;
    }

	// Use this for initialization
	void Start ()
    {
        // reference to the music text
        text = GameObject.FindGameObjectWithTag("Music Text");
        text.GetComponent<Text>().text = defaultValue;

        // the music button
        Button music = GetComponent<Button>();
        // add listener to the button
        music.onClick.AddListener(onClick);
	}

	// enable/disable music when the music button is pressed
    void onClick ()
    {
        MainMenu theme = MainMenu.instance;

        // if music is enabled/disabled then mute/unmute it
        if (defaultValue == "Music: On")
        {
            text.GetComponent<Text>().text = "Music: Off";
            theme.GetComponent<AudioSource>().mute = true;
            defaultValue = "Music: Off";
        } else
        {
            text.GetComponent<Text>().text = "Music: On";
            theme.GetComponent<AudioSource>().mute = false;
            defaultValue = "Music: On";
        }
    }

    // check if the music is on/off
    public string getMusic ()
    {
        return defaultValue;
    }

    // set the music on/off
    public void setMusic (bool option)
    {
        if (option)
        {
            defaultValue = "Music: On";
        } else
        {
            defaultValue = "Music: Off";
        }
    }
}
