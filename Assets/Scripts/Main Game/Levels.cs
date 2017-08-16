using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Levels : MonoBehaviour
{

    // the materials for the textures of the walls
    public Material[] materials;

    // the position of the player on the Y axis
    private float playerPositionY = 0.5f;

    // check if the map can be generated
    private bool canGenerate = false;
    // check if the player has picked up the item
    private bool itemPicked = false;
    // number of items on the map
    private int itemsNumber;

    // seconds until a new level should be generated
    private float currentLevelTime = 4;
    // seconds until the enemies and player can start moving after a new level has been created
    private float newLevelTime = 2;

    // the size of the map
    private int mapSize = 1;
    private bool startedLevel = false;
    private bool endedLevel = false;
    private bool enableCounting = false;
    float counter = 0;

    // Use this for initialization
    void Start()
    {
        // initial number of items on the map
        itemsNumber = 0;

        // generate the obstacles
        Obstacles obstacle = (Obstacles)FindObjectOfType(typeof(Obstacles));
        obstacle.generateObstacles(obstacle.getWallsNumber(), "WallObstacle");
        // generate the enemies
        Enemies enemies = (Enemies)FindObjectOfType(typeof(Enemies));
        enemies.generateEnemies(enemies.getEnemies1Number(), "Enemy1");
    }

    // the delay until the background showing the level and the wave number dissapears -> 2 seconds
    IEnumerator delayLevelStarting(float seconds)
    {
        // remove the item description
        GameObject itemDescription = GameObject.FindGameObjectWithTag("Item Description");
        itemDescription.GetComponent<Text>().enabled = false;

        yield return new WaitForSeconds(seconds);

        // deactivate the level display
        GameObject background = GameObject.Find("Background");
        background.GetComponent<Image>().enabled = false;
        background.transform.GetChild(0).GetComponent<Text>().enabled = false;
    }

    // delay after finishing a level and generating the item for the player
    IEnumerator delayLevelFinished(float seconds)
    {
        HUDBackground hud1 = HUDBackground.instance;
        Enemies enemies = (Enemies)FindObjectOfType(typeof(Enemies));

        int wave1 = hud1.getWave();

        if (wave1 == 3 && enemies.getCurrentEnemiesNumber() == 0)
        {
            Items items = (Items)FindObjectOfType(typeof(Items));
            Dictionary<int, string> itemsIdentity = new Dictionary<int, string>(items.getItemsIdentities());

            int itemKey = Random.Range(1, itemsIdentity.Count);

            // count the total number of items
            int bulletItems = items.getBulletSpeedItemCount() + items.getBulletSizeItemCount() + items.getBulletFireRateItemCount() + items.getBulletRangeItemCount();
            int playerItems = items.getHeartCount() + items.getSpeedItemCount() + items.getDamageItemCount() + items.getBonusItemCount();
            int totalItems = bulletItems + playerItems;

            if (totalItems == 0 && !itemPicked)
            {
                // the player must receive a bonus item
                if (getCounter() <= mapSize * 60)
                {
                    if (items.getBonusItemCount() == 0)
                    {
                        items.setBonusItemCount(1);
                        int number = items.getBonusItemCount();
                        items.generateItems(number, "Bonus", "Player Items");
                        setItemPicked(true);
                    }

                    setCounter(0);
                    setStartedLevel(false);
                    setEndedLevel(false);
                }
                else
                {
                    if (itemsIdentity.ContainsKey(itemKey))
                    {
                        if (itemsIdentity[itemKey] == "Heart")
                        {
                            if (items.getHeartCount() == 0)
                            {
                                items.setHeartCount(1);
                                int number = items.getHeartCount();
                                items.generateItems(number, "Heart", "Player Items");
                                setItemPicked(true);
                            }
                        }
                        else
                        {
                            if (itemsIdentity[itemKey] == "Speed")
                            {
                                if (items.getSpeedItemCount() == 0)
                                {
                                    items.setSpeedItemCount(1);
                                    int number = items.getSpeedItemCount();
                                    items.generateItems(number, "Speed", "Player Items");
                                    setItemPicked(true);
                                }
                            }
                            else
                            {
                                if (itemsIdentity[itemKey] == "Damage")
                                {
                                    if (items.getDamageItemCount() == 0)
                                    {
                                        items.setDamageItemCount(1);
                                        int number = items.getDamageItemCount();
                                        items.generateItems(number, "Damage", "Player Items");
                                        setItemPicked(true);
                                    }
                                }
                                else
                                {
                                    if (itemsIdentity[itemKey] == "Bullet Speed")
                                    {
                                        if (items.getBulletSpeedItemCount() == 0)
                                        {
                                            items.setBulletSpeedItemCount(1);
                                            int number = items.getBulletSpeedItemCount();
                                            items.generateItems(number, "Bullet Speed", "Player Items");
                                            setItemPicked(true);
                                        }
                                    }
                                    else
                                    {
                                        if (itemsIdentity[itemKey] == "Bullet Range")
                                        {
                                            if (items.getBulletRangeItemCount() == 0)
                                            {
                                                items.setBulletRangeItemCount(1);
                                                int number = items.getBulletRangeItemCount();
                                                items.generateItems(number, "Bullet Range", "Player Items");
                                                setItemPicked(true);
                                            }
                                        }
                                        else
                                        {
                                            if (itemsIdentity[itemKey] == "Bullet Fire Rate")
                                            {
                                                if (items.getBulletFireRateItemCount() == 0)
                                                {
                                                    items.setBulletFireRateItemCount(1);
                                                    int number = items.getBulletFireRateItemCount();
                                                    items.generateItems(number, "Bullet Fire Rate", "Player Items");
                                                    setItemPicked(true);
                                                }
                                            }
                                            else
                                            {
                                                if (itemsIdentity[itemKey] == "Bullet Size")
                                                {
                                                    if (items.getBulletSizeItemCount() == 0)
                                                    {
                                                        items.setBulletSizeItemCount(1);
                                                        int number = items.getBulletSizeItemCount();
                                                        items.generateItems(number, "Bullet Size", "Player Items");
                                                        setItemPicked(true);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        yield return new WaitForSeconds(seconds);

        // destroy the item if it was not picked up
        GameObject gameItems = GameObject.FindGameObjectWithTag("Items");
        if (gameItems.transform.childCount != 0)
        {
            //Transform child = gameItems.transform.GetChild(0);
            foreach (Transform child in gameItems.transform)
            {
                if (child.tag == "Player Items")
                {
                    if (child.transform.childCount != 0)
                    {
                        foreach (Transform children in child.transform)
                        {
                            string clone = "(Clone)";
                            string name = children.transform.name;
                            string childName = name.Substring(0, name.Length - clone.Length);

                            Items items = (Items)FindObjectOfType(typeof(Items));
                            if (childName == "Heart")
                            {
                                items.setHeartCount(0);
                            }
                            else
                            {
                                if (childName == "Speed")
                                {
                                    items.setSpeedItemCount(0);
                                }
                                else
                                {
                                    if (childName == "Damage")
                                    {
                                        items.setDamageItemCount(0);
                                    }
                                    else
                                    {
                                        if (childName == "Bullet Speed")
                                        {
                                            items.setBulletSpeedItemCount(0);
                                        }
                                        else
                                        {
                                            if (childName == "Bullet Size")
                                            {
                                                items.setBulletSizeItemCount(0);
                                            }
                                            else
                                            {
                                                if (childName == "Bullet Fire Rate")
                                                {
                                                    items.setBulletFireRateItemCount(0);
                                                }
                                                else
                                                {
                                                    if (childName == "Bullet Range")
                                                    {
                                                        items.setBulletRangeItemCount(0);
                                                    }
                                                    else
                                                    {
                                                        if (childName == "Bonus")
                                                        {
                                                            items.setBonusItemCount(0);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            // destroy the item
                            Destroy(children.gameObject);
                        }
                    }
                }
            }
        }

        // destroy player projectiles when the wave has been finished
        /*GameObject playerProjectiles = GameObject.FindGameObjectWithTag("Player Projectiles");
        if (playerProjectiles.transform.childCount != 0)
        {
            foreach(Transform children in playerProjectiles.transform)
            {
                if (children != null)
                {
                    Destroy(children.gameObject);
                }
            }
        }*/

        // destroy enemy projectiles when the wave has been finished
        GameObject enemyProjectiles = GameObject.FindGameObjectWithTag("Enemy Projectiles");
        if (enemyProjectiles.transform.childCount != 0)
        {
            foreach (Transform children in enemyProjectiles.transform)
            {
                if (children != null)
                {
                    Destroy(children.gameObject);
                }
            }
        }

        // mark that the item is not picked
        setItemPicked(false);
        // check if the map can be generated
        canGenerate = true;
    }

    // Update is called once per frame
    void Update()
    {
        Enemies enemies = (Enemies)FindObjectOfType(typeof(Enemies));
        HUDBackground hud = HUDBackground.instance;

        int wave = hud.getWave();
        int level = hud.getLevel();

        // if the player finishes a level within a certain time, it receives a bonus
        if (wave == 1 && !startedLevel)
        {
            enableCounting = true;
            startedLevel = true;
        }

        if (enableCounting)
        {
            counter += Time.deltaTime;
        }

        if (wave == 3 && enemies.getCurrentEnemiesNumber() == 0 && !endedLevel)
        {
            endedLevel = true;
        }

        // if there are no more enemies left, then a new wave must be started
        if (enemies.getCurrentEnemiesNumber() == 0)
        {
            if (wave != 3)
            {
                // if the level is finshed the player should be able to still move for a couple of seconds
                StartCoroutine(delayLevelFinished(currentLevelTime));
            }
            else
            {
                // if the level is finshed the player should be able to still move for a couple of seconds
                StartCoroutine(delayLevelFinished(currentLevelTime * 2));
            }

            if (canGenerate)
            {
                // generate the map with the enemies and obstacles
                generateMap();
                canGenerate = false;
            }
        }
        else
        {
            canGenerate = false;
        }
    }

    // generate the levels of the game and the enemies
    void generateMap()
    {
        Enemies enemies = (Enemies)FindObjectOfType(typeof(Enemies));

        // if there are no more enemies left, then a new wave must be started
        if (enemies.getCurrentEnemiesNumber() == 0)
        {
            HUDBackground hud = HUDBackground.instance;
            int wave = hud.getWave();
            int level = hud.getLevel();

            // check the current wave number and update it
            if (wave < 3)
            {
                wave++;
                hud.setWave(wave);
                hud.setLevelWave();
            }
            else
            {
                // generate the wall texture for each level
                GameObject walls = GameObject.FindGameObjectWithTag("Walls");
                if (walls.transform.childCount != 0)
                {
                    int index = Random.Range(0, materials.Length);
                    foreach (Transform child in walls.transform)
                    {
                        Renderer renderer = child.GetComponent<Renderer>();
                        renderer.material = materials[index];
                    }
                }

                level++;
                hud.setLevel(level);
                wave = 1;
                itemPicked = false;
                hud.setWave(wave);
                hud.setLevelWave();

                Obstacles obstacle1 = (Obstacles)FindObjectOfType(typeof(Obstacles));
                // list of free positions
                Obstacles obstacle2 = (Obstacles)FindObjectOfType(typeof(Obstacles));
                //List<Vector3> positions = new List<Vector3>(obstacle2.getInitialPositions());
                List<Vector3> positions = new List<Vector3>(obstacle2.getInitialPositions());

                print(obstacle1.positions.Count);

                if (obstacle1.positions.Count <= (positions.Count / 2))
                {
                    GameObject obs = GameObject.FindGameObjectWithTag("World Obstacles");
                    if (obs.transform.childCount >= 0)
                    {
                        foreach (Transform child in obs.transform)
                        {
                            if (child != null)
                            {
                                Destroy(child.gameObject);
                            }
                        }
                    }

                    obstacle2.updateList(positions);
                }

                Playlist track = (Playlist)FindObjectOfType(typeof(Playlist));
                track.changeTrack();
            }

            // activate the level display
            GameObject background = GameObject.Find("Background");
            background.GetComponent<Image>().enabled = true;
            background.transform.GetChild(0).GetComponent<Text>().enabled = true;

            // show the background with the level and wave number
            StartCoroutine(delayLevelStarting(newLevelTime));

            Obstacles obstacle = (Obstacles)FindObjectOfType(typeof(Obstacles));
            Dictionary<int, string> obstaclesDictionary = new Dictionary<int, string>(obstacle.getObstaclesIdentities());

            int levelNumber = hud.getLevel();
            int waveNumber = hud.getWave();

            // load the map size that was given in the main menu
            MapSizeButton map = MapSizeButton.instance;
            mapSize = map.getMapSize();

            // checking every obstacle type in order to generate their appearences corresponding to the level number
            // if the beginning of a level is encountered, then start generating the level
            if (levelNumber >= 1 && waveNumber == 1)
            {
                foreach (int key in obstaclesDictionary.Keys)
                {
                    // for the wall obstacles
                    if (obstaclesDictionary[key] == "WallObstacle")
                    {
                        // if it can appear in the curretn level
                        if (key <= levelNumber)
                        {
                            // generate a random number for its occurrences
                            int count = Random.Range(levelNumber, mapSize * levelNumber);
                            obstacle.setWallsNumber(count);
                            // generate the walls
                            obstacle.generateObstacles(obstacle.getWallsNumber(), "WallObstacle");
                        }
                    }
                    else
                    {
                        // for the spikes obstacles
                        if (obstaclesDictionary[key] == "Spikes")
                        {
                            // if it can appear in the curretn level
                            if (key <= levelNumber)
                            {
                                if (levelNumber > 10)
                                {
                                    // generate a random number for its occurrences
                                    int count = Random.Range(10, mapSize * 10);
                                    obstacle.setSpikesNumber(count);
                                    // generate the walls
                                    obstacle.generateObstacles(obstacle.getSpikesNumber(), "Spikes");
                                }
                                else
                                {
                                    // generate a random number for its occurrences
                                    int count = Random.Range(levelNumber, mapSize * levelNumber);
                                    obstacle.setSpikesNumber(count);
                                    // generate the walls
                                    obstacle.generateObstacles(obstacle.getSpikesNumber(), "Spikes");
                                }
                            }
                        }
                        else
                        {
                            // for the spikes obstacles
                            if (obstaclesDictionary[key] == "Dynamic Spikes")
                            {
                                // if it can appear in the current level
                                if (key <= levelNumber)
                                {
                                    if (levelNumber > 10)
                                    {
                                        // generate a random number for its occurrences
                                        int count = Random.Range(10, mapSize * 10);
                                        obstacle.setDynamicSpikesNumber(count);
                                        // generate the walls
                                        obstacle.generateObstacles(obstacle.getDynamicSpikesNumber(), "Dynamic Spikes");
                                    }
                                    else
                                    {
                                        // generate a random number for its occurrences
                                        int count = Random.Range(levelNumber, mapSize * levelNumber);
                                        obstacle.setDynamicSpikesNumber(count);
                                        // generate the walls
                                        obstacle.generateObstacles(obstacle.getDynamicSpikesNumber(), "Dynamic Spikes");
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Enemies enemy = (Enemies)FindObjectOfType(typeof(Enemies));
            Dictionary<int, string> enemiesDictionary = new Dictionary<int, string>(enemy.getEnemiesIdentities());

            foreach (int key in enemiesDictionary.Keys)
            {
                // for the "Enemy1" type enemies
                if (enemiesDictionary[key] == "Enemy1")
                {
                    // if it can appear in the current level
                    if (key <= levelNumber)
                    {
                        // generate a random number for its occurrences
                        int count = Random.Range(waveNumber, mapSize * waveNumber);
                        enemy.setEnemies1Number(count);
                        // generate the enemies
                        enemy.generateEnemies(enemy.getEnemies1Number(), "Enemy1");
                    }
                }
                else
                {
                    // for the "Enemy2" type enemies
                    if (enemiesDictionary[key] == "Enemy2")
                    {
                        // if it can appear in the current level
                        if (key <= levelNumber)
                        {
                            // generate a random number for its occurrences
                            int count = Random.Range(waveNumber, mapSize * waveNumber);
                            enemy.setEnemies2Number(count);
                            // generate the enemies
                            enemy.generateEnemies(enemy.getEnemies2Number(), "Enemy2");
                        }
                    }
                    else
                    {
                        // for the "Enemy3" type enemies
                        if (enemiesDictionary[key] == "Enemy3")
                        {
                            // if it can appear in the current level
                            if (key <= levelNumber)
                            {
                                // generate a random number for its occurrences
                                int count = Random.Range(waveNumber, mapSize * waveNumber);
                                enemy.setEnemies3Number(count);
                                // generate the enemies
                                enemy.generateEnemies(enemy.getEnemies3Number(), "Enemy3");
                            }
                        }
                        else
                        {
                            // for the "Enemy4" type enemies
                            if (enemiesDictionary[key] == "Enemy4")
                            {
                                // if it can appear in the current level
                                if (key <= levelNumber)
                                {
                                    // generate a random number for its occurrences
                                    int count = Random.Range(waveNumber, mapSize * waveNumber);
                                    enemy.setEnemies4Number(count);
                                    // generate the enemies
                                    enemy.generateEnemies(enemy.getEnemies4Number(), "Enemy4");
                                }
                            }
                            else
                            {
                                // for the "Enemy5" type enemies
                                if (enemiesDictionary[key] == "Enemy5")
                                {
                                    // if it can appear in the current level
                                    if (key <= levelNumber)
                                    {
                                        // generate a random number for its occurrences
                                        int count = Random.Range(waveNumber, mapSize * waveNumber);
                                        enemy.setEnemies5Number(count);
                                        // generate the enemies
                                        enemy.generateEnemies(enemy.getEnemies5Number(), "Enemy5");
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // update the current number of enemies
            int currentEnemiesNumber = enemy.getEnemies1Number() + enemy.getEnemies2Number() + enemy.getEnemies3Number() + enemy.getEnemies4Number() + enemy.getEnemies5Number();
            enemy.setCurrentEnemiesNumber(currentEnemiesNumber);
            Obstacles obstacle3 = (Obstacles)FindObjectOfType(typeof(Obstacles));
            print("Total " + currentEnemiesNumber + " Size " + obstacle3.positions.Count);
            if (obstacle3.positions.Count == 0)
            {
                GameObject itemDescription = GameObject.FindGameObjectWithTag("Item Description");
                itemDescription.GetComponent<Text>().enabled = true;
                itemDescription.GetComponent<Text>().text = "You have won! Congratulations!";
            }
        }
    }

    // check if the player has picked the item
    public bool getItemPicked()
    {
        return itemPicked;
    }

    // set if the player has picked an item or not
    public void setItemPicked(bool picked)
    {
        itemPicked = picked;
    }

    // get the counter for the time elapsed for a level
    public float getCounter()
    {
        return counter;
    }

    // set the counter for the time required to finish a level
    public void setCounter(float count)
    {
        counter = count;
    }

    // enable the counting to begin
    public void setEnableCounter(bool mark)
    {
        enableCounting = mark;
    }

    // mark if the level has started
    public void setStartedLevel(bool mark)
    {
        startedLevel = mark;
    }
    // mark if the level has finished
    public void setEndedLevel(bool mark)
    {
        endedLevel = mark;
    }
}
