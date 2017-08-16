using UnityEngine;

public class Heart : MonoBehaviour
{

    // set the value of the heart
    private int health = 1;

    // Use this for initialization
    void Start()
    {
        // get the child of the object
        Transform child = transform.GetChild(0);
        // get the rigidbody of the child
        Rigidbody rb = child.GetComponent<Rigidbody>();

        // set constraints on the rigidbody
        rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }
    // Update is called once per frame
    void Update()
    {
        // rotate the "Heart" object alongside it's Y axis
        transform.Rotate(0, 50 * Time.deltaTime, 0);
    }

    // detect collisions
    void OnCollisionEnter(Collision collision)
    {
        // if the gameobject and it's children collide with the player, then destroy it
        if (collision.gameObject.transform.GetChild(0).CompareTag("Player") || collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    // get the value of the heart
    public int getHeartItem()
    {
        return health;
    }

    // set the value of the heart
    public void setHeartItem(int health)
    {
        this.health = health;
    }
}
