using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterTopTrigger : MonoBehaviour {

	public CounterTop parent;

	Dictionary<Collider, Throwable> colliding = new Dictionary<Collider, Throwable>();
	Collider[] colliderKeys;

	void Start() {
		parent = transform.parent.GetComponent<CounterTop> ();
	}

	void OnTriggerEnter(Collider other) {
		if (parent.currentlyHolding == null) {
			if (other.CompareTag ("Ingredient")) {
				if (other.transform.parent == null) {
					Throwable ingredient = other.GetComponent<Throwable> ();

					// If object is being (flicked AND on the same side as this) OR is being held
					if ((ingredient.flicked && ingredient.side == parent.side) || ingredient.beingHeld) {
						if (!colliding.ContainsKey(other))
							colliding.Add (other, ingredient);

					} else {
						parent.HoldObject (other, ingredient);
					}
				}
			} else if (other.CompareTag("PickUp") || other.CompareTag("Plate")) {
				Throwable pickUp = other.GetComponent<Throwable> ();

				if ((pickUp.flicked && pickUp.side == parent.side) || pickUp.beingHeld) {
					if (!colliding.ContainsKey(other))
						colliding.Add (other, pickUp);

				} else {
					parent.HoldObject (other, pickUp);
				}
			}
		}
	}

	void OnTriggerStay(Collider other) {
		if(parent.currentlyHolding == null && colliding.ContainsKey(other) && !colliding[other].beingHeld) {
			Throwable script = colliding [other];
			if (script.side != parent.side && script.flicked || script.side == parent.side && !script.flicked)
				parent.HoldObject (other, colliding[other]);
//			colliding.Remove (other);
		}

		// If the collider is the one we currently have
		if (parent.currentlyHolding != null && parent.currentlyHolding.Equals(other)) {

			// If it's in the dictionary and the collider is being picked up
			if (colliding.ContainsKey(other) && colliding[other].beingHeld) {
				parent.currentlyHolding = null;

			}
		}
	}

	void OnTriggerExit(Collider other) {
		if (parent.currentlyHolding != null && parent.currentlyHolding.Equals(other)) {
			parent.currentlyHolding = null;
		}
		colliding.Remove (other);
	}
}
