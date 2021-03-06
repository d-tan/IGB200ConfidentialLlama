﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ScoreManager : MonoBehaviour {

	public GameObject scoreObject;
	public GameObject scoreboard;
	public GameObject endCard;
	public Text scoreAddL;
	public Text scoreAddR;

	public Text scoreText;
	private int scoreValue = 0;
	private const int scoreBase = 5;
	int pizzasServed = 0;
	[HideInInspector]
	public bool gameHasEnded = false;

	// Score Addition
	Vector2 scoreFadeTimer = new Vector2 (0, 0);
	Color scoreFadeColour;
	float scoreFadeTime = 1.5f;

	public bool debug = false;

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
		scoreFadeColour = scoreAddR.color;
		ResetPoints ();

		Debug.Log("Checking platforms");
		#if (PLATFORM_IPHONE)
			if (!File.Exists(Path.Combine(Application.persistentDataPath, fileName + ".json"))) {
				Debug.Log("Creaing file");
				File.Create(Path.Combine(Application.persistentDataPath, fileName + ".json")).Dispose();
				string content = JsonUtility.ToJson(new ScoreBoardSave());
				File.WriteAllText(Path.Combine(Application.persistentDataPath, fileName + ".json"), content);
			}
			Debug.Log("Path exists: " + File.Exists(Path.Combine(Application.persistentDataPath, fileName + ".json")));
			Debug.Log("JsonFile: " + JsonUtility.ToJson(new ScoreBoardSave()));
			Debug.Log("Stored file: " + File.ReadAllText(Path.Combine(Application.persistentDataPath, fileName + ".json")));
		#endif

		if (Application.platform == RuntimePlatform.Android) {
			if (!File.Exists (Path.Combine (Application.persistentDataPath, fileName + ".json"))) {
				Debug.Log ("Creaing file");
				File.Create (Path.Combine (Application.persistentDataPath, fileName + ".json")).Dispose ();
				string content = JsonUtility.ToJson (new ScoreBoardSave ());
				File.WriteAllText (Path.Combine (Application.persistentDataPath, fileName + ".json"), content);
			}

			Debug.Log ("Path exists: " + File.Exists (Path.Combine (Application.persistentDataPath, fileName + ".json")));
			Debug.Log ("JsonFile: " + JsonUtility.ToJson (new ScoreBoardSave ()));
			Debug.Log ("Stored file: " + File.ReadAllText (Path.Combine (Application.persistentDataPath, fileName + ".json")));
		}

		ReadScoreboardFile ();
		UpdateScoreboard ();
	}

	void Update() {
		UpdateScoreAddition ();
	}

	void UpdateScoreAddition() {
		if (scoreFadeTimer.x > 0.01f) {
			scoreFadeColour.a = Mathf.Lerp (0, 1, scoreFadeTimer.x / scoreFadeTime);
			scoreAddL.color = scoreFadeColour;

			scoreFadeTimer.x -= Time.deltaTime;
		} else {
			if (scoreFadeTimer.x != 0) {
				scoreFadeTimer.x = 0;
				scoreFadeColour.a = Mathf.Lerp (0, 1, scoreFadeTimer.x / scoreFadeTime);
				scoreAddL.color = scoreFadeColour;
			}
		}

		if (scoreFadeTimer.y > 0.01f) {
			scoreFadeColour.a = Mathf.Lerp (0, 1, scoreFadeTimer.y / scoreFadeTime);
			scoreAddR.color = scoreFadeColour;

			scoreFadeTimer.y -= Time.deltaTime;
		} else {
			if (scoreFadeTimer.y != 0) {
				scoreFadeTimer.y = 0;
				scoreFadeColour.a = Mathf.Lerp (0, 1, scoreFadeTimer.y / scoreFadeTime);
				scoreAddL.color = scoreFadeColour;
			}
		}
	}

	// Increase value of score upon obtaining points
	public void AwardPoints(int numOfIngredients, int side = 0) {
		if (!gameHasEnded) {
			scoreValue += scoreBase * numOfIngredients;
			pizzasServed++;
			scoreText.text = scoreValue.ToString ();

			if (side > 0) {
				scoreFadeTimer.y = scoreFadeTime;
				scoreAddR.text = "+" + (scoreBase * numOfIngredients).ToString ();
			} else if (side < 0) {
				scoreFadeTimer.x = scoreFadeTime;
				scoreAddL.text = "+" + (scoreBase * numOfIngredients).ToString ();
			}
		}
	}


	public void ResetPoints() {
		scoreValue = 0;
		pizzasServed = 0;
		if (scoreText != null)
			scoreText.text = scoreValue.ToString();
	}

	// For SubmitButton
	public void SubmitName() {
		player1Name = P1Input.text;
		player2Name = P2Input.text;
//		submitButton.SetActive (false);
//		scoreValue = Random.Range(400, 500);
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
		Debug.Log ("Reading scoreboard file...");
		string file = "";

		#if (PLATFORM_IPHONE)
			file = File.ReadAllText(Path.Combine(Application.persistentDataPath, fileName + ".json"));
		#endif

		if (Application.platform == RuntimePlatform.Android) {
			file = File.ReadAllText(Path.Combine(Application.persistentDataPath, fileName + ".json"));
		} else if (Application.platform == RuntimePlatform.WindowsPlayer) {
			file = File.ReadAllText (Path.Combine(Application.streamingAssetsPath, fileName + ".json"));
		}

		Debug.Log ("Inside File: " + file);
		// Read from Json
		saveClass = JsonUtility.FromJson<ScoreBoardSave> (file);

		Debug.Log ("Saveclass: " + (saveClass != null).ToString ());

		ScoreEntry entry;

		Debug.Log ("Storing scoreboard entries...");
		for (int i = 0; i < numOfLeaders; i++) {
			
			entry = new ScoreEntry (saveClass.P1 [i], saveClass.P2 [i], saveClass.scores [i], saveClass.serves [i]);

			entries [i] = entry;
		}
	}

	void UpdateScoreboard() {
		Debug.Log ("UpdateScoreboard()");

		if (scoreValue > entries[numOfLeaders - 1].score) {
			ScoreEntry newEntry = new ScoreEntry (player1Name, player2Name, scoreValue, pizzasServed);
			entries [numOfLeaders - 1] = newEntry;
			Debug.Log ("Current entries qualify");

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

		Debug.Log ("Updating display");
		UpdateScoreboardDisplay ();
	}

	void SaveScoreboard() {
		Debug.Log ("Saving scoreboard...");
//		AddText ("Storing entries... ");
		for (int i = 0; i < numOfLeaders; i++) {
			saveClass.P1 [i] = entries [i].P1Name;
			saveClass.P2 [i] = entries [i].P2Name;
			saveClass.scores [i] = entries [i].score;
			saveClass.serves [i] = entries [i].pizzasServed;
		}

		Debug.Log ("Writing to file");

		string jsonString = JsonUtility.ToJson (saveClass, true);

		string pathDir = "";

		#if (PLATFORM_IPHONE)
			pathDir = Path.Combine(Application.persistentDataPath, fileName + ".json");
		#else
			pathDir = Path.Combine(Application.streamingAssetsPath, fileName + ".json");
		#endif

		if (Application.platform == RuntimePlatform.Android) {
			pathDir = Path.Combine(Application.persistentDataPath, fileName + ".json");
		}

		Debug.Log ("Path: " + pathDir);

		if (File.Exists (pathDir)) {
			File.WriteAllText (pathDir, jsonString);
		}
		Debug.Log ("Done Saving");
	}

	public void ResetScoreboard() {
		string path = "";
		#if (PLATFORM_IPHONE || UNITY_ANDROID)
		path = Path.Combine(Application.persistentDataPath, fileName + ".json");
		#else
		path = Path.Combine(Application.streamingAssetsPath, fileName + ".json");
		#endif

		if (Application.platform == RuntimePlatform.Android) {
			path = Path.Combine(Application.persistentDataPath, fileName + ".json");
		}

		if (File.Exists(path)) {
			string json = JsonUtility.ToJson (new ScoreBoardSave (), true);
			File.WriteAllText (path, json);
			ReadScoreboardFile ();
			UpdateScoreboard ();
			Debug.Log ("Rewritten File");
		}
	}

	public void ShowScoreboard() {
		scoreboard.SetActive (true);
	}

	public void BackButton() {
		scoreboard.SetActive (false);
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