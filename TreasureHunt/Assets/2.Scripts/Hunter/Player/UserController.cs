using UnityEngine;
using System.Collections;

public class UserController : MonoBehaviour {

	public void Move(Vector3 newLocation){
		transform.position = newLocation;
	}

	public void Rotate(Quaternion newRot){
		transform.rotation = newRot;
	}

	public void MoveTemp(Vector3 targetPos){

		float moveSpeed = 50f;
		transform.position = Vector3.MoveTowards (transform.position, targetPos, moveSpeed * Time.deltaTime);
	}
}
