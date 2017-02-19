using UnityEngine;
using System.Collections;
using SimpleJSON;

public class CacheController : MonoBehaviour {

	public static CacheController instance = null;

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

	void Start(){
		StartCoroutine ("FlushCache");
	}

	public static string userGameTreasureCache = "";
	public static string searchNewGamesCache = "";
	public static string inventoryCache = "";
	public static string myGamesCache = "";

	string moduleName = "";
	string variable = "";

	public void SendSignal (string whichModule, string[] temp){
	
		string str = "";

		switch (whichModule)
		{
		case "DropOutOfGame":
			if (temp.Length != 1) 
			{
				Debug.Log ("wrong number of variables for temp in sendsignal, drop out of game");
			}
			str = "{\"flag\":16, \"usn\":" + LoginButtonCtrl.userID + ", \"game_id\":" + temp [0] + "}";
			userGameTreasureCache = ""; // flush cache
			searchNewGamesCache = "";
			break;
		case "JoinGame":
			if (temp.Length != 1) {
				Debug.Log ("wrong number of variables for temp in sendsignal, joingame");
			}
			str = "{\"flag\":15, \"usn\":" + LoginButtonCtrl.userID + ", \"game_id\":" + temp [0] + "}";
			userGameTreasureCache = "";
			searchNewGamesCache = ""; // flush cache
			break;
		case "UseTreasure":
			if (temp.Length != 1) {
				Debug.Log ("wrong number of variables for temp in sendsignal, joingame");
			}
			str = "{\"flag\":20, \"usn\":" + LoginButtonCtrl.userID + ", \"treasure_id\":" + temp [0] + "}";
			inventoryCache = "";
			break;
		default:
			break;
		}
		NetworkManager.instance.SendData(str);
	}

	public void GetContent(string whichModule, string temp)
	{
		string cacheData = "";
		string jsonToServer = "";
		moduleName = whichModule;
		variable = temp;

		switch (whichModule) 
		{
		// these are for M_GameList View
		case "OnGamesIMade":
		case "OffGamesIMade":
		case "OnGamesIMadePopup":
		case "OffGamesIMadePopup":
			cacheData = myGamesCache;
			jsonToServer = "{\"flag\":2, \"usn\":" + LoginButtonCtrl.userID + "}";
			break;
		// this is for H_Field View
		case "FieldTreasures":
		// these are for H_GameList View
		case "PlayingGames":
		case "PlayingGamesPopup":
			Debug.Log("CacheController, Playing Games Popup temp is: " + temp + ", variable is: " + variable);
			cacheData = userGameTreasureCache;
			jsonToServer = "{\"flag\":3, \"usn\":\"" + LoginButtonCtrl.userID + "\"}";
			break;
		case "SearchGames":
		case "SearchGamesPopup":
			cacheData = searchNewGamesCache;
			jsonToServer = "{\"flag\":7, \"usn\":\"" + LoginButtonCtrl.userID + "\"}";
			break;
		// these are for H_Inventory View
		case "UnusedInventory":
		case "UsedInventory":
		case "UnusedInventoryPopup":
			cacheData = inventoryCache;
			jsonToServer = "{\"flag\":1, \"usn\":\"" + LoginButtonCtrl.userID + "\"}";
			break;
		default:
			break;
		}

		if (cacheData == "") 
		{
			// communicate with server to fetch needed data
			NetworkManager.instance.SendData (jsonToServer);
			// now networkManager would call DoIt
		} 
		else 
		{
			// pass on cache data
			DoIt(cacheData);
		}
	}

	public void DoIt(string data)
	{
		var jsonData = JSON.Parse(data);
		int flag = jsonData["flag"].AsInt;

		switch (moduleName) 
		{
		// These are for M_GameList View
		case "OnGamesIMade":
		case "OffGamesIMade":
			// check if flag is right
			if (flag != 2) 
			{
				Debug.Log ("wrong response from server");
				return;
			}
			// save at cache
			myGamesCache = data;
			// call according function
			GameListSetup.instance.ForEachGame (data, moduleName);
			break;
		case "OnGamesIMadePopup":
			// check if flag is right
			if (flag != 2) 
			{
				Debug.Log ("wrong response from server");
				return;
			}
			// save at cache
			myGamesCache = data;
			// call according function
			GameListSetup.instance.OnGamePopup (data, variable);
			break;
		// This is for H_Field View
		case "FieldTreasures":
			// check if flag is right
			if (flag != 3) 
			{
				Debug.Log ("wrong response from server");
				return;
			}
			// save at cache
			userGameTreasureCache = data;
			// call according function
			TreasureSetupController.instance.ForEachGame (data);
			break;
		// These are for H_GameList View 
		case "PlayingGames":
			// check if flag is right
			if (flag != 3) 
			{
				Debug.Log ("wrong response from server");
				return;
			}
			// save at cache
			userGameTreasureCache = data;
			// call according function
			GameListSetup.instance.ForEachGame(data, moduleName);
			break;
		case "SearchGames":
			// check if flag is right
			if (flag != 7) 
			{
				Debug.Log ("wrong response from server");
				return;
			}
			// save at cache
			searchNewGamesCache = data;
			// call according function
			GameListSetup.instance.ForEachGame(data, moduleName);
			break;
		case "PlayingGamesPopup":
			// check if flag is right
			if (flag != 3) 
			{
				Debug.Log ("wrong response from server");
				return;
			}
			// save at cache
			userGameTreasureCache = data;
			// call according function
			Debug.Log("CacheController, Playing Games Popup variable is: " + variable);
			GameListSetup.instance.SetupPopup(data, moduleName, variable);
			break;
		case "SearchGamesPopup":
			// check if flag is right
			if (flag != 7) 
			{
				Debug.Log ("wrong response from server");
				return;
			}
			// save at cache
			searchNewGamesCache = data;
			// call according function
			GameListSetup.instance.SetupPopup(data, moduleName, variable);
			break;
		// These are for H_Inventory View
		case "UnusedInventory":
			// check if flag is right
			if (flag != 1) 
			{
				Debug.Log ("wrong response from server");
				return;
			}
			// save at cache
			inventoryCache = data;
			// call according function
			InventorySetupController.instance.ForEachItem(data, 0);
			break;
		case "UsedInventory":
			// check if flag is right
			if (flag != 1) 
			{
				Debug.Log ("wrong response from server");
				return;
			}
			// save at cache
			inventoryCache = data;
			// call according function
			InventorySetupController.instance.ForEachItem(data, 1);
			break;
		case "UnusedInventoryPopup":
			// check if flag is right
			if (flag != 1) 
			{
				Debug.Log ("wrong response from server");
				return;
			}
			// save at cache
			inventoryCache = data;
			// call according function
			InventoryPopupSetup.instance.SetupPopup(data, variable, false);
			break;
		default:
			break;
		}
	}

	IEnumerator FlushCache(){

		yield return new WaitForSeconds (30f);
		userGameTreasureCache = "";
		searchNewGamesCache = "";
		inventoryCache = "";
		myGamesCache = "";

	}
}
