using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu; // the pause menu screen
    public GameObject pauseButton; // the pause button

    // if the pause button is pressed
    public void PauseGame()
    {
        Time.timeScale = 0f; // setting the speed of the game to be 0 
        pauseMenu.SetActive(true); // turn on the pause menu
    }

    // if the resume button is pressed
    public void ResumeGame()
    {        
        Time.timeScale = 1.0f; // set the speed of the game to be 1, full speed
        pauseMenu.SetActive(false);
    }

    // if the restart button is pressed
    public void Restart()
    {
        Time.timeScale = 1.0f; // set the speed of the game to be 1, full speed
        pauseMenu.SetActive(false); // turn off the pause menu
        pauseButton.SetActive(true); // turn on the pause button, incase it is still off after you die and restart
        FindObjectOfType<GameManager>().Reset(); // reset the game
    }

    // if the quit button is pressed
    public void Quit()
    {
        SceneManager.LoadScene(0); // load the main menu
    }
}
