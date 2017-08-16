using UnityEngine;
using System.Collections;

public class DamageItem : MonoBehaviour
{

    // set the value of the damage item
    private float damageItem = 1;

    // Use this for initialization
    void Start()
    {
        // get the rigidbody
        Rigidbody rb = GetComponent<Rigidbody>();

        // set constraints on the rigidbody
        rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;

        // the start and the end position of the damage item movement
        Vector3 start = transform.rotation.eulerAngles;
        Vector3 end = new Vector3(transform.rotation.eulerAngles.x - 45, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        // rotate the item along it's X axis
        StartCoroutine(startDamageItem(transform, start, end, 4.0f));
    }

    // start the items movement
    IEnumerator startDamageItem(Transform itemTransform, Vector3 start, Vector3 end, float time)
    {
        while (true)
        {
            yield return moveDamageItem(itemTransform, start, end, time);
            yield return moveDamageItem(itemTransform, end, start, time);
        }
    }

    // create the routine that is going to be used to generate the speed item's movement
    IEnumerator moveDamageItem(Transform itemTransform, Vector3 start, Vector3 end, float time)
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

    // get the value of the damage item
    public float getDamageItem()
    {
        return damageItem;
    }

    // set the value of the damage item
    public void setDamageItem(float damage)
    {
        damageItem = damage;
    }
}
