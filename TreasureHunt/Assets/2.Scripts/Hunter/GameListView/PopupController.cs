using UnityEngine;
using System.Collections;
using SimpleJSON;
using UnityEngine.UI;
using System;

public class PopupController : MonoBehaviour {

	public static PopupController instance = null;

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
		
	string data;

	public void GetContent(string userName, bool isMyGame)
	{
		if (!gameObject.GetComponent<NetworkManager> ().enabled) 
		{
			TextAsset jsonData = null;
			if (isMyGame) 
			{
				jsonData = Resources.Load<TextAsset> ("TestForTreasureSetup");
			} 
			else 
			{
				jsonData = Resources.Load<TextAsset> ("TestForSearch");
			}
			var strJsonData = jsonData.text;
			Debug.Log (strJsonData);
			data = strJsonData;
		}
		else 
		{
			if (isMyGame && (TreasureSetupController.userGameTreasureData != "")) 
			{
				data = TreasureSetupController.userGameTreasureData;
			} 
			else 
			{
				Debug.Log ("userGameTreasureData not initiated in TreasureSetupController. error.");
				// or, if want to flush cache at some interval,
				// connect to server again at this condition
			}
		}
	}

	public void SetupPopup(string gameID, bool isMyGame)
	{
		// if there is already a popup, do not open another one
		if (GameObject.FindWithTag("Popup") != null) 
		{
			return;
		}
		Debug.Log (gameID);
		// get data
		if (!gameObject.GetComponent<NetworkManager> ().enabled)
		{
			// Todo: get user id and pass it as an argument, instead of gg
			GetContent ("gg", isMyGame);		
		}
		// make popup
		GameObject popup = null;
		if (isMyGame) 
		{
			popup = (GameObject)Resources.Load ("MyGamePopup");
		} 
		else 
		{
			popup = (GameObject)Resources.Load ("SearchGamePopup");
			Debug.Log ("wow");
		}
		GameObject newPopup = (GameObject) Instantiate (popup);
		newPopup.transform.SetParent (GameObject.Find ("Canvas").transform);
		newPopup.transform.localScale = Vector3.one;
		newPopup.transform.localPosition = new Vector3(0, 0, 0); 
		Debug.Log ("yes");
		// parse data
		var jsonData = JSON.Parse (data);
		var games = jsonData ["Games"];
		for (int i = 0; i < games.Count; i++) {
			var curr = games [i];
			if (String.Compare(curr ["game_id"], gameID)==0) {
				Debug.Log ("I'm in if");
				// attach popup attributes
				newPopup.transform.FindChild ("Changing Objects/GameName").GetComponent<Text>().text = curr ["game_name"];
				newPopup.transform.FindChild ("Changing Objects/Whom").GetComponent<Text>().text = curr ["maker_id"];
				newPopup.transform.FindChild ("Changing Objects/howManyPart").GetComponent<Text>().text = curr ["participant"];
				newPopup.transform.FindChild ("Changing Objects/howManyTrea").GetComponent<Text>().text = curr ["treasure_count"];
				newPopup.transform.tag = "Popup";
				// assign onClick event to BackButton
				Transform backButton = newPopup.transform.FindChild("BackButton");
				Button backB = backButton.GetComponent<Button> ();
				backB.onClick.AddListener (TurnDownPopup);
				// assign onClick event to DropOutButton/JoinButton
				if(isMyGame)
				{
					Transform dropOutButton = newPopup.transform.FindChild("DropOutButton");
					Button dropB = dropOutButton.GetComponent<Button> ();
					dropB.onClick.AddListener (() => DropOrJoin(true, "gg", curr["game_id"])); // todo: put valid usn instead of "gg"
				}
				else
				{
					Transform joinButton = newPopup.transform.FindChild ("JoinButton");
					Button joinB = joinButton.GetComponent<Button> ();
					joinB.onClick.AddListener (() => DropOrJoin (false, "gg", curr ["game_id"])); // todo: same as above
				}
				// attach treasure list
				// parse treasures
				var treasures = curr ["Treasures"];
				// check if there is an error
				if (curr["treasure_count"].AsInt != treasures.Count) {
					Debug.Log ("something wrong with treasure_count");
				}
				// make treasure objects 
				for (int j = 0; j < treasures.Count; j++) 
				{
					// todo: 
				}
			}
		}
	}

	public void TurnDownPopup(){
		Debug.Log ("called");
		Destroy(GameObject.FindWithTag("Popup"));
		Debug.Log ("turned down popup");
	}

	public void DropOrJoin(bool isDrop, string userName, string gameID){
		// inform server that this user drops out of the game
		if (!gameObject.GetComponent<NetworkManager> ().enabled) 
		{
			Debug.Log ("DropOrJoin called when NetworkManager is enabled");
		}
		else 
		{
			string str = "";
			if (isDrop) 
			{
				str = "{\"flag\":16, \"usn\":\"" + userName + "\", \"game_id\":" + gameID + "\"}";
			} 
			else 
			{
				str = "{\"flag\":14, \"usn\":\"" + userName + "\", \"game_id\":" + gameID + "\"}";
			}
			NetworkManager.instance.SendData (str);
		}
		// reset data so that if you go back to Field View, treasure setup would start again
		TreasureSetupController.userGameTreasureData = ""; // needed if we use cache flush
		// close popup and delete this gamelist from scroll view
		Destroy(GameObject.Find("Canvas/Scroll View/Viewport/Content/" + gameID));
		TurnDownPopup();
	}

	/*
	void PutAttribute(GameObject newPopup, string child, string gameText, string jsonID){
		var thisGame = JSON.Parse(gameText);
		newPopup.transform.FindChild (child).GetComponent<Text> ().text = thisGame [jsonID];
	}
	*/
}
