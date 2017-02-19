using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuButtonController : MonoBehaviour {

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
		target.SetActive (!target.activeSelf);
	}

}
