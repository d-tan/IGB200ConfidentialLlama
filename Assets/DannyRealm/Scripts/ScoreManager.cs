using UnityEngine;
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

	// Leaderboard Stuff
	// Player 1, Player 2, Score, Pizzas Served
	string filePath = "Assets/Resources/";
	string fileName = "PizzaFlickScores";
	const int stringCap = 10;
	const int numOfLeaders = 10; // Also in the LeaderBoardSave class
	string player1Name = "Player 1";
	string player2Name = "Player 2";

	public InputField P1Input;
	public InputField P2Input;

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
		string completeFilePath = filePath + fileName + ".json";
		string file = File.ReadAllText (completeFilePath);

		// Read from Json
		saveClass = JsonUtility.FromJson<ScoreBoardSave> (file);

		ScoreEntry entry;

		for (int i = 0; i < numOfLeaders; i++) {
			entry = new ScoreEntry (saveClass.P1 [i], saveClass.P2 [i], saveClass.scores [i], saveClass.serves [i]);

			entries [i] = entry;
		}
	}

	void UpdateScoreboard() {
		if (scoreValue > entries[numOfLeaders - 1].score) {
			ScoreEntry newEntry = new ScoreEntry (player1Name, player2Name, scoreValue, pizzasServed);
			entries [numOfLeaders - 1] = newEntry;

			for (int i = numOfLeaders - 2; i >= 0; i--) {
				if (entries[i].score < newEntry.score) {

					entries [i + 1] = entries [i];
					entries [i] = newEntry;
				} else {
					break;
				}
			}

			SaveScoreboard ();
			UpdateScoreboardDisplay ();
		}
	}

	void SaveScoreboard() {
		string completeFilePath = filePath + fileName + ".json";

		for (int i = 0; i < numOfLeaders; i++) {
			saveClass.P1 [i] = entries [i].P1Name;
			saveClass.P2 [i] = entries [i].P2Name;
			saveClass.scores [i] = entries [i].score;
			saveClass.serves [i] = entries [i].pizzasServed;
//			Debug.Log(saveClass.P1 [i] + " " + saveClass.scores [i]);
		}

		File.WriteAllText (completeFilePath, JsonUtility.ToJson (saveClass, true));
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