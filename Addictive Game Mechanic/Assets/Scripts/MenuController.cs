using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject ScorePanel; // the object in the scene
    public Text highScoreText; // the text component on the object
    private float highScoreFloat; // the high score

    private void Start()
    {
        UpdateHighScoreText(); // used to update the UI on the main menu
    }

    // when the play button is clicked
    public void Play()
    {
        SceneManager.LoadScene("Main"); // loads the game
    }

    // when the reset button is clicked
    public void Reset()
    {
        PlayerPrefs.DeleteKey("HighScore"); // deleted the saved float    
        UpdateHighScoreText(); // update the text on the main menu
    }

    // when the quit button is clicked
    public void Quit()
    {
        Application.Quit(); // quits the game
    }

    // used to update the UI on the main menu
    void UpdateHighScoreText()
    {
        if (PlayerPrefs.HasKey("HighScore")) // if the player has played the game then they have created a key called "HighScore"
        {
            ScorePanel.SetActive(true); // turns on the score on the main menu
            highScoreFloat = PlayerPrefs.GetFloat("HighScore"); // sets the float
            highScoreText.text = "HIGH SCORE: " + Mathf.RoundToInt(highScoreFloat); // sets the text to show the high score
        }
        else // else if there is now player pref called "HighScore"
        {
            ScorePanel.SetActive(false); // if the player hasnt played the game yet, dont show the score text
        }
    }
}
