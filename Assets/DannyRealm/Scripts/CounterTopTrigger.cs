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

					if (ingredient.beingHeld) {
						if (!colliding.ContainsKey(other))
							colliding.Add (other, ingredient);

					} else {
						parent.HoldObject (other);
					}
				}
			} else if (other.CompareTag("PickUp")) {
				Throwable pickUp = other.GetComponent<Throwable> ();

				if (pickUp.beingHeld) {
					if (!colliding.ContainsKey(other))
						colliding.Add (other, pickUp);

				} else {
					parent.HoldObject (other);
				}
			}
		}
	}

	void OnTriggerStay(Collider other) {
		if(parent.currentlyHolding == null && colliding.ContainsKey(other) && !colliding[other].beingHeld) {
			Debug.Log ("Colliding and not being held");
			parent.HoldObject (other);
//			colliding.Remove (other);
		}

		if (parent.currentlyHolding != null && parent.currentlyHolding.Equals(other)) {

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
