using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicButtonPauseMenu : MonoBehaviour
{

    private GameObject text;

    // Use this for initialization
    void Start()
    {
        // add listener to the button
        Button musicButton = GetComponent<Button>();
        musicButton.onClick.AddListener(onClick);

        // reference the text of the button
        if (transform.GetChild(0) != null)
        {
            text = transform.GetChild(0).gameObject;
        }
    }

    // check if the button has been pressed
    void onClick()
    {
        Playlist playlist = (Playlist)FindObjectOfType(typeof(Playlist));
        string textValue = text.GetComponent<Text>().text;

        // if music is enabled/disabled then mute/unmute it
        if (textValue.Equals("Music: On"))
        {
            text.GetComponent<Text>().text = "Music: Off";
            playlist.muteTrack(true);
        }
        else
        {
            text.GetComponent<Text>().text = "Music: On";
            playlist.muteTrack(false);
        }
    }

    // return the text value of the button
    public string getMusicButtonText()
    {
        // reference the text of the button
        if (transform.GetChild(0) != null)
        {
            return text.GetComponent<Text>().text;
        }
        else
        {
            return null;
        }
    }
}
