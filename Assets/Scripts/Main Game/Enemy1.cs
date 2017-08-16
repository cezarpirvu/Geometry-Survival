using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy1 : MonoBehaviour
{

    // speed for the enemy's movement
    public float speed = 2f;

    // the initial life of the "Enemy1"
    public float enemy1Life = 5;

    // the damage of the enemy
    public int damage = 1;

    // the destination margins for the player
    private float borderX;
    private float borderZ;

    // the boundaries of the plane
    private float boundsX;
    private float boundsZ;

    // check if the enemy can move
    private bool canMove = false;

    // the score that is generated when this enemy is destroyed
    public int score = 5;

    // Use this for initialization
    void Start()
    {
        // the plane of the map
        MapSettings plane = (MapSettings)FindObjectOfType(typeof(MapSettings));

        // restrict the "Enemy1" to move on the Y axis and the rotation
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;

        // get the size of the plane
        boundsX = plane.getBoundsX() - 2;
        boundsZ = plane.getBoundsZ() - 2;

        // generate random destination for the enemy
        // list of enemies
        Obstacles obstacles = (Obstacles)FindObjectOfType(typeof(Obstacles));
        List<Vector3> enemyList = new List<Vector3>(obstacles.getFreePositions());

        if (enemyList.Count != 0)
        {
            int length = enemyList.Count;
            int position = Random.Range(0, length);

            borderX = enemyList[position].x;
            borderZ = enemyList[position].z;
        }

        // initially the enemy cannot move for 3 seconds
        StartCoroutine(delay(3));

        // !!!!!!!!!!just for testing the highscores
        score = Random.Range(0, 10);
    }

    // delay the movement of the enemy
    IEnumerator delay(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        // the enemy can move after a number of seconds
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {

            // current position of the enemy
            Vector3 currentPosition = transform.position;

            // destination of the enemy
            Vector3 randomPosition = new Vector3(borderX, transform.position.y, borderZ);

            // the direction of the "Enemy1" object
            //transform.LookAt(randomPosition);

            if (currentPosition == randomPosition)
            {
                // regenerate the destination's coordinates
                // list of enemies
                Obstacles obstacles = (Obstacles)FindObjectOfType(typeof(Obstacles));
                List<Vector3> enemyList = new List<Vector3>(obstacles.getFreePositions());

                if (enemyList.Count != 0)
                {
                    int length = enemyList.Count;
                    int position = Random.Range(0, length);

                    borderX = enemyList[position].x;
                    borderZ = enemyList[position].z;
                }
                randomPosition = new Vector3(borderX, transform.position.y, borderZ);
            }

            // the plane of the map
            MapSettings plane = MapSettings.instance;

            // enemy must stay in the area
            if (currentPosition.x <= plane.getWest() & currentPosition.x >= plane.getEast() & currentPosition.z <= plane.getNorth() & currentPosition.z >= plane.getSouth())
            {
                // move the enemy from it's current position to the destination
                transform.position = Vector3.MoveTowards(currentPosition, randomPosition, Time.deltaTime * speed);
            }
            else
            {
                // list of enemies
                Obstacles obstacles = (Obstacles)FindObjectOfType(typeof(Obstacles));
                List<Vector3> enemyList = new List<Vector3>(obstacles.getFreePositions());

                if (enemyList.Count != 0)
                {
                    int length = enemyList.Count;
                    int position = Random.Range(0, length);

                    borderX = enemyList[position].x;
                    borderZ = enemyList[position].z;
                }

                randomPosition = new Vector3(borderX, transform.position.y, borderZ);

                // move towards the player
                transform.position = Vector3.MoveTowards(currentPosition, randomPosition, Time.deltaTime * speed);
            }

            // add rotation to the enemy
            Quaternion deltaRotation = Quaternion.Euler(transform.forward * Time.deltaTime * 10);

            // select the rigidbody of the enemy and apply rotation
            Rigidbody rb = GetComponent<Rigidbody>();
            //rb.MoveRotation(rb.rotation * deltaRotation);
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
        // if the enemy collides with an wall, obstacle or spike, then generate a new direction for it
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Spike") || collision.gameObject.CompareTag("Enemy1")
            || collision.gameObject.CompareTag("Enemy2") || collision.gameObject.CompareTag("Enemy3") || collision.gameObject.CompareTag("Enemy4") || collision.gameObject.CompareTag("Enemy5") ||
            collision.gameObject.CompareTag("Projectile1"))
        {
            // regenerate the destination's coordinates
            // list of enemies
            Obstacles obstacles = (Obstacles)FindObjectOfType(typeof(Obstacles));
            List<Vector3> enemyList = new List<Vector3>(obstacles.getFreePositions());

            if (enemyList.Count != 0)
            {
                int length = enemyList.Count;
                int position = Random.Range(0, length);

                borderX = enemyList[position].x;
                borderZ = enemyList[position].z;
            }

            // move towards the player
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(borderX, transform.position.y, borderZ), Time.deltaTime * speed);
        }

        // if the enemy is hit by a "Projectile1" then it is destroyed
        if (collision.gameObject.CompareTag("Projectile1"))
        {
            // the player
            Player player = (Player)FindObjectOfType(typeof(Player));

            // if hit by the "Projectile1" then lose HP
            enemy1Life = enemy1Life - player.playerDamage;

            if (enemy1Life <= 0)
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
}