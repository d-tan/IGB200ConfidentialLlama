using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVoting : MonoBehaviour {

    public GameObject GameManager;
    public GameObject Player01Response;
    public GameObject Player02Response;

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
        tutorialScript.voteToProgress++;
        
        if(gameObject.tag == "Player01Tutorial") {
            Player01Response.SetActive(false);
        }
        if (gameObject.tag == "Player02Tutorial") {
            Player02Response.SetActive(false);
        }
        if (voteToProgress == 2) {
            tutorialScript.progressTutorial();
            voteToProgress = 0;
            print("All Players Voted - Progressing");
        }
    }
}
