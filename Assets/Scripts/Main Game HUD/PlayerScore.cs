using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour {

    // instantiate the class so that it can be reffered to from other scripts
    public static PlayerScore instance;

    // when the script instance is awaken, store the instance class (used for references in scripts)
    void Awake ()
    {
        instance = this;
    }

    // the score at the beginning of the game
    public int score;

    // the score text on the HUD
    private string scoreText = "Score: ";

	// Use this for initialization
	void Start ()
    {
        // initially set the score to 0
        score = 0;
        // the score text component initialized to 0
        GetComponent<Text>().text = scoreText + score.ToString();
	}
	
	// Update is called once per frame
	void Update ()
    {
        // continuously update the players score
        GetComponent<Text>().text = scoreText + score.ToString();
	}

    // add to the score of the player
    public void addScore (int score)
    {
        this.score += score;
    }

    // get the score of the current player
    public int getScore ()
    {
        return score;
    }
}
