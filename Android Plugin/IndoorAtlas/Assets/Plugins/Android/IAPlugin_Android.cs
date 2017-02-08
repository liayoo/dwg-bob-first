using UnityEngine;
using UnityEngine.UI;
using System.Collections;


namespace IA.Plugin
{
	// Subclass of IAPlugin for Android platform.
	public class IAPlugin_Android : IAPlugin
	{
		// Uses the base constructor.
		public IAPlugin_Android(string gameObjectName) : base(gameObjectName) { }

		#region Native setup

		static AndroidJavaClass androidClass;
		public static AndroidJavaObject androidInstance { get { return androidClass.GetStatic<AndroidJavaObject> ("instance"); } }


		override protected void Setup() 
		{
			Debug.Log ("in Setup");
			androidClass = new AndroidJavaClass ("com.ylyoo.iaplugin.IA_Plugin");
			Debug.Log ("created AndroidJavaClass");
			androidClass.CallStatic ("start", gameObjectName);
			// This will start requesting for location update.
			//androidInstance.CallStatic ("getUpdateRequestFromUnity");
		}

		#endregion


		// Calls java function 'updateText' in Android IA_Plugin class.
		// updateText will in return call getTextFromAndroid() in UserLocation.cs that is
		// added to Canvas GameObject as a component, and send a string message.
		override public void requestTextFromAndroid() 
		{
			Debug.Log ("requesting text from android");
			androidInstance.Call ("updateText");
		}

		// Gets static longitude and latitude values from the Android instance created and
		// returns a formatted string with the information: (<longitude>, <latitude>).
		// Called in Update() in UserLocation.cs.
		override public string updateLocation()
		{
			string lon = androidInstance.GetStatic<string> ("longitude");
			string lat = androidInstance.GetStatic<string> ("latitude");
			Debug.Log ("updating location to... " + "(" + lon + ", " + lat + ")");
			return "(" + lon + ", " + lat + ")";
		}

	}
}