using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;
    public float scoreFloat;

    public Text highScoreText;
    public float highScoreFloat;

    public float pointsPerSecond;
    private float basePointsPerSecond;
    public bool ScoreIncreasing;

    public bool shouldDouble;

    void Start()
    {
        basePointsPerSecond = pointsPerSecond;
        scoreFloat = 0;
        if (PlayerPrefs.HasKey("HighScore"))
        {
            highScoreFloat = PlayerPrefs.GetFloat("HighScore");
        }
    }

    void Update()
    {
        if (ScoreIncreasing)
        {
            scoreFloat += pointsPerSecond * Time.deltaTime;
        }

        if (scoreFloat > highScoreFloat)
        {
            highScoreFloat = scoreFloat;
            PlayerPrefs.SetFloat("HighScore", highScoreFloat);
        }


        scoreText.text = "SCORE: " + Mathf.RoundToInt(scoreFloat);
        highScoreText.text = "HIGH SCORE: " + Mathf.RoundToInt(highScoreFloat);
    }

    public void AddScore(int pointsToAdd)
    {
        if(shouldDouble)
        {
            scoreFloat += pointsToAdd;
        }
        scoreFloat += pointsToAdd;
    }

    public void ResetPointsPerSecond()
    {
        pointsPerSecond = basePointsPerSecond;
    }
}
