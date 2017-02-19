using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SimpleJSON;

public class LoginButtonCtrl : MonoBehaviour {

	public static LoginButtonCtrl instance = null;

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
			string str = "{\"flag\":6, \"nickname\":\"" + userName + "\"}";
			NetworkManager.instance.SendData (str);
		}
	}

	public void SaveGetDataNMove(string data)
	{
		// parse data
		var jsonData = JSON.Parse(data);
		var userInfo = jsonData ["user_info"][0];

		// save Data
		userID = userInfo ["usn"].AsInt.ToString();
		totPoint = userInfo ["tot_point"].AsInt;

		// move scene
		SceneManager.LoadScene ("H_Field");
	}


}
