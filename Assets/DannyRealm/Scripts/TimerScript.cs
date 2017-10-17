using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour {

	public Text timerText;
	public float gametime = 300.0f;

	public GameObject endScreen;

	// Use this for initialization
	void Start () {
//		endScreen.SetActive (false);
	}

	public void StartTimer() {
		SetGameTime ();
		InvokeRepeating ("IncrementCounter", 1.0f, 1.0f);
	}

	void SetGameTime() {
		timerText.text = gametime.ToString();
	}

	void IncrementCounter() {
		if (gametime > 0.0f) {
			gametime = gametime - 1.0f;
			gametime = Mathf.Round (gametime * 100f) / 100f;
			SetGameTime ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(gametime <= 0.0f) { // When time runs out...
			// Show score screen
			endScreen.SetActive (true);
		}
	}
}
