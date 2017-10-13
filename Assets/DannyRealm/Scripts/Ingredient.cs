using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : Throwable {

	public IngredientID ingredientID;

	protected override void Start() {
		base.Start ();
//		myRenderer.enabled = false;
	}
		
	public void OnPlate() {
		myCollider.enabled = false;
		rb.isKinematic = true;
	}
}
