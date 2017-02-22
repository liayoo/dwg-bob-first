using UnityEngine;
using System.Collections;
using SimpleJSON;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
			// client side: 
			//add up treasure point 
			LoginButtonCtrl.totPoint += TreasureSetupController.currPoint;
			//get rid of treasure box of THIS treasure on field view
			LoginButtonCtrl.treasuresIGot [LoginButtonCtrl.treasuresIGotNextIndex++] = TreasureSetupController.currTreasureID;

			// to server:
			// inform server that the user got the treasure
			string[] temp = {TreasureSetupController.currTreasureID};
			CacheController.instance.SendSignal("GetTreasure", temp);

			// client UI:
			// turn down popup
			Destroy (popup);
			// move back to Field View
			SceneManager.LoadScene("H_FieldView");
		} 
		// if answer is wrong
		else 
		{
			responseField.text = "";
		}
			
	}



}
