using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MakerModeButtonController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Click(){
		//Scene #6 is M_GameList View
		SceneManager.LoadScene (6);
	}
}
