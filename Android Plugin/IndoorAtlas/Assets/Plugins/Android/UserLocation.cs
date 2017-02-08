using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace IA.Plugin
{
	// Class for creating an instance of IndoorAtlas plugin and using its utilities.
	public class UserLocation : MonoBehaviour 
	{
		// UI objects.
		public Button testButton;
		public Text testText;
		public Text longlat;
		// IndoorAtlas plugin.
		private IAPlugin plugin;
		string gameObjectName;

		void Start () 
		{
			Debug.Log ("in UserLocation.Start");
			// In this case, gameObject is Canvas.
			gameObjectName = gameObject.name;
			Debug.Log ("gameObject name: "+gameObjectName);
			// Create a singleton plugin instance with the gameObject's name.
			plugin = IAPlugin.pluginWithGameObjectName (gameObjectName);

			// Button for testing Unity–Android communication.
			// When user clicks on the button, an empty string text ("") in Canvas should be changed.
			testButton.onClick.AddListener (plugin.requestTextFromAndroid);
			Debug.Log ("exiting UserLocation.Start");
		}

		void Update () 
		{
			// Update the Text UI's text to the user's updated location.
			// TODO: update location only when the user location has actually been changed.
			longlat.text = plugin.updateLocation ();
		}

		// A java function (updateText) calls this function using UnitySendMessage.
		public void getTextFromAndroid(string text) 
		{
			Debug.Log ("getting text from android");
			Debug.Log ("gameObject: " + gameObjectName);
			testText.text = text;
		}
	}
}