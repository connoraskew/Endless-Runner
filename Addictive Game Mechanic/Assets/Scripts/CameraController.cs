using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PlayerController thePlayer; // the player we want to follow

    private Vector3 lastPlayerPosition; // variable assigned to the players transform
    private float distanceToMove; // how far to move each frame

    void Start ()
    {
        thePlayer = FindObjectOfType<PlayerController>(); // finding the player
        lastPlayerPosition = thePlayer.transform.position; // assigning the players position
    }

    void Update ()
    {
        distanceToMove = thePlayer.transform.position.x - lastPlayerPosition.x; // calculating how far to move
        transform.position = new Vector3(transform.position.x + distanceToMove, transform.position.y, transform.position.z); // moving the camera
        lastPlayerPosition = thePlayer.transform.position; // assigning the new position to move to
    }
}
