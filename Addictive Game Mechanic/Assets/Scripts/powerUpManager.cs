using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class powerUpManager : MonoBehaviour
{
    public float ScoreMultiplyer;

    private bool doublePoints; // to see if we need to double the points
    private bool hasDoubled; // to see if we have already set stuff to double
    private Image doubleImage; // the image used for UI
    public GameObject doubleImageExtra; // the image used for UI, X2 Text
    public float timeDoubleWasCalled; // the time the player collected the the double powerup
    private float doubleDuration; // the duration of the buff, used to calculate the total time of the buff's duration, divided by that of its other part, doublecounter

    private bool safeMode; // if we want the spikes to go
    private bool hasMadeItSafe; // if we have already removed the spikes
    private Image spikeImage; // the image used for UI
    public GameObject spikeImageExtra; // the image used for UI, spikeless image
    public float timeSafeModeWasCalled; // the time the player collected the safe powerup
    private float safeDuration; // duration of the buff, used to calculate the total time of the buff's duration, divided by that of its other part, spikecounter

    // how long the buffs are active, decrease over time
    private float doublerCounter;
    private float spikeCounter;

    private PlatformDestroyer[] spikeList; // list of the spikes

    // other scripts with variables we want to change
    private ScoreManager myScoreManager;
    private PlatformGenerator myPlatformGenerator;
    private GameManager myGameManager;

    public GameObject brokenSpike; // broken spike pool

    // a base line for the original variables
    private float normalpointsPerSecond;
    private float spikeRate;

    public bool dead; // if the player is dead or not
    public bool canSpikebreak; // if the sound have been played or not

    // Use this for initialization
    void Start()
    {
        // finding stuff in the game
        myGameManager = FindObjectOfType<GameManager>();
        myScoreManager = FindObjectOfType<ScoreManager>();
        myPlatformGenerator = FindObjectOfType<PlatformGenerator>();

        // setting the normal values
        normalpointsPerSecond = myScoreManager.pointsPerSecond;
        spikeRate = myPlatformGenerator.RandomSpikethreshold;

        // getting and setting the power up images
        doubleImage = GameObject.FindGameObjectWithTag("doubleImage").GetComponent<Image>();
        doubleImage.fillAmount = 0.0f;
        spikeImage = GameObject.FindGameObjectWithTag("spikeImage").GetComponent<Image>();
        spikeImage.fillAmount = 0.0f;

        doubleImageExtra.SetActive(false); // turn off the extra detail image
        spikeImageExtra.SetActive(false); // turn off the extra detail image

        dead = false; // bool used to stop the power up images from moving once we are dead
        canSpikebreak = true;
    }

    // Update is called once per frame
    void Update()
    {
        // before we do anything, check if the game manager wants us to reset
        if (myGameManager.powerUpReset)
        {
            // set the counters to be 0
            doublerCounter = 0;
            spikeCounter = 0;
            myGameManager.powerUpReset = false;
        }
        else // if we dont need to reset, check if power ups are active
        {
            if (doublePoints) // if double points is active
            {
                if (!hasDoubled) // if we havent doubled the points yet
                {
                    myScoreManager.pointsPerSecond = normalpointsPerSecond * ScoreMultiplyer; // setting the new points per second
                    myScoreManager.shouldDouble = true; // seeting the bool, so if the player picks up a coin, it doubles the coin as well
                    hasDoubled = true; // sets the bool so we dont constantly come here
                }

                if (doublerCounter <= 0) // if the timer is set to 0
                {
                    myScoreManager.ResetPointsPerSecond(); // reset points per seconed back to normal
                    myScoreManager.shouldDouble = false; // set their bool back to normal
                    doublePoints = false; // set the bool here to normal
                    hasDoubled = false; // set the first instance checker to normal
                    doubleImageExtra.SetActive(false); // turn off the extra detail image
                }
                doublerCounter -= Time.deltaTime; // reduce the counter
            }

            if (safeMode) // if safe mode is active
            {
                if (!hasMadeItSafe) // if we havent removed the spikes yet
                {
                    myPlatformGenerator.RandomSpikethreshold = 0; // set the spike spawn rate to be 0
                    hasMadeItSafe = true; // set the first instance checker to be true
                }

                if (spikeCounter <= 0) // if the powerup is over
                {
                    myPlatformGenerator.RandomSpikethreshold = spikeRate; // reset the spike rate
                    safeMode = false; // set the biik here to normal
                    hasMadeItSafe = false; // set the instance checker back to normal
                    spikeImageExtra.SetActive(false); // turn off the extra detail image
                    canSpikebreak = true;
                }
                spikeCounter -= Time.deltaTime; // reduce the counter
            }
        }

        // if the player isnt dead, update the UI timers of the power ups
        if (!dead)
        {
            // 1 - the time after we collected the power up divided by the duration of the powerup
            spikeImage.fillAmount = 1 - ((Time.time - timeSafeModeWasCalled) / safeDuration);

            // 1 - the time after we collected the power up divided by the duration of the powerup 
            doubleImage.fillAmount = 1 - ((Time.time - timeDoubleWasCalled) / doubleDuration);
        }
    }

    // the function that is called once a doubler power up is collected
    public void ActivateDoubler(float time)
    {
        doublerCounter = time; // set the counter 
        doubleDuration = time; // set the overall duration   
        doublePoints = true; // set the bool to be true
        timeDoubleWasCalled = Time.time; // set the time that the powerup was collected
        doubleImageExtra.SetActive(true); // turn on the extra detail image
    }

    // the function that is called once a spikeless power up is collected
    public void Activatespikeless(float time)
    {
        spikeCounter = time; // set the counter
        safeDuration = time; // set the duration
        safeMode = true; // set the local bool 
        timeSafeModeWasCalled = Time.time; // set the time that the power up was collected
        spikeImageExtra.SetActive(true); // turn on the extra detail image

        spikeList = FindObjectsOfType<PlatformDestroyer>(); // find all of the objects with the destroyer script on it

        if (canSpikebreak)
        {
            // for every object it find
            for (int i = 0; i < spikeList.Length; i++)
            {
                // if the object has Spike in its name
                if (spikeList[i].gameObject.name.Contains("Spike(Clone)"))
                {
                    spikeList[i].gameObject.SetActive(false); // turn the spikes off
                    Instantiate(brokenSpike, spikeList[i].transform.position, Quaternion.identity);
                }
            }
            canSpikebreak = false;
        }
    }
}