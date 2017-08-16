using UnityEngine;
using System.Collections;

public class Enemy2 : MonoBehaviour
{

    // speed for the enemy's movement
    public float enemy2Speed = 2f;
    // the initial life of the "Enemy1"
    public float enemy2Life = 5;
    // the damage of the enemy
    private int enemy2Damage = 1;

    // check if the enemy can move
    public bool canMove = false;

    // the score that is generated when this enemy is destroyed
    public int score = 5;

    private Player player;

    // Use this for initialization
    void Start()
    {
        // the player
        player = (Player)FindObjectOfType(typeof(Player));

        // restrict the "Enemy2" to move on the Y axis
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;

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
        // make sure the enemy does not change his movement on the Y axis
        transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);

        // current position of the enemy
        Vector3 currentPosition = transform.position;

        // the player
        player = (Player)FindObjectOfType(typeof(Player));
        // player position
        Vector3 playerPosition = player.transform.position;

        // the direction of the "Enemy2" object
        transform.LookAt(player.transform);

        // if the "Enemy2" can move
        if (canMove)
        {
            // if the distance between the enemy and the player is greater than
            if (Vector3.Distance(currentPosition, playerPosition) >= 1)
            {
                // move towards the player
                transform.position += transform.forward * enemy2Speed * Time.deltaTime;
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
            // if hit by the "Projectile1" then lose HP
            enemy2Life = enemy2Life - player.playerDamage;

            if (enemy2Life <= 0)
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

    // get the damage of the enemy
    public int getDamage()
    {
        return enemy2Damage;
    }

    // set the damage of the enemy
    public void setDamage(int damage)
    {
        enemy2Damage = damage;
    }
}
