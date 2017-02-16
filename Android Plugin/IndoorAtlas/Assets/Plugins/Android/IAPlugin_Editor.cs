using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace IA.Plugin
{
	// Subclass of IAPlugin for Unity Editor without any meaningful functionalities.
	// Exists just so that Unity doesn't throw "JNI: Init'd AndroidJavaObject with null ptr!" error.
	public class IAPlugin_Editor : IAPlugin 
	{
		// Uses the base constructor.
		public IAPlugin_Editor(string gameObjectName) : base(gameObjectName) { }
		
		// necessary overriding functions
		protected override void Setup () { }
		override public void requestTextFromAndroid() { }
		override public string updateLocation() { return ""; }
	}
}