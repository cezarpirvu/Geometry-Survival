using UnityEngine;
using System.Collections;

public class Projectile1 : MonoBehaviour
{

    // the player
    private Player player;

    // the projectile properties
    private float projectileFireRate;
    private float projectileSize;
    private float projectileSpeed;
    private float projectileRange;

    private Vector3 direction;
    private float nextFire = 0.0f;

    // check if the projectile can fire
    private bool canFire;

    // Use this for initialization
    void Start()
    {
        // the player
        player = (Player)FindObjectOfType(typeof(Player));

        // retrieve the projectile properties from the player
        projectileFireRate = player.playerFireRate;
        projectileSize = player.projectileSize;
        projectileSpeed = player.projectileSpeed;
        projectileRange = player.projectileRange;

        // convert the fire rate so that it can be scaled from 1 to 10 (min, max)
        if (projectileFireRate != 0 && projectileSize != 0 && projectileRange != 0)
        {
            projectileFireRate = 1 / projectileFireRate;
            projectileSize = projectileSize / 10;
            projectileRange = projectileRange / 2.5f;
            canFire = true;
        }
        else
        {
            canFire = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // select the player
        Player player = (Player)FindObjectOfType(typeof(Player));

        // select the prefab for the projectile from the resources
        GameObject projectile1Prefab = Resources.Load("Projectile1") as GameObject;

        // get the current player position
        Vector3 playerPosition = player.GetComponent<CharacterController>().transform.position;

        // if the WASD keys are pressed, then fire
        if (Input.GetKeyDown("w") || Input.GetKeyDown("a") || Input.GetKeyDown("s") || Input.GetKeyDown("d"))
        {
            // if the projectile can fire
            if (canFire)
            {
                // the projectile is shoot based on a fire rate
                if (Time.time > nextFire + projectileFireRate)
                {
                    nextFire = Time.time + projectileFireRate;
                    // the position at which the projectile is created
                    Vector3 position = playerPosition + player.transform.forward;
                    GameObject projectile = Instantiate(projectile1Prefab, position, Quaternion.identity) as GameObject;

                    // shoot up
                    if (player.getPlayerDirection().Equals("North"))
                    {
                        if (Input.GetKeyDown("w"))
                        {
                            if (Input.GetAxisRaw("Vertical1") > 0)
                            {
                                direction = new Vector3(0, 0, Input.GetAxisRaw("Vertical1"));
                                if (direction != Vector3.zero)
                                    projectile.transform.rotation = Quaternion.LookRotation(direction);
                            }
                        }
                    }
                    
                    if (player.getPlayerDirection().Equals("West"))
                    {
                        // shoot left
                        if (Input.GetKeyDown("a"))
                        {
                            if (Input.GetAxisRaw("Horizontal1") < 0)
                            {
                                direction = new Vector3(Input.GetAxisRaw("Horizontal1"), 0, 0);
                                if (direction != Vector3.zero)
                                    projectile.transform.rotation = Quaternion.LookRotation(direction);
                            }
                        }
                    }
                    
                    if (player.getPlayerDirection().Equals("South"))
                    {
                        // shoot down
                        if (Input.GetKeyDown("s"))
                        {
                            if (Input.GetAxisRaw("Vertical1") < 0)
                            {
                                direction = new Vector3(0, 0, Input.GetAxisRaw("Vertical1"));
                                if (direction != Vector3.zero)
                                    projectile.transform.rotation = Quaternion.LookRotation(direction);
                            }
                        }
                    }
                    
                    if (player.getPlayerDirection().Equals("East"))
                    {
                        // shoot right
                        if (Input.GetKeyDown("d"))
                        {
                            if (Input.GetAxisRaw("Horizontal1") > 0)
                            {
                                direction = new Vector3(Input.GetAxisRaw("Horizontal1"), 0, 0);
                                if (direction != Vector3.zero)
                                    projectile.transform.rotation = Quaternion.LookRotation(direction);
                            }
                        }
                    }
                    
                    // add the instantiated projectile to a parent
                    GameObject playerProjectiles = GameObject.FindGameObjectWithTag("Player Projectiles");
                    projectile.transform.SetParent(playerProjectiles.transform);

                    // set the size of the projectile
                    projectile.transform.localScale = new Vector3(15 + (2 * projectileSize * 10), 15 + (projectileSize * 10), 15 + (projectileSize * 10)) * projectileSize;

                    Rigidbody rb = projectile.GetComponent<Rigidbody>();
                    rb.velocity = transform.forward * projectileSpeed;

                    // add constraint to the projectile such that the rotation is disabled
                    rb.constraints = RigidbodyConstraints.FreezeRotation;
                    // add constraint so that the movement on the Y axis is disabled
                    rb.constraints = RigidbodyConstraints.FreezePositionY;

                    // destroy the projectile after a specific time -> range of the projectile
                    Destroy(projectile, projectileRange);
                }
            }
        }
    }
}