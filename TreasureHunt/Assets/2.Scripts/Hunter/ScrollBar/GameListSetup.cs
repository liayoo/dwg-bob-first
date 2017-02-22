using UnityEngine;
using System.Collections;
using SimpleJSON;
using UnityEngine.UI;
using System;

public class GameListSetup : MonoBehaviour 
{
	public static GameListSetup instance = null;

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

	public GameObject firstTabButton;
	public GameObject secondTabButton;
	public string firstTabModule;
	public string secondTabModule;

	void Start()
	{
		// assign onClick event to Buttons
		Button firstTabB = firstTabButton.GetComponent<Button> ();
		firstTabB.onClick.AddListener (() => CacheController.instance.GetContent(firstTabModule, ""));
		Button secondTabB = secondTabButton.GetComponent<Button> ();
		secondTabB.onClick.AddListener (() => CacheController.instance.GetContent (secondTabModule, ""));
		// call function
		CacheController.instance.GetContent(firstTabModule, "");	
	}
		
	public GameObject scrollbar;
	public GameObject gameList;
	public GameObject miniTreasureList;

	public void ForEachGame(string data, string whichModule)
	{
		Debug.Log ("gamelist foreachgame called "+whichModule);
		// Destroy old game lists
		GameObject content = GameObject.Find("Canvas/Scroll View/Viewport/Content");
		for (int i = 0; i < content.transform.childCount; i++) 
		{
			Destroy (content.transform.GetChild (i).gameObject);
		}
		// make new game lists
		// parse data
		var jsonData = JSON.Parse (data);
		var games = jsonData ["user_game_list"];
		// iterate through
		for (int i = 0; i < games.Count; i++) 
		{
			// filters for on/off games
			if (whichModule.Equals("OnGamesIMade") && games [i] ["status"].AsInt != 1) 
			{
				continue;
			}
			if (whichModule.Equals("OffGamesIMade") && games [i] ["status"].AsInt != 2) 
			{
				continue;
			}
			// make new game list
			GameObject newGame = (GameObject) Instantiate (gameList);
			// attach attributes
			newGame.transform.parent = scrollbar.transform.FindChild ("Viewport/Content");
			newGame.transform.localScale = Vector3.one;
			newGame.transform.FindChild("GameName").GetComponent<Text>().text = games[i]["game_name"];
			newGame.name = games [i] ["game_id"].AsInt.ToString();
			newGame.tag = "GameList";
			Debug.Log ("attached attributes");
			// parse treasures to setup "treasures" text
			var treasures = games [i] ["treasures"];
			// set "treasures" attribute
			int howMany = 3;
			string str = "";
			if (treasures.Count < 3) 
			{
				howMany = treasures.Count;
			}
			for (int j = 0; j < howMany; j++)
			{
				str += treasures [j] ["treasure_name"] + " ";
				newGame.transform.FindChild ("Treasures").GetComponent<Text> ().text = str;
			}
			// attach onclick event
			Button btn = newGame.GetComponent<Button>();
			btn.onClick.AddListener
			(
				delegate
				{
					CacheController.instance.GetContent(whichModule+"Popup", newGame.name);
				}
			);
		}
	}

	public void SetupPopup(string data, string whichModule, string gameID)
	{
		// if there is already a popup, do not open another one
		if (GameObject.FindWithTag("Popup") != null) 
		{
			return;
		}
		Debug.Log (gameID);
		// make popup
		GameObject popup = null;
		switch(whichModule) 
		{
		case "PlayingGamesPopup":
			popup = (GameObject)Resources.Load ("MyGamePopup");
			break;
		case "SearchGamesPopup":
			popup = (GameObject)Resources.Load ("SearchGamePopup");
			break;
		default:
			break;
		}
		// set attributes
		GameObject newPopup = (GameObject) Instantiate (popup);
		newPopup.transform.SetParent (GameObject.Find ("Canvas").transform);
		newPopup.transform.localScale = Vector3.one;
		newPopup.transform.localPosition = new Vector3(0, 0, 0); 
		newPopup.transform.tag = "Popup";
		Debug.Log ("set attributes of popup");
		// find the right match for the game popup is called for
		// parse data
		var jsonData = JSON.Parse (data);
		var games = jsonData ["user_game_list"];
		for (int i = 0; i < games.Count; i++) 
		{
			var curr = games [i];
			if ((curr ["game_id"].AsInt.ToString()).Equals(gameID)) 
			{
				Debug.Log ("I'm in if");
				// attach popup attributes
				newPopup.transform.FindChild ("Changing Objects/GameName").GetComponent<Text>().text = curr ["game_name"];
				newPopup.transform.FindChild ("Changing Objects/Whom").GetComponent<Text>().text = curr ["maker_id"].AsInt.ToString();
				newPopup.transform.FindChild ("Changing Objects/howManyPart").GetComponent<Text>().text = curr ["participant"];
				newPopup.transform.FindChild ("Changing Objects/howManyTrea").GetComponent<Text> ().text = curr ["treasure_count"].AsInt.ToString();
				// assign onClick event to BackButton
				Transform backButton = newPopup.transform.FindChild("BackButton");
				Button backB = backButton.GetComponent<Button> ();
				backB.onClick.AddListener (TurnDownPopup);
				// assign onClick event to DropOutButton/JoinButton
				GameObject parentObject = new GameObject();
				switch(whichModule)
				{
				case "PlayingGamesPopup":
					parentObject = GameObject.Find ("Canvas/MyGamePopup(Clone)/Treasures/Viewport/Content");
					Transform dropOutButton = newPopup.transform.FindChild("DropOutButton");
					Button dropB = dropOutButton.GetComponent<Button> ();
					dropB.onClick.AddListener (() => DropOrJoin(true, curr["game_id"].AsInt.ToString())); 
					break;
				case "SearchGamesPopup":
					parentObject = GameObject.Find ("Canvas/SearchGamePopup(Clone)/Treasures/Viewport/Content");
					Transform joinButton = newPopup.transform.FindChild ("JoinButton");
					Button joinB = joinButton.GetComponent<Button> ();
					joinB.onClick.AddListener (() => DropOrJoin (false, curr ["game_id"].AsInt.ToString()));
					break;
				default:
					break;
				}
				// attach treasure list
				// parse treasures
				var treasures = curr ["treasures"];
				// check if there is an error
				if (curr["treasure_count"].AsInt != treasures.Count) 
				{
					Debug.Log ("something wrong with treasure_count");
				}
				// make treasure objects 
				for (int j = 0; j < treasures.Count; j++) 
				{
					GameObject treasureText = (GameObject) Instantiate (miniTreasureList);
					treasureText.transform.SetParent (parentObject.transform);
//					treasureText.transform.SetParent (GameObject.Find ("SmallTreasureList(Clone)").transform);
					treasureText.transform.localScale = Vector3.one;
					treasureText.name = treasures [j] ["treasure_id"].AsInt.ToString();
					treasureText.transform.FindChild ("TreasureName").GetComponent<Text>().text = treasures [j] ["treasure_name"];
					treasureText.transform.FindChild ("Point").GetComponent<Text>().text = treasures [j] ["treasure_point"].AsInt.ToString();
                    treasureText.transform.FindChild("TreasureImg").GetComponent<Image>().sprite = Resources.Load<Sprite>(treasures[j]["treasure_img_name"]);
					// todo: treasure img and target img
					// attach onclick listener
					string trName = treasures [j] ["treasure_name"];
					string point = treasures [j] ["treasure_point"];
					string description = treasures [j] ["description"];
					int catchGame = treasures [j] ["catchgame_cat"].AsInt;
					treasureText.GetComponent<Button>().onClick.AddListener(() => TreasureDetailPopup(trName, description, point, catchGame));
				}
			}
		}
	}

	public GameObject treasureDetailPopup;

	public void TreasureDetailPopup(string trName, string description, string point, int catchGame){
		Debug.Log ("TreasureDetailPopup for " + trName + description + point);
		Debug.Log (catchGame);
		GameObject popup = (GameObject) Instantiate (treasureDetailPopup);
		popup.transform.SetParent(GameObject.Find("Canvas").transform);
		popup.transform.localScale = Vector3.one;
		popup.transform.localPosition = new Vector3(0, 0, 0); 
		popup.tag = "TreasureDetailPopup";
		// attach popup attributes
		popup.transform.FindChild ("Changing Objects/TreasureName").GetComponent<Text>().text = trName;
		popup.transform.FindChild ("Changing Objects/Description").GetComponent<Text>().text = description;
		popup.transform.FindChild ("Changing Objects/HowManyPoint").GetComponent<Text>().text = point;
		Debug.Log ("done with attaching");
		if ( catchGame < 3) 
		{
			popup.transform.FindChild ("Changing Objects/WhichMiniGame").GetComponent<Text> ().text = "SlimePang";
		}
		else 
		{
			popup.transform.FindChild ("Changing Objects/WhichMiniGame").GetComponent<Text> ().text = "Quiz";
		}
		// todo: TreasureImage & TargetImage
		// attach onclick event to backButton
		Transform backButton = popup.transform.FindChild("BackButton");
		Button backB = backButton.GetComponent<Button> ();
		backB.onClick.AddListener (TurnDownDetailPopup);
	}

	public void TurnDownDetailPopup()
	{
		Destroy(GameObject.FindWithTag("TreasureDetailPopup"));
	}

	public void TurnDownPopup()
	{
		Debug.Log ("called");
		Destroy(GameObject.FindWithTag("Popup"));
		Debug.Log ("turned down popup");
	}

	public void DropOrJoin(bool isDrop, string gameID)
	{
		// inform server that this user drops out of the game
		if (!gameObject.GetComponent<NetworkManager> ().enabled) 
		{
			Debug.Log ("DropOrJoin called when NetworkManager is not enabled");
		}
		else 
		{
			if (isDrop) 
			{
				string [] temp = {gameID};
				CacheController.instance.SendSignal ("DropOutOfGame", temp);
			} 
			else 
			{
				string [] temp = {gameID};
				CacheController.instance.SendSignal ("JoinGame", temp);
			}
		}
		// close popup and delete this gamelist from scroll view
		Destroy(GameObject.Find("Canvas/Scroll View/Viewport/Content/" + gameID));
		TurnDownPopup();
	}


}
