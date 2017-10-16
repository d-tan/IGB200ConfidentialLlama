using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public PileOf[] piles;

	// Managers
	OrderManager orderManager;
	public TimerScript timeManager;

	void Start() {
		orderManager = GetComponent<OrderManager> ();
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

		timeManager.StartTimer ();
	}
}
