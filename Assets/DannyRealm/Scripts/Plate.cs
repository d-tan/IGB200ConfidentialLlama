using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IngredientID {
	None,
	Sauce,
	Cheese,
	Ham,
	Pineapple,
	Onion
}

public class Plate : MonoBehaviour {

	Collider myCollider;
	public Throwable throwScript;
	float colliderHeight = 0f;

	// Ingredients
	const int numOfIngredients = 4;
	public Ingredient[] ingredients = new Ingredient[numOfIngredients];
//	public Transform[] positions = new Transform[numOfIngredients];
	public GameObject topping;
	Vector3 toppingOffset = new Vector3(0, 0.12f, 0);
	Vector3 toppingSpacing = new Vector3 (0, 0.01f, 0);
	int currentToppingLayer = 2;

	// Use this for initialization
	void Start () {
		myCollider = GetComponent<Collider> ();
		throwScript = GetComponent<Throwable> ();

		colliderHeight = myCollider.bounds.extents.y;
	}

	public void IngredientTrigger(Ingredient script) {
		IngredientID id = IngredientID.None;

		if (CheckContainsIngredient (script.ingredientID))
			return;

		for (int i = 0; i < ingredients.Length; i++) {
			if (ingredients[i] == null) {
				Debug.Log ("Ingredient added");
				ingredients [i] = script;
				script.OnPlate ();
				script.canvas.SetActive (false); // turn off canvas for the ingredient
				throwScript.degradeTimer = 0f; // reset degrade timer

				script.transform.parent = this.transform;

				script.transform.localPosition = new Vector3(0, 0.1f, 0);

				script.transform.localPosition += new Vector3(0, 0.1f * i, 0);

				script.gameObject.SetActive (false);

				id = script.ingredientID;

				break;
			}
		}

		if (id == IngredientID.Sauce) {
			SpawnTopping (toppingOffset, id);
		} else if (id == IngredientID.Cheese) {
			SpawnTopping (toppingOffset + toppingSpacing, id);
		} else {
			SpawnTopping (toppingOffset + (toppingSpacing * currentToppingLayer), id);
			currentToppingLayer++;
		}
	}

	void SpawnTopping(Vector3 relativePos, IngredientID id) {
		GameObject newTopping = Instantiate (topping, transform);
//		Debug.Log (newTopping.transform.position + " " + relativePos);
		newTopping.transform.localPosition = relativePos;
		newTopping.GetComponent<Topping> ().AssignMaterial (id);
	}

	public bool CheckIfFull() {
		for (int i = 0; i < ingredients.Length; i++) {
			if (ingredients [i] == null)
				return false;
		}

		return true;
	}

	public bool CheckContainsIngredient(IngredientID id) {
		for (int i = 0; i < ingredients.Length; i++) {
			if (ingredients[i]) {
				if (ingredients [i].ingredientID == id)
					return true;
			}
		}

		return false;
	}
}
