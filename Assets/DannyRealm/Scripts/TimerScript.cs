using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour {

	public Text timerText;
	public float gametime = 300.0f;
	string minutes;
	string seconds;

	public GameObject endScreen;
	GameManager gameManager;

	// Use this for initialization
	void Start () {
		//		endScreen.SetActive (false);
		gameManager = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameManager> ();
	}

	public void StartTimer() {
		SetGameTime ();
		InvokeRepeating ("IncrementCounter", 1.0f, 1.0f);
	}

	void SetGameTime() {
		timerText.text = minutes + ":" + seconds ;
	}

	void IncrementCounter() {
		if (gametime > 0.0f) {
			gametime = gametime - 1.0f;
			gametime = Mathf.Round (gametime * 100f) / 100f;
			minutes = Mathf.Floor(gametime / 60).ToString("00");
			seconds = Mathf.Floor(gametime % 60).ToString("00");
			SetGameTime ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(gametime <= 0.0f) { // When time runs out...
			// Show score screen
			if (!endScreen.activeSelf) {
				endScreen.SetActive (true);
				gameManager.GameEnded ();
			}
		}
	}
}
