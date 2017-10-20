using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public PileOf[] piles;
	public GameObject timer;

	// Managers
	OrderManager orderManager;
	ScoreManager scoreManager;
	public TimerScript timeManager;

	void Start() {
		orderManager = GetComponent<OrderManager> ();
		scoreManager = GetComponent<ScoreManager> ();
	}

	public void TutorialBegin() {
		for (int i = 0; i < piles.Length; i++) {
			piles [i].ToggleDisplayObjects (false);
		}
	}

	public void TutorialEnded() {
		for (int i = 0; i < piles.Length; i++) {
			piles [i].ToggleDisplayObjects (true);
		}
		scoreManager.ResetPoints ();
		timer.SetActive (true);
		timeManager.StartTimer ();
	}

	public void GoToMenu() {
		SceneManager.LoadScene (0);
	}

	public void Reload() {
		SceneManager.LoadScene (1);
	}
}
