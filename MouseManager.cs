﻿using UnityEngine;
using System.Collections;

public class MouseManager : MonoBehaviour {

	public GameObject selectedObject;

	// Update is called once per frame
	void Update () {
		Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );

		RaycastHit hitInfo;

		if( Physics.Raycast( ray, out hitInfo ) ) {
			//			Debug.Log("Mouse is over: " + hitInfo.collider.name );

			// The collider we hit may not be the "root" of the object
			// You can grab the most "root-est" gameobject using
			// transform.root, though if your objects are nested within
			// a larger parent GameObject (like "All Units") then this might
			// not work.  An alternative is to move up the transform.parent
			// hierarchy until you find something with a particular component.

			GameObject hitObject = hitInfo.transform.root.gameObject;
			if (Input.GetMouseButtonDown(0)) {
				Debug.Log ("HitObj: " + hitObject.name);
				SelectObject(hitObject);
			}
		}
		else {
			ClearSelection();
		}

	}


  //Used to set the colors of units to green to let user know that a unit has been selected.
	void SelectObject(GameObject obj) {
		if(selectedObject != null) {
			if(obj == selectedObject)
				return;

			ClearSelection();
		}

		selectedObject = obj;


		Renderer[] rs = selectedObject.GetComponentsInChildren<Renderer>();
		foreach(Renderer r in rs) {
			Material m = r.material;
			m.color = Color.green;
			r.material = m;
		}
	}

	void ClearSelection() {
		if(selectedObject == null)
			return;

		Renderer[] rs = selectedObject.GetComponentsInChildren<Renderer>();
		foreach(Renderer r in rs) {
			Material m = r.material;
			m.color = Color.white;
			r.material = m;
		}


		selectedObject = null;
	}
}
