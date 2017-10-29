using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oven : MonoBehaviour {

	public OrderReceiver rightOrder;
	public OrderReceiver leftOrder;
	int numOfMatches = 0;

	public ParticleSystem inputParticlesL;
	ParticleSystem.MainModule inputLmain;
	ParticleSystem.ForceOverLifetimeModule inputLforce;
	public ParticleSystem inputParticlesR;
	ParticleSystem.MainModule inputRmain;
	ParticleSystem.ForceOverLifetimeModule inputRforce;
	int inputSide = 0;

	// Audio Setup
	public AudioSource source;
	public AudioClip successSound;
	public AudioClip failSound;

	ScoreManager scoreManager;
    Tutorial tutorial;

	// Use this for initialization
	void Start () {
        scoreManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<ScoreManager>();
        tutorial = GameObject.FindGameObjectWithTag("GameController").GetComponent<Tutorial>();

		inputLmain = inputParticlesL.main;
		inputLforce = inputParticlesL.forceOverLifetime;

		inputRmain = inputParticlesR.main;
		inputRforce = inputParticlesR.forceOverLifetime;
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

		if (plateScript.transform.position.x >= 0)
			inputSide = 1;
		else
			inputSide = -1;

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
			source.PlayOneShot(successSound, 1.0f);

        } else if (side < 0) {
			// Left side
			Debug.Log ("Recipe Left");
			OrderCompletedActions ();
			leftOrder.OrderCompleted ();
			source.PlayOneShot(successSound, 1.0f);

        } else {
			Debug.Log ("No Match");
			if (rightOrder.currentOrder == null && leftOrder.currentOrder == null) {
				SetInputParticlesIncorrect (Color.white);
			} else {
				SetInputParticlesIncorrect (Color.black);
			}
			if (inputSide == -1) { // Left
				inputParticlesL.time = 0;
				inputParticlesL.Play ();
			} else {
				inputParticlesR.time = 0;
				inputParticlesR.Play ();
			}

			source.PlayOneShot(failSound, 1.0f);
		}
	}

	void OrderCompletedActions() {
		SetInputParticlesCorrect ();
		if (inputSide == -1) {
			inputParticlesL.time = 0;
			scoreManager.AwardPoints (numOfMatches, -1);
			inputParticlesL.Play ();
		} else {
			inputParticlesR.time = 0;
			scoreManager.AwardPoints (numOfMatches, 1);
			inputParticlesR.Play ();
		}
			

		TutorialProgress ();
		OrderManager.numOrders--;
		OrderManager.rampTimer += 1;
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

	void SetInputParticlesCorrect() {
		ParticleSystem.MinMaxCurve newCurve = new ParticleSystem.MinMaxCurve();
		newCurve.constantMin = 5;
		newCurve.constantMax = 10;

		if (inputSide == -1) {
			// Set main module
			inputLmain.startColor = Color.green;
			inputLmain.startSpeed = newCurve;

			// Set force module
			inputLforce.xMultiplier = 10;
			inputLforce.yMultiplier = 10;
			inputLforce.zMultiplier = -10;

		} else {
			inputRmain.startColor = Color.green;
			inputRmain.startSpeed = newCurve;

			// Set force module
			inputRforce.xMultiplier = -10;
			inputRforce.yMultiplier = 10;
			inputRforce.zMultiplier = -10;
		}
	}

	void SetInputParticlesIncorrect(Color particleColour) {
		ParticleSystem.MinMaxCurve newCurve = new ParticleSystem.MinMaxCurve();
		newCurve.constantMin = 1;
		newCurve.constantMax = 6;

		if (inputSide == -1) {
			// Set main module
			inputLmain.startColor = particleColour;
			inputLmain.startSpeed = newCurve;

			// Set force module
			inputLforce.xMultiplier = 10;
			inputLforce.yMultiplier = 5;
			inputLforce.zMultiplier = 0;

		} else {
			inputRmain.startColor = particleColour;
			inputRmain.startSpeed = newCurve;

			// Set force module
			inputRforce.xMultiplier = -10;
			inputRforce.yMultiplier = 5;
			inputRforce.zMultiplier = 0;
		}
	}
}
