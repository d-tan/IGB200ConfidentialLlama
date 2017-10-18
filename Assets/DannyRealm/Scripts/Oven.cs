using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oven : MonoBehaviour {

	public OrderReceiver rightOrder;
	public OrderReceiver leftOrder;
	int numOfMatches = 0;

	ScoreManager scoreManager;
    Tutorial tutorial;

	// Use this for initialization
	void Start () {
        scoreManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<ScoreManager>();
        tutorial = GameObject.FindGameObjectWithTag("GameController").GetComponent<Tutorial>();
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
		numOfMatches = 0;

		for (int i = 0; i < pizzaIngredients.Length; i++) {
			if (plateScript.ingredients [i]) {
				pizzaIngredients [i] = plateScript.ingredients [i].ingredientID;
				numOfMatches++;
			} else {
				pizzaIngredients [i] = IngredientID.None;
			}
		}

		int side = CheckPizza (pizzaIngredients);

		CompleteOrder (side);
		// Start timers
		Destroy(plateScript.gameObject);
	}

	void CompleteOrder(int side) {
		if (side > 0) {
			// Right side
			Debug.Log("Recipe Right");
			OrderCompletedActions ();
			rightOrder.OrderCompleted ();
            


        } else if (side < 0) {
			// Left side
			Debug.Log ("Recipe Left");
			OrderCompletedActions ();
			leftOrder.OrderCompleted ();


        } else {
			Debug.Log ("No Match");
		}
	}

	void OrderCompletedActions() {
		Debug.Log ("ingredients: " + numOfMatches);
		scoreManager.AwardPoints (numOfMatches);
		TutorialProgress ();
		OrderManager.numOrders--;
	}

	void TutorialProgress() {
		if (tutorial.tutorialProgression == 6) {
			tutorial.tutorialOrdersCompleted++;
		}
        if (tutorial.tutorialProgression == 13) {
            tutorial.tutorialTestOrdersCompleted++;
        }
    }

	int CheckPizza(IngredientID[] ingredients) {
		bool right = true;
		bool left = true;
		IngredientID[] IDs = new IngredientID[ingredients.Length];


		if (rightOrder.currentOrder) {
			IDs = rightOrder.currentOrder.ingredients;
			right = CompareArrays (ingredients, IDs);

			if (right) 
				return 1;
		}


		if (leftOrder.currentOrder) {
			IDs = leftOrder.currentOrder.ingredients;
			left = CompareArrays (ingredients, IDs);

			if (left)
				return -1;
		}

		return 0;
	}

	bool CompareArrays(IngredientID[] pizza, IngredientID[] order) {
		bool match = true;
		IngredientID id;
		bool noMatch = true;
		bool[] checkList = new bool[pizza.Length];

		for (int i = 0; i < checkList.Length; i++) {
			checkList [i] = false;
		}

		for (int i = 0; i < pizza.Length; i++) {
			id = pizza [i];
			noMatch = true;

			for (int j = 0; j < order.Length; j++) {
				if (!checkList[j] && id == order [j]) {
					checkList [j] = true;
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
