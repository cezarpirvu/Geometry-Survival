using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Playlist : MonoBehaviour
{

    // instantiate the class so that it can be reffered to from other scripts
    public static Playlist instance;

    // the playlsit for the game
    public AudioClip[] playlist;

    private AudioClip track;
    private AudioSource currentTrack;

    private int index;
    private string song;
    private bool paused = false;

    // when the script instance is awaken, store the instance class (used for references in scripts)
    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        // play a random track at the beginning
        index = Random.Range(0, playlist.Length);
        song = playlist[index].name;

        currentTrack = GetComponent<AudioSource>();
        track = Resources.Load("Audio/" + song) as AudioClip;
        currentTrack.PlayOneShot(track);

        // check if the sounds were muted
        MusicButton mainMenuMusic = MusicButton.instance;
        MusicButtonPauseMenu gameMusic = (MusicButtonPauseMenu)FindObjectOfType(typeof(MusicButtonPauseMenu));
        GameObject musicButtonText = gameMusic.transform.GetChild(0).gameObject;

        // if the music is enabled/disabled, inform the main menu
        if (mainMenuMusic.getMusic() == "Music: Off")
        {
            muteTrack(true);
            if (musicButtonText != null)
            {
                musicButtonText.GetComponent<Text>().text = mainMenuMusic.getMusic();
                print(musicButtonText.GetComponent<Text>().text);
            }
        }
        else
        {
            muteTrack(false);
            if (musicButtonText != null)
            {
                musicButtonText.GetComponent<Text>().text = mainMenuMusic.getMusic();
            }
        }
    }

    // Update is called once per frame
    public void Update()
    {
        // if the track has finished, then play another one
        if (!currentTrack.isPlaying && !paused)
        {
            changeTrack();
        }
    }

    // change the song that is currently playing
    public void changeTrack()
    {
        index = Random.Range(0, playlist.Length);
        song = playlist[index].name;
        currentTrack.Stop();

        track = Resources.Load("Audio/" + song) as AudioClip;
        currentTrack.PlayOneShot(track);
    }

    // pause the track that is playing
    public void setPause(bool check)
    {
        if (check)
        {
            currentTrack.Pause();
            paused = true;
        }
        else
        {
            currentTrack.UnPause();
            paused = false;
        }
    }

    // mute/unmute the track
    public void muteTrack(bool check)
    {
        if (check)
        {
            currentTrack.mute = true;
        }
        else
        {
            currentTrack.mute = false;
        }
    }
}
