using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order : Throwable {

	const int numOfIngredients = 4;

	public IngredientID[] ingredients = new IngredientID[numOfIngredients];

	public bool returnMe = true;

	// This is to stop it from access the Canvas
	protected override void Degrade () {
	}
}
