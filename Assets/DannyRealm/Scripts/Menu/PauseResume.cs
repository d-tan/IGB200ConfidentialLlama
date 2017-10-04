using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseResume : MonoBehaviour {

	public bool paused = false;
	public TouchControls controls;

	// Use this for initialization
	void Start () {
		
	}


	public void PauseGame() {
		if (paused == false) {
			Time.timeScale = 0.0f;
			controls.enabled = false;
			paused = true;
		} else if (paused == true) {
			Time.timeScale = 1.0f;
			controls.enabled = true;
			paused = false;
		}
	}

	public void ChangeToTutorial() {
		SceneManager.LoadScene (1);
	}

	public void ChangeToMenu() {
		SceneManager.LoadScene (0);
	}

	public void QuitGame() {
		Application.Quit();
	}
}
