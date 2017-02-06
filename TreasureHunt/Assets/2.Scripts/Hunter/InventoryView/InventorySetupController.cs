using UnityEngine;
using System.Collections;
using SimpleJSON;
using UnityEngine.UI;
using System;

public class InventorySetupController : MonoBehaviour 
{
	public static InventorySetupController instance = null;

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

	public GameObject unusedButton;
	public static string inventoryData = "";

	void Start()
	{
		// assign onClick event to myGameButton
		Button unusedB = unusedButton.GetComponent<Button> ();
		unusedB.onClick.AddListener (() => ForEachItem ("gg", false));
		// Todo: get user id and pass it as an argument, instead of gg
		ForEachItem ("gg", false);		
	}

	public void GetContent(string userName)
	{
		if (!gameObject.GetComponent<NetworkManager> ().enabled) 
		{
			TextAsset jsonData = Resources.Load<TextAsset> ("TestForInventory");
			var strJsonData = jsonData.text;
			inventoryData = strJsonData;
			Debug.Log ("GetContent: "+strJsonData);
			//ForEachItem (strJsonData, false);
		}
		else 
		{
			if (inventoryData != "") 
			{
				//ForEachItem (inventoryData, false);
			} 
			else
			{
				string str = "{\"flag\":1, \"usn\":\"" + userName + "\"}";
				NetworkManager.instance.SendData (str);
			}
		}
	}

	public GameObject scrollbar;
	public GameObject treasureList;

	public void ForEachItem (string userName, bool isUsed)
	{
		// Destroy old treasure lists, that is, usedLists or old unusedists
		GameObject content = GameObject.Find ("Canvas/Scroll View/Viewport/Content");
		for (int i = 0; i < content.transform.childCount; i++) {
			Destroy (content.transform.GetChild (i).gameObject);
		}
		// get data
		GetContent(userName);
		string data = inventoryData;
		Debug.Log ("came back: "+data);
		// make new treasure lists, that is, myGameLists
		var jsonData = JSON.Parse (data);
		var treasures = jsonData ["Treasures"];

		for (int i = 0; i < treasures.Count; i++) {
			var curr = treasures [i];
			if (curr ["used"].AsBool == isUsed) 
			{
				// make new treasure list
				GameObject newTreasure = (GameObject)Instantiate (treasureList, new Vector3 (0, 0, 0), Quaternion.identity);
				newTreasure.transform.parent = scrollbar.transform.FindChild ("Viewport/Content");
				newTreasure.transform.localScale = Vector3.one;
				newTreasure.tag = "TreasureList";
				// attach attributes
				newTreasure.name = curr ["treasure_id"];
				newTreasure.transform.FindChild ("TreasureName").GetComponent<Text> ().text = curr ["treasure_name"];
				newTreasure.transform.FindChild ("Description").GetComponent<Text> ().text = curr ["description"];
				newTreasure.transform.FindChild ("Point").GetComponent<Text> ().text += curr ["point"];
				newTreasure.transform.FindChild ("DateTime").GetComponent<Text> ().text = curr ["date_time"];
				// attach onclick event
				if (!isUsed) 
				{
					Button btn = newTreasure.GetComponent<Button> ();
					btn.onClick.AddListener
					(
						delegate {
							InventoryPopupSetup.instance.SetupPopup (data, newTreasure.name, isUsed);
						}
					);
				}
			}
		}
	}

}
