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

	public Tutorial tutorialScript;

	void Start() {
		if (item.GetComponent<Plate> ())
			isPlate = true;
	}

	void Update() {
		if (canSpawn) {
			canSpawn = false;
			SpawnItem ();
		} else {
			if (spawnedObject == null || spawnedObject.transform.parent != null)
				canSpawn = true;
		}

	}

	public void GiveItem(Vector3 coords) {
		Instantiate (item, coords, Quaternion.identity);

	}

	void SpawnItem() {
		spawnedObject = Instantiate (item, transform.position, Quaternion.identity) as GameObject;
		currentObject = spawnedObject.GetComponent<Throwable> ();

		if (isPlate && !tutorialScript.completedTutorial) {
			Debug.Log ("Added plate");
			tutorialScript.plateList.Add (spawnedObject.GetComponent<Plate> ());

			for (int i = 0; i < tutorialScript.plateList.Count; i++) {

				if (tutorialScript.plateList [i] == null)
					tutorialScript.plateList.RemoveAt (i);
				Debug.Log (tutorialScript.plateList [i].name);
			}
		}

		Debug.Assert (currentObject, "Spawned item should have Throwable script attached");
	}

	void OnTriggerStay(Collider other) {
		if (other.CompareTag(tagToWatch) || other.CompareTag("PickUp") || other.CompareTag("Plate")) {
			canSpawn = false;
		}
	}

	void OnTriggerExit(Collider other) {
		if (currentObject.myCollider.Equals(other)) {
			canSpawn = true;
		}
	}
}
