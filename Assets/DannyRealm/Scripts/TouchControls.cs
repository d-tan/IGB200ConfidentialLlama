using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControls : MonoBehaviour {

	// Touch


	// Mouse
	RaycastHit m_RayCast;
	GameObject m_HeldObject;
	Vector3 mousePos;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Touch ();
		Mouse ();

		mousePos = Camera.main.ScreenToWorldPoint (new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
		mousePos.y = 0f;

		if (m_HeldObject)
			m_HeldObject.transform.position = mousePos;


	}

	void Touch() {

	}

	void Mouse() {
		if (Input.GetMouseButton(0)) {
			
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
