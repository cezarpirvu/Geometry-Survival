using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemies : MonoBehaviour
{

    // dictionary that keeps the enemies name and their unique id
    public Dictionary<int, string> enemiesIdentity;

    // the numbers of "Enemy1" enemies on the map
    private int enemies1Number;
    // the numbers of "Enemy2" enemies on the map
    private int enemies2Number;
    // the numbers of "Enemy3" enemies on the map
    private int enemies3Number;
    // the numbers of "Enemy4" enemies on the map
    private int enemies4Number;
    // the numbers of "Enemy5" enemies on the map
    private int enemies5Number;
    // the total number of enemies on the map
    private int totalEnemies;
    // the current number of enemies on the map
    private int currentEnemiesNumber;

    // the size of the plane
    private float borderX;
    private float borderY;
    private float borderZ;

    // the position of the player on the Y axis
    private float playerPositionY = 0.5f;

    // Use this for initialization
    void Start()
    {
        // the plane of the map
        MapSettings plane = (MapSettings)FindObjectOfType(typeof(MapSettings));

        // get the size of the plane
        Mesh mesh = plane.GetComponent<MeshFilter>().mesh;
        Bounds bounds = mesh.bounds;

        float boundsX = transform.localScale.x * bounds.size.x;
        float boundsZ = transform.localScale.z * bounds.size.z;

        // calculate the playable size of the plane
        borderX = boundsX - plane.GetComponent<Transform>().parent.localScale.x;
        borderZ = boundsZ - plane.GetComponent<Transform>().parent.localScale.z;

        // initial enemy types numbers on the map
        enemies1Number = 1;
        enemies2Number = 0;
        enemies3Number = 0;
        enemies4Number = 0;
        enemies5Number = 0;

        // the total number of enemies
        totalEnemies = enemies1Number + enemies2Number + enemies3Number + enemies4Number + enemies5Number;

        // set the current number of enemies as the sum of all the enemy types
        currentEnemiesNumber = totalEnemies;

        // insert the enemies and their correspondent identities
        enemiesIdentity = new Dictionary<int, string>();
        enemiesIdentity.Add(1, "Enemy1");
        enemiesIdentity.Add(2, "Enemy2");
        enemiesIdentity.Add(3, "Enemy3");
        enemiesIdentity.Add(4, "Enemy4");
        enemiesIdentity.Add(5, "Enemy5");
    }

    // generate the enemies random on the map
    public void generateEnemies(int enemiesNumber, string enemyName)
    {
        // generate only if the number of enemies objects is positive
        if (enemiesNumber > 0)
        {
            // instantiate the enemy object
            GameObject enemyPrefab = Resources.Load(enemyName) as GameObject;

            // list of enemies
            Obstacles obstacles = (Obstacles)FindObjectOfType(typeof(Obstacles));
            List<Vector3> enemyList = new List<Vector3>(obstacles.getFreePositions());

            int count = 0;
            Player player = (Player)FindObjectOfType(typeof(Player));
            float enemyMarginY = playerPositionY;

            // count the number of searches for positions in the list of free positions
            int iterations = enemyList.Count;
            // generate enemies position
            while (true)
            {
                int enemyMarginX = Random.Range(-(int)Mathf.Round(borderX), (int)Mathf.Round(borderX));
                int enemyMarginZ = Random.Range(-(int)Mathf.Round(borderZ), (int)Mathf.Round(borderZ));

                // random enemies position inside the borders of the map
                Vector3 enemyPosition = new Vector3(enemyMarginX, enemyMarginY, enemyMarginZ);

                // check if there are no "Wall" obstacles blocking the enemy
                Vector3 northPosition = new Vector3(enemyMarginX + 1, enemyMarginY, enemyMarginZ);
                Vector3 southPosition = new Vector3(enemyMarginX - 1, enemyMarginY, enemyMarginZ);
                Vector3 eastPosition = new Vector3(enemyMarginX, enemyMarginY, enemyMarginZ - 1);
                Vector3 westPosition = new Vector3(enemyMarginX, enemyMarginY, enemyMarginZ + 1);

                // remove the position that is blocked from the lsit of free positions for the enemies
                if (!enemyList.Contains(northPosition) && !enemyList.Contains(southPosition) && !enemyList.Contains(eastPosition) && !enemyList.Contains(westPosition))
                {
                    enemyList.Remove(enemyPosition);
                }

                // if the generated enemy position is a free position, then mark it as occupied and add the enemy
                if (enemyList.Contains(enemyPosition))
                {
                    GameObject enemy = Instantiate(enemyPrefab) as GameObject;
                    enemy.transform.position = enemyPosition;

                    // add the instantiated enemy to a parent
                    GameObject enemies = GameObject.FindGameObjectWithTag("Enemies");
                    enemy.transform.SetParent(enemies.transform);
                    // set the same layer to the enemies as their parent
                    enemy.layer = enemies.layer;

                    //remove the occupied position from the list of free positions
                    enemyList.Remove(enemyPosition);
                    count++;
                }
                else
                {
                    iterations--;
                }

                // generate enemies positions until the enemy1 number is satisfied
                if (count == enemiesNumber)
                {
                    break;
                }

                // if the number of maximum iterations has been surpassed
                if (iterations <= 0)
                {
                    // if there are still positions free in the list and positions
                    // manually add the enemy to a random free position
                    if (enemyList.Count != 0)
                    {
                        int length = enemyList.Count;
                        int position = Random.Range(0, length);

                        GameObject enemy = Instantiate(enemyPrefab) as GameObject;
                        enemy.transform.position = enemyList[position];

                        // add the instantiated enemy to a parent
                        GameObject enemies = GameObject.FindGameObjectWithTag("Enemies");
                        enemy.transform.SetParent(enemies.transform);

                        //remove the occupied position from the list of free positions
                        enemyList.RemoveAt(position);
                        count++;
                    }
                    else
                    {
                        break;
                    }

                    if (count == enemiesNumber)
                    {
                        break;
                    }
                }
            }

            // update the list of free positions of the map
            obstacles.updateList(enemyList);
        }
    }

    // set the number of enemies to be generated
    public void setEnemies1Number(int enemies)
    {
        enemies1Number = enemies;
    }

    // set the number of enemies to be generated
    public void setEnemies2Number(int enemies)
    {
        enemies2Number = enemies;
    }

    // set the number of enemies to be generated
    public void setEnemies3Number(int enemies)
    {
        enemies3Number = enemies;
    }

    // set the number of enemies to be generated
    public void setEnemies4Number(int enemies)
    {
        enemies4Number = enemies;
    }

    // set the number of enemies to be generated
    public void setEnemies5Number(int enemies)
    {
        enemies5Number = enemies;
    }

    // decrease the number of enemies currently on the map if they are destroyed
    public void enemyKilled()
    {
        currentEnemiesNumber--;
    }

    // get the number of enemies currently on the map
    public int getCurrentEnemiesNumber()
    {
        return currentEnemiesNumber;
    }

    // set the number of enemies currently on the map
    public void setCurrentEnemiesNumber(int enemiesNumber)
    {
        currentEnemiesNumber = enemiesNumber;
    }

    // get the number of enemies of type "Enemy1"
    public int getEnemies1Number()
    {
        return enemies1Number;
    }

    // get the number of enemies of type "Enemy2"
    public int getEnemies2Number()
    {
        return enemies2Number;
    }

    // get the number of enemies of type "Enemy3"
    public int getEnemies3Number()
    {
        return enemies3Number;
    }

    // get the number of enemies of type "Enemy4"
    public int getEnemies4Number()
    {
        return enemies4Number;
    }

    // get the number of enemies of type "Enemy5"
    public int getEnemies5Number()
    {
        return enemies5Number;
    }

    // return the dictionary with the enemies types and their identities
    public Dictionary<int, string> getEnemiesIdentities()
    {
        return enemiesIdentity;
    }
}
