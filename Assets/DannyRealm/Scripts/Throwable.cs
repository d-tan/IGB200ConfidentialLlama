using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour {

	public Collider myCollider;
	public Rigidbody rb;
	public bool beingHeld = false;

	public bool isIngredient = false;

	// Use this for initialization
	void Start () {
		myCollider = GetComponent<Collider> ();
		rb = GetComponent<Rigidbody> ();
	}
}
