using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDBackground : MonoBehaviour {

    // instantiate the class so that it can be reffered to from other scripts
    public static HUDBackground instance;

    // the level gameobject of the background
    private GameObject levelWave;

    // the level display
    private string displayLevel = "Level: ";

    // the wave display
    private string displayWave = "Wave: ";

    // the display
    private string display = "";

    // level number
    public int levelNumber;

    // the wave number
    public int waveNumber;

    // when the script instance is awaken, store the instance class (used for references in scripts)
    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start ()
    {
        // initial value of the level and the wave
        levelNumber = 1;
        waveNumber = 1;

        // update the display
        setLevelWave();

        // wait until the delay has passed
        StartCoroutine(delay());
    }

    // the delay until the background dissapears -> 2 seconds
    IEnumerator delay()
    {
        yield return new WaitForSeconds(2);

        // deactivate the level display
        GameObject background = GameObject.Find("Background");
        background.GetComponent<Image>().enabled = false;
        background.transform.GetChild(0).GetComponent<Text>().enabled = false;
    }

    // set the level number
    public int setLevel(int levelNumber)
    {
        this.levelNumber = levelNumber;
        return levelNumber;
    }

    // set the wave number
    public int setWave(int waveNumber)
    {
        this.waveNumber = waveNumber;
        return waveNumber;
    }

    // get the level number
    public int getLevel ()
    {
        return levelNumber;
    }

    // get the wave number
    public int getWave ()
    {
        return waveNumber;
    }

    // display the correct level and wave number
    public void setLevelWave ()
    {
        // the level component
        levelWave = GameObject.FindGameObjectWithTag("Level Wave Number");

        // update the display
        string level = displayLevel + levelNumber.ToString();
        string wave = displayWave + waveNumber.ToString();
        display = level + " " + wave;
        levelWave.GetComponent<Text>().text = display;
    }
}
