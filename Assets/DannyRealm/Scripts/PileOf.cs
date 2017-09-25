using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PileOf : MonoBehaviour {

	public GameObject item;
	public bool canSpawn = true;
	string tagToWatch = "Ingredient";
	GameObject spawnedObject;

	Throwable currentObject;

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
