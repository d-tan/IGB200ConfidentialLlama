using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PileOf : MonoBehaviour {

	public GameObject item;
	public bool canSpawn = true;
	string tagToWatch = "Ingredient";
	GameObject spawnedObject;

	Throwable currentObject;
	bool isPlate = false;

	// Tutorial stuff
	[Header("Tutorial Stuff")]
	public Tutorial tutorialScript;
	public GameObject[] displayObjects;

	void Start() {
		if (item.GetComponent<Plate> ())
			isPlate = true;
	}

	void Update() {
		if (canSpawn) {
			canSpawn = false;
			SpawnItem ();
		} else {
			if (tutorialScript.completedTutorial && (spawnedObject == null || spawnedObject.transform.parent != null))
				canSpawn = true;
		}

		// Tutorial Only
		if (!tutorialScript.completedTutorial && currentObject == null) {
			canSpawn = true;
		}
	}

	public void GiveItem(Vector3 coords) {
		Instantiate (item, coords, Quaternion.identity);
	}

	void SpawnItem() {
		spawnedObject = Instantiate (item, transform.position, Quaternion.identity) as GameObject;
		currentObject = spawnedObject.GetComponent<Throwable> ();

		// Tutorial only
		if (isPlate && !tutorialScript.completedTutorial) {
			tutorialScript.plate = spawnedObject.GetComponent<Plate> ();
		}

		Debug.Assert (currentObject, "Spawned item should have Throwable script attached");
	}

	public void ToggleDisplayObjects(bool state) {
		for (int i = 0; i < displayObjects.Length; i++) {
			displayObjects [i].SetActive (state);
		}
	}

	void OnTriggerStay(Collider other) {
		if (other.CompareTag(tagToWatch) || other.CompareTag("PickUp") || other.CompareTag("Plate")) {
			canSpawn = false;
		}
	}

	void OnTriggerExit(Collider other) {
		if (currentObject.myCollider.Equals(other) && tutorialScript.completedTutorial) {
			canSpawn = true;
		}
	}
}
