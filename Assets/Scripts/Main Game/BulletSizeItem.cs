using UnityEngine;
using System.Collections;

public class BulletSizeItem : MonoBehaviour
{

    // set the value of the "Bullet Size" item
    private float bulletSizeItem = 1;

    // Use this for initialization
    void Start()
    {
        // get the rigidbody
        Rigidbody rb = GetComponent<Rigidbody>();

        // set constraints on the rigidbody
        rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;

        // the start and the end position of the "Bullet Size" item movement
        Vector3 start = transform.localScale;
        Vector3 end = new Vector3(0.15f, 0.15f, 0.15f);

        // maxe the item expand and collapse
        StartCoroutine(startBulletSizeItem(transform, start, end, 4.0f));
    }

    // start the items movement
    IEnumerator startBulletSizeItem(Transform itemTransform, Vector3 start, Vector3 end, float time)
    {
        while (true)
        {
            yield return moveBulletSizeItem(itemTransform, start, end, time);
            yield return moveBulletSizeItem(itemTransform, end, start, time);
        }
    }

    // create the routine that is going to be used to generate the "Bullet Size" item's expansion and collapsing
    IEnumerator moveBulletSizeItem(Transform itemTransform, Vector3 start, Vector3 end, float time)
    {
        float i = 0.0f;
        if (time != 0.0f)
        {
            while (i < 1.0f)
            {
                i += Time.deltaTime / time;
                itemTransform.localScale = Vector3.Lerp(start, end, i);
                yield return null;
            }
        }
    }

    // get the value of the "Bullet Size" item
    public float getBulletSizeItem()
    {
        return bulletSizeItem;
    }

    // set the value of the "Bullet Size" item
    public void setBulletSizeItem(float size)
    {
        bulletSizeItem = size;
    }
}
