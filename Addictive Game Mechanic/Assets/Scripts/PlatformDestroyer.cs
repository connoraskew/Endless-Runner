using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDestroyer : MonoBehaviour
{
    
    public GameObject PlatformDestructionPoint; // the object following behind the player, destroying this it comes into contact with

	void Start ()
    {
        PlatformDestructionPoint = GameObject.Find("PlatformDestructionPoint"); // finding it
	}	

	void Update ()
    {
        // if the platform this is attached to is behind the destruction point
		if(transform.position.x < PlatformDestructionPoint.transform.position.x)
        {
            // set it to false, ready to be reused in the object pooler script
            gameObject.SetActive(false);
        }
	}
}
