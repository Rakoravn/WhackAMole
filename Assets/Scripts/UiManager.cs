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
    public TextMeshProUGUI newHighscoreTxt;

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
            highscore = PlayerPrefs.GetInt("Highscore");
        }
        highscoreTxt.text = highscore.ToString();
    }

    public void UpdateScore() {
        currentScore++;
        currentScoreTxt.text = currentScore.ToString();
    }

    public int GetCurrentScore() {
        return currentScore;
    }

    public void CheckIfHighscoreIsBeaten() {
        if(currentScore > highscore) {
            PlayerPrefs.SetInt("Highscore", currentScore);
            PlayerPrefs.Save();
            newHighscoreTxt.gameObject.SetActive(true);
            CheckSetHighscore();
        } else {
            newHighscoreTxt.gameObject.SetActive(false);
        }
    }

    public void ResetCurrentScore() {
        currentScore = 0;
        currentScoreTxt.text = currentScore.ToString();
    }
}
