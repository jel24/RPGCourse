using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{

	[SerializeField] float walkMoveStopRadius = 0.2f;

	ThirdPersonCharacter m_Character;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentClickTarget;

	private bool isInDirectMode = false; //TODO: Consider making static.


    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        m_Character = GetComponent<ThirdPersonCharacter>();
        currentClickTarget = transform.position;
    }

    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
		if (Input.GetKeyDown (KeyCode.G)) { // TODO: Allow player to remap later.
			isInDirectMode = !isInDirectMode;
		}

		if (isInDirectMode) {
			ProcessDirectMovement ();

		} else {
			ProcessMouseMovement ();

		}
			
    }

	private void ProcessDirectMovement(){
		print ("Direct movement.");

		float h = Input.GetAxis ("Horizontal");
		float v = Input.GetAxis ("Vertical");


		Vector3 m_CamForward = Vector3.Scale (Camera.main.transform.forward, new Vector3 (1, 0, 1)).normalized;
		Vector3 m_Move = v * m_CamForward + h * Camera.main.transform.right;

		m_Character.Move (m_Move, false, false);
	}

	private void ProcessMouseMovement(){

		print ("Mouse movement.");
		if (Input.GetMouseButton(0))
		{
			print("Cursor raycast hit" + cameraRaycaster.hit.collider.gameObject.name.ToString());

			switch (cameraRaycaster.layerHit) {
			case Layer.Walkable:
				currentClickTarget = cameraRaycaster.hit.point; 
				break;
			case Layer.Enemy:
				print ("Not moving to enemy.");
				break;
			default:
				print ("Shouldn't be here!");
				return;
			}
		}
		var playerToClickPoint = currentClickTarget - transform.position;
		if (playerToClickPoint.magnitude >= walkMoveStopRadius) {
			m_Character.Move (playerToClickPoint, false, false);
		} else {
			m_Character.Move (Vector3.zero, false, false);

		}
	}
}

