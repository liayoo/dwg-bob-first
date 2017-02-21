using UnityEngine;
using System.Collections;
using SimpleJSON;
using UnityEngine.UI;
using System;

public class InventoryPopupSetup : MonoBehaviour {
	// this script is called only when unused treasure list is clicked on

	public static InventoryPopupSetup instance = null;

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

	public GameObject popup;

	// no chance that data has changed, so it just gets data directly 
	public void SetupPopup(string data, string treasureID, bool isUsed)
	{
		if (isUsed) 
		{
			Debug.Log ("used item tries to access popup. error.");
			return;
			// not gonna provide popup
		}
		// if there is already a popup, do not open another one
		if (GameObject.FindWithTag("Popup") != null) 
		{
			return;
		}
		Debug.Log (treasureID);
		// make popup
		GameObject newPopup = (GameObject) Instantiate (popup);
		newPopup.transform.SetParent (GameObject.Find ("Canvas").transform);
		newPopup.transform.localScale = Vector3.one;
		newPopup.transform.localPosition = new Vector3(0, 0, 0); 
		newPopup.transform.tag = "Popup";
		Debug.Log ("yes");
		// parse data
		var jsonData = JSON.Parse (data);
		var treasures = jsonData ["user_info"];
		for (int i = 0; i < treasures.Count; i++) {
			var curr = treasures [i];
			if (String.Compare(curr ["treasure_id"].AsInt.ToString(), treasureID)==0) {
				Debug.Log ("I'm in if");
				// attach popup attributes
				newPopup.transform.FindChild ("Changing Objects/TreasureName").GetComponent<Text>().text = curr ["treasure_name"];
				newPopup.transform.FindChild ("Changing Objects/Description").GetComponent<Text>().text = curr ["description"];
				newPopup.transform.FindChild ("Changing Objects/HowManyPoint").GetComponent<Text>().text = curr ["point"].AsInt.ToString();
				newPopup.transform.FindChild ("Changing Objects/DateTime").GetComponent<Text>().text = curr ["date_time"];
                newPopup.transform.FindChild("Changing Objects/TargetImage").GetComponent<Image>().sprite = InventorySetupController.instance.targetImg;
                
                // todo: TreasureImage
                // assign onClick event to BackButton
                Transform backButton = newPopup.transform.FindChild("BackButton");
				Button backB = backButton.GetComponent<Button> ();
				backB.onClick.AddListener (TurnDownPopup);
				// assign onClick event to UseButton
				Transform dropOutButton = newPopup.transform.FindChild("UseButton");
				Button dropB = dropOutButton.GetComponent<Button> ();
				dropB.onClick.AddListener (() => UseIt(curr["treasure_id"].AsInt.ToString())); // todo: put valid usn instead of "gg"				
			}
		}
	}

	public void TurnDownPopup(){
		Debug.Log ("called");
		Destroy(GameObject.FindWithTag("Popup"));
		Debug.Log ("turned down popup");
	}

	public void UseIt(string treasureID){
		// inform server that this user drops out of the game
		if (!gameObject.GetComponent<NetworkManager> ().enabled) 
		{
			Debug.Log ("Useit called when NetworkManager is not enabled");
		}
		else 
		{
			string [] temp = {treasureID};
			CacheController.instance.SendSignal ("UseTreasure", temp);
		}
		// close popup and delete this gamelist from scroll view
		Destroy(GameObject.Find("Canvas/Scroll View/Viewport/Content/" + treasureID));
		TurnDownPopup();
	}
}
