using UnityEngine;
using System.Collections;

namespace IA.Plugin 
{
	// Virtual class for the plugin.
	public class IAPlugin 
	{

		#region Native setup
		protected string gameObjectName;

		// Static factory method that deals with runtime platforms.
		public static IAPlugin pluginWithGameObjectName(string gameObjectName)
		{
			IAPlugin plugin;
			// Only considers the case of being Android for now.
			// Can run on a device AND in Editor without getting "JNI: Init'd AndroidJavaObject with null ptr!".
			#if UNITY_ANDROID
			plugin = (Application.isEditor) 
				? (IAPlugin)new IAPlugin_Editor(gameObjectName) 
				: (IAPlugin)new IAPlugin_Android(gameObjectName);
			#endif
			return plugin;
		}

		// Base constructor for the plugin.
		public IAPlugin(string gameObjectName)
		{
			this.gameObjectName = gameObjectName;
			Setup ();
		}

		virtual protected void Setup() { }
		#endregion

		virtual public void requestTextFromAndroid() { }
		virtual public string updateLocation() { return ""; }
	}
}
