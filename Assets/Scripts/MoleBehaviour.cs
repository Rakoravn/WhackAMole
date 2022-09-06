using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoleBehaviour : MonoBehaviour
{
    public static MoleBehaviour instance;

    [SerializeField]
    private List<Transform> minionList = new List<Transform>();

    private float TIMER_INTERVAL;

    private bool showMoles = false;
    private int randomTileOne;
    private int randomTileTwo;

    public bool isPlaying;

    public MistakeBehaviour mb;

    private Color startColor = new Color(0, 0, 0);
    private Color correctColor = new Color(0.3f, 1, 0);
    private Color wrongColor = new Color(1, 0, 0.1f);

    [SerializeField]
    private GameObject gameOverMenu;

    private void Awake() {
    }

    void Start()
    {
        if (!instance) {
            instance = this;
        }
        isPlaying = false;
        TIMER_INTERVAL = 3.0f;
    }

    void Update()
    {
        if (isPlaying) {
            Debug.Log("HAA");   
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

    IEnumerator ResetWhackModule(int index) {
        if(index == randomTileOne) {
            minionList[index].parent.GetComponent<Renderer>().material.color = correctColor;
            UiManager.instance.UpdateScore();
            minionList[randomTileOne].transform.localPosition = Vector3.Lerp(minionList[randomTileOne].transform.localPosition,
                new Vector3(minionList[randomTileOne].transform.localPosition.x, -3f, minionList[randomTileOne].transform.localPosition.z)
                , 1f);
        } else {
            minionList[index].parent.GetComponent<Renderer>().material.color = wrongColor;
            MistakeBehaviour.instance.CheckMistakes();
        }
        yield return new WaitForSeconds(.5f);
        minionList[index].parent.GetComponent<Renderer>().material.color = startColor;
    }

    IEnumerator WhackGenerator() {
        showMoles = true;
        System.Random rnd = new System.Random();
        randomTileOne = rnd.Next(minionList.Count);
        minionList[randomTileOne].transform.localPosition = Vector3.Lerp(minionList[randomTileOne].transform.localPosition,
            new Vector3(minionList[randomTileOne].transform.localPosition.x, 2.5f, minionList[randomTileOne].transform.localPosition.z),
            1f);
        yield return new WaitForSeconds(TIMER_INTERVAL);
        minionList[randomTileOne].transform.localPosition = Vector3.Lerp(minionList[randomTileOne].transform.localPosition,
            new Vector3(minionList[randomTileOne].transform.localPosition.x, -3f, minionList[randomTileOne].transform.localPosition.z)
            , 1f);
        showMoles = false;
    }

    public void StartGame() {
        isPlaying = true;
        gameOverMenu.SetActive(false);
    }

    public void GameOver() {
        isPlaying = false;
        gameOverMenu.SetActive(true);
    }
}
