using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalOptionController : MonoBehaviour {

    public GameObject GameManager;
    public GameObject selfButton;
    public GameObject counterButton;

    public int voteToProgress = 0;
    public int skipTutorial = 0;

    Tutorial tutorialScript;

    // Use this for initialization
    void Start () {
        tutorialScript = GameManager.GetComponent<Tutorial>();
    }

    // Update is called once per frame
    void Update () {

    }

    public void playerVote() {
        if (tutorialScript.tutorialProgression == 15) {
            if (selfButton.tag == "RedoTutorialButtons") {
                print("REDO");
                tutorialScript.voteToProgress = -100;
            }
            if (selfButton.tag == "EndTutorialButtons") {
                print("Voted to End");
            }
            selfButton.SetActive(false);
            counterButton.SetActive(false);

            tutorialScript.playerVote();
        }

        if (tutorialScript.tutorialProgression == 0) {
            if (selfButton.tag == "SkipTutorial&RedoTutorialButtons") {
                print("Voted to Skip");
                tutorialScript.voteToProgress += 100;
            }
            if (selfButton.tag == "StartTutorial&EndTutorialButtons") {
                print("START");
                //tutorialScript.voteToProgress = 100;
            }
            selfButton.SetActive(false);
            counterButton.SetActive(false);

            tutorialScript.playerVote();
        }
    }
}
