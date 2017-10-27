using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour {

	public static ParticleManager singleton;

	// Splat Particles
	public ParticleSystem[] splats;

	void Awake() {
		if (singleton == null)
			singleton = this;
	}

	public void DoSplat(Vector3 pos, Ingredient[] ingredients) {

		for (int i = 0; i < ingredients.Length; i++) {
			if (ingredients [i])
				SetupParticle (pos, ingredients [i].ingredientID);
		}
	}

	public void DoSplat(Vector3 pos, IngredientID id) {

		splats [(int)id].transform.position = pos;
		splats [(int)id].Play ();

	}

	void StopSplatParticles() {
		for (int i = 0; i < splats.Length; i++) {
			splats [i].Stop ();
		}
	}

	void SetupParticle(Vector3 pos, IngredientID id) {

		splats [(int)id].transform.position = pos;
		splats [(int)id].Simulate (0);
		splats [(int)id].Play ();

	}
}
