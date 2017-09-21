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
	public bool flicked = false;
	public int side = 0;

	// Use this for initialization
	void Start () {
		myCollider = GetComponent<Collider> ();
		rb = GetComponent<Rigidbody> ();
	}

	void Update() {
		CheckVelocity ();
	}

	public void StoreParent() {
		if (transform.parent)
			prevParent = transform.parent;
	}

	void CheckVelocity() {
		if (flicked && rb.velocity.sqrMagnitude <= minVel) {
			// move back
//			if (prevParent)
//				transform.position = new Vector3 (prevParent.position.x, 0f, prevParent.position.z);
//			else
//				Destroy (this.gameObject);

			// Reset counter top


			flicked = false;
		}
	}

	void OnCollisionEnter(Collision col) {
		if (col.transform.CompareTag ("Wall"))
			side = 0;
	}
}
