using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformColour : MonoBehaviour
{
    public Color colourtoLerpTo; // assigned in the inspector, the colour we want the object to bounce between
    public Color lerpedColor; // just to see in the inspector , this is the current colour rendered on the object
    private Renderer myRenderer; // renderer component
    public float lerpTime; // how fast we want it to bounce between, this doesnt increase the speed, just the time it waits before going to the next colour, 
    // if lerptime is 0.5 it would go half way before turning back to the other colour, 
    // if lerptime is 1 it will go all the way to the other colour before coming back

    // Use this for initialization
    void Start ()
    {
        myRenderer = gameObject.GetComponent<Renderer>(); // getting the renderer component
	}
	
	// Update is called once per frame
	void Update ()
    {        
        lerpedColor = Color.Lerp(colourtoLerpTo, Color.black, Mathf.PingPong(Time.time, lerpTime)); // calculating the colour to lerp to
        myRenderer.material.color = lerpedColor; // assigning the colour to the object
    }
}
