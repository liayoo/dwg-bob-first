using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(UserMovementController))]
public class UserMovementControllerEditor : Editor 
{
	

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector ();

		UserMovementController myCon = (UserMovementController) target;

		if(GUILayout.Button("Send Position"))
		{
			myCon.ForTest ();
		}
	}
			
}
