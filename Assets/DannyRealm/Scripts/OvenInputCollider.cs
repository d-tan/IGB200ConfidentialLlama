using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvenInputCollider : MonoBehaviour {

	public Oven parent;

	// Use this for initialization
	void Start () {
		parent = transform.parent.GetComponent<Oven> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other) {
		Plate script = other.GetComponent<Plate> ();

		if (script) {
			parent.InputCollider (script);
		}
	}
}
