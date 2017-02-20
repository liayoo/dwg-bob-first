using UnityEngine;
using System.Collections;
using SimpleJSON;
using UnityEngine.UI;

public class QuizGameSetup : MonoBehaviour {

	public static QuizGameSetup instance = null;

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

	public GameObject quizPopup;

	public void QuizSetup(string data)
	{
		// parsing json data using SimpleJSON
		var jsonData = JSON.Parse (data);
		var quizInfo = jsonData ["treasure_info"];
		// make popup
		GameObject popup = (GameObject) Instantiate (quizPopup);
		// give question
		popup.transform.FindChild("Panel/ScrollView/Content/Text").GetComponent<Text>().text = quizInfo ["question"];
		// attach button listener
		popup.transform.FindChild("Panel/Button").GetComponent<Button>().onClick.AddListener(() => CheckRightOrWrong(popup, quizInfo ["answer"]));

	}

	public void CheckRightOrWrong(GameObject popup, string answer)
	{
		// check if the answer is right or wrong
		InputField responseField = popup.transform.FindChild ("Panel/InputField").GetComponent<InputField> ();
		string response = responseField.text;
		// if answer is right
		if (response.Equals (answer)) 
		{
			// client side: add up treasure point, get rid of treasure box of THIS treasure on field view
			LoginButtonCtrl.totPoint += TreasureSetupController.currPoint;
			// todo: getting rid of the treasure would be done by server 
			// inform server that the user got the treasure
			string[] temp = {TreasureSetupController.currTreasureID.ToString()};
			CacheController.instance.SendSignal("GetTreasure", temp);
			// turn down popup
			Destroy (popup);
		} 
		// if answer is wrong
		else 
		{
			responseField.text = "";
		}
			
	}



}
