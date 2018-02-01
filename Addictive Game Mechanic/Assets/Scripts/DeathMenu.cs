using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    public GameObject pauseButton; // the pause button

    // this is called when the button is pressed
    public void Restart()
    {
        pauseButton.SetActive(true); // when the player wants to restart we enable the pause button, after turning it off, turned it off because it overlapped the pause and death screen 
        FindObjectOfType<GameManager>().Reset(); // reset the game
    }

    public void Quit()
    {
        SceneManager.LoadScene(0); // loads the main menu
    }
}
