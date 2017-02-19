using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SimpleJSON;

public class GamesIMadeSetup : MonoBehaviour {
	/*
	public static GamesIMadeSetup instance = null;

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

	public GameObject currGameButton;

	void Start()
	{
		// assign onClick event to myGameButton
		Button currGameB = currGameButton.GetComponent<Button> ();
		currGameB.onClick.AddListener (() => CacheController.GetContent ("GamesIMade"));
		CacheController.GetContent ("GamesIMade");	
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
	*/
}
