using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderReceiver : MonoBehaviour {

	public Order currentOrder;
	public GameObject orderUI;
	public Image clipboard;
	public Sprite[] clipboardSprites = new Sprite[2];
	public Image[] ingredientSprite = new Image[4];
	public Sprite[] sprites = new Sprite[6];
	Vector3 parentPos;

	Dictionary<Collider, Order> colliding = new Dictionary<Collider, Order>();
	Collider[] colliderKeys;

	void Start() {
		parentPos = transform.parent.position;
		parentPos.y += 0.05f;
	}

	public void OrderCompleted() {
		Destroy (currentOrder.gameObject);
		currentOrder = null;

		clipboard.sprite = clipboardSprites [0];

		for (int i = 0; i < ingredientSprite.Length; i++) {
			ingredientSprite [i].sprite = sprites [0];
		}
	}

	void InitialiseOrder() {
		if (currentOrder) {
			currentOrder.rb.velocity = new Vector3 (0, 0, 0);
			currentOrder.transform.position = parentPos;
			currentOrder.myCollider.enabled = false;
			currentOrder.gameObject.SetActive (false);
			OrderManager.RemoveOrder (currentOrder);

			clipboard.sprite = clipboardSprites [1];

			// Display Recipe
			for (int i = 0; i < ingredientSprite.Length; i++) {
				for (int j = 0; j < sprites.Length; j++) {
					if (currentOrder.ingredients[i].ToString() == sprites[j].name) {
						ingredientSprite [i].sprite = sprites [j];
					}
				}
			}

		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Order")) {
			Order script = other.GetComponent<Order> ();
			Debug.Log("Order detected");
			if (currentOrder == null) {
				script.returnMe = false;
				Debug.Log("I don't have an order");
				if (script.beingHeld) {
					Debug.Log("It's being held");
					if (!colliding.ContainsKey (other))
						colliding.Add (other, script);
				} else {
					currentOrder = script;
					InitialiseOrder ();
					Debug.Log ("storing order");
				}
			} else {
				script.returnMe = true;
			}
		}
	}

	void OnTriggerStay(Collider other) {
		if (other.CompareTag("Order")) {
			if (currentOrder == null) {
				// Check if dictionary has collider
				if (colliding.ContainsKey (other)) {
					colliding [other].returnMe = false;

					// Check if object is still being held
					if (!colliding [other].beingHeld) {
						currentOrder = colliding [other];
						colliding.Remove (other);
						InitialiseOrder ();
					}
					
				} else {
					// Add Collider to dictionary
					Order script = other.GetComponent<Order> ();
					script.returnMe = false;

					if (script.beingHeld) {
						if (!colliding.ContainsKey (other))
							colliding.Add (other, script);
					} else {
						currentOrder = script;
						InitialiseOrder ();
					}
				}
			} else {
				if (colliding.ContainsKey (other))  {
					colliding [other].returnMe = true;
				}
			}

		}
	}

	void OnTriggerExit(Collider other) {
		if (other.CompareTag("Order") && colliding.ContainsKey(other)) {
			colliding [other].returnMe = true;
			colliding.Remove (other);
		}
	}
}
