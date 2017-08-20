using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{

	[SerializeField] float walkMoveStopRadius = 0.2f;

	ThirdPersonCharacter character;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentClickTarget;

	private bool isInDirectMode = false; //TODO: Consider making static.


    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        character = GetComponent<ThirdPersonCharacter>();
        currentClickTarget = transform.position;
    }

    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
		if (Input.GetKeyDown (KeyCode.G)) { 
			isInDirectMode = !isInDirectMode;
			currentClickTarget = transform.position;

		}

		if (isInDirectMode) {
			ProcessDirectMovement ();

		} else {
			ProcessMouseMovement ();

		}
			
    }

	private void ProcessDirectMovement(){
		//print ("Direct movement.");

		float h = Input.GetAxis ("Horizontal");
		float v = Input.GetAxis ("Vertical");


		Vector3 camForward = Vector3.Scale (Camera.main.transform.forward, new Vector3 (1, 0, 1)).normalized;
		Vector3 move = v * camForward + h * Camera.main.transform.right;

		character.Move (move, false, false);
	}

	private void ProcessMouseMovement(){

		//print ("Mouse movement.");
		if (Input.GetMouseButton(0))
		{
			//print("Cursor raycast hit" + cameraRaycaster.hit.collider.gameObject.name.ToString());

			switch (cameraRaycaster.layerHit) {
			case Layer.Walkable:
				currentClickTarget = cameraRaycaster.hit.point; 
				break;
			case Layer.Enemy:
				//print ("Not moving to enemy.");
				break;
			default:
				//print ("Shouldn't be here!");
				return;
			}
		}
		var playerToClickPoint = currentClickTarget - transform.position;
		if (playerToClickPoint.magnitude >= walkMoveStopRadius) {
			character.Move (playerToClickPoint, false, false);
		} else {
			character.Move (Vector3.zero, false, false);

		}
	}
}

