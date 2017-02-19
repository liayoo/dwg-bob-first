using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SearchButtonController : MonoBehaviour {

	public static SearchButtonController instance = null;
	public static string searchGameData = "";

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

	public GameObject searchButton;

	void Start(){
		Button searchB = searchButton.transform.GetComponent<Button> ();
		searchB.onClick.AddListener(() => GetData("gg"));// todo: usn instead of "gg"
 	}

	public void GetData(string userName)
	{
		if (!gameObject.GetComponent<NetworkManager> ().enabled) 
		{
			TextAsset jsonData = Resources.Load<TextAsset> ("TestForSearch");
			var strJsonData = jsonData.text;
			Debug.Log (strJsonData);
			SetupScrollBar (strJsonData);
		}
		else 
		{
			string str = "{\"flag\":7, \"usn\":\"" + userName + "\"}";
			NetworkManager.instance.SendData (str);
		}
	}

	public void SetupScrollBar(string data)
	{
		// call ForEachGame of ScrollBarContentSetup
		ScrollBarContentSetup.instance.ForEachGame (data, false);
	}
}
