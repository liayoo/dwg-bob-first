using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using SimpleJSON;

public class TreasureSetupController : MonoBehaviour 
{
	public static TreasureSetupController instance = null;
	public static string userGameTreasureData = "";

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

	void Start()
	{
		// Todo: get user id and pass it as an argument, instead of gg
		GetGameTreasure ("gg");
	}

	public void GetGameTreasure(string userName)
	{
		if (!gameObject.GetComponent<NetworkManager> ().enabled) 
		{
			TextAsset jsonData = Resources.Load<TextAsset> ("TestForTreasureSetup");
			var strJsonData = jsonData.text;
			Debug.Log (strJsonData);
			ForEachGame (strJsonData);
		}
		else 
		{
			string str = "{\"flag\":3, \"usn\":\"" + userName + "\"}";
			NetworkManager.instance.SendData (str);
		}
	}

	//infos included in json:
	//	flag,
	//	game_id, game_name, treasure_count, maker_id, status, participant
	//	treasure_id, treasure_name, description, game_id, location, point, catchgame_cat, target_img_url
	public GameObject game;
	public GameObject treasure;
	public static List<string> m_Data = new List<string>();

	public GameObject ForEachGame(string data)
	{
		// parsing json data using SimpleJSON
		var jsonData = JSON.Parse (data);
		var games = jsonData ["Games"];

		// Make the root object to save all game & treasure objects
		GameObject gameTreasurePanel = new GameObject();
		gameTreasurePanel.tag = "GameTreasurePanel";

		// Make game objects
		for (int i = 0; i < games.Count; i++) 
		{
			//make new game panel
			GameObject newGame = (GameObject) Instantiate (game);
			GameAttributes ga = newGame.GetComponent<GameAttributes> ();
			var cur = games [i];
			// attach game attributes
			ga.SetAsChildOf(gameTreasurePanel);
			ga.SetAttributes(cur["game_id"], cur["game_name"], cur["treasure_count"].AsInt, cur["maker_id"], cur["status"].AsInt);
			newGame.name = cur ["game_id"];
			newGame.tag = "Games";
			// parse treasures
			var treasures = cur ["Treasures"];
			// check if there is an error
			if (cur["treasure_count"].AsInt != treasures.Count) 
			{
				Debug.Log ("something wrong with treasure_count");
				return gameTreasurePanel;
			}
			// make treasure objects 
			for (int j = 0; j < treasures.Count; j++) 
			{
				var curT = treasures [j];
				string treasure_id = curT ["treasure_id"];
				MakeNewTreasure (newGame, curT["treasure_id"], curT["treasure_name"], curT["destination"],
					curT["game_id"], curT["location"], curT["point"].AsInt, curT["catchgame_cat"].AsInt, curT["target_img_url"]);
			}
		}

		return gameTreasurePanel;

	}


	GameObject MakeNewTreasure(GameObject parent, string trId, string trName, string trDes, 
		string gameId, string trLoc, int trPoint, int trCatchGame, string trTargetImg){
		GameObject newTreasure = (GameObject) Instantiate (treasure, StringToVector3(trLoc), Quaternion.identity);
		newTreasure.transform.localScale = Vector3.one;
		TreasureAttributes tr = newTreasure.GetComponent<TreasureAttributes> ();
		tr.setAttributes (trId, trName, trDes, gameId, StringToVector3(trLoc), trPoint, trCatchGame, trTargetImg);
		tr.setAsChildOf (parent);
		newTreasure.name = trId;
		newTreasure.tag = "Treasures";
		return newTreasure; 
	}

	Vector3 StringToVector3 (string str)
	{
		// need to include System.Globalization;

		// define where to splict string
		char[] delimiterChars = { ',' };

		// get rid of empty spaces
		str.Replace(" ", "");

		// get rid of parentheses
		if (str [0] == '(' && str [str.Length - 1] == ')') 
		{
			str = str.Substring (1, str.Length - 2);
		}

		// divide string
		string[] words = str.Split (delimiterChars);

		//check if it's parse properly
		if (words.Length != 3) 
		{
			Debug.Log ("incorrect treasue location format in StringToVector3");
		}

		// get x, y, z of target vector3
		float x = float.Parse (words[0], CultureInfo.InvariantCulture.NumberFormat);
		float y = float.Parse (words[1], CultureInfo.InvariantCulture.NumberFormat);
		float z = float.Parse (words[2], CultureInfo.InvariantCulture.NumberFormat);

		//Todo: if y should be 0, double check and make sure it must be 0

		//construct vector3 from x, y, z and return it;
		Vector3 result = new Vector3 (x, y, z);
		return result;
	}

}
