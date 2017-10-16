using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Topping : MonoBehaviour {

	public MeshRenderer meshRenderer;

	public Material[] toppings;

	void Start() {
//		meshRenderer = GetComponent<MeshRenderer> ();
		Debug.Log (meshRenderer);
	}

	public void AssignMaterial(IngredientID ingredient) {
		meshRenderer.material = toppings [(int)ingredient];
	}
}
