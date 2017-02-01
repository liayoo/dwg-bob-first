using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(UserMovementController))]
public class UserMovementControllerEditor : Editor 
{
	string newLocation;

	public override void OnInspectorGUI()
	{
		UserMovementController myCon = (UserMovementController) target;

		newLocation = EditorGUILayout.TextField("Position", newLocation);
		EditorGUILayout.LabelField ("Location", newLocation);
		if(GUILayout.Button("Send Position"))
		{
			myCon.ToNewStringSpot(newLocation);
		}
	}
			
}
