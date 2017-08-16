using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Items : MonoBehaviour
{

    // the number of hearts on the map
    private int heartsNumber = 0;
    // the number of speed items on the map
    private int speedItemsNumber = 0;
    // the number of damage items on the map
    private int damageItemsNumber = 0;
    // the number of bullet speed items on the map
    private int bulletSpeedItemsNumber = 0;
    // the number of bullet range items on the map
    private int bulletRangeItemsNumber = 0;
    // the number of bullet fire rate items on the map
    private int bulletFireRateItemsNumber = 0;
    // the number of bullet size items on the map
    private int bulletSizeItemsNumber = 0;
    // the number of bonus items on the map
    private int bonusItemsNumber = 0;

    // dictionary that keeps the items name and their unique id
    public Dictionary<int, string> itemsIdentity;

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

        // the initial number of items on the map
        heartsNumber = 0;
        speedItemsNumber = 0;
        damageItemsNumber = 0;

        // insert the items and their correspondent identities
        itemsIdentity = new Dictionary<int, string>();
        itemsIdentity.Add(1, "Heart");
        itemsIdentity.Add(2, "Speed");
        itemsIdentity.Add(3, "Damage");
        itemsIdentity.Add(4, "Bullet Speed");
        itemsIdentity.Add(5, "Bullet Range");
        itemsIdentity.Add(6, "Bullet Fire Rate");
        itemsIdentity.Add(7, "Bullet Size");
        itemsIdentity.Add(8, "Bonus");
    }

    // generate the items for the player
    public void generateItems(int itemsNumber, string itemName, string parentName)
    {
        // generate only if the number of item objects is positive
        if (itemsNumber > 0)
        {
            // instantiate the item object
            GameObject itemPrefab = Resources.Load(itemName) as GameObject;

            // set true if the items will be generated in the center of the map or random
            bool center = true;
            if (center)
            {
                float itemMarginY = playerPositionY;

                GameObject item = Instantiate(itemPrefab) as GameObject;

                if (item.gameObject.name == "Bonus(Clone)")
                {
                    item.transform.position = new Vector3(0, itemMarginY * 1.5f, 0);
                } else
                {
                    item.transform.position = new Vector3(0, itemMarginY, 0);
                }

                // add the instantiated item to a parent
                GameObject playerItems = GameObject.FindGameObjectWithTag(parentName);
                item.transform.SetParent(playerItems.transform);
                // set the layer of the child the same as the one from the parent
                item.gameObject.layer = playerItems.layer;
                if (item.gameObject.name == "Heart(Clone)")
                {
                    Transform child = item.transform.GetChild(0);
                    child.gameObject.layer = playerItems.layer;
                    // set the tag
                    child.gameObject.tag = item.tag;
                }
            }
            else
            {
                int count = 0;
                float itemMarginY = playerPositionY;

                // list of items
                Obstacles obstacles = (Obstacles)FindObjectOfType(typeof(Obstacles));
                List<Vector3> itemsList = new List<Vector3>(obstacles.getFreePositions());

                // count the number of searches for positions in the list of free positions
                int iterations = itemsList.Count;
                // generate item position
                while (true)
                {
                    int itemMarginX = Random.Range(-(int)Mathf.Round(borderX), (int)Mathf.Round(borderX));
                    int itemMarginZ = Random.Range(-(int)Mathf.Round(borderZ), (int)Mathf.Round(borderZ));

                    // random item position inside the borders of the map
                    Vector3 itemPosition = new Vector3(itemMarginX, itemMarginY, itemMarginZ);

                    // if the generated item position is a free position, then mark it as occupied and add the heart
                    if (itemsList.Contains(itemPosition))
                    {
                        GameObject item = Instantiate(itemPrefab) as GameObject;
                        if (item.gameObject.name == "Bonus(Clone)")
                        {
                            item.transform.position = new Vector3(itemMarginX, itemMarginY * 1.5f, itemMarginZ);
                        }
                        else
                        {
                            item.transform.position = itemPosition;
                        }
                        
                        // add the instantiated item to a parent
                        GameObject playerItems = GameObject.FindGameObjectWithTag(parentName);
                        item.transform.SetParent(playerItems.transform);
                        // set the layer of the child the same as the one from the parent
                        item.gameObject.layer = playerItems.layer;
                        if (item.gameObject.name == "Heart(Clone)")
                        {
                            Transform child = item.transform.GetChild(0);
                            child.gameObject.layer = playerItems.layer;
                            // set the tag
                            child.gameObject.tag = item.tag;
                        }

                        //remove the occupied position from the list of free positions
                        itemsList.Remove(itemPosition);
                        count++;
                    }
                    else
                    {
                        iterations--;
                    }

                    // generate item positions until the enemy1 number is satisfied
                    if (count == itemsNumber)
                    {
                        break;
                    }

                    // if the number of maximum iterations has been surpassed
                    if (iterations <= 0)
                    {
                        // if there are still positions free in the list and positions
                        // manually add the enemy to a random free position
                        if (itemsList.Count != 0)
                        {
                            int length = itemsList.Count;
                            int position = Random.Range(0, length);

                            GameObject item = Instantiate(itemPrefab) as GameObject;
                            item.transform.position = itemsList[position];

                            // add the instantiated items to a parent
                            GameObject playerItems = GameObject.FindGameObjectWithTag(parentName);
                            item.transform.SetParent(playerItems.transform);

                            //remove the occupied position from the list of free positions
                            itemsList.RemoveAt(position);
                            count++;
                        }
                        else
                        {
                            break;
                        }

                        if (count == itemsNumber)
                        {
                            break;
                        }
                    }
                }

                // update the list of free positions of the map
                obstacles.updateList(itemsList);
            }
        }
    }

    // get the number of "Heart" items on the map
    public int getHeartCount()
    {
        return heartsNumber;
    }

    // set the number of "Heart" items on the map
    public void setHeartCount(int count)
    {
        heartsNumber = count;
    }

    // get the number of "Speed" items on the map
    public int getSpeedItemCount()
    {
        return speedItemsNumber;
    }

    // set the number of "Speed" items on the map
    public void setSpeedItemCount(int count)
    {
        speedItemsNumber = count;
    }

    // get the number of "Damage" items on the map
    public int getDamageItemCount()
    {
        return damageItemsNumber;
    }

    // set the number of "Damage" items on the map
    public void setDamageItemCount(int count)
    {
        damageItemsNumber = count;
    }

    // get the number of "Bullet Speed" items on the map
    public int getBulletSpeedItemCount()
    {
        return bulletSpeedItemsNumber;
    }

    // set the number of "Bullet Speed" items on the map
    public void setBulletSpeedItemCount(int count)
    {
        bulletSpeedItemsNumber = count;
    }

    // get the number of "Bullet Range" items on the map
    public int getBulletRangeItemCount()
    {
        return bulletRangeItemsNumber;
    }

    // set the number of "Bullet Range" items on the map
    public void setBulletRangeItemCount(int count)
    {
        bulletRangeItemsNumber = count;
    }

    // get the number of "Bullet Fire Rate" items on the map
    public int getBulletFireRateItemCount()
    {
        return bulletFireRateItemsNumber;
    }

    // set the number of "Bullet Fire Rate" items on the map
    public void setBulletFireRateItemCount(int count)
    {
        bulletFireRateItemsNumber = count;
    }

    // get the number of "Bullet Size" items on the map
    public int getBulletSizeItemCount()
    {
        return bulletSizeItemsNumber;
    }

    // set the number of "Bullet Size" items on the map
    public void setBulletSizeItemCount(int count)
    {
        bulletSizeItemsNumber = count;
    }

    // get the number of "Bonus" items on the map
    public int getBonusItemCount()
    {
        return bonusItemsNumber;
    }

    // set the number of "Bonus" items on the map
    public void setBonusItemCount(int count)
    {
        bonusItemsNumber = count;
    }

    // return the dictionary with the items names and thier identities
    public Dictionary<int, string> getItemsIdentities()
    {
        return itemsIdentity;
    }
}
