using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Obstacles : MonoBehaviour
{
    // the materials for the textures of the walls
    public Material[] materials;

    // list of free positions
    public List<Vector3> positions;
    //list of initial free positions
    public List<Vector3> initialPositions;

    // dictionary that keeps the obstacle name and their unique id
    public Dictionary<int, string> obstaclesIdentity;

    // the number of obstacles on the map
    private int wallsNumber;
    // the number of spikes on the map
    private int spikesNumber;
    // the number of dynamic spikes on the map
    private int dynamicSpikesNumber;

    // the damage that is done by hitting the spikes
    public int spikeDamage = 1;

    // the size of the plane
    private float borderX;
    private float borderY;
    private float borderZ;

    // the position of the player on the Y axis
    private float playerPositionY = 0.5f;

    void Awake()
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

        // initial values for the number of obstacles on the map
        wallsNumber = 1;
        spikesNumber = 0;
        dynamicSpikesNumber = 0;

        // insert the obstacles and their correspondent identities
        obstaclesIdentity = new Dictionary<int, string>();
        obstaclesIdentity.Add(1, "WallObstacle");
        obstaclesIdentity.Add(2, "Spikes");
        obstaclesIdentity.Add(3, "Dynamic Spikes");

        // clear the list of positions
        initializeList(positions);
        initializeList(initialPositions);
    }

    // start the dynamic spike movement
    IEnumerator startDynamicSpike(Transform obstacleTransform, Vector3 start, Vector3 end, float time)
    {
        while (true)
        {
            yield return moveDynamicSpike(obstacleTransform, start, end, time);
            yield return moveDynamicSpike(obstacleTransform, end, start, time);
        }
    }

    // create the routine that is going to be used to generate the dynamic spike movement
    IEnumerator moveDynamicSpike(Transform obstacleTransform, Vector3 start, Vector3 end, float time)
    {
        float i = 0.0f;
        if (time != 0.0f)
        {
            while (i < 1.0f)
            {
                i += Time.deltaTime / time;
                if (obstacleTransform != null)
                {
                    obstacleTransform.position = Vector3.Lerp(start, end, i);
                }
                yield return null;
            }
        }
    }

    // initialize the list of free positions
    public void initializeList(List<Vector3> list)
    {
        // get the player position
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        Mesh mesh = player.GetComponent<MeshFilter>().mesh;
        Bounds bounds = mesh.bounds;
        float boundsX = player.transform.localScale.x * bounds.size.x;
        float boundsZ = player.transform.localScale.z * bounds.size.z;

        // size of the area around the player that is used as a buffer space when generating obstacles
        float buffer = 2;

        // convert from float to int
        int marginX = (int)Mathf.Round(borderX);
        borderY = playerPositionY;
        int marginZ = (int)Mathf.Round(borderZ);

        //list.Clear();

        // create the free positions excluding the position of the player and allow him to have space
        for (int i = -marginX; i <= marginX; i++)
        {
            for (int j = -marginZ; j <= marginZ; j++)
            {
                if ((i <= (boundsX + buffer) && i >= (-boundsX - buffer)) && (j <= (boundsZ + buffer) && j >= (-boundsZ - buffer)) ||
                    (i <= (player.transform.localPosition.x + buffer) && i >= (-player.transform.localPosition.x - buffer)) && 
                    (j <= (player.transform.localPosition.z + buffer) && j >= (-player.transform.localPosition.z - buffer)))
                {

                }
                else
                {
                    list.Add(new Vector3(i, borderY, j));
                }
            }
        }

        updateList(list);
    }

    // generate obstacles on the map
    public void generateObstacles(int obstaclesNumber, string obstacleName)
    {
        // generate only if the number of obstacle objects is positive
        if (obstaclesNumber > 0)
        {
            // instantiate the obstacle object
            GameObject obstaclePrefab = Resources.Load(obstacleName) as GameObject;

            // set a random material on each wall obstacle
            if (obstacleName.Equals("WallObstacle"))
            {
                int index = Random.Range(0, materials.Length);
                Renderer renderer = obstaclePrefab.GetComponent<Renderer>();
                renderer.material = materials[index];
            }

            int count = 0;
            float obstacleMarginY = borderY;
            // count the number of searches for positions in the list of free positions
            int freePositions = positions.Count;
            // generate obstacle position
            while (true)
            {
                int obstacleMarginX = Random.Range(-(int)Mathf.Round(borderX), (int)Mathf.Round(borderX));
                int obstacleMarginZ = Random.Range(-(int)Mathf.Round(borderZ), (int)Mathf.Round(borderZ));

                // random obstacle position inside the borders of the map
                Vector3 obstaclePosition = new Vector3(obstacleMarginX, obstacleMarginY, obstacleMarginZ);

                // if the generated obstacle position is a free position, then mark it as occupied and add the obstacle
                if (positions.Contains(obstaclePosition))
                {
                    GameObject obstacle = Instantiate(obstaclePrefab) as GameObject;
                    obstacle.transform.position = obstaclePosition;

                    if (obstacleName == "Spikes")
                    {
                        // get the size of the spike on the Y axis
                        float sizeY = obstacle.transform.GetChild(0).GetComponent<MeshCollider>().bounds.extents.y;
                        obstacle.transform.position = new Vector3(obstaclePosition.x, sizeY, obstaclePosition.z);
                    } else
                    {
                        if (obstacleName == "Dynamic Spikes")
                        {
                            // get the size of the dynamic spike on the Y axis
                            float sizeY = obstacle.transform.GetChild(0).GetComponent<MeshCollider>().bounds.extents.y;

                            // the start and the end position of the dynamic spike movement
                            Vector3 start = new Vector3(obstaclePosition.x, sizeY, obstaclePosition.z);
                            Vector3 end = new Vector3(obstaclePosition.x, -sizeY * 2, obstaclePosition.z);

                            if (obstacle.transform != null)
                            {
                                // start the coroutine for the dynamic spike movement
                                StartCoroutine(startDynamicSpike(obstacle.transform, start, end, 4.0f));
                            }
                        }
                    }

                    // add the instantiated obstacles to a parent
                    GameObject worldObstacles = GameObject.FindGameObjectWithTag("World Obstacles");
                    obstacle.transform.SetParent(worldObstacles.transform);

                    //remove the occupied position from the list of free positions
                    positions.Remove(obstaclePosition);
                    count++;
                }
                else
                {
                    freePositions--;
                }

                // generate wall positions until the obstacle number is satisfied
                if (count == obstaclesNumber)
                {
                    break;
                }

                // if the list doesn't contain any more free positions
                if (freePositions == 0)
                {
                    break;
                }
            }
        }
    }

    // update the list
    public void updateList(List<Vector3> list)
    {
        positions = new List<Vector3>(list);
    }

    // return the list of free initial positions
    public List<Vector3> getInitialPositions()
    {
        return initialPositions;
    }

    // get the list of free positions
    public List<Vector3> getFreePositions()
    {
        return positions;
    }

    // set the number of wall obstacles
    public int getWallsNumber()
    {
        return wallsNumber;
    }

    // get the number of wall obstacles
    public void setWallsNumber(int wallsNumber)
    {
        this.wallsNumber = wallsNumber;
    }

    // get the number of spike obstacles
    public int getSpikesNumber()
    {
        return spikesNumber;
    }

    // set the number of spike obstacles
    public void setSpikesNumber(int spikesNumber)
    {
        this.spikesNumber = spikesNumber;
    }

    // get the number of dynamic spike obstacles
    public int getDynamicSpikesNumber()
    {
        return dynamicSpikesNumber;
    }

    // set the number of dynamic spike obstacles
    public void setDynamicSpikesNumber(int dynamicSpikesNumber)
    {
        this.dynamicSpikesNumber = dynamicSpikesNumber;
    }

    // return the dictionary with the obstacles names and thier identities
    public Dictionary<int, string> getObstaclesIdentities()
    {
        return obstaclesIdentity;
    }
}
