using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BackButtonController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Click(){
		// scene #3 is Field View
		SceneManager.LoadScene (3);
	}

}

