/* ResourceGeneratorInteractor.cs - Attached to Player object (especially object with PlayerInventory.cs from Master Inventory)
 * Note: This script uses main camera, hence scene needs main camera.
 * 
 *  Written by: [Trello Username] dannyt8, 13 Feb 2017
 * 	Description: A script that allows the player to interact with resource generators
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceGeneratorInteractor : MonoBehaviour {

	// how far the player can reach
    public float reach = 5.0f;

	[Tooltip("Temporary placements. To be replaced with proper input methods.")]
	public KeyCode harvestKey = KeyCode.J;
	[Tooltip("Temporary placements. To be replaced with proper input methods.")]
	public KeyCode destroyKey = KeyCode.H;

	// Timer UI
	public Image timer;

	// Raycasting
    Ray ray;
    RaycastHit hitInfo;

	// Scripts for reference
    ResourceGenerator generatorScript;
//    Inventory myInventory;

    void Start () {
		// Get Inventory script from player inventory
//        PlayerInventory playerInventory = GetComponent<PlayerInventory> ();
//        myInventory = playerInventory.inventory.GetComponent<Inventory> ();
    }

    // Update is called once per frame
    void Update () {
		// Check if an inventory is open, NOTE: doesn't work, not sure why bool doesn't change
//		if (!Inventory.inventoryOpen) {
//			// Check if player is interacting with resource generator
//			CheckAimAtResourceGenerator ();
//			RayCastAtResourceGenerator ();
//		}
    }

	// Checks if player is looking at a resource generator while pressing the correct button
    void CheckAimAtResourceGenerator () {
        if (Input.GetKey (harvestKey)) {
//			// Do raycast
//            RayCastAtResourceGenerator ();

			// Check if script is not null
            if (generatorScript) {
				// Start the harvest loop
//                generatorScript.HarvestLoop (true, myInventory);
            }
		} else if (Input.GetKeyUp (harvestKey)) {
			// Check if script is not null
            if (generatorScript) {
				// Stop harvest loop and de-reference script
//                generatorScript.HarvestLoop (false);
                generatorScript = null;
            }
        }

		if (Input.GetKey (destroyKey)) {
//			// Do raycast
//            RayCastAtResourceGenerator ();
			// Check if script is not null
            if (generatorScript) {
				// Start the destroy loop
//                generatorScript.DestroyLoop (true);
            }
		} else if (Input.GetKeyUp (destroyKey)) {
			// Check if script is not null
            if (generatorScript) {
				// Stop destroy loop and de-reference script
//                generatorScript.DestroyLoop (false);
                generatorScript = null;
            }
        }
    }

	// Raycasts from main camera centre outwards
    void RayCastAtResourceGenerator () {
		// Create a ray
        ray = Camera.main.ViewportPointToRay (new Vector3 (0.5f, 0.5f, 0));
        Debug.DrawRay (ray.origin, ray.direction); // Debugging

		// Do raycast
        if (Physics.Raycast (ray, out hitInfo, reach)) {
			// get generator script
            generatorScript = hitInfo.collider.GetComponent<ResourceGenerator> ();
			if (generatorScript) {
				CooldownTimerUI ();
			}
        } else {
			// Check if script is not null
            if (generatorScript) {
				// reset the interaction loop and de-reference
                generatorScript.ResetInteract ();
                generatorScript = null;
				// hide timer
				timer.gameObject.SetActive (false);
            }
        }
    }

	// Shows the timer ui if generator is on cooldown
	void CooldownTimerUI() {
		// Check if generator is cooling down
		if (generatorScript.coolingDown) {
			// Show timer
			timer.gameObject.SetActive (true);
			// Set timer
			timer.fillAmount = generatorScript.cooldownTimer / generatorScript.cooldownTime;
		} else {
			// stopped cooling down
			timer.gameObject.SetActive (false);
		}
	}
}