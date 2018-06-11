using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public Transform platformGenerator; // the position of the platform generator
    private Vector3 platformStartPoint; // the position of the starting point

    public PlayerController thePlayer; // the player
    private Vector3 playerStartPoint; // the starting point of the player

    private PlatformDestroyer[] platformList; // list of platforms

    private ScoreManager theScoreManager; // the score manager
    private powerUpManager thePowerUpManager; // the power up manager

    public DeathMenu theDeathScreen; // the death screen

    public bool powerUpReset; // used to reset the powerup durations

    void Start()
    {
        // assigning the players/platform generators start position
        platformStartPoint = platformGenerator.position; 
        playerStartPoint = thePlayer.transform.position;

        // finding the score manager
        theScoreManager = FindObjectOfType<ScoreManager>();
        thePowerUpManager = FindObjectOfType<powerUpManager>();

		//GameAnalytics.Initialize ();
		//GameAnalytics.NewDesignEvent ("Game Loaded");
    }

    // used to show the player they have died
    public void RestartGame()
    {
        theScoreManager.ScoreIncreasing = false; // turn off scoring before the delay so they can see the score
        thePlayer.gameObject.SetActive(false); // turn the player off so the player + camera stops moving

        theDeathScreen.gameObject.SetActive(true); // turn on the death screen
        thePowerUpManager.dead = true; // stop the power up UI from changing
    }

    // used to reset variables and objects
    public void Reset()
    {
        theDeathScreen.gameObject.SetActive(false); // turn off the death screen
        thePlayer.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero; // resets the players velocity
        platformList = FindObjectsOfType<PlatformDestroyer>(); // find all of the platforms

        // for every platform
        for (int i = 0; i < platformList.Length; i++)
        {
            // turn them off
            platformList[i].gameObject.SetActive(false);
        }

        thePlayer.transform.position = playerStartPoint; // reset player pos
        platformGenerator.position = platformStartPoint; // reset platform pos
        thePlayer.gameObject.SetActive(true); // turn the player back on


        thePowerUpManager.dead = false; // let the UI change again

        thePowerUpManager.timeDoubleWasCalled = -100;
        thePowerUpManager.timeSafeModeWasCalled = -100;

        theScoreManager.scoreFloat = 0; // reset the score
        theScoreManager.ScoreIncreasing = true; // allow the score to start increasing again
        powerUpReset = true; // used to reset the powerup durations
        thePlayer.dead = false; // after resetting everything, turn the player back on
    }
}