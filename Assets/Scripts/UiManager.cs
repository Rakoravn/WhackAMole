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
    public GameObject errorPanel; 
    public Image errorImage;

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
            highscore = PlayerPrefs.GetInt("highscore");
        }
        highscoreTxt.text = highscore.ToString();
    }

    public void UpdateScore() {
        currentScore++;
        currentScoreTxt.text = currentScore.ToString();
    }
}
