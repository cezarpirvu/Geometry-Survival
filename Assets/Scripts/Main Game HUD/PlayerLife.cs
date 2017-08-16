using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour {

    // the life of the player
    private string playerLife;

    // initial text
    private string initialText = "Lives Remaining: ";

    // no life
    private string noLife = "0";

	// Use this for initialization
	void Start ()
    {
        // get the current number of lives left from the player
        Player player = (Player)FindObjectOfType(typeof(Player));
        string lifeCount = player.getPlayerLife().ToString();

        // the life of the player at the beginning of the game
        GetComponent<Text>().text = initialText + lifeCount;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // get the current number of lives left from the player
        Player player = (Player)FindObjectOfType(typeof(Player));
        string lifeCount = player.getPlayerLife().ToString();

        // set the new value of lives
        if (int.Parse(lifeCount) >= 0)
        {
            // set the new lives number
            GetComponent<Text>().text = initialText + lifeCount;
        } else
        {
            // reset the lives number
            GetComponent<Text>().text = initialText + noLife;
        }
    }
}
