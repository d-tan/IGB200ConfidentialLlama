using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControls : MonoBehaviour {

	// Touch
	const int numTouches = 4; 
	RaycastHit[] t_Raycasts = new RaycastHit[numTouches];
	GameObject[] t_HeldObjects = new GameObject[numTouches];
	Vector3[] t_fingerPos = new Vector3[numTouches];

	// Mouse
	RaycastHit m_RayCast;
	GameObject m_HeldObject;
	Vector3 m_mousePos;
	
	// Update is called once per frame
	void Update () {
		Touch ();
		Mouse ();

	}

	void Touch() {

		foreach (Touch touch in Input.touches) {
			
			if (touch.fingerId < numTouches) {
				
				switch (touch.phase) {
				case TouchPhase.Began:
					InitialiseTouch (touch);
					break;

				case TouchPhase.Moved:
					DragTouch (touch);
					break;

				case TouchPhase.Stationary:
					break;

				case TouchPhase.Ended:
					FinishTouch (Touch);
					break;

				case TouchPhase.Canceled:
					FinishTouch (Touch);
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

				t_HeldObjects [touch.fingerId] = t_Raycasts [touch.fingerId].transform.gameObject;
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
		}
	}

	/// <summary>
	/// Concludes the touch and stops dragging
	/// </summary>
	/// <param name="touch">Touch.</param>
	void FinishTouch(Touch touch) {
		if (t_HeldObjects[touch.fingerId] != null) {

			t_HeldObjects [touch.fingerId] = null;
		}
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
