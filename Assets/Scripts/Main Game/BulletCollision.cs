using UnityEngine;
using System.Collections;

public class BulletCollision : MonoBehaviour
{

    // the tag of the projectile indicating from where it was fired
    private string projectileTag = "";

    // check if the projectile has collided
    void OnCollisionEnter(Collision collision)
    {
        // destroy the projectile on collision
        Destroy(gameObject);
    }

    // set the tag of the projectile
    public void setBulletTag(string tag)
    {
        projectileTag = tag;
    }

    // get the tag of the projectile
    public string getBulletTag()
    {
        return projectileTag;
    }
}
