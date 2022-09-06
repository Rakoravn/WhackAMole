using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;

    public TextMeshProUGUI highscoreTxt;
    public TextMeshProUGUI currentScoreTxt;

    private int highscore = 0;
    private static int currentScore = 0;

    void Start()
    {
        if (!instance) {
            instance = this;
        }
        CheckSetHighscore();
    }

    private void CheckSetHighscore() {
        if (!PlayerPrefs.HasKey("Highscore")) {
            PlayerPrefs.SetInt("Highscore", highscore);
            PlayerPrefs.Save();
        } else {
            Debug.Log("AAAA");
            highscore = PlayerPrefs.GetInt("Highscore");
            Debug.Log(highscore);
        }
            Debug.Log("BBBB");
        highscoreTxt.text = highscore.ToString();
    }

    public void UpdateScore() {
        currentScore++;
        currentScoreTxt.text = currentScore.ToString();
    }

    public void CheckIfHighscoreIsBeaten() {
        Debug.Log(currentScore + " - " + highscore);
        if(currentScore > highscore) {
            Debug.Log(currentScore);
            PlayerPrefs.SetInt("Highscore", currentScore);
            PlayerPrefs.Save();
            CheckSetHighscore();
        }
    }

    public void resetCurrentScore() {
        currentScore = 0;
        currentScoreTxt.text = currentScore.ToString();
    }
}
