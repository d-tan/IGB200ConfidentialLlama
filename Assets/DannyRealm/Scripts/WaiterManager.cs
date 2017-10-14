using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaiterManager : MonoBehaviour {

	public GameObject waiterObject;
	public Vector3 spawnPoint;

	// Timer
	public Vector2 spawnTime = new Vector2 (3, 9);
	public Vector2 tutorialSpawnTime = new Vector2 (4, 7);
	float spawnTimer = 0f;

	Tutorial tutorialScript;
	public bool tutorialSpawn = false;

	void Start() {
		tutorialScript = GetComponent<Tutorial> ();
	}

	void Update() {
		if (tutorialScript.completedTutorial || tutorialSpawn) {
			spawnTimer -= Time.deltaTime;

			if (spawnTimer <= 0) {
				SpawnWaiter ();
			}
		}
	}


	public void SpawnWaiter() {
		Instantiate (waiterObject, spawnPoint, Quaternion.identity);
		if (!tutorialScript.completedTutorial && tutorialSpawn) {
			spawnTimer = Random.Range (tutorialSpawnTime.x, tutorialSpawnTime.y);
		} else {
			spawnTimer = Random.Range (spawnTime.x, spawnTime.y);
		}
	}
}
