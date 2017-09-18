using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour {

	private static OrderManager singleton;

	// Spawning
	public GameObject orderObject;
	public Vector3 spawnPos = new Vector3();
	public Vector3 lastPos = new Vector3();
	GameObject spawnedObject;

	public bool moveOrders = true;
	float orderSpacing = 1.5f;
	float beltSpeed = 3.0f;
	static List<Order> ordersList = new List<Order>();
	static List<Vector3> virtualPos = new List<Vector3>();

	void Awake() {
		// Create singleton
		if (singleton == null)
			singleton = this;
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.P)) {
			CreateOrder ();
		}
		MoveUpOrders ();
	}

	public void CreateOrder() {
		// Spawn order
		spawnedObject = Instantiate (orderObject, spawnPos, Quaternion.identity) as GameObject;
		Order script = spawnedObject.GetComponent<Order> ();

		// Add to list
		ordersList.Add (script);
		virtualPos.Add (spawnPos);

		RandomiseOrderIngredients (script);

	}

	/// <summary>
	/// Creates an Order with the specified ingredients
	/// </summary>
	/// <param name="ingredients">Array of ingredients. Must be less than or equal to 4.</param>
	public void CreateOrder(IngredientID[] ingredients) {
		// Spawn Order
		spawnedObject = Instantiate (orderObject, spawnPos, Quaternion.identity) as GameObject;
		Order script = spawnedObject.GetComponent<Order> ();

		// Add to list
		ordersList.Add (script);
		virtualPos.Add (spawnPos);

		// Assign ingredients to order
		for (int i = 0; i < ingredients.Length; i++) {
			script.ingredients [i] = ingredients [i];
		}
	}

	void RandomiseOrderIngredients(Order script) {

		// Randomise Order
	}

	void MoveUpOrders() {
		if (moveOrders) {
			for (int i = 0; i < ordersList.Count; i++) {

				// Move virtual postiion
				if (virtualPos [i].z < lastPos.z * (i + 1) - orderSpacing * i) {
					virtualPos [i] += new Vector3 (0, 0, 1) * beltSpeed * Time.deltaTime;
				} else {
					virtualPos [i] = lastPos * (i + 1) - new Vector3 (0, 0, orderSpacing * i);
				}

				// if not being held, move up
				if (!ordersList [i].beingHeld && ordersList [i].returnMe) {
					ordersList [i].transform.position = virtualPos [i];
				}
			}
		}
	}

	public static void RemoveOrder(Order order) {
		int index = ordersList.IndexOf (order);
		virtualPos.RemoveAt (index);
		ordersList.Remove (order);
	}
}
