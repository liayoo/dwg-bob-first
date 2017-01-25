using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuButtonController : MonoBehaviour {

	bool flag = false;
	public GameObject target;

	Button myButton;

	// Use this for initialization
	void Awake(){
		myButton = GetComponent<Button> ();
		myButton.onClick.AddListener (Gotit);
	}
	void Start(){
		target.SetActive (false);
	}
	// Update is called once per frame
	void Update () {

	}

	void Gotit(){
		if (flag) {
			target.SetActive (false);
			flag = false;
		} else {
			target.SetActive (true);
			flag = true;
		}
	}
}

