using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ButtonToMoveScene : MonoBehaviour {

	public void moveSene(string nextScene){
		SceneManager.LoadScene (nextScene);
	}
}
