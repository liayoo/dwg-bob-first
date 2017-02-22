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
 * 			- location
 * 			- point
 * 			- catchgame_cat
 * 			- target_img_name
 * 			- treasure_img_name (int)
 * 			- question
 * 			- answer
 * 
 */
namespace IA.Plugin {

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
		public string targetImage;
		public int treasureImage;
		string question;
		string answer;
		GameObject popup1;
		GameObject popup2;
		GameObject popup3;

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
			GameObject.Find ("Canvas/PopupPanel/Panel").gameObject.GetComponent<Button> ().onClick.AddListener (ClosePopup);
			foreach (GameObject popup in GameObject.FindGameObjectsWithTag ("Popup")) 
			{
				if (popup.name == "Popup1") 
				{
					popup1 = popup;
					// Initialize the scrollSnapRect with starting page index of 0.
					scrollSnapRect.Init (0);
					scrollSnapRect.LerpToPage (0);
					// Get user input components for name, description, points and catch game category of the treasure.
					treasureName = popup.transform.Find ("Dynamic Objects/TreasureNameInput").GetComponent<InputField> ();
					points = popup.transform.Find ("Dynamic Objects/PointsSlider").GetComponent<Slider> ();
					catchGame = popup.transform.Find ("Dynamic Objects/CatchGameDropdown").GetComponent<Dropdown> ();
					question = " ";
					answer = " ";

				} 
				else if (popup.name == "Popup2") 
				{
					popup2 = popup;
					description = popup.transform.Find ("TreasureDesc/DescriptionInput").GetComponent<InputField> ();
					targetImage = "";
					location = "";
				} 
				else 
				{
					popup3 = popup;
					// Let user choose a treasure image.
					//treasureImage = "https://ipetcompanion.com/feedapuppy/styles/media/puppy.jpg";
					treasureImage = 0;
				}
			}
		}

		// Called when a treasure is added to gather user's input.
		public string GetContents()
		{
			Debug.Log ("GetContents()");
			// Go back to the first pop-up page, if the name is null
			if (treasureName.text == "") {
				scrollSnapRect.Init (0);
				scrollSnapRect.LerpToPage (0);
			}
//			* 			- treasure_name
//			* 			- description
//			* 			- location
//			* 			- point
//			* 			- catchgame_cat
//			* 			- target_img_name
//			* 			- treasure_img_name (int)
//			* 			- question
//			* 			- answer
			// Mini game categories (1: easy slime pop, 2: medium slime pop, 3: hard slime pop, 4: quiz)
			int gameCat = 4;
			if (catchGame.value == 0) 
			{
				gameCat = (int)popup1.transform.Find ("GameLevels").GetComponentInChildren<Slider> ().value;
			} 
			else 
			{
				question = popup1.transform.Find ("QnA/QuestionInput").GetComponent<InputField> ().text;
				answer = popup1.transform.Find ("QnA/AnswerInput").GetComponent<InputField> ().text;
			}
			// Get the target image name
			targetImage = GameObject.Find ("Canvas").GetComponent<TargetImageController> ().targetImage;
            // Get the treasure location
            //location = GameObject.Find ("Canvas").GetComponent<TargetImageController> ().locationUpdates;
            location = "(127.0362, 0.0, 37.50005)";

			data = "{\"treasure_name\":\"" + treasureName.text + "\", \"description\":\"" + description.text
				+ "\", \"location\":\"" + location + "\", \"point\":" + points.value.ToString ()
				+ ", \"catchgame_cat\":" + gameCat.ToString () + ", \"target_img_name\":\"" + targetImage
				+ "\", \"treasure_img_name\":" + treasureImage.ToString () + ", \"question\":\"" + question
				+ "\", \"answer\":\"" + answer + "\"}";

			Debug.Log ("New treasure: " + data);
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
			targetImage = "";
			treasureImage = 0;
			question = " ";
			answer = " ";
			if (GameObject.Find ("Canvas/PopupPanel/Panel").activeInHierarchy) 
			{
				GameObject.Find ("Canvas/PopupPanel/Panel").SetActive (false);
			}
			scrollSnapRect.Init (0);
			scrollSnapRect.LerpToPage (0);
			Debug.Log ("Popup closed");
		}




	}

}