using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class PauseResume : MonoBehaviour {

	public bool paused = false;
	public GameObject pauseScreen;
	public TouchControls controls;

	// Use this for initialization
	void Start () {
		if (!PlayerPrefs.HasKey("FirstTimeTutorial")) {
			PlayerPrefs.SetInt ("FirstTimeTutorial", 0);
		}

		if (!PlayerPrefs.HasKey("StartWithTutorial")) {
			PlayerPrefs.SetInt ("StartWithTutorial", 0);
		}
	}


	public void PauseGame() {
		if (paused == false) {
			Time.timeScale = 0.0f;
			controls.enabled = false;
			paused = true;
			pauseScreen.SetActive(true);
		} else if (paused == true) {
			Time.timeScale = 1.0f;
			controls.enabled = true;
			paused = false;
			pauseScreen.SetActive(false);
		}
	}

	public void PlayTutorial() {
		PlayerPrefs.SetInt ("StartWithTutorial", 1);
		ChangeToTutorial ();
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
