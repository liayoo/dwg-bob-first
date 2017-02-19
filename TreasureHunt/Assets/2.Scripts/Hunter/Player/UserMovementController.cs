using UnityEngine;
using System.Collections;
using System.Globalization;
using UnityEngine.UI;

public class UserMovementController : MonoBehaviour {

	public UserController uc;

	public Vector3 newLocation;

	float moveSpeed = 50f;
	float turnSpeed = 540f;

	void Awake() {
	
		uc = GetComponent <UserController> ();
	
	}

	public void ToNewStringSpot(string input){
	
		ToNewSpot(StringToVector3(input));
	
	}

	public void ToNewSpot(Vector3 newLoc){
		newLoc = (newLoc - new Vector3(127.036f, 0.0f, 37.500f))*500000;
		GameObject.Find ("Canvas/Text").GetComponent<Text> ().text = newLoc.ToString();
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

	Vector3 StringToVector3 (string str){
		// need to include System.Globalization;

		// define where to splict string
		char[] delimiterChars = { ',' };

		// get rid of empty spaces
		str.Replace(" ", "");

		// get rid of parentheses
		if (str [0] == '(' && str [str.Length - 1] == ')') {
			str = str.Substring (1, str.Length - 2);
		}

		// divide string
		string[] words = str.Split (delimiterChars);

		//check if it's parse properly
		if (words.Length != 3) {
			Debug.Log ("incorrect treasue location format in StringToVector3");
		}

		// get x, y, z of target vector3
		float x = float.Parse (words[0], CultureInfo.InvariantCulture.NumberFormat);
		float y = float.Parse (words[1], CultureInfo.InvariantCulture.NumberFormat);
		float z = float.Parse (words[2], CultureInfo.InvariantCulture.NumberFormat);

		//Todo: if y should be 0, double check and make sure it must be 0

		//construct vector3 from x, y, z and return it;
		Vector3 result = new Vector3 (x, y, z);
		return result;
	}
		

}
