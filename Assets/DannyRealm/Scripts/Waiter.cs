using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waiter : MonoBehaviour {

	// Movement
	public Vector3[] waypoints;
	float speed = 8f;
	int currentWaypoint = 0;
	const float minDistance = 0.05f;

	public MeshRenderer childRenderer;
	Color originalColour;

	// Use this for initialization
	void Start () {
		originalColour = childRenderer.material.color;
		NextWaypoint ();
	}
	
	// Update is called once per frame
	void Update () {
		if (currentWaypoint < waypoints.Length) {
			float distance = (waypoints [currentWaypoint] - transform.position).sqrMagnitude;

			if (distance <= minDistance) {
				NextWaypoint ();
			} else {
				transform.position += transform.forward * speed * Time.deltaTime;
			}
		}
	}

	void NextWaypoint() {
		transform.position = waypoints [currentWaypoint];

		currentWaypoint++;

		if (currentWaypoint < waypoints.Length) {
			transform.LookAt (waypoints [currentWaypoint]);
		} else {

			Destroy (this.gameObject);

		}
	}

	void OnCollisionEnter(Collision col) {
		if (col.transform.CompareTag("Ingredient") || col.transform.CompareTag("Plate")) {
			StartCoroutine (FlashRed ());
		}
	}

	IEnumerator FlashRed() {
		childRenderer.material.color = Color.red;
		yield return new WaitForSeconds (0.3f);
		childRenderer.material.color = originalColour;

	}
}
