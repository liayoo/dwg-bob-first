using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameListButtonController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Click(){
		//Scene #4 is H_GameList View
		SceneManager.LoadScene (4);
	}

}
