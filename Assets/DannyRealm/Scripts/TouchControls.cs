using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControls : MonoBehaviour {

	public bool mouseControl = false;

	// Touch
	const int numTouches = 4;
	Touch currentTouch;
	RaycastHit[] t_Raycasts = new RaycastHit[numTouches];
	public GameObject[] t_HeldObjects = new GameObject[numTouches];
	Vector3[] t_fingerPos = new Vector3[numTouches];
	bool[] t_stationary = new bool[numTouches];
	Vector3 resetPos;

	// Flick
	Vector3[] t_velocity = new Vector3[numTouches];
	Vector3[] t_prevPos = new Vector3[numTouches];
	float minVelSqr = 1.5f;
	float flickVelMultiplier = 7f;

	// Mouse
	RaycastHit m_RayCast;
	GameObject m_HeldObject;
	Vector3 m_mousePos;

	void Start() {
		for (int i = 0; i < t_stationary.Length; i++) {
			t_stationary [i] = true;
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (mouseControl)
			Mouse ();
		
		Touch ();

	}

	void Touch() {

//		foreach (Touch touch in Input.touches) {
		int touchCount = Input.touchCount;
		for (int i = 0; i < touchCount; i++) {
			currentTouch = Input.GetTouch (i);
			if (currentTouch.fingerId < numTouches) {
				
				switch (currentTouch.phase) {
				case TouchPhase.Began:
					InitialiseTouch (currentTouch);
					break;

				case TouchPhase.Moved:
					DragTouch (currentTouch);
					break;

				case TouchPhase.Stationary:
					StationaryTouch (currentTouch);
					break;

				case TouchPhase.Ended:
					FinishTouch (currentTouch);
					break;

				case TouchPhase.Canceled:
					FinishTouch (currentTouch);
					break;
				}
			}

		}

	}

	/// <summary>
	/// Initialises the touch to be ready for interaction
	/// </summary>
	/// <param name="touch">Touch.</param>
	void InitialiseTouch(Touch touch) {
		if (Physics.Raycast(Camera.main.ScreenPointToRay(touch.position), out t_Raycasts[touch.fingerId])) {

			if (t_HeldObjects[touch.fingerId] == null && t_Raycasts[touch.fingerId].transform.CompareTag("PickUp")) {

				if (!CheckAlreadyHolding (t_Raycasts [touch.fingerId].transform.gameObject)) {
					// Store gameObject
					t_HeldObjects [touch.fingerId] = t_Raycasts [touch.fingerId].transform.gameObject;

					// Zero the velocity
					t_HeldObjects [touch.fingerId].GetComponent<Rigidbody> ().velocity = Vector3.zero;

					// Change the local scale
					t_HeldObjects [touch.fingerId].transform.localScale *= 2;

					// Store the position for calculating velocity
					t_prevPos [touch.fingerId] = t_HeldObjects [touch.fingerId].transform.position;
				}
			}
		}
	}

	/// <summary>
	/// Moves the object that is held
	/// </summary>
	/// <param name="touch">Touch.</param>
	void DragTouch(Touch touch) {
		if (t_HeldObjects[touch.fingerId] != null) {
			t_fingerPos [touch.fingerId] = Camera.main.ScreenToWorldPoint (new Vector3 (touch.position.x, touch.position.y, 10));
			t_fingerPos [touch.fingerId].y = 0;

			t_HeldObjects [touch.fingerId].transform.position = t_fingerPos [touch.fingerId];

			if (t_stationary [touch.fingerId])
				t_stationary [touch.fingerId] = false;

			CalculateVelocity (touch.fingerId, t_fingerPos[touch.fingerId]);
		}
	}

	void StationaryTouch(Touch touch) {
		if (!t_stationary [touch.fingerId])
			t_stationary [touch.fingerId] = true;
		
		CalculateVelocity (touch.fingerId, t_fingerPos[touch.fingerId]);
	}

	void CalculateVelocity(int touchId, Vector3 pos) {
		t_velocity [touchId] = pos - t_prevPos [touchId];
		t_prevPos[touchId] = pos;
	}

	/// <summary>
	/// Concludes the touch and stops dragging
	/// </summary>
	/// <param name="touch">Touch.</param>
	void FinishTouch(Touch touch) {
		if (t_HeldObjects[touch.fingerId] != null) {
			t_HeldObjects [touch.fingerId].transform.localScale = Vector3.one;

			if (t_velocity[touch.fingerId].sqrMagnitude > minVelSqr) {
				t_HeldObjects [touch.fingerId].GetComponent<Rigidbody> ().velocity = t_velocity [touch.fingerId] * flickVelMultiplier;
			}

			Debug.Log ("Flick: " + !t_stationary [touch.fingerId] + " counted: " + (t_velocity[touch.fingerId].sqrMagnitude > minVelSqr) + " vel: " + t_velocity[touch.fingerId].sqrMagnitude);


			if (!t_stationary [touch.fingerId])
				t_stationary [touch.fingerId] = true;

			t_HeldObjects [touch.fingerId] = null;
		}
	}

	bool CheckAlreadyHolding(GameObject obj) {
		for (int i = 0; i < t_HeldObjects.Length; i++) {

			if (obj.Equals (t_HeldObjects [i]))
				return true;
		}

		return false;
	}

	/// <summary>
	/// Mouse controls for the game
	/// </summary>
	void Mouse() {

		if (Input.GetMouseButton(0)) {

			m_mousePos = Camera.main.ScreenToWorldPoint (new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
			m_mousePos.y = 0f;

			if (m_HeldObject)
				m_HeldObject.transform.position = m_mousePos;
			
			if(Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out m_RayCast)) {

				Debug.DrawLine (Camera.main.ScreenPointToRay (Input.mousePosition).origin, Camera.main.ScreenPointToRay (Input.mousePosition).direction * 10);

				if (m_HeldObject == null && m_RayCast.transform.CompareTag ("PickUp")) {

					m_HeldObject = m_RayCast.transform.gameObject;
				}

			}

		} else if (Input.GetMouseButtonUp(0)) {
			m_HeldObject = null;
		}
	}
}
