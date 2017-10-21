using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSceneSwitcher : MonoBehaviour {

	public bool isStart;
	public bool isQuit;

	// Use this for initialization
	void Awake () {
		if (!PlayerPrefs.HasKey("FirstTimeTutorial")) {
			PlayerPrefs.SetInt ("FirstTimeTutorial", 0);
		}

		if (!PlayerPrefs.HasKey("StartWithTutorial")) {
			PlayerPrefs.SetInt ("StartWithTutorial", 0);
		}
	}

	// Update is called once per frame
	void Update () {

	}

	void OnMouseUp() {
		if (isStart) {
			SceneManager.LoadScene (1);
			Debug.Log ("LoadLevel");
		}
		if (isQuit) {
			Application.Quit ();
			Debug.Log ("Quit");
		}
	}
}
