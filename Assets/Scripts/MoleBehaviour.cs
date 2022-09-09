using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MoleBehaviour : MonoBehaviour {
    public static MoleBehaviour instance;

    [SerializeField]
    private List<Transform> minionList = new List<Transform>();

    private float TIMER_INTERVAL_MIN;
    private float TIMER_INTERVAL_MAX;

    private bool showMoles = false;
    private int randomTileOne;
    private int tempTileOne;
    private int randomTileTwo;
    private int tempTileTwo;
    private bool scoreCountedFirst = false;
    private bool scoreCountedSecond = false;
    private int chanceToSpawnTwo;
    private bool spawnSecondMole = false;
    private bool minionSpawned = false;

    private bool gotFirstMole = false;
    private bool gotSecondMole = false;

    float duration = 0.1f;
    float popOutTime = 0;
    float popInTime = 0;

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

    private void Awake() {
        if (!instance) {
            instance = this;
        }
    }

    void Start() {
        SetDifficulty();
        isPlaying = false;
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
        } else {
            if (Input.GetKeyDown(KeyCode.S)) {
                if (!isPlaying) {
                    StartGame();
                }
            }
        }
    }

    IEnumerator ResetWhackModule(int index) {
        if (minionSpawned) {
            if (!scoreCountedFirst && index == randomTileOne) {
                minionList[index].parent.GetComponent<Renderer>().material.color = correctColor;
                UiManager.instance.UpdateScore();
                minionList[index].transform.localPosition = Vector3.Lerp(minionList[index].transform.localPosition,
                    new Vector3(minionList[index].transform.localPosition.x, -3f, minionList[index].transform.localPosition.z), 1f);
                SetDifficulty();
                scoreCountedFirst = true;
                gotFirstMole = true;
                SoundBehaviour.instance.PlaySuccessSound();
            } else if (spawnSecondMole && !scoreCountedSecond && index == randomTileTwo) {
                minionList[index].parent.GetComponent<Renderer>().material.color = correctColor;
                UiManager.instance.UpdateScore();
                minionList[index].transform.localPosition = Vector3.Lerp(minionList[index].transform.localPosition,
                    new Vector3(minionList[index].transform.localPosition.x, -3f, minionList[index].transform.localPosition.z), 1f);
                SetDifficulty();
                scoreCountedSecond = true;
                gotSecondMole = true;
                SoundBehaviour.instance.PlaySuccessSound();
            } else {
                minionList[index].parent.GetComponent<Renderer>().material.color = wrongColor;
                MistakeBehaviour.instance.CheckMistakes();
                SoundBehaviour.instance.PlayErrorSound();
            }
            yield return new WaitForSeconds(.5f);
            minionList[index].parent.GetComponent<Renderer>().material.color = startColor;
        } else {
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator WhackGenerator() {
        showMoles = true;
        scoreCountedFirst = false;
        scoreCountedSecond = false;
        gotFirstMole = false;
        gotSecondMole = false;
        float timer = Random.Range(TIMER_INTERVAL_MIN, TIMER_INTERVAL_MAX);
        while (randomTileOne == tempTileOne) { randomTileOne = Random.Range(0, minionList.Count - 1); }
        tempTileOne = randomTileOne;
        StartCoroutine(PopUpMinion(randomTileOne));
        minionSpawned = true;
        int spawnTwoGenerator = Random.Range(0, 100);
        if (spawnTwoGenerator <= chanceToSpawnTwo) {
            while (randomTileTwo == tempTileTwo && randomTileTwo == randomTileOne) { randomTileTwo = Random.Range(0, minionList.Count - 1); }
            tempTileTwo = randomTileTwo;
            StartCoroutine(PopUpMinion(randomTileTwo));
            spawnSecondMole = true;
        }
        yield return new WaitForSeconds(timer);
        if (!gotFirstMole) {
            StartCoroutine(PopInMinion(randomTileOne));
        }
        if (spawnSecondMole && !gotSecondMole) {
            StartCoroutine(PopInMinion(randomTileTwo));
            spawnSecondMole = false;
        }
        showMoles = false;
    }

    IEnumerator PopUpMinion(int index) {
        popOutTime = 0;
        while (popOutTime < duration) {
            minionList[index].transform.localPosition = Vector3.Lerp(minionList[index].transform.localPosition,
                new Vector3(minionList[index].transform.localPosition.x, 2.5f, minionList[index].transform.localPosition.z), popOutTime / duration);
            popOutTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        SoundBehaviour.instance.PlayPushOutSound();
    }

    IEnumerator PopInMinion(int index) {
        popInTime = 0;
        while (popInTime < duration) {
            minionList[index].transform.localPosition = Vector3.Lerp(minionList[index].transform.localPosition,
                new Vector3(minionList[index].transform.localPosition.x, -3f, minionList[index].transform.localPosition.z), popInTime / duration);
            popInTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        SoundBehaviour.instance.PlayPushInSound();
    }

    private void SetDifficulty() {
        int currScore = UiManager.instance.GetCurrentScore();
        if (currScore < 10) {
            difficulty = Difficulty.Easy;
            CheckDifficulty();
        }
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
                TIMER_INTERVAL_MAX = 4.0f;
                TIMER_INTERVAL_MIN = 3.5f;
                break;
            case Difficulty.Medium:
                chanceToSpawnTwo = 30;
                TIMER_INTERVAL_MAX = 3.5f;
                TIMER_INTERVAL_MIN = 3.0f;
                break;
            case Difficulty.Hard:
                chanceToSpawnTwo = 40;
                TIMER_INTERVAL_MAX = 3.0f;
                TIMER_INTERVAL_MIN = 2.5f;
                break;
            case Difficulty.Impossible:
                chanceToSpawnTwo = 50;
                TIMER_INTERVAL_MAX = 2.5f;
                TIMER_INTERVAL_MIN = 2.0f;
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
