using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateTrigger : MonoBehaviour {

	public Plate parent;
	Dictionary<Collider, Ingredient> colliding = new Dictionary<Collider, Ingredient>();
	Collider[] colliderKeys;

	void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Ingredient")) {
			Debug.Log ("Inredient detected");

			if (other.transform.parent == null) {
				Debug.Log ("Ingredient has no parent");

				if (!parent.CheckIfFull ()) {
					Ingredient ingredient = other.GetComponent<Ingredient> ();

					if (!parent.throwScript.beingHeld && ingredient.beingHeld == false) {
						parent.IngredientTrigger (ingredient);
					} else {
						Debug.Log ("Still being held");
						if (!colliding.ContainsKey(other)) {
							colliding.Add (other, ingredient);
						}
					}
				} else {
					Debug.Log ("Plate is already full");
				}
			}
				
		}
	}

	void OnTriggerStay(Collider other) {

		if(!parent.throwScript.beingHeld && colliding.ContainsKey(other) && !colliding[other].beingHeld) {
			Debug.Log ("Colliding and not being held");
			parent.IngredientTrigger (colliding[other]);
			colliding.Remove (other);
		}

	}

	void OnTriggerExit(Collider other) {
		colliding.Remove (other);
	}
}
