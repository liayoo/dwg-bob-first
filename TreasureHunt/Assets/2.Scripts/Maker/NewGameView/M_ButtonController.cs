using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class M_ButtonController : MonoBehaviour 
{
	public Button newTreasureButton;
	public Button saveTreasureButton;
	public Button newGameButton;
	public GameObject scrollbar;
	public GameObject newTreasures;
	Transform popupPanel;
	Transform mainPanel;

	void Start() 
	{
		// Find panels.
		popupPanel = GameObject.Find ("PopupPanel").transform.Find ("Panel");
		mainPanel = GameObject.Find ("MainPanel").transform;
		// Add listeners to buttons.
		saveTreasureButton.onClick.AddListener (AddTreasure);
		newGameButton.onClick.AddListener (SubmitNewGame);
		newTreasureButton.onClick.AddListener(() => { popupPanel.gameObject.SetActive(true); });
	}

	// Called when user clicks Add button to add a new treasure to the game.
	public void AddTreasure()
	{
		Debug.Log ("AddTreasure()");
		string contents = M_PopupController.instance.GetContents ();
		// Do not add a treasure if the name is empty (causes problems!).
		if (!AddScrollContent (contents)) 
		{
			return;
		}
		M_PopupController.instance.ClosePopup ();
	}

	// Called when user adds a treasure.
	// Creates a scrollbar content with the new treasure's information.
	public bool AddScrollContent(string treasureInfo)
	{
		GameObject treasure = (GameObject)Instantiate (newTreasures);
		var jsonData = JSON.Parse (treasureInfo);
		var treasureName = jsonData["treasure_name"];
		// Return false without creating a content panel if the name is empty (causes problems!).
		if (treasureName.ToString() == "") 
		{
			Debug.Log ("Treasure name cannot be empty.");
			return false;
		}
		// Create a new scrollbar content
		M_ScrollBarContent scrollBarContent = treasure.GetComponent<M_ScrollBarContent> ();
		scrollBarContent.data = treasureInfo;
		treasure.transform.SetParent (scrollbar.transform.FindChild("Viewport/Content"));
		treasure.transform.localScale = Vector3.one;
		Debug.Log (treasureName);
		// Show the name of the treasure.
		treasure.transform.FindChild("Name").GetComponent<Text>().text = treasureName;
		// Set the game object name.
		treasure.name = treasureName;
		Button treasureBtn = treasure.GetComponentsInChildren<Button> ()[0];
		treasureBtn.onClick.AddListener (() => 
			{ 
				DeleteTreasure(treasure.name.ToString());
			});
		return true;
	}

	// Called when user clicks a delete button of each treasure content, and destroys the content.
	public void DeleteTreasure(string name)
	{
		Debug.Log("destroying "+name);
		Destroy (scrollbar.transform.FindChild ("Viewport/Content/"+name).gameObject);
	}

	// Called when user clicks a Create! button to submit a new game to the server.
	public void SubmitNewGame()
	{
		Debug.Log ("SubmitNewGame()");
		// TODO: get the player id (maker id)
		string gameData = "{\"flag\":12, \"game_name\":\""+mainPanel.Find("Dynamic Objects/GameNameInput").GetComponent<InputField>().text+
			"\", \"maker_id\":\""+"<maker_id>\", \"Treasures\":[";
		string treasureData = "";
		int numTreasures = scrollbar.transform.childCount;
		Debug.Log ("numTreasures: "+numTreasures);
		// Gather data stored in each of the treasure scrollbar contents.
		foreach (Transform treasure in scrollbar.transform.FindChild("Viewport/Content")) 
		{
			Debug.Log (treasure.name);
			treasureData += treasure.GetComponent<M_ScrollBarContent> ().data.ToString ();
			if (numTreasures > 1) 
			{
				treasureData += ", ";
			}
			numTreasures--;
		}
		gameData += (treasureData + "]}");
		Debug.Log(JSON.Parse (gameData));
		NetworkManager.instance.SendData (gameData);
	}
}
