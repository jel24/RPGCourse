using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{

	[SerializeField] float walkMoveStopRadius = 0.2f;
	[SerializeField] float attackMoveStopRadius = 5f;

	ThirdPersonCharacter character;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
	Vector3 currentDestination, clickPoint;

	private bool isInDirectMode = false; //TODO: Consider making static.


    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        character = GetComponent<ThirdPersonCharacter>();
		currentDestination = transform.position;
    }

    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
		if (Input.GetKeyDown (KeyCode.G)) { 
			isInDirectMode = !isInDirectMode;
			currentDestination = transform.position;

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
			clickPoint = cameraRaycaster.hit.point;
			switch (cameraRaycaster.layerHit) {
			case Layer.Walkable:
				currentDestination = ShortDestination (clickPoint, walkMoveStopRadius);
				break;
			case Layer.Enemy:
				currentDestination = ShortDestination (clickPoint, attackMoveStopRadius);
				//print ("Not moving to enemy.");
				break;
			default:
				//print ("Shouldn't be here!");
				return;
			}
		}
		WalkToDestination ();


	}

	void WalkToDestination ()
	{
		var playerToClickPoint = currentDestination - transform.position;
		if (playerToClickPoint.magnitude >= walkMoveStopRadius) {
			character.Move (playerToClickPoint, false, false);
		}
		else {
			character.Move (Vector3.zero, false, false);
		}
	}

	private Vector3 ShortDestination(Vector3 destination, float shortening){
		Vector3 reductionVector = (destination - transform.position).normalized * shortening;
		return destination - reductionVector;
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.black;
		Gizmos.DrawLine (transform.position, currentDestination);
		Gizmos.DrawSphere ( currentDestination, 0.1f);
		Gizmos.DrawSphere ( clickPoint, 0.15f);

		Gizmos.color = new Color (255f, 0f, 0f, .5f);
		Gizmos.DrawWireSphere ( transform.position, attackMoveStopRadius);
	}
}

