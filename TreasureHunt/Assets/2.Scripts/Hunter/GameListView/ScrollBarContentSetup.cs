using UnityEngine;
using System.Collections;
using SimpleJSON;
using UnityEngine.UI;

public class ScrollBarContentSetup : MonoBehaviour 
{
	public static ScrollBarContentSetup instance = null;

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

	public GameObject myGameButton;

	void Start()
	{
		// assign onClick event to myGameButton
		Button myGameB = myGameButton.GetComponent<Button> ();
		myGameB.onClick.AddListener (() => GetContent (LoginButtonCtrl.userID));
		GetContent (LoginButtonCtrl.userID);	
	}
		
	public void GetContent(string userName)
	{
		if (!gameObject.GetComponent<NetworkManager> ().enabled) 
		{
			TextAsset jsonData = Resources.Load<TextAsset> ("TestForTreasureSetup");
			var strJsonData = jsonData.text;
			Debug.Log (strJsonData);
			ForEachGame (strJsonData, true);
		}
		else 
		{
			if (TreasureSetupController.userGameTreasureData != "") 
			{
				ForEachGame (TreasureSetupController.userGameTreasureData, true);
			} 
			else
			{
				Debug.Log ("userGameTreasureData not initiated in TreasureSetupController. error.");
				// or, if want to flush cache at some interval,
				// connect to server again at this condition
			}
		}
	}

	public GameObject scrollbar;
	public GameObject gameList;

	public void ForEachGame(string data, bool isMyGame)
	{
		// Destroy old game lists
		GameObject content = GameObject.Find("Canvas/Scroll View/Viewport/Content");
		for (int i = 0; i < content.transform.childCount; i++) 
		{
			Destroy (content.transform.GetChild (i).gameObject);
		}
		// make new game lists
		// parse data
		var jsonData = JSON.Parse (data);
		var games = jsonData ["Games"];
		// iterate through
		for (int i = 0; i < games.Count; i++) 
		{
			// make new game list
			GameObject newGame = (GameObject) Instantiate (gameList);
			// attach attributes
			newGame.transform.parent = scrollbar.transform.FindChild ("Viewport/Content");
			newGame.transform.localScale = Vector3.one;
			newGame.transform.FindChild("GameName").GetComponent<Text>().text = games[i]["game_name"];
			newGame.name = games [i] ["game_id"];
			newGame.tag = "GameList";
			// parse treasures to setup "treasures" text
			var treasures = games [i] ["Treasures"];
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
					PopupController.instance.SetupPopup(newGame.name, isMyGame);
				}
			);
		}
	}

}
