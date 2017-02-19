using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject player;
	private Vector3 offsetPos;
	private Quaternion offsetRot;

	// Use this for initialization
	void Start () {
		offsetPos = transform.position - player.transform.position;
		offsetRot = transform.rotation;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		transform.position = player.transform.position + offsetPos;
		//todo : rotation
		//transform.LookAt(player.transform);
	}
}
