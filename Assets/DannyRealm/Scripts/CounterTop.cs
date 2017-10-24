using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterTop : MonoBehaviour {

//	GameObject child;
//	Collider trigger;

	public int side = 0;

	// Holding
	public Collider currentlyHolding;
	public Throwable currentScript;


	void Start() {
//		child = transform.GetChild (0).gameObject;
//		trigger = child.GetComponent<Collider> ();
	}

	void Update() {
		
	}

	public void HoldObject(Collider col, Throwable script) {
		currentlyHolding = col;
		currentScript = script;
		script.side = side;
		col.attachedRigidbody.velocity = new Vector3 (0, 0, 0);
		col.attachedRigidbody.isKinematic = true;

		col.transform.parent = this.transform;
		col.transform.localPosition = new Vector3 (0, 0.01f, 0);
	}


}
