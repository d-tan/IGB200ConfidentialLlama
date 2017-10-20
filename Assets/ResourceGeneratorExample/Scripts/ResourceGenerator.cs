/* ResourceGenerator.cs - Attach script to a Resource Generator
 * Can extend class and have custom interactions
 * 
 *  Written by: [Trello Username] dannyt8, 13 Feb 2017
 * Description: A base Resource Generator script to attach to resource generators
 * 
 * Requires: JsonHelper.cs class for serialistion of lists 
 * 		(placed in scripts folder, does not work if it's only in the Editor folder)
 * 
 * Adding new interactions:
 * When adding new interactions you need to be sure this interaction is applicable to all Resource Generators.
 * If not, please create a subclass that extends this class and add the unique functions there.
 * If so, please add the required variables in a neat block and the required functions/methods in the appropiate place.
 * If the interaction is timed (i.e. takes time to complete the interaction) please add a boolean that the interaction is
 *  dependent on and set it to false in the StopAllInteractions() function. This is to prevent multiple interactions
 *  happening at once, but if this is not applicable to your interaction you can skip this. To implement this please 
 *  observe the existing functions
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ResourceGenerator : MonoBehaviour {

	// Cooldown 
	public float cooldownTime = 5.0f;
	[HideInInspector]
	public float cooldownTimer = 0.0f;
	[HideInInspector]
	public bool coolingDown = false;


	// Destroying
	int destroyedResources; // number of resources gained from destroying


	// Interaction
//	public KeyCode harvestKey = KeyCode.E;
	public float holdTime = 2.0f;
	float holdTimer = 0.0f;
	protected bool harvesting = false;
	protected bool destroying = false;

	// Generator Attributes
	public bool renewable = true;
	public int uses = 0;

	// Resources from this generator
	[HideInInspector]
	public List<ResourceMeta> harvestResources;
	[HideInInspector]
	public List<ResourceMeta> destroyResources;

	// Serialisation - Needs to move to a database and reference the database in this script
	string path = "Assets/"; 
	string fileName = "ResourceList";
	Resource[] resourceArray;

//	// UI
//	public Text displayText;
//	public Text collectedResourcesText;
//	public Slider interactTimerSlider;

	void Start() {
//		// Reset UI
//		collectedResourcesText.text = "";
//		displayText.text = "";

		// Deserialise - Not needed if reference to resource database is avaliable
		string fileString = File.ReadAllText (path + fileName + ".json"); // Read file
		resourceArray = JsonHelper.FromJson<Resource> (fileString); // convert to array
	}
		
	void Update() {
		
		CoolDown (); // Cooldown Function
	}

	// ------------------ Timers ------------------

	// Timer function for when player is interacting
	protected bool HoldTimer() {
		holdTimer += Time.deltaTime; // Timer

		// display timer on slider UI
//		interactTimerSlider.value = holdTimer / holdTime;

		// Check if timer is done
		if (holdTimer >= holdTime) {
			holdTimer = 0.0f; // reset timer 
//			interactTimerSlider.value = interactTimerSlider.minValue; // reset slider UI
			return true; 
		}

		return false;

	}

	// Cooldown function, to be called every frame (stick in update function)
	protected void CoolDown() {
		// Check for Coolingdown
		if (coolingDown) {
			cooldownTimer += Time.deltaTime; // Timer
			DisplayUIText ("Cooldown: " + (cooldownTime - cooldownTimer)); // Timer display

			// Check if timer is done
			if (cooldownTimer >= cooldownTime) {
				// Reset timer
				coolingDown = false;
				cooldownTimer = 0.0f;

			}
		}
	}

	// ------------------ Interactions ------------------

//	// Destroying loop, to be called, by player script, every frame when interaction is progressing
//	public void DestroyLoop(bool loop, Inventory playerInventory = null) {
//		// Check if other script wants to loop
//		if (loop) { 
//			// Check if destroying interaction has started
//			if (destroying) {
//				// Check if hold timer is done
//				if (HoldTimer ()) {
//					Destroy (playerInventory); // Commit action
//				}
//			} else { // if not
//				StopAllInteractions();
//				destroying = true;
//				ResetInteract(); // reset interaction
//			}
//			
//		} else {
//			ResetInteract ();
//		}
//	}
//
//	// Harvesting loop, to be called, by player script, every frame when interaction is progressing
//	public void HarvestLoop(bool loop, Inventory playerInventory = null) {
//		// Check if other script wants to loop
//		if (loop) {
//			// Check if cooldown is off
//			if (!coolingDown) {
//				// Check if harvesting interaction has started
//				if (harvesting) {
//					// Check if generator is renewable OR has uses left
//					if (renewable || uses > 0) {
//						// Check if hold timer is done
//						if (HoldTimer ()) {
//							Harvest (playerInventory); // Commit action
//							// Check if generator is not renewable and has uses
//							if (!renewable && uses > 0) {
//								uses -= 1; // decrement uses
//								// display message
////								collectedResourcesText.text += "Generator: " + this.name + " now has " + uses.ToString () + " uses left.\n";
//							}
//						}
//					} else { // generator is no longer harvestable
//						DisplayUIText ("This resource generator has been exhausted of its resources. It can no longer be harvestable.");
//					}
//				} else { // if not
//					StopAllInteractions();
//					harvesting = true;
//					ResetInteract(); // reset interaction
//				}
//
//			}
//		} else {
//			ResetInteract ();
//		}
//	}
//
//	// Harvest action
//	protected void Harvest(Inventory playerInventory) {
////		harvested = true;
//		coolingDown = true; // start cooling down
//		SideEffects (); // give side effects if any
//
//		GiveResources (playerInventory, harvestResources); // give the player resources
//		// Display message
////		DisplayUIText ("Harvested " + harvestedResources + " of " + myResource.name);
//	}
//
//	// Destory action
//	protected void Destroy(Inventory playerInventory) {
//		// Check if cooling down
//		if (!coolingDown) {
//			// Display message
//			DisplayUIText ("You cannot destroy me!");
//			SideEffects (); // give side effects if any
//			GiveResources(playerInventory, destroyResources);
//		} else {
//			// Display message
////			collectedResourcesText.text += "You get nothing for destroying while on cooldown\n";
//		}
//	}
//
//	// Side effects applied to the player
//	protected virtual void SideEffects() {
////		collectedResourcesText.text += "You have recieved a side effect from this resource generator.\n";
//	}
//
//	// Give resources to the player
//	protected virtual void GiveResources(Inventory playerInventory, List<ResourceMeta> resourcesList) {
//		
//		if (playerInventory) {
//			// Loop through each item
//			for (int i = 0; i < resourcesList.Count; i++) {
//				// Check if item already exists
//				if (!playerInventory.checkIfItemAllreadyExist (resourcesList[i].resource.id, resourcesList[i].RandomCollect())) {
//					playerInventory.addItemToInventory (resourcesList[i].resource.id, resourcesList[i].amountCollected);
//					playerInventory.stackableSettings (); // redo stack ui
//				}
////				collectedResourcesText.text += "+" + resourcesList[i].amountCollected + " " + resourcesList[i].resource.name + "\n";
//			}
//		}
//	}

	// Resets the holt timer and slider UI
	public void ResetInteract() {
		holdTimer = 0.0f;
//		interactTimerSlider.value = interactTimerSlider.minValue;
	}

	// Stops all interactions by setting booleans false. Mainly used for timed interactions
	protected void StopAllInteractions() {
		destroying = false;
		harvesting = false;
	}

	// ------------------ UI ------------------

	// Display give text to UI
	protected void DisplayUIText(string message) {
//		displayText.text = message;
	}
}

// Hold extra information about Resources specific to resource generators
[System.Serializable]
public class ResourceMeta {
	public int index; // for popup
	public Resource resource;
	public int min = 1;
	public int max = 11;
	public int amountCollected = 1;

	public int RandomCollect() {
		amountCollected = Random.Range (min, max);

		return amountCollected;
	}
}

// Resource Type Class
[System.Serializable]
public class Resource {
	public int id = 0;
	public string name = "";
	public string description = "";
	public string source = "";
}
