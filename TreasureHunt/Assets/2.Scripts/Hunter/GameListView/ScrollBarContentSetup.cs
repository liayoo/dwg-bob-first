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

	void Start()
	{
		// Todo: get user id and pass it as an argument, instead of gg
		GetContent ("gg");		
	}

	public void GetContent(string userName)
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
			if (TreasureSetupController.userGameTreasureData != "") 
			{
				ForEachGame (TreasureSetupController.userGameTreasureData);
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

	public void ForEachGame(string data)
	{
		var jsonData = JSON.Parse (data);
		var games = jsonData ["Games"];

		for (int i = 0; i < games.Count; i++) 
		{
			// make new game button
			GameObject newGame = (GameObject) Instantiate (gameList, new Vector3(0,0,0), Quaternion.identity);
			// attach button attributes
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
					PopupController.instance.SetupPopup(newGame.name);
				}
			);
		}
	}

}
