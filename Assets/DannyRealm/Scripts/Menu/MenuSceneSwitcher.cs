using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSceneSwitcher : MonoBehaviour {

	public bool isStart;
	public bool isQuit;

	// Use this for initialization
	void Start () {

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
