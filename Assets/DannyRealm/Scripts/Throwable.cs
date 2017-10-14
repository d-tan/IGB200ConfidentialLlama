//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Throwable : MonoBehaviour {

	public Collider myCollider;
	public Rigidbody rb;
	protected MeshRenderer myRenderer;
	public float minVel = 0.08f;

	public bool beingHeld = false;
	public float heldTimer = 0f;
	public Transform prevParent;

	// Degradation
	public float degradeTime = 5f;
	public float degradeTimer = 0f;
	public Image circle;
	public GameObject canvas;

	// Meta
	public bool isIngredient = false;
	public bool flicked = false;
	public int side = 0;

	void Awake() {
		myRenderer = GetComponent<MeshRenderer> ();
	}

	// Use this for initialization
	void Start () {
		myCollider = GetComponent<Collider> ();
		rb = GetComponent<Rigidbody> ();

	}

	void Update() {
		CheckVelocity ();
		Degrade ();

		if (beingHeld)
			heldTimer += Time.deltaTime;
	}

	protected virtual void Degrade() {
		if (!beingHeld && !flicked && transform.parent == null) {
			if (!canvas.activeSelf) {
				canvas.SetActive (true);
			}

			if (degradeTimer < degradeTime) {
				degradeTimer += Time.deltaTime;

				float percent = 1 - degradeTimer / degradeTime;
				circle.fillAmount = percent;
				circle.color = new Color (1.5f - percent, percent, 0f, 1);

			} else {
				Destroy (this.gameObject);
			}

		}
	}

	public void StoreParent() {
		if (transform.parent)
			prevParent = transform.parent;
	}

	void CheckVelocity() {
		if (flicked && rb.velocity.sqrMagnitude <= minVel) {
			// move back
//			if (prevParent)
//				transform.position = new Vector3 (prevParent.position.x, 0f, prevParent.position.z);
//			else
//				Destroy (this.gameObject);

			// Reset counter top


			flicked = false;
		}
	}

	void OnCollisionEnter(Collision col) {
		if (!beingHeld && col.transform.CompareTag ("Wall")) {
			side = 0;
			Debug.Log ("Hit a wall");
		} else if (!this.CompareTag("Order") && col.transform.CompareTag("Waiter")) {
			Debug.Log ("Hit waiter");
			Destroy (this.gameObject);
		}

	}

	public void ToggleRender(bool state) {
		
		myRenderer.enabled = state;
	}
}
