using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{

    // instantiate the class so that it can be reffered to from other scripts
    public static MainMenu instance;

    // check if music is muted
    public static bool muteMusic = false;
    // the current player's name
    public static string playerName = "";
    // the score of the current player
    public static int score = 0;
    // the player name plus it's obtained score
    private string playerScore = "";

    // when the script instance is awaken, store the instance class (used for references in scripts)
    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        // initialize the canvas
        Canvas mainMenuCanvas = GetComponent<Canvas>();
        CanvasGroup mainMenuCanvasGroup = mainMenuCanvas.GetComponent<CanvasGroup>();

        // set the order so that the main menu always show first
        mainMenuCanvas.sortingOrder = 1;
        mainMenuCanvasGroup.alpha = 1;
        mainMenuCanvasGroup.interactable = true;

        // the panel name canvas
        GameObject panelCanvas = GameObject.FindGameObjectWithTag("Panel Name Canvas");
        // hide the panel in the main menu
        panelCanvas.GetComponent<Canvas>().enabled = false;

        // load the main menu music
        AudioSource theme = GetComponent<AudioSource>();
        // in the beginning it plays by default
        theme.mute = muteMusic;

        // set the music button's text correspondingly
        setMusicButton();

        // set the welcome message with the corresponding name after returning from the game
        if (!playerName.Equals(""))
        {
            GameObject welcome = GameObject.FindGameObjectWithTag("Welcome Message");
            string text = welcome.GetComponent<Text>().text;
            welcome.GetComponent<Text>().text = text.Substring(0, text.Length - 1) + ", " + playerName + "!";

            // the current player and it's corresponding score
            playerScore = playerName + " " + score.ToString();
        }

        // update the highscores leaderboard
        updateHighscores();
    }

    // set the text of the music button from the options menu
    private void setMusicButton()
    {
        // set the default value of the music button text
        MusicButton button = MusicButton.instance;
        // if the music is muted set the defaut value to true, else to false
        if (!muteMusic)
        {
            button.setMusic(true);
        }
        else
        {
            button.setMusic(false);
        }
    }

    // check if music is played or not
    public bool checkMute()
    {
        if (muteMusic)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // set if the music should be playing or not
    public void setMute(bool mute)
    {
        muteMusic = mute;
    }


    // set the current player name
    public void setPlayerName(string name)
    {
        // set the name if it is different than null
        if (!name.Equals(""))
        {
            playerName = name;
        }
    }

    // get the player name from this menu
    public string getPlayerName()
    {
        return playerName;
    }

    // set the current's player score
    public void setScore(int number)
    {
        score = number;
    }

    // get the current player's name plus the score that he obtained during the game
    public string getPlayerAndScore()
    {
        return playerScore;
    }

    // compare method for the score keys
    private static int Compare(KeyValuePair<string, int> a, KeyValuePair<string, int> b)
    {
        if (b.Value == a.Value)
        {
            return a.Key.CompareTo(b.Key);
        }

        return b.Value.CompareTo(a.Value);
    }

    // update the highscores
    public void updateHighscores()
    {
        int i = 1;

        // if the current player's name and it's corresponding name are not null, then update the highscores
        if (!getPlayerAndScore().Equals(""))
        {
            while (true)
            {
                // check if the player entry is found
                if (!PlayerPrefs.HasKey("Highscore" + i))
                {
                    // add it if not and interrupt the cycle
                    PlayerPrefs.SetString("Highscore" + i, getPlayerAndScore());

                    break;
                }

                i++;
            }
        }

        // reset the counter
        i = 1;
        // the list that will be used for sorting the entries in the leaderboard
        List<KeyValuePair<string, int>> highscoreEntries = new List<KeyValuePair<string, int>>();
        // if the player name is null, it means the user has just launched the main menu and the highscores must be loaded
        while (true)
        {
            if (PlayerPrefs.HasKey("Highscore" + i))
            {
                // separate the name of the player and it's score
                string[] part = PlayerPrefs.GetString("Highscore" + i).Split(new char[] { ' ' });
                // insert them into the list of pairs
                highscoreEntries.Insert(0, new KeyValuePair<string, int>(part[0], int.Parse(part[1])));

                i++;
            }
            else
            {
                // sort the list
                highscoreEntries.Sort(Compare);
                // the number of entries in the list
                int count = 0;
                // iterate the pairs in the list so that the leaderboard is updated with their values
                foreach (KeyValuePair<string, int> kvp in highscoreEntries)
                {
                    // the maximum number of entries in the database is set to 15
                    if (count < 15)
                    {
                        // select the prefab for the player list scores from the resources
                        GameObject prefab = Resources.Load("Player List Scores") as GameObject;
                        // instantiate the prefab
                        GameObject entry = GameObject.Instantiate(prefab);
                        // refer to the parent of the prefab under which it will be cloned
                        GameObject leaderboard = GameObject.FindGameObjectWithTag("Leaderboard");

                        // set the entry under the leaderboard parent
                        entry.transform.SetParent(leaderboard.transform, false);

                        // update the rank, player name field and the score correspondingly
                        leaderboard.transform.GetChild(count).GetChild(0).GetComponent<Text>().text = (count + 1).ToString();
                        leaderboard.transform.GetChild(count).GetChild(1).GetComponent<Text>().text = kvp.Key;
                        leaderboard.transform.GetChild(count).GetChild(2).GetComponent<Text>().text = kvp.Value.ToString();

                        count++;
                    }
                    else
                    {
                        break;
                    }
                }

                if (count > 15)
                {
                    // remove the unnecessary pairs from the list
                    highscoreEntries.RemoveAt(count);
                }

                // after the sorting has been completed, exit the loop
                break;
            }
        }
    }

    // reset the highscore database
    public void resetHighscores()
    {
        PlayerPrefs.DeleteAll();
    }
}
