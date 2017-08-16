using UnityEngine;
using System.Collections;

public class BulletRangeItem : MonoBehaviour
{

    // set the value of the bullet range item
    private float bulletRangeItem = 1;

    // Use this for initialization
    void Start()
    {
        // get the rigidbody
        Rigidbody rb = GetComponent<Rigidbody>();

        // set constraints on the rigidbody
        rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;

        // the start and the end position of the bullet speed item movement
        Vector3 start = transform.rotation.eulerAngles;
        Vector3 end = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + 360);

        // rotate the item along it's Z axis
        StartCoroutine(startBulletRangeItem(transform, start, end, 4.0f));
    }

    // start the items movement
    IEnumerator startBulletRangeItem(Transform itemTransform, Vector3 start, Vector3 end, float time)
    {
        while (true)
        {
            yield return moveBulletRangeItem(itemTransform, start, end, time);
        }
    }

    // create the routine that is going to be used to generate the bullet range item's movement
    IEnumerator moveBulletRangeItem(Transform itemTransform, Vector3 start, Vector3 end, float time)
    {
        float i = 0.0f;
        if (time != 0.0f)
        {
            while (i < 1.0f)
            {
                i += Time.deltaTime / time;
                itemTransform.eulerAngles = Vector3.Lerp(start, end, i);
                yield return null;
            }
        }
    }

    // get the value of the bullet range item
    public float getBulletRangeItem()
    {
        return bulletRangeItem;
    }

    // set the value of the bullet range item
    public void setBuletRangeItem(float range)
    {
        bulletRangeItem = range;
    }
}
