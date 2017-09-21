using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour {

	public Collider myCollider;
	public Rigidbody rb;
	public float minVel = 0.04f;

	public bool beingHeld = false;
	public Transform prevParent;

	public bool isIngredient = false;

	// Use this for initialization
	void Start () {
		myCollider = GetComponent<Collider> ();
		rb = GetComponent<Rigidbody> ();
	}

	public void StoreParent() {
		if (transform.parent)
			prevParent = transform.parent;
	}

	void CheckVelocity() {
		if (rb.velocity.sqrMagnitude <= minVel) {
			// move back
			// Reset counter top

		}
			
	}
}
