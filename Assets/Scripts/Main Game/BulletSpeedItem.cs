using UnityEngine;
using System.Collections;

public class BulletSpeedItem : MonoBehaviour
{

    // set the value of the bullet speed item
    private float bulletSpeedItem = 1;

    // Use this for initialization
    void Start()
    {
        // get the rigidbody
        Rigidbody rb = GetComponent<Rigidbody>();

        // set constraints on the rigidbody
        rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        // the start and the end position of the bullet speed item movement
        Vector3 start = transform.rotation.eulerAngles;
        Vector3 end = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 360, transform.rotation.eulerAngles.z);

        // rotate the item along it's Y axis
        StartCoroutine(startBulletSpeedItem(transform, start, end, 4.0f));
    }

    // start the items movement
    IEnumerator startBulletSpeedItem(Transform itemTransform, Vector3 start, Vector3 end, float time)
    {
        while (true)
        {
            yield return moveBulletSpeedItem(itemTransform, start, end, time);
        }
    }

    // create the routine that is going to be used to generate the bullet speed item's movement
    IEnumerator moveBulletSpeedItem(Transform itemTransform, Vector3 start, Vector3 end, float time)
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

    // get the value of the bullet speed item
    public float getBulletSpeedItem()
    {
        return bulletSpeedItem;
    }

    // set the value of the bullet speed item
    public void setBuletSpeedItem(float speed)
    {
        bulletSpeedItem = speed;
    }
}
