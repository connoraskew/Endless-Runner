using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerUps : MonoBehaviour
{

    public float Duration; // how long the power ups last for

    public int powerUpSelector; // used to get which power up it will be

    public powerUpManager myPowerManager; // reference to the powerup manager

    public Color[] myColours; // list of colours to change to depending on the powerup

    void Start()
    {
        myPowerManager = FindObjectOfType<powerUpManager>();
    }

    void Awake()
    {
        AssigningStuff();
    }

    public void AssigningStuff()
    {
        powerUpSelector = Random.Range(0, 3);

        GetComponent<PlatformColour>().colourtoLerpTo = myColours[powerUpSelector];        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {

            switch (powerUpSelector)
            {
                case 0:
                    myPowerManager.ActivateDoubler(Duration);
                    break;
                case 1:
                    myPowerManager.Activatespikeless(Duration);
                    break;
                case 2:
                    myPowerManager.ActivateDoubler(Duration);
                    myPowerManager.Activatespikeless(Duration);
                    break;
            }
        }
        gameObject.SetActive(false);
    }
}
