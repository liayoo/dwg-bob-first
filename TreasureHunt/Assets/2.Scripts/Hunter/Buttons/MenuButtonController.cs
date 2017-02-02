using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuButtonController : MonoBehaviour {

	bool flag = false;
	public GameObject target;

	Button myButton;

	void Awake(){
		myButton = GetComponent<Button> ();
		myButton.onClick.AddListener (GotIt);
	}

	void Start(){
		target.SetActive (false);
	}

	void GotIt(){
		if (flag) {
			target.SetActive (false);
			flag = false;
		} else {
			target.SetActive (true);
			flag = true;
		}
	}

}
