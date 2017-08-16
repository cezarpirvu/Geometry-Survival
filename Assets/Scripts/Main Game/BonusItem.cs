using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusItem : MonoBehaviour
{

    // set the value of the "Bonus" item
    private float bonusItem = 1;

    // Use this for initialization
    void Start()
    {
        // get the rigidbody
        Rigidbody rb = GetComponent<Rigidbody>();
        // set constraints on the rigidbody
        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;

        // the start and the end position of the "Bonus" item movement
        Vector3 start = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Vector3 end = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

        // move the item up and down
        StartCoroutine(startBonusItem(transform, start, end, 4.0f));
    }

    // start the items movement
    IEnumerator startBonusItem(Transform itemTransform, Vector3 start, Vector3 end, float time)
    {
        while (true)
        {
            yield return moveBonusItem(itemTransform, start, end, time);
            yield return moveBonusItem(itemTransform, end, start, time);
        }
    }

    // create the routine that is going to be used to generate the "Bonus" item's movement
    IEnumerator moveBonusItem(Transform itemTransform, Vector3 start, Vector3 end, float time)
    {
        float i = 0.0f;
        if (time != 0.0f)
        {
            while (i < 1.0f)
            {
                i += Time.deltaTime / time;
                itemTransform.position = Vector3.Lerp(start, end, i);
                yield return null;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // rotate the "Bonus" item object alongside it's Y axis
        transform.Rotate(0, 50 * Time.deltaTime, 0);
    }

    // get the value of the "Bullet Fire Rate" item
    public float getBonusItem()
    {
        return bonusItem;
    }

    // set the value of the "Bonus" item
    public void setBonusItem(float value)
    {
        bonusItem = value;
    }
}