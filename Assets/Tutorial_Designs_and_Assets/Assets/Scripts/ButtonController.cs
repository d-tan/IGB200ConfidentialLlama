using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour {

    public GameObject GameManager;
    public GameObject responseButton;

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
        //tutorialScript.voteToProgress++;
        
        responseButton.SetActive(false);

        tutorialScript.playerVote();

        
    }
}
