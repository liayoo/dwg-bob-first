using UnityEngine;
using System.Collections;

public class UserMovementController : MonoBehaviour {

	public UserController uc;

	public Vector3 newLocation;

	float moveSpeed = 50f;
	float turnSpeed = 540f;

	void Awake() {
		uc = GetComponent <UserController> ();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}



	public void ToNewSpot(Vector3 newLoc){

		newLocation = newLoc;
		StartCoroutine ("ToNewSpotRoutine");
	
	}

	IEnumerator ToNewSpotRoutine()
	{
		//Declaration
		Vector3 targetPos, framePos;
		Quaternion targetRot, frameRot;

		//routine
		do {
			yield return new WaitForSeconds (0.02f);

			//calculate location
			Vector3 dir = newLocation - transform.position;
			Vector3 dirXZ = new Vector3 (dir.x, 0f, dir.z);

			//Rotate
			if (dirXZ != Vector3.zero) {
				targetRot = Quaternion.LookRotation (dirXZ);
				frameRot = Quaternion.RotateTowards (transform.rotation, targetRot, turnSpeed * Time.deltaTime);
				uc.Rotate (frameRot);
			}
		
			//Move
			targetPos = transform.position + dirXZ;
			framePos = Vector3.MoveTowards (transform.position, targetPos, moveSpeed * Time.deltaTime);
			uc.Move (framePos);				

		} while (targetPos != framePos);
	}
		

}
