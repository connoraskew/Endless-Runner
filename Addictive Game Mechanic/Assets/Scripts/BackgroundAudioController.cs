using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundAudioController : MonoBehaviour
{
    private float maxVolume; // max volume for the audio to increase to, fade in effect
    public float delay; // how long the fade it will be
    private float Actualdelay; // used to get the inverse of delay, if delay = 7, actual delay = 1/7

    // component added to this objects
    private AudioSource myAudioSource;

    // Use this for initialization
    void Start()
    {
        myAudioSource = GetComponent<AudioSource>(); // get the audio source component
        maxVolume = myAudioSource.volume; // get the max volume at the start 
        myAudioSource.volume = 0; // reset the actual volume
        Actualdelay = 1 / delay; // calculate the actual delay
    }

    // Update is called once per frame
    void Update()
    {
        if (myAudioSource.volume < maxVolume) // if the current volume is less than the max we want it to be
        {
            myAudioSource.volume += (Actualdelay * maxVolume * Time.deltaTime); // increase the current volume over time
        }
    }
}
