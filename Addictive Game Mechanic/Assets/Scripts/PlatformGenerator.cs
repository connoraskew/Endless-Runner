using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    //public GameObject thePlatform; // current platform
    public Transform GenerationPoint; // place to instantiate it
    private float platformWidth; // tracks the width of the platform

    private float distanceBetween; // private float used to get a random number between the two floats below it
    public float distanceBetweenMin;
    public float distanceBetweenMax;

    // getting all the widths of the platforms we are gonna use
    public float[] platformWidths;

    // used to get a random platform
    private int platformSelector;

    // array of all the platform widths we are using, 2X,4X,6X,8X,10X
    public ObjectPooler[] theObjectPool;

    // minimum point to spawn a platform
    private float heightMin;
    // max point to spawn a platform
    public Transform heightMaxPoint;
    // assigned from the transform above this ^^
    private float heightMax;
    // how far above or below we want to spawn a platform, used in random range
    public float heightMaxChange;
    // tracking how far above or below the next platform is gonna be from the previous one
    private float heightChange;

    private CoinGenerator theCoinGenerator; // the coin generator
    public float randomCoinThreshold; // used to calculate the chance of spawning coins on a platform

    public ObjectPooler spikePool; // the spikes controller
    public float RandomSpikethreshold; // used to calculate if the game should spawn a spike on a platform

    public ObjectPooler powerUpPool; // the powerup controller
    public float powerUpHeight; // distance in height to spawn the powerup
    public float PowerUpThreshold; // chance of spawning a powerup


    // Use this for initialization
    void Start()
    {
        // assigning the length based on how many objects in the pools
        platformWidths = new float[theObjectPool.Length];
        // for every object in the object pool
        for (int i = 0; i < theObjectPool.Length; i++)
        {
            // assigning the widths
            platformWidths[i] = theObjectPool[i].pooledObject.transform.localScale.x;
        }
        // assigning more stuff...
        heightMin = transform.position.y;
        heightMax = heightMaxPoint.position.y;

        theCoinGenerator = FindObjectOfType<CoinGenerator>(); // finding the coin generator
    }

    // Update is called once per frame
    void Update()
    {
        // if we have moved far enough to spawn a new platform....
        if (transform.position.x < GenerationPoint.position.x)
        {
            // assign distance between current platform and the next
            distanceBetween = Random.Range(distanceBetweenMin, distanceBetweenMax);
            // pick a platform to create
            platformSelector = Random.Range(0, theObjectPool.Length);
            // pick a height change
            heightChange = transform.position.y + Random.Range(heightMaxChange, -heightMaxChange);

            // check if the height change isnt out of bounds
            if (heightChange > heightMax)
            {
                heightChange = heightMax; // if it is then we set it to be at the bounds
            }
            else if (heightChange < heightMin) // if it is below 
            {
                heightChange = heightMin; // assign it to be at the bounds
            }

            // if a random number between 0 and 100 is less than the powerupthreshold
            if (Random.Range(0f, 100f) < PowerUpThreshold)
            {
                GameObject newPowerUp = powerUpPool.GetPooledObject(); // create a new powerup
                newPowerUp.GetComponent<powerUps>().AssigningStuff();
                newPowerUp.transform.position = transform.position + new Vector3(distanceBetween / 2, Random.Range(-powerUpHeight / 2, powerUpHeight / 2), 0); // assign its position
                newPowerUp.SetActive(true); // turn it on so the player can interact with it
            }

            // move this object to the place we want to spawn the platform
            transform.position = new Vector3(transform.position.x + (platformWidths[platformSelector] * 0.5f) + distanceBetween,
                                             heightChange,
                                             transform.position.z);

            // make a local instance and assign it to be one of the inactive pooled objects
            GameObject newPlatform = theObjectPool[platformSelector].GetPooledObject();
            // assign the local gameobjects transform
            newPlatform.transform.position = transform.position;
            // assign the local gameobjects rotation
            newPlatform.transform.rotation = transform.rotation;
            // set it to be active
            newPlatform.SetActive(true);

            // if the platforms width is smaller than 6, making is more of a challenge to get the coins
            if (platformWidths[platformSelector] <= 6)
            {
                // get a random number between 0 and 100, if it is les than the randomCoinThreshold
                if (Random.Range(0, 100) < randomCoinThreshold)
                {
                    theCoinGenerator.SpawnCoins(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), newPlatform.transform.localScale.x); // spawn the coins on the platform
                }
            }

            // if the platforms width is mroe than 4, giving the player more space to jump on the platform and avoid the spikes
            if (platformWidths[platformSelector] >= 4)
            {
                // get a random number between 0 and 100, if it is less than the RandomSpikethreshold
                if (Random.Range(0, 100) < RandomSpikethreshold)
                {
                    GameObject newSpike = spikePool.GetPooledObject(); // create a variable and find a disabled Spike

                    float spikeXPos = Random.Range(-platformWidths[platformSelector] / 2 + 1, platformWidths[platformSelector] / 2 - 1); // half the platform to the left, half the platform to the right
                    Vector3 spikePos = new Vector3(spikeXPos, 0.5f, 0f); // 0.5f = half the width of our platforms + half the spikes width
                    newSpike.transform.position = transform.position + spikePos; // assign the position
                    newSpike.transform.rotation = transform.rotation; // assigning the rotation
                    newSpike.SetActive(true); // make the spike active
                }
            }
            // move this spawner to the end of the platform preparing it for the next time we want to make a platform
            transform.position = new Vector3(transform.position.x + (platformWidths[platformSelector] * 0.5f),
                             transform.position.y,
                             transform.position.z);
        }
    }
}
