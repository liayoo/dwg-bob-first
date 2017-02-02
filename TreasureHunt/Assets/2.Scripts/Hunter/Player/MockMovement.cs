using UnityEngine;
using System.Collections;
using SimpleJSON;

public class MockMovement : MonoBehaviour {

	void Start(){
		StartIt ();
	}

	string data;

	public void StartIt(){
		TextAsset jsonData = Resources.Load<TextAsset> ("PlayerLocations");
		var strJsonData = jsonData.text;
		Debug.Log (strJsonData);
		data = strJsonData;
		StartCoroutine("DoIt");
	}

	public GameObject player;

	IEnumerator DoIt(){
		var jsonData = JSON.Parse (data);
		var locs = jsonData ["Locs"];

		UserMovementController con = player.GetComponent<UserMovementController>();

		for (int i = 0; i < locs.Count; i++) {
			yield return new WaitForSeconds (1.5f);
			con.ToNewStringSpot (locs [i] ["id"]);
		}
	}
}
