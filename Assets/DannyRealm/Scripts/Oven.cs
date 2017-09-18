using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oven : MonoBehaviour {

	public OrderReceiver rightOrder;
	public OrderReceiver leftOrder;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// Check if pizza is inputed
		// Start timer + update timer
		// Check order with pizza (Identify pizza)
		// When timer is up, Send Waiter to collect pizza
		// Award points
		// Remove Pizza
		// Remove Order
		// 
	}

	public void InputCollider(Plate plateScript) {
		IngredientID[] pizzaIngredients = new IngredientID[plateScript.ingredients.Length];

		for (int i = 0; i < pizzaIngredients.Length; i++) {
			if (plateScript.ingredients [i])
				pizzaIngredients [i] = plateScript.ingredients [i].ingredientID;
			else
				pizzaIngredients [i] = IngredientID.None;
		}

		int side = CheckPizza (ref pizzaIngredients);

		if (side > 0) {
			// Right side
			Debug.Log("Recipe Right");
		} else if (side < 0) {
			// Left side
			Debug.Log ("Recipe Left");
		} else {
			// No match
			Debug.Log("No Match");
		}
	}

	int CheckPizza(ref IngredientID[] ingredients) {
		bool right = true;
		bool left = true;
		IngredientID[] IDs = rightOrder.currentOrder.ingredients;

		if (rightOrder) {
			right = CompareArrays (ref ingredients, ref IDs);

			if (right) 
				return 1;
		}

		IDs = leftOrder.currentOrder.ingredients;
		if (leftOrder) {
			left = CompareArrays (ref ingredients, ref IDs);

			if (left)
				return -1;
		}

		return 0;
	}

	bool CompareArrays(ref IngredientID[] pizza, ref IngredientID[] order) {
		bool match = true;
		IngredientID id;
		bool noMatch = true;

		for (int i = 0; i < pizza.Length; i++) {
			id = pizza [i];
			noMatch = true;

			for (int j = 0; j < order.Length; j++) {
				if (id == order [j]) {
					noMatch = false;
					break;
				}
			}

			if (noMatch) {
				match = false;
				break;
			}
		}

		return match;
	}
}
