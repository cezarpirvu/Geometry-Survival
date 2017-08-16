using UnityEngine;
using System.Collections;

public class Enemy4 : MonoBehaviour
{

    // the initial life of the "Enemy4"
    private float enemy4Life = 5;
    // speed for the enemy's movement
    private float enemy4Speed = 2f;
    // the damage of the enemy
    private int damage = 1;
    // fire rate of the projectile
    private float projectileSpeed = 5;
    // size of the projectile
    private float projectileSize = 5;
    // the fire rate of the enemy
    private float projectileFireRate = 5;
    // the range of the projectile
    private float projectileRange = 5;

    // check if the enemy can fire
    public bool canFire = false;
    public bool canFire1 = false;
    // check if the enemy can move
    public bool canMove = false;

    // the score that is generated when this enemy is destroyed
    public int score = 5;

    private float nextFire = 0.0f;

    private Player player;

    // Use this for initialization
    void Start()
    {
        // set the fire rate of the enemy
        setFireRate(projectileFireRate);

        // the player
        player = (Player)FindObjectOfType(typeof(Player));

        // restrict the "Enemy4" to move on the Y axis
        transform.position = new Vector3(transform.position.x, 1, transform.position.z);
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePositionY;
        //rb.constraints = RigidbodyConstraints.FreezeRotation;

        // initially the enemy cannot fire for 3 seconds
        StartCoroutine(delay(3));

        // convert the fire rate so that it can be scaled from 1 to 10 (min, max)
        if (projectileFireRate != 0 && projectileSize != 0 && projectileRange != 0)
        {
            projectileFireRate = (5.5f - 0.5f * projectileFireRate) / projectileFireRate;
            projectileSize = projectileSize / 10;
            projectileRange = projectileRange / 2.5f;
            canFire1 = true;
        }
        else
        {
            canFire = false;
        }

        // !!!!!!!!!!just for testing the highscores
        score = Random.Range(0, 10);
    }

    // delay the movement of the enemy
    IEnumerator delay(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        // the enemy can fires after a number of seconds
        canFire = true;
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        // make sure the enemy does not change his movement on the Y axis
        transform.position = new Vector3(transform.position.x, 1, transform.position.z);

        // current position of the enemy
        Vector3 currentPosition = transform.position;

        // the player
        player = (Player)FindObjectOfType(typeof(Player));
        // player position
        Vector3 playerPosition = player.transform.position;

        // the direction of the "Enemy4" object
        transform.LookAt(player.transform);

        // if the "Enemy2" can move
        if (canMove)
        {
            // if the distance between the enemy and the player is greater than
            if (Vector3.Distance(currentPosition, playerPosition) >= 1)
            {
                // move towards the player
                transform.position += transform.forward * enemy4Speed * Time.deltaTime;
            }
        }

        // select the prefab for the projectile from the resources
        GameObject projectile1Prefab = Resources.Load("EnemyProjectile1") as GameObject;

        // if the projectile can fire
        if (canFire && canFire1)
        {
            // the projectile is shoot based on a fire rate
            if (Time.time > nextFire + projectileFireRate)
            {
                nextFire = Time.time + projectileFireRate;
                // the position at which the projectile is created
                player = (Player)FindObjectOfType(typeof(Player));

                Vector3 position = transform.position + transform.forward;
                transform.LookAt(player.transform.position);
                GameObject projectile = Instantiate(projectile1Prefab, position, Quaternion.identity) as GameObject;
                projectile.transform.LookAt(player.transform.position);
                BulletCollision bullet = (BulletCollision)FindObjectOfType(typeof(BulletCollision));
                bullet.setBulletTag("Enemy4");

                // add the instantiated projectile to a parent
                GameObject enemyProjectiles = GameObject.FindGameObjectWithTag("Enemy Projectiles");
                projectile.transform.SetParent(enemyProjectiles.transform);

                // set the size of the projectile
                projectile.transform.localScale = new Vector3(15 + (2 * projectileSize * 10), 15 + (projectileSize * 10), 15 + (projectileSize * 10)) * projectileSize;

                Rigidbody rb = projectile.GetComponent<Rigidbody>();
                rb.velocity = transform.forward * projectileSpeed;

                // add constraint to the projectile such that the rotation and the movement on the Y axis are disabled
                rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;

                // destroy the projectile after a specific time -> range of the projectile
                Destroy(projectile, projectileRange);
            }
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Enemies enemy = (Enemies)FindObjectOfType(typeof(Enemies));
            enemy.enemyKilled();

            // destroy the enemy
            Destroy(gameObject);
        }
    }

    // check if the enemy has collided with the obstacles
    void OnCollisionEnter(Collision collision)
    {
        // if the enemy is hit by a "Projectile1" then it is destroyed
        if (collision.gameObject.CompareTag("Projectile1"))
        {
            // the player
            Player player = (Player)FindObjectOfType(typeof(Player));

            // if hit by the "Projectile1" then lose HP
            enemy4Life = enemy4Life - player.playerDamage;

            if (enemy4Life <= 0)
            {
                // update the score of the player when this enemy life reaches 0
                PlayerScore score = PlayerScore.instance;
                score.addScore(this.score);

                Enemies enemy = (Enemies)FindObjectOfType(typeof(Enemies));
                enemy.enemyKilled();

                // destroy the enemy
                Destroy(gameObject);
            }
        }
    }

    // set the enemy's fire rate
    public void setFireRate(float value)
    {
        projectileFireRate = value;
    }

    // retrieve the enemy's damage
    public int getDamage()
    {
        return damage;
    }
}
