using UnityEngine;
using System.Collections;

public class WallTiles : MonoBehaviour
{

    // the materials for the textures of the walls
    public Material[] materials;

    // Use this for initialization
    void Start()
    {
        // get the plane dimensions
        MapSettings plane = MapSettings.instance;
        int boundsX = (int)plane.getBoundsX();
        int boundsZ = (int)plane.getBoundsZ();

        GameObject wallTilesPrefab = Resources.Load("Wall Tiles") as GameObject;

        // set a random material on each tile
        int index = Random.Range(0, materials.Length);
        Renderer renderer = wallTilesPrefab.GetComponent<Renderer>();
        renderer.material = materials[index];

        for (int i = -boundsX; i <= boundsX; i++)
        {
            GameObject wallTileNorth = Instantiate(wallTilesPrefab) as GameObject;
            GameObject wallTileSouth = Instantiate(wallTilesPrefab) as GameObject;
            GameObject wallTileWest = Instantiate(wallTilesPrefab) as GameObject;
            GameObject wallTileEast = Instantiate(wallTilesPrefab) as GameObject;

            if (i != 0)
            {
                //place the texture on the wall
                wallTileNorth.transform.SetParent(GameObject.FindGameObjectWithTag("Walls").transform);
                wallTileNorth.transform.position = new Vector3(i, 0.5f, boundsZ);

                wallTileSouth.transform.SetParent(GameObject.FindGameObjectWithTag("Walls").transform);
                wallTileSouth.transform.position = new Vector3(i, 0.5f, -boundsZ);

                wallTileWest.transform.SetParent(GameObject.FindGameObjectWithTag("Walls").transform);
                wallTileWest.transform.position = new Vector3(-boundsZ, 0.5f, i);

                wallTileEast.transform.SetParent(GameObject.FindGameObjectWithTag("Walls").transform);
                wallTileEast.transform.position = new Vector3(boundsZ, 0.5f, -i);
            }
            else
            {
                //place the texture on the wall
                wallTileNorth.transform.SetParent(GameObject.FindGameObjectWithTag("Walls").transform);
                wallTileNorth.transform.position = new Vector3(0, 0.5f, boundsZ);

                wallTileSouth.transform.SetParent(GameObject.FindGameObjectWithTag("Walls").transform);
                wallTileSouth.transform.position = new Vector3(0, 0.5f, -boundsZ);

                wallTileWest.transform.SetParent(GameObject.FindGameObjectWithTag("Walls").transform);
                wallTileWest.transform.position = new Vector3(-boundsZ, 0.5f, 0);

                wallTileEast.transform.SetParent(GameObject.FindGameObjectWithTag("Walls").transform);
                wallTileEast.transform.position = new Vector3(boundsZ, 0.5f, 0);
            }
        } 
    }
}
