using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MoleBehaviour : MonoBehaviour {
    public static MoleBehaviour instance;

    [SerializeField]
    private List<Transform> minionList = new List<Transform>();

    private float TIMER_INTERVAL_MIN = 3.0f;
    private float TIMER_INTERVAL_MAX = 3.5f;

    private bool showMoles = false;
    private int randomTileOne;
    private int randomTileTwo;
    private bool scoreCountedFirst = false;
    private bool scoreCountedSecond = false;
    private int chanceToSpawnTwo;
    private bool spawnSecondMole = false;

    private bool isPlaying;

    private Color startColor = new Color(0, 0, 0);
    private Color correctColor = new Color(0.3f, 1, 0);
    private Color wrongColor = new Color(1, 0, 0.1f);

    [SerializeField]
    private GameObject gameOverMenu;

    [SerializeField]
    private TextMeshProUGUI countDownTxt;

    private float countdownTimer = 3f;
    private bool startGame = false;
    private Difficulty difficulty;

    private enum Difficulty {
        Easy,
        Medium,
        Hard,
        Impossible
    }

    void Start() {
        if (!instance) {
            instance = this;
        }
        difficulty = Difficulty.Easy;
        isPlaying = false;
        chanceToSpawnTwo = 20;
        TIMER_INTERVAL_MIN = 2.5f;
        TIMER_INTERVAL_MAX = 3.0f;
    }

    void Update() {
        if (isPlaying) {
            if (countdownTimer > 0 && !startGame) {
                countdownTimer -= Time.deltaTime;
                if (countdownTimer > 0.5f) {
                    countDownTxt.text = countdownTimer.ToString("F0");
                } else {
                    countDownTxt.text = "";
                }
            } else {
                startGame = true;
                countDownTxt.gameObject.SetActive(false);
            }
            if (startGame) {
                if (!showMoles) {
                    StartCoroutine(WhackGenerator());
                }
                if (Input.GetKeyDown(KeyCode.Q)) {
                    StartCoroutine(ResetWhackModule(0));
                } else if (Input.GetKeyDown(KeyCode.W)) {
                    StartCoroutine(ResetWhackModule(1));
                } else if (Input.GetKeyDown(KeyCode.E)) {
                    StartCoroutine(ResetWhackModule(2));
                } else if (Input.GetKeyDown(KeyCode.A)) {
                    StartCoroutine(ResetWhackModule(3));
                } else if (Input.GetKeyDown(KeyCode.S)) {
                    StartCoroutine(ResetWhackModule(4));
                } else if (Input.GetKeyDown(KeyCode.D)) {
                    StartCoroutine(ResetWhackModule(5));
                }
            }

        }
    }

    IEnumerator ResetWhackModule(int index) {
        if (!scoreCountedFirst && index == randomTileOne) {
            minionList[index].parent.GetComponent<Renderer>().material.color = correctColor;
            UiManager.instance.UpdateScore();
            minionList[index].transform.localPosition = Vector3.Lerp(minionList[index].transform.localPosition,
                new Vector3(minionList[index].transform.localPosition.x, -3f, minionList[index].transform.localPosition.z), 1f);
            SetDifficulty();
            scoreCountedFirst = true;
        } else if (spawnSecondMole && !scoreCountedSecond && index == randomTileTwo) {
            minionList[index].parent.GetComponent<Renderer>().material.color = correctColor;
            UiManager.instance.UpdateScore();
            minionList[index].transform.localPosition = Vector3.Lerp(minionList[index].transform.localPosition,
                new Vector3(minionList[index].transform.localPosition.x, -3f, minionList[index].transform.localPosition.z), 1f);
            SetDifficulty();
            scoreCountedSecond = true;
        }
        else {
            minionList[index].parent.GetComponent<Renderer>().material.color = wrongColor;
            MistakeBehaviour.instance.CheckMistakes();
        }
        yield return new WaitForSeconds(.5f);
        minionList[index].parent.GetComponent<Renderer>().material.color = startColor;
    }

    IEnumerator WhackGenerator() {
        showMoles = true;
        scoreCountedFirst = false;
        scoreCountedSecond = false;
        float timer = Random.Range(TIMER_INTERVAL_MIN, TIMER_INTERVAL_MAX);
        randomTileOne = Random.Range(0, minionList.Count - 1);
        minionList[randomTileOne].transform.localPosition = Vector3.Lerp(minionList[randomTileOne].transform.localPosition,
            new Vector3(minionList[randomTileOne].transform.localPosition.x, 2.5f, minionList[randomTileOne].transform.localPosition.z), 1f);
        int spawnTwoGenerator = Random.Range(0, 100);
        if (spawnTwoGenerator <= chanceToSpawnTwo) {
            randomTileTwo = Random.Range(0, minionList.Count - 1);
            if (randomTileTwo != randomTileOne) {
                minionList[randomTileTwo].transform.localPosition = Vector3.Lerp(minionList[randomTileTwo].transform.localPosition,
                    new Vector3(minionList[randomTileTwo].transform.localPosition.x, 2.5f, minionList[randomTileTwo].transform.localPosition.z), 1f);
                spawnSecondMole = true;
            }
        }
        yield return new WaitForSeconds(timer);
        minionList[randomTileOne].transform.localPosition = Vector3.Lerp(minionList[randomTileOne].transform.localPosition,
            new Vector3(minionList[randomTileOne].transform.localPosition.x, -3f, minionList[randomTileOne].transform.localPosition.z), 1f);
        if (spawnSecondMole) {
            minionList[randomTileOne].transform.localPosition = Vector3.Lerp(minionList[randomTileOne].transform.localPosition,
                new Vector3(minionList[randomTileOne].transform.localPosition.x, -3f, minionList[randomTileOne].transform.localPosition.z), 1f);
            spawnSecondMole = false;
        }
        showMoles = false;
    }

    private void SetDifficulty() {
        int currScore = UiManager.instance.GetCurrentScore();
        if (currScore == 10) {
            difficulty = Difficulty.Medium;
            CheckDifficulty();
        }
        if (currScore == 20) {
            difficulty = Difficulty.Hard;
            CheckDifficulty();
        }
        if (currScore == 30) {
            difficulty = Difficulty.Impossible;
            CheckDifficulty();
        }
    }

    private void CheckDifficulty() {
        switch (difficulty) {
            case Difficulty.Easy:
                chanceToSpawnTwo = 20;
                TIMER_INTERVAL_MIN = 2.5f;
                TIMER_INTERVAL_MAX = 3.0f;
                break;
            case Difficulty.Medium:
                chanceToSpawnTwo = 35;
                TIMER_INTERVAL_MIN = 2.0f;
                TIMER_INTERVAL_MAX = 2.5f;
                break;
            case Difficulty.Hard:
                chanceToSpawnTwo = 50;
                TIMER_INTERVAL_MIN = 1.5f;
                TIMER_INTERVAL_MAX = 2.0f;
                break;
            case Difficulty.Impossible:
                chanceToSpawnTwo = 65;
                TIMER_INTERVAL_MIN = 1.0f;
                TIMER_INTERVAL_MAX = 1.5f;
                break;
        }
    }

    public void StartGame() {
        isPlaying = true;
        UiManager.instance.ResetCurrentScore();
        MistakeBehaviour.instance.ResetErrors();
        countDownTxt.gameObject.SetActive(true);
        gameOverMenu.SetActive(false);
    }

    public void GameOver() {
        isPlaying = false;
        startGame = false;
        countdownTimer = 3f;
        gameOverMenu.SetActive(true);
    }
}
