using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpPoints : MonoBehaviour
{
    public int scoreToGive; // the amount we add to the score when the player picks up a coin

    private ScoreManager theScoreManager; // the score manager

    public Color lerpedColor = Color.yellow; // colour of the coin
    private Renderer myRenderer; // the renderer component

    // Use this for initialization
    void Start ()
    {
        theScoreManager = FindObjectOfType<ScoreManager>(); // finding the score manager
        myRenderer = gameObject.GetComponent<Renderer>(); // finding the renderer component
    }

    void Update ()
    {
        lerpedColor = Color.Lerp(Color.black, Color.yellow, Mathf.PingPong(Time.time, 1)); // assign lerpcolour to a colour that bounces between black and yellow
        myRenderer.material.color = lerpedColor; // assign the material colour to the bouncing colour calculated above
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player") // if a coin hits the player
        {
            theScoreManager.AddScore(scoreToGive); // increases the score
            gameObject.SetActive(false); // turn off the coin
        }
    }
}
