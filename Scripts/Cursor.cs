using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour {


	CameraRaycaster cameraRaycaster;

	void Start () {
		cameraRaycaster = GetComponent<CameraRaycaster> ();
	}

	void Update() {
		if (Input.GetMouseButton (0)) {
			//Debug.Log (cameraRaycaster.layerHit);
		}
	}


}
