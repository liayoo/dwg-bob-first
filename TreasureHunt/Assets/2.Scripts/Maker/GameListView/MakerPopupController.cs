using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MakerPopupController : MonoBehaviour {

	public static MakerPopupController instance = null;

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

	public GameObject panel;
	public ScrollSnapRect scrollSnapRect;

	void Start()
	{
		Debug.Log ("Start");
		panel.GetComponent<Button> ().onClick.AddListener (ClosePopup);
		foreach (GameObject popup in GameObject.FindGameObjectsWithTag ("Popup")) 
		{
			if (popup.name == "Popup1") 
			{
				// Initialize the scrollSnapRect with starting page index of 0.
				scrollSnapRect.Init (0);
				scrollSnapRect.LerpToPage (0);
				// to do:
			} 
			else 
			{
				// to do:

			} 
		}
	}

	public GameObject popupPanel;

	public void OnGamePopup(string data, string gameID){
		panel.SetActive(true);
		// todo: attach scroll contents on panels
	}



	// Called when the area outside of the pop-up has been clicked, 
	// or the new treasure has been added.
	public void ClosePopup()
	{
		Debug.Log ("ClosePopup()");
		if (panel.activeInHierarchy) 
		{
			panel.SetActive (false);
		}
		scrollSnapRect.Init (0);
		scrollSnapRect.LerpToPage (0);
		Debug.Log ("Popup closed");
	}
}
