using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : Throwable {

	public IngredientID ingredientID;
		
	public void OnPlate() {
		myCollider.enabled = false;
		rb.isKinematic = true;
	}
}
