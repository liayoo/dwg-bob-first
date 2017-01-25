using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class InventoryButtonController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Click(){
		//Scene #5 is H_Inventory View
		SceneManager.LoadScene (5);
	}
}
