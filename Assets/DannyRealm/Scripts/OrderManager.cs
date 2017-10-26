using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour {

	private static OrderManager singleton;

	// Spawning
	[Header("Spawning")]
	public GameObject orderObject;
	public Vector3 spawnPos = new Vector3();
	public Vector3 lastPos = new Vector3();
	GameObject spawnedObject;

	[Header("Conveyor Belt")]
	public bool moveOrders = true;
	float orderSpacing = 1f;
	float beltSpeed = 5.0f;
	[HideInInspector]
	public static int numOrders = 0;
	const int maxOrders = 10;

	static List<Order> ordersList = new List<Order>();
	static List<Vector3> virtualPos = new List<Vector3>();

	// Order generation
	[Header("Order generation")]
	Tutorial tutorialScript;
	float spawnTime = 7f;
	float timerVariation = 2f;
	float spawnTimer = 0f;

	// Order randomiser
	int numOfIngredients;
	List<IngredientID> availableIngredients = new List<IngredientID>();

	// Difficulty curve
	float variationStep = 0.2f;
	float spawnTimeStep = 1.2f;
	float rampTimeMultiplier = 0.1f;
	float rampTime = 2.5f;
	float rampTimeStep = 2f;
	float rampTimeStepStep = 0.5f;
	public static float rampTimer = 0f;
	public float displayRampTimer = 0f;
	int lowerBoundComplex = 0;
	int uppwerBoundComplex = 1;
	int rampStage = 0;

	void Awake() {
		// Create singleton
		if (singleton == null)
			singleton = this;
	}

	void Start() {
		tutorialScript = GetComponent<Tutorial> ();
		spawnTimer = 0;
		numOrders = 0;

		// Get Number of Ingredients in the game
		numOfIngredients = System.Enum.GetNames (typeof(IngredientID)).Length;
		ordersList.Clear ();
		virtualPos.Clear ();
	}

	void Update() {

		if (tutorialScript.completedTutorial) 
			OrderSpawner ();

		MoveUpOrders ();

		RampStages ();
	}

	public void CreateOrder() {
		// Spawn order
		spawnedObject = Instantiate (orderObject, spawnPos, Quaternion.identity) as GameObject;
		Order script = spawnedObject.GetComponent<Order> ();

		// Add to list
		ordersList.Add (script);
		virtualPos.Add (spawnPos);

		script.ingredients = RandomiseOrderIngredients ();

		numOrders++;
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

		numOrders++;
	}

	IngredientID[] RandomiseOrderIngredients(int arrayLength = 4) {
		// Randomise Number of ingredients in the order
		int ingredientCount = Random.Range (lowerBoundComplex, Mathf.Clamp(arrayLength - 1 - uppwerBoundComplex, 1, 3));
		IngredientID[] ingredientsList = new IngredientID[arrayLength]; // Create array for ingredients
		
		// Clear list and Initialise list
		availableIngredients.Clear ();
		for (int i = 0; i < numOfIngredients; i++) {
			IngredientID id = (IngredientID)i;
			if (id != IngredientID.None && id != IngredientID.Sauce && id != IngredientID.Cheese)
				availableIngredients.Add (id);
		}

		// Initialise Order
		for (int i = 0; i < arrayLength; i++) {
			ingredientsList [i] = IngredientID.None;
		}
		ingredientsList [0] = IngredientID.Sauce;
		ingredientsList [1] = IngredientID.Cheese;

		// Randomise Order
		for (int i = 0; i < ingredientCount; i++) {
			int availableCount = availableIngredients.Count;
			int chosenIndex = -1;

			// Check if there are any ingredients left
			if (availableCount > 0) {
				// Pick and Ingredient
				chosenIndex = Random.Range (0, availableCount);
				ingredientsList [i + 2] = availableIngredients [chosenIndex]; // Add ingredient

				availableIngredients.RemoveAt (chosenIndex); // Remove ingredient availability
			}
		}

		return ingredientsList;
	}

	void OrderSpawner() {
		if (numOrders < maxOrders)
			spawnTimer -= Time.deltaTime;

		if (spawnTimer <= 0) {
			spawnTimer = Random.Range (spawnTime - timerVariation, spawnTime + timerVariation);

			// Spawn Order
			CreateOrder ();
		}


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

	void RampStages() {
		rampTimer = Mathf.Clamp (rampTimer - Time.deltaTime * rampTimeMultiplier, 0, 30);
		displayRampTimer = rampTimer;

		if (rampTimer >= rampTime) {
			Ramp ();
			rampTime += rampTimeStep;
			rampTimeStep += rampTimeStepStep;
		}
	}

	void Ramp() {
		rampStage++;

		if (lowerBoundComplex < 3)
			lowerBoundComplex += Mathf.Clamp (rampStage % 2 - 1, 0, 1);
		if (uppwerBoundComplex > 0)
			uppwerBoundComplex -= rampStage % 2;
	}
}
