using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

    // the player
    public Player player;

    // the offset distance between the player and camera
    private Vector3 offset;

    // Use this for initialization
    void Start ()
    {
        // reference the player
        player = (Player)FindObjectOfType(typeof(Player));

        // calculate and store the offset value by getting the distance between the player's position and camera's position
        offset = transform.position - player.transform.position;
    }

    // LateUpdate is called after Update each frame
    void LateUpdate ()
    {
        // set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance
        transform.position = player.transform.position + offset;
    }
}
