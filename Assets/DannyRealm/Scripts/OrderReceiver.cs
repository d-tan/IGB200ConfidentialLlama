using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderReceiver : MonoBehaviour {

	public Order currentOrder;
	Vector3 parentPos;

	Dictionary<Collider, Order> colliding = new Dictionary<Collider, Order>();
	Collider[] colliderKeys;

	void Start() {
		parentPos = transform.parent.position;
	}

	void InitialiseOrder() {
		if (currentOrder) {
			currentOrder.rb.velocity = new Vector3 (0, 0, 0);
			currentOrder.transform.position = parentPos;
			currentOrder.myCollider.enabled = false;

			OrderManager.RemoveOrder (currentOrder);
		}
	}

	void OnTriggerEnter(Collider other) {
		if (currentOrder == null && other.CompareTag("Order")) {
			Order script = other.GetComponent<Order> ();

			if (script.beingHeld) {
				if (!colliding.ContainsKey(other))
					colliding.Add (other, script);
			} else {
				currentOrder = script;
				InitialiseOrder ();
			}
		}
	}

	void OnTriggerStay(Collider other) {
		if (currentOrder == null && other.CompareTag("Order")) {

			// Check if dictionary has collider
			if (colliding.ContainsKey(other)) {
				// Check if object is still being held
				if (!colliding[other].beingHeld) {
					currentOrder = colliding [other];
					colliding.Remove (other);
					InitialiseOrder ();
				}
					
			} else {
				// Add Collider to dictionary
				Order script = other.GetComponent<Order> ();

				if (script.beingHeld) {
					if (!colliding.ContainsKey(other))
						colliding.Add (other, script);
				} else {
					currentOrder = script;
					InitialiseOrder ();
				}
			}


		}
	}

	void OnTriggerExit(Collider other) {
		if (other.CompareTag("Order") && colliding.ContainsKey(other)) {
			colliding.Remove (other);
		}
	}
}
