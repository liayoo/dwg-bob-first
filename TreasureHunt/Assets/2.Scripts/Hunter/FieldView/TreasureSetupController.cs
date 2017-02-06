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
		string str = "{\"flag\":3, \"usn\":\"" + userName + "\"}";
		if (!gameObject.GetComponent<NetworkManager> ().enabled) 
		{
			TextAsset jsonData = Resources.Load<TextAsset> ("TestForTreasureSetup");
			var strJsonData = jsonData.text;
			Debug.Log (strJsonData);
			ForEachGame (strJsonData);
		}
		else 
		{
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
		// save data at static variable to use later for GameList View...
		userGameTreasureData = data;

		// parsing json data using SimpleJSON
		var jsonData = JSON.Parse (data);
		var games = jsonData ["Games"];

		// Make the root object to save all game & treasure objects
		GameObject gameTreasurePanel = new GameObject();
		gameTreasurePanel.tag = "GameTreasurePanel";
		DontDestroyOnLoad (gameTreasurePanel);

		// Make game objects
		for (int i = 0; i < games.Count; i++) 
		{
			//make new game panel
			GameObject newGame = (GameObject) Instantiate (game, new Vector3(0,0,0), Quaternion.identity);
			GameAttributes ga = newGame.GetComponent<GameAttributes> ();
			// attach game attributes
			ga.SetAsChildOf(gameTreasurePanel);
			ga.SetAttributes(games[i]["game_id"], games[i]["game_name"], games[i]["treasure_count"].AsInt, games[i]["maker_id"], games[i]["status"].AsInt);
			newGame.name = games [i] ["game_id"];
			newGame.tag = "Games";
			DontDestroyOnLoad (newGame);
			// parse treasures
			var treasures = games [i] ["Treasures"];
			// check if there is an error
			if (games[i]["treasure_count"].AsInt != treasures.Count) {
				Debug.Log ("something wrong with treasure_count");
			}
			// make treasure objects 
			for (int j = 0; j < treasures.Count; j++) 
			{
				string treasure_id = treasures [j] ["treasure_id"];
				MakeNewTreasure (newGame, treasures[j]["treasure_id"], treasures[j]["treasure_name"], treasures[j]["destination"],
					treasures[j]["game_id"], treasures[j]["location"], treasures[j]["point"].AsInt, treasures[j]["catchgame_cat"].AsInt, treasures[j]["target_img_url"]);
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
		newTreasure.name = trName;
		newTreasure.tag = "Treasures";
		DontDestroyOnLoad (newTreasure);
		return newTreasure; 
	}

	Vector3 StringToVector3 (string str){
		// need to include System.Globalization;

		// define where to splict string
		char[] delimiterChars = { ',' };

		// get rid of empty spaces
		str.Replace(" ", "");

		// get rid of parentheses
		if (str [0] == '(' && str [str.Length - 1] == ')') {
			str = str.Substring (1, str.Length - 2);
		}

		// divide string
		string[] words = str.Split (delimiterChars);

		//check if it's parse properly
		if (words.Length != 3) {
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
