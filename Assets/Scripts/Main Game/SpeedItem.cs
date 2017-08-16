using UnityEngine;
using System.Collections;

public class SpeedItem : MonoBehaviour
{

    // set the value of the speed item
    private float speedItem = 1;

    // Use this for initialization
    void Start()
    {
        // get the rigidbody
        Rigidbody rb = GetComponent<Rigidbody>();

        // set constraints on the rigidbody
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;

        // the start and the end position of the speed item movement
        Vector3 start = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Vector3 end = new Vector3(transform.position.x + 0.2f, transform.position.y, transform.position.z);

        // move the item forward and backward
        StartCoroutine(startSpeedItem(transform, start, end, 4.0f));
    }

    // start the items movement
    IEnumerator startSpeedItem(Transform itemTransform, Vector3 start, Vector3 end, float time)
    {
        while (true)
        {
            yield return moveSpeedItem(itemTransform, start, end, time);
            yield return moveSpeedItem(itemTransform, end, start, time);
        }
    }

    // create the routine that is going to be used to generate the speed item's movement
    IEnumerator moveSpeedItem(Transform itemTransform, Vector3 start, Vector3 end, float time)
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

    // get the value of the speed item
    public float getSpeedItem()
    {
        return speedItem;
    }

    // set the value of the speed item
    public void setSpeedItem(float speed)
    {
        speedItem = speed;
    }
}
