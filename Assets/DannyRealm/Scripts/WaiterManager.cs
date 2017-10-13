using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaiterManager : MonoBehaviour {

	public GameObject waiterObject;
	public Vector3 spawnPoint;

	// Timer
	public Vector2 spawnTime = new Vector2 (3, 9);
	float spawnTimer = 0f;

	Tutorial tutorialScript;

	void Start() {
		tutorialScript = GetComponent<Tutorial> ();
	}

	void Update() {
		if (tutorialScript.completedTutorial) {
			spawnTimer -= Time.deltaTime;

			if (spawnTimer <= 0) {
				SpawnWaiter ();
			}
		}
	}

	public void SpawnWaiter() {
		Instantiate (waiterObject, spawnPoint, Quaternion.identity);
		spawnTimer = Random.Range (spawnTime.x, spawnTime.y);
	}
}
