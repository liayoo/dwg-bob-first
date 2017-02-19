using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UsedButtonController : MonoBehaviour {
	
	public static UsedButtonController instance = null;

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

	public GameObject usedButton;

	void Start(){
		Button searchB = usedButton.transform.GetComponent<Button> ();
		searchB.onClick.AddListener(() => SetupScrollBar("gg"));// todo: usn instead of "gg"
	}

	public void SetupScrollBar(string userName)
	{
		// make new ones, that is, search game lists
//		InventorySetupController.instance.ForEachItem (userName, true);
	}

}
