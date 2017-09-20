using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour {

	public GameObject scoreObject;

	public Text scoreText;
	private int scoreValue = 0;
	private const int scoreBase = 5;

	public bool debug = false;


	void Start () {
		
	}


	void Update() {
		scoreText.text = scoreValue.ToString();

		// DEBUG
		if (Input.GetKeyDown ("i") && debug == true) {
			AwardPoints ();
		}

	}


	// Increase value of score upon obtaining points
	public void AwardPoints() {
		scoreValue += scoreBase;
	}

}