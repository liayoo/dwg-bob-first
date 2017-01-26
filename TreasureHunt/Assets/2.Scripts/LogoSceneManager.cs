using UnityEngine;
using System.Collections;

public class LogoSceneManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(MoveNextScene());
	}
	
	IEnumerator MoveNextScene()
    {
        yield return new WaitForSeconds(4f);
        GameManager.instance.MoveScene("Login");
    }
}
