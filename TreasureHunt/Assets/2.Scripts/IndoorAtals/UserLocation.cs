using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace IA.Plugin
{
	// Class for creating an instance of IndoorAtlas plugin and using its utilities.
	public class UserLocation : MonoBehaviour 
	{
		// UI objects.
		public Button startLocationUpdatesButton;
		public Text lnglat;
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
//			startLocationUpdatesButton.onClick.AddListener (plugin.startLocationUpdates);
			Debug.Log ("exiting UserLocation.Start");
		}

		public UserMovementController umc;
		// A java function (updateText) calls this function using UnitySendMessage.
		public void getLocationChangeNotification(string text) 
		{
			Debug.Log ("getting new user location from android");
			//lnglat.text = plugin.updateLocation ();
			umc = GetComponent<UserMovementController> ();
			umc.ToNewStringSpot (plugin.updateLocation());
		}
	}
}