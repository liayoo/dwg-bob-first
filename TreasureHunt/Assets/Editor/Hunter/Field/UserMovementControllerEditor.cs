using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(UserMovementController))]
public class UserMovementControllerEditor : Editor 
{
	Vector3 newLocation;

	public override void OnInspectorGUI()
	{
		UserMovementController myCon = (UserMovementController) target;

		newLocation = EditorGUILayout.Vector3Field("Position", newLocation);

		if(GUILayout.Button("Send Position"))
		{
			myCon.ToNewSpot(newLocation);
		}
	}
			
}
