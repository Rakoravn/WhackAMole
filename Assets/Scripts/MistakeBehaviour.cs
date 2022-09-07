using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MistakeBehaviour : MonoBehaviour
{
    public static MistakeBehaviour instance;
    private int ERRORS_MADE;

    [SerializeField]
    private GameObject errorImagePanel;

    void Start() {
        if (!instance) {
            instance = this;
        }
        ERRORS_MADE = 0;
    }

    public void CheckMistakes() {
        GameObject obj = errorImagePanel.transform.Find("ErrorImage" + ERRORS_MADE).gameObject;
        obj.SetActive(true);
        ERRORS_MADE++;
        if(ERRORS_MADE >= 3) {
            UiManager.instance.CheckIfHighscoreIsBeaten();
            MoleBehaviour.instance.GameOver();
        }
    }

    public void ResetErrors() {
        foreach (Transform child in errorImagePanel.transform) {
            child.gameObject.SetActive(false);
        }
        ERRORS_MADE = 0;
    }
}
