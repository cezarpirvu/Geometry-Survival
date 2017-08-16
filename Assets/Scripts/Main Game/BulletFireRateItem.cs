using UnityEngine;
using System.Collections;

public class BulletFireRateItem : MonoBehaviour
{

    // set the value of the "Bullet Fire Rate" item
    private float bulletFireRateItem = 1;

    // Use this for initialization
    void Start()
    {
        // get the rigidbody
        Rigidbody rb = GetComponent<Rigidbody>();
        // set constraints on the rigidbody
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;

        // the start and the end position of the "Bullet Fire Rate" item movement
        Vector3 start = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Vector3 end = new Vector3(transform.position.x + 0.2f, transform.position.y, transform.position.z);

        // move the item forward and backward
        StartCoroutine(startBulletFireRateItem(transform, start, end, 4.0f));
    }

    // start the items movement
    IEnumerator startBulletFireRateItem(Transform itemTransform, Vector3 start, Vector3 end, float time)
    {
        while (true)
        {
            yield return moveBulletFireRateItem(itemTransform, start, end, time);
            yield return moveBulletFireRateItem(itemTransform, end, start, time);
        }
    }

    // create the routine that is going to be used to generate the "Bullet Fire Rate" item's movement
    IEnumerator moveBulletFireRateItem(Transform itemTransform, Vector3 start, Vector3 end, float time)
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

    // get the value of the "Bullet Fire Rate" item
    public float getBulletFireRateItem()
    {
        return bulletFireRateItem;
    }

    // set the value of the "Bullet Fire Rate" item
    public void setBulletFireRateItem(float fireRate)
    {
        bulletFireRateItem = fireRate;
    }
}
