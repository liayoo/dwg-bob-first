using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CatchGameController : MonoBehaviour {
	public Dropdown gameCat;
	public GameObject gameLevelView;
	public GameObject qnaView;

	void Start()
	{
		gameCat.onValueChanged.AddListener (delegate{ gameCatViewChange(); });
	}

	void gameCatViewChange()
	{
		if (gameCat.value == 0) 
		{
			gameLevelView.SetActive (true);
			qnaView.SetActive (false);
		} 
		else 
		{
			gameLevelView.SetActive (false);
			qnaView.SetActive (true);
		}
	}
}
