using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalOptionController : MonoBehaviour {

    public GameObject GameManager;
    public GameObject selfButton;

    public int voteToProgress = 0;

    Tutorial tutorialScript;

    // Use this for initialization
    void Start () {
        tutorialScript = GameManager.GetComponent<Tutorial>();
    }

    // Update is called once per frame
    void Update () {

    }

    public void playerVote() {
        if (selfButton.tag == "RedoTutorialButtons") {
            print("REDO");
            tutorialScript.voteToProgress = -100;
        }
        if (selfButton.tag == "EndTutorialButtons") {
            print("Voted to End");
        }
        selfButton.SetActive(false);

        tutorialScript.playerVote();


    }
}
