﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ScoreManager : MonoBehaviour {

	public GameObject scoreObject;
	public GameObject scoreboard;
	public GameObject endCard;

	public Text scoreText;
	private int scoreValue = 0;
	private const int scoreBase = 5;
	int pizzasServed = 0;
	[HideInInspector]
	public bool gameHasEnded = false;

	public bool debug = false;
	public Text debugText;

	// Leaderboard Stuff
	// Player 1, Player 2, Score, Pizzas Served
	string fileName = "PizzaFlickScores";
	const int stringCap = 10;
	const int numOfLeaders = 10; // Also in the LeaderBoardSave class
	string player1Name = "Player 1";
	string player2Name = "Player 2";

	public InputField P1Input;
	public InputField P2Input;
	public GameObject submitButton;

	public Text p1Text;
	public Text p2Text;
	public Text scoresText;
	public Text servesText;

	ScoreEntry[] entries = new ScoreEntry[numOfLeaders];
	ScoreBoardSave saveClass = new ScoreBoardSave();

	void Start () {
		player1Name = "Player 1";
		player2Name = "Player 2";
		ResetPoints ();

		ReadScoreboardFile ();
	}


	void Update() {

	}


	// Increase value of score upon obtaining points
	public void AwardPoints(int numOfIngredients) {
		if (!gameHasEnded) {
			scoreValue += scoreBase * numOfIngredients;
			pizzasServed++;
			scoreText.text = scoreValue.ToString ();
		}
	}


	public void ResetPoints() {
		scoreValue = 0;
		pizzasServed = 0;
		scoreText.text = scoreValue.ToString();
	}

	// For SubmitButton
	public void SubmitName() {
		player1Name = P1Input.text;
		player2Name = P2Input.text;
//		submitButton.SetActive (false);
//		AddText ("Submitting Names...\n");
		scoreValue = Random.Range(400, 500);
		pizzasServed = Random.Range(10, 40);
		Debug.Log ("Score: " + scoreValue + " " + pizzasServed);

		UpdateScoreboard ();
		endCard.SetActive (false);
		scoreboard.SetActive (true);
	}

	void UpdateScoreboardDisplay() {
		string p1s = "";
		string p2s = "";
		string scores = "";
		string serves = "";

		for (int i = 0; i < numOfLeaders; i++) {
			p1s += entries [i].P1Name + "\n";
			p2s += entries [i].P2Name + "\n";
			scores += entries [i].score.ToString() + "\n";
			serves += entries [i].pizzasServed.ToString() + "\n";
		}

		p1Text.text = p1s;
		p2Text.text = p2s;
		scoresText.text = scores;
		servesText.text = serves;

	}

	void ReadScoreboardFile() {
		AddText ("Reading scoreboard file...");
		string file = File.ReadAllText (Path.Combine(Application.streamingAssetsPath, fileName + ".json"));

		// Read from Json
		saveClass = JsonUtility.FromJson<ScoreBoardSave> (file);

		ScoreEntry entry;

//		AddText ("Storing scoreboard entries... ");
		for (int i = 0; i < numOfLeaders; i++) {
			entry = new ScoreEntry (saveClass.P1 [i], saveClass.P2 [i], saveClass.scores [i], saveClass.serves [i]);

			entries [i] = entry;
			AddText (saveClass.P1 [i] + ": " + saveClass.scores [i] + " || ");
		}
//		AddText ("\n");
	}

	void UpdateScoreboard() {
//		AddText("Updating scoreboard...\n");
		if (scoreValue > entries[numOfLeaders - 1].score) {
			ScoreEntry newEntry = new ScoreEntry (player1Name, player2Name, scoreValue, pizzasServed);
			entries [numOfLeaders - 1] = newEntry;

			AddText ("Current entry qualifies... ");
			for (int i = numOfLeaders - 2; i >= 0; i--) {
				if (entries[i].score < newEntry.score) {

					entries [i + 1] = entries [i];
					entries [i] = newEntry;
				} else {
					break;
				}
			}
			SaveScoreboard ();
		}

		AddText ("Updating display... \n");
		UpdateScoreboardDisplay ();
	}

	void SaveScoreboard() {
		AddText ("Saving scoreboard... ");
//		AddText ("Storing entries... ");
		for (int i = 0; i < numOfLeaders; i++) {
			saveClass.P1 [i] = entries [i].P1Name;
			saveClass.P2 [i] = entries [i].P2Name;
			saveClass.scores [i] = entries [i].score;
			saveClass.serves [i] = entries [i].pizzasServed;
		}

		AddText ("Writing to file...");
		AddText ("Path: " + File.Exists (Path.Combine (Application.streamingAssetsPath, fileName + ".json")));
		AddText(Path.Combine (Application.streamingAssetsPath, fileName + ".json") + "\n");
//		AddText ("ToJson: " + JsonUtility.ToJson (saveClass, false));
		string jsonString = JsonUtility.ToJson (saveClass, true);
		File.WriteAllText (Path.Combine(Application.streamingAssetsPath, fileName + ".json"), jsonString);
		AddText ("Done Saving.\n");
	}

	void AddText(string words) {
		if (debug)
			debugText.text += words;
	}
}

[System.Serializable]
public class ScoreEntry {

	public string P1Name = "";
	public string P2Name = "";
	public int score = 0;
	public int pizzasServed = 0;

	public ScoreEntry(string p1, string p2, int scr, int serves) {
		this.P1Name = p1;
		this.P2Name = p2;
		this.score = scr;
		this.pizzasServed = serves;
	}
}

[System.Serializable]
public class ScoreBoardSave {
	const int numOfLeaders = 10;

	public string[] P1 = new string[numOfLeaders];
	public string[] P2 = new string[numOfLeaders];
	public int[] scores = new int[numOfLeaders];
	public int[] serves = new int[numOfLeaders];

	public ScoreBoardSave() {
		for (int i = 0; i < numOfLeaders; i++) {
			P1 [i] = "";
			P2 [i] = "";
			scores[i] = 0;
			serves[i] = 0;
		}
	}
}