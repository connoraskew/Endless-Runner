using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditiveForce : MonoBehaviour
{
    private Rigidbody2D RB; // the rigid body on this object

    // two variables to pick between when getting the sideways velocity
    public float XForce;


    // two variables to pick between when getting the upwards velocity
    public float YForceLower;
    public float YForceUpper;

    // Use this for initialization
    void Start ()
    {
        RB = GetComponent<Rigidbody2D>(); // get the rigid body on this object
        float ForceX = Random.Range(-XForce, XForce); // getting a random number between two floats
        float ForceY = Random.Range(YForceLower, YForceUpper); // getting a random number between two floats
        RB.velocity = new Vector2(ForceX, ForceY); // add the force making it go upwards
        //RB.AddForce(transform.up * Force);
    }   
}
