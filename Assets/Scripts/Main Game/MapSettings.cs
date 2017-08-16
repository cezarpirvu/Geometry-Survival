using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MapSettings : MonoBehaviour
{

    // instantiate the class so that it can be reffered to from other scripts
    public static MapSettings instance;

    //the size of the map
    private int mapSize;

    // the bounds of the mesh
    private float boundsX;
    private float boundsY;
    private float boundsZ;

    // the scale of the walls
    private GameObject scaleWalls;

    // the pause game menu canvas
    private PauseMenu pauseMenu;

    // the player
    private Player player;

    // check if the player has paused
    private bool pause = false;
    private bool escapePressed = false;
    private bool pausePressed = false;

    // when the script instance is awaken, store the instance class (used for references in scripts)
    void Awake()
    {
        instance = this;

        // load the map size that was given in the main menu
        MapSizeButton map = MapSizeButton.instance;
        mapSize = map.getMapSize();

        // get the mesh component and the bounds
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Bounds bounds = mesh.bounds;

        // modify the size of the map accordingly
        transform.localScale = new Vector3(transform.localScale.x * mapSize, transform.localScale.y, transform.localScale.z * mapSize);
        // the scale of the walls
        scaleWalls = GameObject.FindGameObjectWithTag("Walls");
        scaleWalls.transform.localScale = new Vector3(scaleWalls.transform.localScale.x * mapSize, scaleWalls.transform.localScale.y, scaleWalls.transform.localScale.z * mapSize);

        // get the size of the plane
        boundsX = transform.localScale.x * bounds.size.x;
        boundsY = transform.localScale.y * bounds.size.y;
        boundsZ = transform.localScale.z * bounds.size.z;

        // ignore collisions between bullets
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Bullets"), LayerMask.NameToLayer("Bullets"), true);
        // ignore collisions between bullets and items
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Bullets"), LayerMask.NameToLayer("Items"), true);
        // ignore collisions between the enemy's bullets and spikes obstacles
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Bullets"), LayerMask.NameToLayer("Spikes"), true);

        // ignore collisions between enemies and items
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemies"), LayerMask.NameToLayer("Items"), true);
        // ignore collisions between the enemies and spikes obstacles
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemies"), LayerMask.NameToLayer("Spikes"), true);
        // ignore collisions between enemies and their bullets
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemies"), LayerMask.NameToLayer("Bullets"), true);

        // ignore collisions between the player and it's bullets
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player Bullets"), LayerMask.NameToLayer("Player"), true);
        // ignore collisions between the player's bullets and the bullets of enemies
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player Bullets"), LayerMask.NameToLayer("Bullets"), true);
        // ignore collisions between the player's bullets and spikes obstacles
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player Bullets"), LayerMask.NameToLayer("Spikes"), true);

        // ignore collisions between enemies
        //Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemies"), LayerMask.NameToLayer("Enemies"), true);
    }

    // Use this for initialization
    void Start()
    {
        // reference to the pause game menu
        pauseMenu = (PauseMenu)FindObjectOfType(typeof(PauseMenu));
        player = (Player)FindObjectOfType(typeof(Player));

        // initially the game is unpaused
        pause = false;
        escapePressed = false;
        pausePressed = false;
        // deactivate the pause display
        GameObject paused = GameObject.Find("Game Paused");
        paused.GetComponent<Text>().enabled = false;

        // remove the item description
        GameObject itemDescription = GameObject.FindGameObjectWithTag("Item Description");
        itemDescription.GetComponent<Text>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // if the player pressed "Escape" then show the pause menu and pause
        if (Input.GetKeyDown("escape") && !pause && !pausePressed)
        {
            // freeze the game
            Time.timeScale = 0;

            // show the pause game canvas
            pauseMenu.GetComponent<Canvas>().sortingOrder = 1;
            pauseMenu.GetComponent<CanvasGroup>().alpha = 1;
            pauseMenu.GetComponent<CanvasGroup>().interactable = true;

            // the game is paused
            pause = true;
            escapePressed = true;
        }
        else
        {
            // if the pause menu is shown and the player presses "Escape" then resume
            if (Input.GetKeyDown("escape") && pause && !pausePressed)
            {
                player = (Player)FindObjectOfType(typeof(Player));
                if (player.getPlayerLife() > 0)
                {
                    // unfreeze the game
                    Time.timeScale = 1;
                }
                // show the pause game canvas
                pauseMenu.GetComponent<Canvas>().sortingOrder = 0;
                pauseMenu.GetComponent<CanvasGroup>().alpha = 0;
                pauseMenu.GetComponent<CanvasGroup>().interactable = false;

                // the game is no longer paused
                pause = false;
                escapePressed = false;
            }
        }

        if (player.getPlayerLife() > 0)
        {
            // if the player presses "P" then the game pauses
            if (Input.GetKeyDown("p") && !pause && !escapePressed)
            {
                // freeze the game
                Time.timeScale = 0;

                // pause the track that is currently playing
                Playlist currentTrack = (Playlist)FindObjectOfType(typeof(Playlist));
                currentTrack.setPause(true);

                // activate the pause display
                GameObject paused = GameObject.Find("Game Paused");
                paused.GetComponent<Text>().enabled = true;

                // the game is paused
                pause = true;
                pausePressed = true;
            }
            else
            {
                // if the player has already pressed "P"
                if (Input.GetKeyDown("p") && pause && !escapePressed)
                {
                    // unfreeze the game
                    Time.timeScale = 1;

                    // unpause the track that is currently playing
                    Playlist currentTrack = (Playlist)FindObjectOfType(typeof(Playlist));
                    currentTrack.setPause(false);

                    // deactivate the pause display
                    GameObject paused = GameObject.Find("Game Paused");
                    paused.GetComponent<Text>().enabled = false;

                    // the game is no longer paused
                    pause = false;
                    pausePressed = false;
                }
            }
        }
    }

    // return the bounds on the X axis
    public float getBoundsX()
    {
        return boundsX;
    }

    // return the bounds on the  axis
    public float getBoundsZ()
    {
        return boundsZ;
    }

    // return the northest bound of the game area
    public float getNorth()
    {
        return boundsZ - scaleWalls.transform.localScale.z;
    }

    // return the northest bound of the game area
    public float getSouth()
    {
        return scaleWalls.transform.localScale.z - boundsZ;
    }

    // return the northest bound of the game area
    public float getWest()
    {
        return boundsX - scaleWalls.transform.localScale.x;
    }

    // return the northest bound of the game area
    public float getEast()
    {
        return scaleWalls.transform.localScale.x - boundsX;
    }

    // get the size of the map
    public int getMapSize()
    {
        return mapSize;
    }

    // set the map size
    public void setMapSize(int size)
    {
        mapSize = size;
    }

    // set the pause true or false
    public void setPause(bool check)
    {
        pause = check;
    }

    // set escape paused
    public void setEscape(bool check)
    {
        escapePressed = check;
    }
}
