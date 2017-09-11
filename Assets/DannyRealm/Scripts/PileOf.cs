using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PileOf : MonoBehaviour {

	public GameObject item;
	public bool canSpawn = true;
	string tagToWatch = "Ingredient";


	void Update() {
		if (canSpawn) {
			canSpawn = false;
			SpawnItem ();
		}
	}

	public void GiveItem(Vector3 coords) {
		Instantiate (item, coords, Quaternion.identity);
	}

	void SpawnItem() {
		Instantiate (item, transform.position, Quaternion.identity);
	}

	void OnTriggerStay(Collider other) {
		if (other.CompareTag(tagToWatch)) {
			canSpawn = false;
		}
	}

	void OnTriggerExit(Collider other) {
		
	}
}
