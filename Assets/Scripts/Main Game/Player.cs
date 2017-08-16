using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    // set the speed of the player's movement
    public float playerSpeed = 5f;
    // the number of lives the player has initially
    public int playerLife = 5;
    // the damage that the player inflicts
    public float playerDamage = 5;

    // fire rate of the projectile
    public float projectileSpeed = 3;
    // size of the projectile
    public float projectileSize = 5;
    // the fire rate of the player
    public float playerFireRate = 5;
    // the range of the projectile
    public float projectileRange = 5;

    // check if the player is invulnerable to damage or not
    public bool invulnerable = false;

    // check if a collision with the player happened
    public bool hasColided = false;

    // the player
    private CharacterController player;

    // determine if the player can move or not
    public bool canMove = false;
    // check if the player has speed
    private bool noSpeed = false;

    private string playerDirection = "";
    public float moveY;
    // the position of the player on the Y axis
    private float playerPositionY = 0.5f;
    public Vector3 movement;

    // the mass of the player gameobject
    private float mass = 3f;

    // the momentum of the player
    private Vector3 impact;

    // Use this for initialization
    void Start()
    {
        impact = Vector3.zero;

        // initialize the controller component
        player = GetComponent<CharacterController>();

        // convert the player speed so that it can be scaled from 1 to 10 (min, max)
        if (playerSpeed != 0)
        {
            playerSpeed = playerSpeed / 20;
            noSpeed = false;
        }
        else
        {
            noSpeed = true;
        }

        // get the initial Y axis value for the player
        moveY = transform.position.y;

        // initially the player cannot move for 3 seconds
        StartCoroutine(delay(3));
    }

    // delay the movement of the player
    IEnumerator delay(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        // the player can move after a number of seconds
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (getPlayerLife() > 0)
        {

            // get the input from the X and Z axes
            float inputX = Input.GetAxis("Horizontal");
            float inputZ = Input.GetAxis("Vertical");

            float moveX = inputX * playerSpeed;
            float moveZ = inputZ * playerSpeed;

            Vector3 direction;

            // if the impact force has not been consumed
            if (impact != Vector3.zero)
            {
                movement = new Vector3(impact.x, 0f, impact.z);

                // the momentum of the impact
                if (impact.magnitude > 0.2)
                {
                    // move the player
                    player.Move(new Vector3(impact.x, 0f, impact.z) * Time.deltaTime);
                }

                // the impact force decreases over time
                impact = Vector3.Lerp(impact, Vector3.zero, 20 * Time.deltaTime);
            }
            else
            {
                // set the movement vector and direction vector
                movement = new Vector3(moveX, 0f, moveZ);
            }

            // move the player on the map
            if (canMove)
            {
                // move the player only if it has speed set to it
                if (!noSpeed)
                {
                    // move the player
                    player.Move(movement * Time.timeScale);

                    // shoot up
                    if (Input.GetKeyDown("w"))
                    {
                        playerDirection = "North";
                        if (Input.GetAxisRaw("Vertical1") > 0)
                        {
                            direction = new Vector3(0, 0, Input.GetAxisRaw("Vertical1"));
                            if (direction != Vector3.zero)
                                transform.rotation = Quaternion.LookRotation(direction);
                        }
                    }

                    // shoot left
                    if (Input.GetKeyDown("a"))
                    {
                        playerDirection = "West";
                        if (Input.GetAxisRaw("Horizontal1") < 0)
                        {
                            direction = new Vector3(Input.GetAxisRaw("Horizontal1"), 0, 0);
                            if (direction != Vector3.zero)
                                transform.rotation = Quaternion.LookRotation(direction);
                        }
                    }

                    // shoot down
                    if (Input.GetKeyDown("s"))
                    {
                        playerDirection = "South";
                        if (Input.GetAxisRaw("Vertical1") < 0)
                        {
                            direction = new Vector3(0, 0, Input.GetAxisRaw("Vertical1"));
                            if (direction != Vector3.zero)
                                transform.rotation = Quaternion.LookRotation(direction);
                        }
                    }

                    // shoot right
                    if (Input.GetKeyDown("d"))
                    {
                        playerDirection = "East";
                        if (Input.GetAxisRaw("Horizontal1") > 0)
                        {
                            direction = new Vector3(Input.GetAxisRaw("Horizontal1"), 0, 0);
                            if (direction != Vector3.zero)
                                transform.rotation = Quaternion.LookRotation(direction);
                        }
                    }
                }
            }

            // if the player has somehow escaped the map, it's position will be reset to the center and decrease the life
            MapSettings plane = MapSettings.instance;
            if (player.transform.localPosition.x >= plane.getBoundsX() || player.transform.localPosition.z >= plane.getBoundsZ())
            {
                // reset position of the player to the center
                transform.position = new Vector3(0, 0.5f, 0);
                playerLife--;
            }
        }
    }

    // the blink effect after the player has lost a life
    IEnumerator DoBlinks(int numBlinks, float seconds)
    {
        for (int i = 0; i < numBlinks * 2; i++)
        {
            // toggle between enablind and disabling the player
            GetComponent<MeshRenderer>().enabled = !GetComponent<MeshRenderer>().enabled;

            hasColided = true;
            // make sure the player does not move on the Y axis
            transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
            yield return new WaitForSeconds(seconds);
            transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
            hasColided = false;
        }

        // make sure the player does not move on the Y axis
        transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);

        GetComponent<MeshRenderer>().enabled = true;
    }

    // the invulnerability period after a player has lost a life
    IEnumerator invulnerableTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        invulnerable = false;
    }

    // detect the collision with the enemies and items
    void OnCollisionEnter(Collision collision)
    {
        // detect other collisions only if the player does not blink meaning that he is no longer invulnerable
        if (!hasColided)
        {
            // for the item description
            HUDBackground hud = HUDBackground.instance;

            // the collision with the "Enemy1" object
            Enemy1 enemy1 = (Enemy1)FindObjectOfType(typeof(Enemy1));

            if (enemy1 != null)
            {
                if (collision.gameObject.name == enemy1.gameObject.name)
                {
                    if (!invulnerable)
                    {
                        if (playerLife > 0)
                        {
                            // reduce player's life
                            playerLife = playerLife - enemy1.damage;

                            if (playerLife <= 0)
                            {
                                GameObject itemDescription = GameObject.FindGameObjectWithTag("Item Description");
                                itemDescription.GetComponent<Text>().enabled = true;
                                itemDescription.GetComponent<Text>().text = "You have died!";

                                // freeze the game
                                Time.timeScale = 0;
                            }
                        }
                            
                        // the player has been hit and becomes invulnerable for a couple of seconds
                        invulnerable = true;

                        // the period of invulnerability
                        StartCoroutine(invulnerableTime(2f));
                    }

                    // blink effect
                    StartCoroutine(DoBlinks(3, 0.3f));
                }
            }

            // the collision with the "Enemy2" object
            Enemy2 enemy2 = (Enemy2)FindObjectOfType(typeof(Enemy2));

            if (enemy2 != null)
            {
                if (collision.gameObject.name == enemy2.gameObject.name)
                {
                    if (!invulnerable)
                    {
                        if (playerLife > 0)
                        {
                            // reduce player's life
                            playerLife = playerLife - enemy2.getDamage();

                            if (playerLife <= 0)
                            {
                                GameObject itemDescription = GameObject.FindGameObjectWithTag("Item Description");
                                itemDescription.GetComponent<Text>().enabled = true;
                                itemDescription.GetComponent<Text>().text = "You have died!";

                                // freeze the game
                                Time.timeScale = 0;
                            }
                        }

                        // the player has been hit and becomes invulnerable for a couple of seconds
                        invulnerable = true;

                        // the period of invulnerability
                        StartCoroutine(invulnerableTime(2f));
                    }

                    // blink effect
                    StartCoroutine(DoBlinks(3, 0.3f));

                    // make sure the player does not move on the Y axis
                    transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
                }
            }

            // the collision with the "Enemy4" object
            Enemy4 enemy4 = (Enemy4)FindObjectOfType(typeof(Enemy4));

            if (enemy4 != null)
            {
                if (collision.gameObject.name == enemy4.gameObject.name)
                {
                    if (!invulnerable)
                    {
                        if (playerLife > 0)
                        {
                            // reduce player's life
                            playerLife = playerLife - enemy4.getDamage();

                            if (playerLife <= 0)
                            {
                                GameObject itemDescription = GameObject.FindGameObjectWithTag("Item Description");
                                itemDescription.GetComponent<Text>().enabled = true;
                                itemDescription.GetComponent<Text>().text = "You have died!";

                                // freeze the game
                                Time.timeScale = 0;
                            }
                        }

                        // the player has been hit and becomes invulnerable for a couple of seconds
                        invulnerable = true;

                        // the period of invulnerability
                        StartCoroutine(invulnerableTime(2f));
                    }

                    // blink effect
                    StartCoroutine(DoBlinks(3, 0.3f));
                }
            }

            // the collision with the "Enemy5" object
            Enemy5 enemy5 = (Enemy5)FindObjectOfType(typeof(Enemy5));

            // if the player was hit by the enemies woth the projectiles
            if (collision.gameObject.CompareTag("EnemyProjectile1"))
            {
                // get the enemy identifier that hit the player
                BulletCollision bullet = (BulletCollision)FindObjectOfType(typeof(BulletCollision));

                // if the enemy was hit by "Enemy3"
                if (bullet.getBulletTag().Equals("Enemy3"))
                {
                    if (!invulnerable)
                    {
                        Enemy3 enemy3 = (Enemy3)FindObjectOfType(typeof(Enemy3));

                        if (playerLife > 0 && enemy3 != null)
                        {
                            // reduce player's life
                            playerLife = playerLife - enemy3.getDamage();

                            if (playerLife <= 0)
                            {
                                GameObject itemDescription = GameObject.FindGameObjectWithTag("Item Description");
                                itemDescription.GetComponent<Text>().enabled = true;
                                itemDescription.GetComponent<Text>().text = "You have died!";

                                // freeze the game
                                Time.timeScale = 0;
                            }
                        }

                        // the player has been hit and becomes invulnerable for a couple of seconds
                        invulnerable = true;

                        // the period of invulnerability
                        StartCoroutine(invulnerableTime(2f));
                    }

                    // blink effect
                    StartCoroutine(DoBlinks(3, 0.3f));
                }
                else
                {
                    if (bullet.getBulletTag().Equals("Enemy4"))
                    {
                        if (!invulnerable)
                        {
                            if (playerLife > 0 && enemy4 != null)
                            {
                                // reduce player's life
                                playerLife = playerLife - enemy4.getDamage();

                                if (playerLife <= 0)
                                {
                                    GameObject itemDescription = GameObject.FindGameObjectWithTag("Item Description");
                                    itemDescription.GetComponent<Text>().enabled = true;
                                    itemDescription.GetComponent<Text>().text = "You have died!";

                                    // freeze the game
                                    Time.timeScale = 0;
                                }
                            }

                            // the player has been hit and becomes invulnerable for a couple of seconds
                            invulnerable = true;

                            // the period of invulnerability
                            StartCoroutine(invulnerableTime(2f));
                        }

                        // blink effect
                        StartCoroutine(DoBlinks(3, 0.3f));
                    }
                    else
                    {
                        if (bullet.getBulletTag().Equals("Enemy5"))
                        {
                            if (!invulnerable)
                            {
                                if (playerLife > 0 && enemy5 != null)
                                {
                                    // reduce player's life
                                    playerLife = playerLife - enemy5.getDamage();

                                    if (playerLife <= 0)
                                    {
                                        GameObject itemDescription = GameObject.FindGameObjectWithTag("Item Description");
                                        itemDescription.GetComponent<Text>().enabled = true;
                                        itemDescription.GetComponent<Text>().text = "You have died!";

                                        // freeze the game
                                        Time.timeScale = 0;
                                    }
                                }

                                // the player has been hit and becomes invulnerable for a couple of seconds
                                invulnerable = true;

                                // the period of invulnerability
                                StartCoroutine(invulnerableTime(2f));
                            }

                            // blink effect
                            StartCoroutine(DoBlinks(3, 0.3f));
                        }
                    }
                }
            }

            // the collision with the "Heart" item object
            Heart heartItem = (Heart)FindObjectOfType(typeof(Heart));

            if (heartItem != null)
            {
                if (collision.gameObject.name == heartItem.gameObject.transform.GetChild(0).name)
                {
                    // increase player's life
                    playerLife = playerLife + heartItem.getHeartItem();

                    // mark that the item has been retrieved
                    Items item = (Items)FindObjectOfType(typeof(Items));
                    item.setHeartCount(0);
                    // show the name of the item picked
                    GameObject itemDescription = GameObject.FindGameObjectWithTag("Item Description");
                    itemDescription.GetComponent<Text>().enabled = true;
                    itemDescription.GetComponent<Text>().text = "Life Up";
                    // mark that an item was picked
                    Levels items = (Levels)FindObjectOfType(typeof(Levels));
                    items.setItemPicked(true);

                    Destroy(heartItem.gameObject);
                    // make sure the position fo the player on the Y axis hasn't changed
                    transform.position = new Vector3(transform.position.x, playerPositionY, transform.position.z);
                }
            }

            // the collision with the "Speed" item object
            SpeedItem speedItem = (SpeedItem)FindObjectOfType(typeof(SpeedItem));

            if (speedItem != null)
            {
                if (collision.gameObject.name == speedItem.gameObject.name)
                {
                    // increase player's speed
                    playerSpeed = playerSpeed + speedItem.getSpeedItem() / 20;

                    // mark that the item has been retrieved
                    Items item = (Items)FindObjectOfType(typeof(Items));
                    item.setSpeedItemCount(0);
                    // show the name of the item picked
                    GameObject itemDescription = GameObject.FindGameObjectWithTag("Item Description");
                    itemDescription.GetComponent<Text>().enabled = true;
                    itemDescription.GetComponent<Text>().text = "Speed Up";
                    // mark that an item was picked
                    Levels items = (Levels)FindObjectOfType(typeof(Levels));
                    items.setItemPicked(true);

                    Destroy(speedItem.gameObject);
                    // make sure the position fo the player on the Y axis hasn't changed
                    transform.position = new Vector3(transform.position.x, playerPositionY, transform.position.z);
                }
            }

            // the collision with the "Damage" item object
            DamageItem damageItem = (DamageItem)FindObjectOfType(typeof(DamageItem));

            if (damageItem != null)
            {
                if (collision.gameObject.name == damageItem.gameObject.name)
                {
                    // increase player's damage
                    playerDamage = playerDamage + damageItem.getDamageItem();

                    // mark that the item has been retrieved
                    Items item = (Items)FindObjectOfType(typeof(Items));
                    item.setDamageItemCount(0);
                    // show the name of the item picked
                    GameObject itemDescription = GameObject.FindGameObjectWithTag("Item Description");
                    itemDescription.GetComponent<Text>().enabled = true;
                    itemDescription.GetComponent<Text>().text = "Damage Up";
                    // mark that an item was picked
                    Levels items = (Levels)FindObjectOfType(typeof(Levels));
                    items.setItemPicked(true);

                    Destroy(damageItem.gameObject);
                    // make sure the position fo the player on the Y axis hasn't changed
                    transform.position = new Vector3(transform.position.x, playerPositionY, transform.position.z);
                }
            }

            // the collision with the "Bullet Speed" item object
            BulletSpeedItem bulletSpeedItem = (BulletSpeedItem)FindObjectOfType(typeof(BulletSpeedItem));

            if (bulletSpeedItem != null)
            {
                if (collision.gameObject.name == bulletSpeedItem.gameObject.name)
                {
                    // increase player's projectile speed
                    projectileSpeed = projectileSpeed + bulletSpeedItem.getBulletSpeedItem();

                    // mark that the item has been retrieved
                    Items item = (Items)FindObjectOfType(typeof(Items));
                    item.setBulletSpeedItemCount(0);
                    // show the name of the item picked
                    GameObject itemDescription = GameObject.FindGameObjectWithTag("Item Description");
                    itemDescription.GetComponent<Text>().enabled = true;
                    itemDescription.GetComponent<Text>().text = "Bullet Speed Up";
                    // mark that an item was picked
                    Levels items = (Levels)FindObjectOfType(typeof(Levels));
                    items.setItemPicked(true);

                    Destroy(bulletSpeedItem.gameObject);
                    // make sure the position fo the player on the Y axis hasn't changed
                    transform.position = new Vector3(transform.position.x, playerPositionY, transform.position.z);
                }
            }

            // the collision with the "Bullet Range" item object
            BulletRangeItem bulletRangeItem = (BulletRangeItem)FindObjectOfType(typeof(BulletRangeItem));

            if (bulletRangeItem != null)
            {
                if (collision.gameObject.name == bulletRangeItem.gameObject.name)
                {
                    // increase player's projectile range
                    projectileRange = projectileRange + bulletRangeItem.getBulletRangeItem();

                    // mark that the item has been retrieved
                    Items item = (Items)FindObjectOfType(typeof(Items));
                    item.setBulletRangeItemCount(0);
                    // show the name of the item picked
                    GameObject itemDescription = GameObject.FindGameObjectWithTag("Item Description");
                    itemDescription.GetComponent<Text>().enabled = true;
                    itemDescription.GetComponent<Text>().text = "Bullet Range Up";
                    // mark that an item was picked
                    Levels items = (Levels)FindObjectOfType(typeof(Levels));
                    items.setItemPicked(true);

                    Destroy(bulletRangeItem.gameObject);
                    // make sure the position fo the player on the Y axis hasn't changed
                    transform.position = new Vector3(transform.position.x, playerPositionY, transform.position.z);
                }
            }

            // the collision with the "Bullet Fire Rate" item object
            BulletFireRateItem bulletFireRateItem = (BulletFireRateItem)FindObjectOfType(typeof(BulletFireRateItem));

            if (bulletFireRateItem != null)
            {
                if (collision.gameObject.name == bulletFireRateItem.gameObject.name)
                {
                    // increase player's fire rate
                    playerFireRate = playerFireRate + bulletFireRateItem.getBulletFireRateItem();

                    // mark that the item has been retrieved
                    Items item = (Items)FindObjectOfType(typeof(Items));
                    item.setBulletFireRateItemCount(0);
                    // show the name of the item picked
                    GameObject itemDescription = GameObject.FindGameObjectWithTag("Item Description");
                    itemDescription.GetComponent<Text>().enabled = true;
                    itemDescription.GetComponent<Text>().text = "Bullet Fire Rate Up";
                    // mark that an item was picked
                    Levels items = (Levels)FindObjectOfType(typeof(Levels));
                    items.setItemPicked(true);

                    Destroy(bulletFireRateItem.gameObject);
                    // make sure the position fo the player on the Y axis hasn't changed
                    transform.position = new Vector3(transform.position.x, playerPositionY, transform.position.z);
                }
            }

            // the collision with the "Bullet Size" item object
            BulletSizeItem bulletSizeItem = (BulletSizeItem)FindObjectOfType(typeof(BulletSizeItem));

            if (bulletSizeItem != null)
            {
                if (collision.gameObject.name == bulletSizeItem.gameObject.name)
                {
                    // increase player's projectile size
                    projectileSize = projectileSize + bulletSizeItem.getBulletSizeItem();

                    // mark that the item has been retrieved
                    Items item = (Items)FindObjectOfType(typeof(Items));
                    item.setBulletSizeItemCount(0);
                    // show the name of the item picked
                    GameObject itemDescription = GameObject.FindGameObjectWithTag("Item Description");
                    itemDescription.GetComponent<Text>().enabled = true;
                    itemDescription.GetComponent<Text>().text = "Bullet Size Up";
                    // mark that an item was picked
                    Levels items = (Levels)FindObjectOfType(typeof(Levels));
                    items.setItemPicked(true);

                    Destroy(bulletSizeItem.gameObject);
                    // make sure the position fo the player on the Y axis hasn't changed
                    transform.position = new Vector3(transform.position.x, playerPositionY, transform.position.z);
                }
            }

            // the collision with the "Bonus" item object
            BonusItem bonusItem = (BonusItem)FindObjectOfType(typeof(BonusItem));

            if (bonusItem != null)
            {
                if (collision.gameObject.name == bonusItem.gameObject.name)
                {
                    // increase player's fire rate
                    playerFireRate = playerFireRate + 1;
                    // increase player's projectile range
                    projectileRange = projectileRange + 1;
                    // increase player's projectile speed
                    projectileSpeed = projectileSpeed + 1;
                    // increase player's damage
                    playerDamage = playerDamage + 1;
                    // increase player's life
                    playerLife = playerLife + 1;

                    // mark that the item has been retrieved
                    Items item = (Items)FindObjectOfType(typeof(Items));
                    item.setBonusItemCount(0);
                    // show the name of the item picked
                    GameObject itemDescription = GameObject.FindGameObjectWithTag("Item Description");
                    itemDescription.GetComponent<Text>().enabled = true;
                    itemDescription.GetComponent<Text>().text = "Power Overwhelming";
                    // mark that an item was picked
                    Levels items = (Levels)FindObjectOfType(typeof(Levels));
                    items.setItemPicked(true);

                    Destroy(bonusItem.gameObject);
                    // make sure the position fo the player on the Y axis hasn't changed
                    transform.position = new Vector3(transform.position.x, playerPositionY, transform.position.z);
                }
            }
        }
    }

    // detect the collisions of the player with the "Static and Dynamic Spikes" and "Enemy3"
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // detect other collisions only if the player does not blink meaning that he is no longer invulnerable
        if (!hasColided)
        {
            // the collisions with the spyke and dynamic spikes objects
            Obstacles spike = (Obstacles)FindObjectOfType(typeof(Obstacles));

            if (spike != null)
            {
                if (hit.gameObject.CompareTag("Spike"))
                {
                    if (!invulnerable)
                    {
                        if (playerLife > 0)
                        {
                            // reduce player's life
                            playerLife = playerLife - spike.spikeDamage;

                            if (playerLife <= 0)
                            {
                                GameObject itemDescription = GameObject.FindGameObjectWithTag("Item Description");
                                itemDescription.GetComponent<Text>().enabled = true;
                                itemDescription.GetComponent<Text>().text = "You have died!";

                                // freeze the game
                                Time.timeScale = 0;
                            }
                        }

                        // the player has been hit and becomes invulnerable for a couple of seconds
                        invulnerable = true;

                        // the period of invulnerability
                        StartCoroutine(invulnerableTime(2f));
                    }

                    // blink effect
                    StartCoroutine(DoBlinks(3, 0.3f));

                    // knockback effect
                    impact += hit.normal.normalized * hit.normal.magnitude / mass;
                }
            }

            // the collision with the "Enemy3" object
            Enemy3 enemy3 = (Enemy3)FindObjectOfType(typeof(Enemy3));

            if (enemy3 != null)
            {
                if (hit.gameObject.name == enemy3.gameObject.name)
                {
                    if (!invulnerable)
                    {
                        if (playerLife > 0)
                        {
                            // reduce player's life
                            playerLife = playerLife - enemy3.getDamage();

                            if (playerLife <= 0)
                            {
                                GameObject itemDescription = GameObject.FindGameObjectWithTag("Item Description");
                                itemDescription.GetComponent<Text>().enabled = true;
                                itemDescription.GetComponent<Text>().text = "You have died!";

                                // freeze the game
                                Time.timeScale = 0;
                            }
                        }

                        // the player has been hit and becomes invulnerable for a couple of seconds
                        invulnerable = true;

                        // the period of invulnerability
                        StartCoroutine(invulnerableTime(2f));
                    }

                    // blink effect
                    StartCoroutine(DoBlinks(3, 0.3f));

                    // knockback effect
                    impact += hit.normal.normalized * hit.normal.magnitude / mass;
                }
            }

            // the collision with the "Enemy5" object
            Enemy5 enemy5 = (Enemy5)FindObjectOfType(typeof(Enemy5));

            if (enemy5 != null)
            {
                if (hit.gameObject.name == enemy5.gameObject.name)
                {
                    if (!invulnerable)
                    {
                        if (playerLife > 0)
                        {
                            // reduce player's life
                            playerLife = playerLife - enemy5.getDamage();

                            if (playerLife <= 0)
                            {
                                GameObject itemDescription = GameObject.FindGameObjectWithTag("Item Description");
                                itemDescription.GetComponent<Text>().enabled = true;
                                itemDescription.GetComponent<Text>().text = "You have died!";

                                // freeze the game
                                Time.timeScale = 0;
                            }
                        }

                        // the player has been hit and becomes invulnerable for a couple of seconds
                        invulnerable = true;

                        // the period of invulnerability
                        StartCoroutine(invulnerableTime(2f));
                    }

                    // blink effect
                    StartCoroutine(DoBlinks(3, 0.3f));

                    // knockback effect
                    impact += hit.normal.normalized * hit.normal.magnitude / mass;
                }
            }
        }
    }

    // get the current number of lives a player has left
    public int getPlayerLife()
    {
        return playerLife;
    }

    // get the current speed of the player
    public float getPlayerSpeed()
    {
        return playerSpeed;
    }

    // get the direction in which the player is looking
    public string getPlayerDirection()
    {
        return playerDirection;
    }
}
