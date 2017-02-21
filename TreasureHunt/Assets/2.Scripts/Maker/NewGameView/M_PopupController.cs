using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using SimpleJSON;

/*
 * Called when user clicks on the + button to add a treasure to the new game she/he is about to create.
 * A pop-up needs to have:
 * 		Treasures
 * 			- treasure_name
 * 			- description
 * 
 * 			- location
 * 			- points
 * 			- catchgame_cat
 * 
 */
public class M_PopupController : MonoBehaviour {
	// Singleton instance.
	public static M_PopupController instance = null;
	public ScrollSnapRect scrollSnapRect;
	string data;
	InputField treasureName; 
	InputField description; 
	Slider points; 
	Dropdown catchGame; 
	string location;
    public GameObject popupPanel;
	void Awake()
	{
		Debug.Log ("Awake");
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
		Debug.Log ("Start");
        popupPanel.gameObject.GetComponent<Button> ().onClick.AddListener (ClosePopup);
		foreach (GameObject popup in GameObject.FindGameObjectsWithTag ("Popup")) 
		{
			if (popup.name == "Popup1") 
			{
				// Initialize the scrollSnapRect with starting page index of 0.
				scrollSnapRect.Init (0);
				scrollSnapRect.LerpToPage (0);
				// Get user input components for name, description, points and catch game category of the treasure.
				treasureName = popup.transform.Find ("Dynamic Objects/TreasureNameInput").GetComponent<InputField> ();
				description = popup.transform.Find ("Dynamic Objects/DescriptionInput").GetComponent<InputField> ();
				points = popup.transform.Find ("Dynamic Objects/PointsSlider").GetComponent<Slider> ();
				catchGame = popup.transform.Find ("Dynamic Objects/CatchGameDropdown").GetComponent<Dropdown> ();

			} 
			else if (popup.name == "Popup2") 
			{
				// Do nothing.
				//targetImage = "https://ipetcompanion.com/feedapuppy/styles/media/puppy.jpg";
				// record user location (IA) at the time of taking the target image and then set the treasure location according to the lng,lat?

			} 
			else 
			{
				// Get treasure location
				// TODO: let them see a floor plan and choose a point from there?
				location = "(37.1234, 0.0, 128.1234)";
			}
		}
	}


	// Called when a treasure is added to gather user's input.
	public string GetContents()
	{
		Debug.Log ("GetContents()");
		// TODO: Fix this.
		location = "(37.1234, 0.0, 128.1234)";
		// Go back to the first pop-up page, if the name is null
		if (treasureName.text == "") {
			scrollSnapRect.Init (0);
			scrollSnapRect.LerpToPage (0);
		}
		data = "{\"treasure_name\":\""+treasureName.text+"\", \"description\":\""+description.text
			+"\", \"location\":\""+location+"\", \"point\":\""+points.value.ToString()
			+"\", \"catchgame_cat\":\""+catchGame.value.ToString()+"\"}";
		return data;
	}

	// Called when the area outside of the pop-up has been clicked, 
	// or the new treasure has been added.
	public void ClosePopup()
	{
		Debug.Log ("ClosePopup()");
		treasureName.text = "";
		description.text = "";
		points.value = 0;
		catchGame.value = 0;
		location = "";
		if (popupPanel.activeInHierarchy) 
		{
            popupPanel.SetActive (false);
		}
		scrollSnapRect.Init (0);
		scrollSnapRect.LerpToPage (0);
		Debug.Log ("Popup closed");
	}


}
