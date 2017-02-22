using UnityEngine;
using System.Collections;
using SimpleJSON;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuizGameSetup : MonoBehaviour {

	public static QuizGameSetup instance = null;
    public GameObject canvas;
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

	public void QuizSetup()
	{
		// parsing json data using SimpleJSON
		//var jsonData = JSON.Parse (data);
		//var quizInfo = jsonData ["treasure_info"];
		// make popup
		GameObject popup = (GameObject) Instantiate (quizPopup);
        popup.transform.SetParent(canvas.transform);
        popup.transform.localScale = Vector3.one;
        popup.transform.localPosition = new Vector3(0, 0, 0);
        // give question
        Debug.Log(GameManager.instance.question);
        popup.transform.FindChild("Panel/Scroll View/Viewport/Content/Text").GetComponent<Text>().text = GameManager.instance.question;
        // attach button listener
        popup.transform.FindChild("Panel/Button").GetComponent<Button>().onClick.AddListener(() => CheckRightOrWrong(popup, GameManager.instance.answer));

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
			LoginButtonCtrl.treasuresIGot [LoginButtonCtrl.treasuresIGotNextIndex++] = TreasureSetupController.currTreasureID.ToString();

            // to server:
            // inform server that the user got the treasure
            string str = "{\"flag\":19,\"usn\":" + LoginButtonCtrl.userID + ",\"treasure_id\":" + GameManager.instance.treasure_id + ",\"point\":" + GameManager.instance.point + "}";
            NetworkManager.instance.SendData(str);
            // client UI:
            // turn down popup
            Destroy (popup);
			// move back to Field View
			SceneManager.LoadScene("H_Field");
		} 
		// if answer is wrong
		else 
		{
			responseField.text = "";
		}
			
	}



}
