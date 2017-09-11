using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterTop : MonoBehaviour {

	GameObject child;
	Collider trigger;

	// Holding
	public Collider currentlyHolding;


	void Start() {
		child = transform.GetChild (0).gameObject;
		trigger = child.GetComponent<Collider> ();
	}

	public void HoldObject(Collider col) {
		currentlyHolding = col;
		col.attachedRigidbody.velocity = new Vector3 (0, 0, 0);

		col.transform.position = this.transform.position + new Vector3 (0, 0.01f, 0);

	}


}
