using Vuforia;
using UnityEngine;
using Vuforia;

using System.Collections;

public class TreasureBoxTouchCtrl : MonoBehaviour {
	RaycastHit hit;
	Ray ray;

	void Update()
	{
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		//HYUN: set Camera Auto-Focus mode
		CameraDevice.Instance.SetFocusMode (CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);

		//Debug.Log(ray);
		if (Physics.Raycast(ray, out hit))
		{
			Debug.Log(" you clicked on " + hit.collider.gameObject.name);

			if (hit.collider.gameObject.tag == "Treasures") {
				hit.collider.gameObject.GetComponent<Animator> ().SetTrigger ("open");
				CacheController.instance.GetContent ("QuizSetup", TreasureSetupController.currTargetName);
			}
		}
	}
}
