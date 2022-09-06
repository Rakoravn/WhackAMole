using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoleBehaviour : MonoBehaviour
{
    [SerializeField]
    private List<Transform> minionList = new List<Transform>();

    private float TIMER_INTERVAL;
    private int ERRORS_MADE;

    private bool showMoles = false;
    private int randomTileOne;
    private int randomTileTwo;

    private void Awake() {
    }

    void Start()
    {
        TIMER_INTERVAL = 3.0f;
        ERRORS_MADE = 0;
    }

    void Update()
    {
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

    IEnumerator ResetWhackModule(int index) {
        if(index == randomTileOne) {
            minionList[index].GetComponent<Image>().color = new Color32(54, 57, 255, 255);
            UiManager.instance.UpdateScore();
            Debug.Log("CORRECT");
        } else {
            minionList[index].GetComponent<Image>().color = new Color32(255, 152, 54, 255);
            CheckMistakes();
            Debug.Log("WRONG");
        }

        yield return new WaitForSeconds(.5f);

        minionList[index].GetComponent<Image>().color = new Color32(165, 255, 54, 255);
    }

    IEnumerator WhackGenerator() {
        showMoles = true;
        System.Random rnd = new System.Random();
        randomTileOne = rnd.Next(minionList.Count);
        Vector3 lerpValue = Vector3.Lerp(minionList[randomTileOne].transform.localPosition,
            new Vector3(minionList[randomTileOne].transform.localPosition.x, 2.5f, minionList[randomTileOne].transform.localPosition.z)
            , 5f);
        minionList[randomTileOne].transform.localPosition = lerpValue;
        yield return new WaitForSeconds(TIMER_INTERVAL);
        //minionList[randomTileOne].transform.localPosition = Vector3.Lerp(minionList[randomTileOne].transform.localPosition,
            //new Vector3(minionList[randomTileOne].transform.localPosition.x, -5, minionList[randomTileOne].transform.localPosition.z)
          //  , 5f);

        showMoles = false;
    }

    private void CheckMistakes() {
        ERRORS_MADE++;
        if(ERRORS_MADE >= 3) {
            //Show game over menu
        }
    }
}
