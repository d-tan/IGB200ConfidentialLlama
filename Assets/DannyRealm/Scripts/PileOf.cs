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
	public bool hideIngredient = true;

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
			if (!displayObjects[0].activeSelf) {
				ToggleDisplayObjects (true);
			}

		// if we can't spawn AND is still tutorial AND we have an object
		} else if (!canSpawn && !tutorialScript.completedTutorial && currentObject != null) {
			// if the object we have is being held AND the table model is still on
			if (currentObject.beingHeld && displayObjects[0].activeSelf) {
				ToggleDisplayObjects (false);
			}
		}
	}

	public void GiveItem(Vector3 coords) {
		Instantiate (item, coords, Quaternion.identity);
	}

	void SpawnItem() {
		spawnedObject = Instantiate (item, transform.position, item.transform.rotation) as GameObject;
		currentObject = spawnedObject.GetComponent<Throwable> ();

		// Tutorial only
		if (isPlate && !tutorialScript.completedTutorial) {
			tutorialScript.plate = spawnedObject.GetComponent<Plate> ();
		}

		if (!isPlate)
			currentObject.ToggleRender (false);

//		if (hideIngredient && !tutorialScript.completedTutorial) {
//			
//			ToggleDisplayObjects (false);
//		}

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
