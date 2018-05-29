using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // movement
    public float moveSpeed; // how fast we move
    private float baseMoveSpeed; // base speed we store for restarting
    public float jumpForce; // how high we jump
    public bool isGrounded; // if we are grounded or not
    public LayerMask ground; // what the ground is
    public float jumpTime; // excess jumpforce
    private float jumpTimeCounter; // private excess jump force
    public float speedMultiplyer; // how much faster we move
    public float speedIncreaseMilestone; // how far before we get faster
    private float baseSpeedIncreaseMilestone; // base distance we store for restarting
    private float speedMilestoneCount; // how many times we have increased speed
    private float baseSpeedMilestoneCount; // base amount we store for restarting

    // variables used for lerping the players colour through
    public Color myColour;
    private Color myColour2;
    private Renderer myRenderer;
    private Color lerpedColor;

    public AudioSource deathSound; // death sound
    public AudioSource jumpSound; // jump sound

    // bools used for checking actions
    public bool canPlayJumpSound;
    private bool canDoubleJump;

    // components
    private Rigidbody2D myRigidbody;
    public GameManager theGameManager;
    private Collider2D mycollider;

    // variables used to track the player to make sure he isnt bugged or already dead
    private Vector3 lastPos;
    float checkTimer = 1.0f;
    public bool dead;

    public GameObject pauseButton; // pause button
    public GameObject shatteredPlayer; // pause button

    void Start()
    {
        // getting components
        myRigidbody = GetComponent<Rigidbody2D>();
        myRenderer = GetComponent<Renderer>();
        mycollider = GetComponent<Collider2D>();

        // assigning privates to the publics and setting up stuff
        jumpTimeCounter = jumpTime;
        baseMoveSpeed = moveSpeed;
        speedMilestoneCount = speedIncreaseMilestone;
        baseSpeedMilestoneCount = speedMilestoneCount;
        baseSpeedIncreaseMilestone = speedIncreaseMilestone;
        canDoubleJump = true;
        dead = false;
        lastPos = transform.position;
        myColour2 = new Color(myColour.r, myColour.g, myColour.b, 0.5f);
        myRenderer.material.color = myColour;
        Time.timeScale = 1.0f;

        // repeatedly call a function to see if the player is stuck/bugged just getting points
        InvokeRepeating("BugChecker", checkTimer, checkTimer);
    }

    void Update()
    {
        // find if we are grounded
        isGrounded = Physics2D.IsTouchingLayers(mycollider, ground);
        // if we have progressed far enough 
        if (transform.position.x > speedMilestoneCount)
        {
            UpDateMilestone();
        }

        // set the movement 
        myRigidbody.velocity = new Vector2(moveSpeed, myRigidbody.velocity.y);

        /*
        // if we press either jump button
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {            
            // if we are currently on the ground
            if (isGrounded)
            {
                // call jump
                Jump();
            }
            // if the player is already in the air and they can jump again
            if (!isGrounded && canDoubleJump)
            {
                // call double jump
                DoubleJump();
            }
        }
        */
        /*
        // if the buttons are held down
        if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))
        {
            // and we still have some gogo juice in the tank
            if (jumpTimeCounter > 0)
            {
                // add more force to the player
                myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jumpForce);
                // use up some gogo juice
                jumpTimeCounter -= Time.deltaTime;
                // if the player can play the jumping sound
                if (canPlayJumpSound)
                {
                    // call play jump sound
                    PlayJumpSound();
                }
            }
        }
        */

        // when we let go of the jump buttons
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonUp(0))
        {
            // empty the gogo juice tank
            jumpTimeCounter = 0;
        }

        // if we hit the ground
        if (isGrounded)
        {
            // get more gogo juice
            jumpTimeCounter = jumpTime;
            // let the player jump again
            canDoubleJump = true;
        }
        // changes the colour of the player, bounces from one to the other
        lerpedColor = Color.Lerp(myColour, myColour2, Mathf.PingPong(Time.time, 1));
        // assign the colour
        myRenderer.material.color = lerpedColor;
    }

    void UpDateMilestone()
    {
        // set the next milestone
        speedMilestoneCount += speedIncreaseMilestone;
        // increase the movement speed
        moveSpeed *= speedMultiplyer;
        speedIncreaseMilestone *= speedMultiplyer;
    }

    public void JumpPressed()
    {
        if (isGrounded)
        {
            // initial burst of energy to make us jump
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jumpForce);
            jumpSound.pitch = Random.Range(1.0f, 1.1f); // change the pitch slightly
            jumpSound.Play(); // play the jump sound
        }
        // if the player is already in the air and they can jump again
        if (!isGrounded && canDoubleJump)
        {
            // call double jump
            DoubleJump();
        }

        if (jumpTimeCounter > 0)
        {
            // add more force to the player
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jumpForce);
            // use up some gogo juice
            jumpTimeCounter -= Time.deltaTime;
            // if the player can play the jumping sound
            if (canPlayJumpSound)
            {
                // call play jump sound
                PlayJumpSound();
            }
        }
    }

    void DoubleJump()
    {
        canDoubleJump = false; // make sure they cant keep jumping
        myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jumpForce); // add upwards force
        jumpTimeCounter = jumpTime; // reset the gogo juice
        jumpSound.pitch = Random.Range(1.0f, 1.1f); // change the pitch slightly
        jumpSound.Play(); // play audio
    }

    public void JumpRelease()
    {
        // empty the gogo juice tank
        jumpTimeCounter = 0;
    }

    void PlayJumpSound()
    {
        jumpSound.pitch = Random.Range(1.0f, 1.3f); // change the pitch slightly
        jumpSound.Play(); // play the jumping sound
        canPlayJumpSound = false; // stop them from spamming the jump sound
    }

    // if our collider hits another collider 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // if the other collider is the killbox under the world
        if (collision.gameObject.tag == "killBox" && !dead)
        {
            TimeToDie(); // time to die...
        }
    }

    // called on repeat to stop the player bugging the game
    private void BugChecker()
    {
        // if the player hasnt move position in a while, the player has bugged out and is afk
        if (lastPos == transform.position && !dead)
        {
            TimeToDie(); // time to die...
        }
        else
        {
            lastPos = transform.position; // update the last known position ready to  do this again
        }
    }

    public void TimeToDie()
    {
        dead = true; // if the player has died but the invokerepeating hasnt checked the players position yet, a bool to make sure they dont enter here twice
        Instantiate(shatteredPlayer, transform.position, Quaternion.identity);
        pauseButton.SetActive(false); // turn off the pause button so the player cant press it, making the pause UI appear on top of the death UI
        theGameManager.RestartGame(); // call the restart function
        moveSpeed = baseMoveSpeed; // reset the players speed 
        speedMilestoneCount = baseSpeedMilestoneCount; // reset how far the player needs to go before increasing speeds
        speedIncreaseMilestone = baseSpeedIncreaseMilestone; // reset the amount we increase the player speed by
        deathSound.Play(); // play the death sound
    }
}