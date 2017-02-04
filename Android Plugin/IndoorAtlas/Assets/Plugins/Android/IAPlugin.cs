using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IAPlugin : MonoBehaviour {

	#if UNITY_ANDROID

	public Text someText;
	public Button someButton;
	AndroidJavaObject _activity;
	AndroidJavaClass jc;


	// Use this for initialization
	void Start () {
		AndroidJavaClass unity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		_activity = unity.GetStatic<AndroidJavaObject> ("currentActivity");
		//_activity.Call ("Launch");
		//jc = new AndroidJavaClass ("com.ylyoo.iaplugin.IA_Plugin");
		//jc.Call ("Launch");
		Button btn = someButton.GetComponent<Button> ();
		btn.onClick.AddListener (requestTextFromAndroid);
		/*_activity.Call ("updateText");*/
		//AndroidJavaClass androidClass = new AndroidJavaClass ("com.ylyoo.iaplugin.IA_Plugin");
		//androidClass.Call ("updateText");
	}

	#endif
	
	// Update is called once per frame
	void Update () {

	}

	// calls java function 'updateText' in IA_Plugin class
	public void requestTextFromAndroid() {
		_activity.Call ("updateText");
	}

	// java function (updateText) calls this function using UnitySendMessage
	public void getTextFromAndroid(string text) {
		someText.text = text;
	}
}
