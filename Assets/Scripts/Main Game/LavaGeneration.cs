using UnityEngine;
using System.Collections;

public class LavaGeneration : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        MapSettings plane = MapSettings.instance;

        // load the lava prefab
        GameObject lavaPrefab = Resources.Load("Lava") as GameObject;
        // generate the outside of the map
        int outerMapArea = plane.getMapSize() + 3;
        int innerMapArea = plane.getMapSize() - 1;

        for (int i = -outerMapArea; i <= outerMapArea; i += 2)
        {
            for (int j = -outerMapArea; j <= outerMapArea; j += 2)
            {
                if (!((i >= -innerMapArea && j >= -innerMapArea) && (i >= -innerMapArea && j <= innerMapArea) && (i <= innerMapArea && j >= -innerMapArea) && (i <= innerMapArea && j <= innerMapArea)))
                {
                    // instantiate the prefab
                    GameObject lava = Instantiate(lavaPrefab) as GameObject;
                    lava.transform.SetParent(GameObject.FindGameObjectWithTag("Lava").transform);
                    lava.transform.position = new Vector3(i * 10, 0, j * 10);
                }
            }
        }
    }
}
