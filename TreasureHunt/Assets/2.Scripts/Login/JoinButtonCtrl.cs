using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SimpleJSON;

public class JoinButtonCtrl : MonoBehaviour {

	public static JoinButtonCtrl instance = null;

	void Awake()
	{
		if (instance == null) 
		{
			instance = this;
		} 
		else if (instance != this) 
		{
			Destroy (gameObject);
		}
	}

	public GameObject goButt;

	void Start(){
		Button but = goButt.GetComponent<Button> ();
		but.onClick.AddListener (AskServer);
	}

	public static string userName;
	public static string userID;
	public static int totPoint;

	void AskServer()
	{
		if (!gameObject.GetComponent<NetworkManager> ().enabled) 
		{

		} 
		else 
		{
			// get user data from server
			userName = GameObject.Find ("Canvas/Panel/IDField/InputField/Text").GetComponent<Text> ().text;
			CacheController.instance.GetContent ("UserJoin", userName);
		}
	}
}
