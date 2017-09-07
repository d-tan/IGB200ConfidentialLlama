﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour {

	Collider myCollider;
	float colliderHeight = 0f;

	const int numOfIngredients = 4;
	public Ingredient[] ingredients = new Ingredient[numOfIngredients];
	public Transform[] positions = new Transform[numOfIngredients]; 

	// Use this for initialization
	void Start () {
		myCollider = GetComponent<Collider> ();

		colliderHeight = myCollider.bounds.extents.y;

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.P)) {
			ingredients [0].transform.localScale = lastScale;
		}
	}

	Vector3 lastScale = new Vector3();

	public void IngredientTrigger(Ingredient script) {

		for (int i = 0; i < ingredients.Length; i++) {
			if (ingredients[i] == null) {
				Debug.Log ("Ingredient added");
				ingredients [i] = script;
				script.OnPlate ();

//				// Calculate scale
//				Vector3 newScale = new Vector3 ();
//				Vector3 curScale = script.transform.localScale;
//				Vector3 parentScale = transform.localScale;
//				newScale.x = curScale.x / parentScale.x;
//				newScale.y = curScale.y / parentScale.y;
//				newScale.z = curScale.z / parentScale.z;
////				Debug.Log (newScale);
//				lastScale = newScale;

				script.transform.parent = this.transform;

				script.transform.position = positions [i].position;
				script.transform.localScale *= 0.4f;

				script.transform.position += new Vector3 (0f, 2 * myCollider.bounds.extents.y + 2 * script.myCollider.bounds.extents.y, 0f);

				break;
			}
		}

	}

	public bool CheckIfFull() {
		for (int i = 0; i < ingredients.Length; i++) {
			if (ingredients [i] == null)
				return false;
		}

		return true;
	}
}
