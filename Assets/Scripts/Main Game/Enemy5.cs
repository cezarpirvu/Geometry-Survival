﻿using UnityEngine;
using System.Collections;

public class Enemy5 : MonoBehaviour
{

    // the initial life of the "Enemy5"
    private float enemy5Life = 5;
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
    private bool canFire = false;
    private bool canFire1 = false;

    // the destination margins for the enemy's projectile
    private float borderX;
    private float borderZ;

    // the boundaries of the plane
    private float boundsX;
    private float boundsZ;

    // the score that is generated when this enemy is destroyed
    private int score = 5;

    private float nextFire = 0.0f;

    // Use this for initialization
    void Start()
    {
        // the plane of the map
        MapSettings plane = (MapSettings)FindObjectOfType(typeof(MapSettings));

        // get the size of the plane
        boundsX = plane.getBoundsX();
        boundsZ = plane.getBoundsZ();

        // generate random destination for the enemy projectile
        borderX = Random.Range(-boundsX, boundsX);
        borderZ = Random.Range(-boundsZ, boundsZ);

        // set the fire rate of the enemy
        setFireRate(projectileFireRate);

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
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);

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
                Vector3 position = transform.position + transform.forward;
                borderX = Random.Range(-boundsX, boundsX);
                borderZ = Random.Range(-boundsZ, boundsZ);
                transform.LookAt(new Vector3(borderX, transform.position.y, borderZ));
                GameObject projectile = Instantiate(projectile1Prefab, position, Quaternion.identity) as GameObject;
                projectile.transform.LookAt(new Vector3(borderX, transform.position.y, borderZ));
                BulletCollision bullet = (BulletCollision)FindObjectOfType(typeof(BulletCollision));
                bullet.setBulletTag("Enemy5");

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
            enemy5Life = enemy5Life - player.playerDamage;

            if (enemy5Life <= 0)
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