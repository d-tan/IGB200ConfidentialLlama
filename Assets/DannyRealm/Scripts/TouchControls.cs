using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControls : MonoBehaviour {

	// Touch


	// Mouse
	RaycastHit m_RayCast;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Touch ();
		Mouse ();
	}

	void Touch() {
		
	}

	void Mouse() {
		if (Input.GetMouseButton(0)) {
//			Physics.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), out m_RayCast);

			if (m_RayCast.transform != null) {
				
			}

		} else if (Input.GetMouseButtonUp(0)) {
			
		}
	}
}
