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
	public static string[] treasuresIGot = new string[400]; // 400 an arbitrary number of max number of treasures
	public static int treasuresIGotNextIndex;

	void AskServer()
	{
		if (!gameObject.GetComponent<NetworkManager> ().enabled) 
		{
			
		} 
		else 
		{
			// get user data from server
			userName = GameObject.Find ("Canvas/Panel/IDField/InputField/Text").GetComponent<Text> ().text;
			CacheController.instance.GetContent ("UserInfo", userName);
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

		// call ConstructTreasuresIGot
		CacheController.instance.GetContent ("TreasuresIGot", userID);

	}

	public void ConstructTreasuresIGot(string data)
	{
		// parse data
		var jsonData = JSON.Parse (data);
		var treasures = jsonData ["user_info"];

		// for each treasure
		for (int i = 0; i < treasures.Count; i++) 
		{
			treasuresIGot [i] = treasures [i] ["treasure_id"];
		}
		treasuresIGotNextIndex = treasures.Count;

		DebugTreasuresIGot ();

		// move scene
		SceneManager.LoadScene ("H_Field");
	}

	void DebugTreasuresIGot()
	{
		string str = "";
		for (int i = 0; i < 400; i++) 
		{
			str += treasuresIGot [i];
			str += " ";
		}
		Debug.Log (str);
	}
}
