using UnityEngine;
using System.Collections;

public class FloorTiles : MonoBehaviour
{

    // the materials for the textures of the floor
    public Material[] materials;

    // Use this for initialization
    void Start()
    {
        // get the plane dimensions
        MapSettings plane = MapSettings.instance;
        int boundsX = (int)plane.getBoundsX() - 1;
        int boundsZ = (int)plane.getBoundsZ() - 1;

        for (int i = - boundsX; i <= boundsX; i++)
        {
            for (int j = - boundsZ; j <= boundsZ; j++)
            {
                GameObject floorTilesPrefab = Resources.Load("Floor Tiles") as GameObject;
                GameObject floorTile = Instantiate(floorTilesPrefab) as GameObject;

                //place the tile on the plane
                floorTile.transform.SetParent(GameObject.FindGameObjectWithTag("Floor").transform);
                floorTile.transform.position = new Vector3(i, 0.05f, j);

                // set a random material on each tile
                int index = Random.Range(0, materials.Length);
                Renderer renderer = floorTile.GetComponent<Renderer>();
                renderer.material = materials[index];
            }
        }
    }
}
