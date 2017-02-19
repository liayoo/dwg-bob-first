using Vuforia;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FieldViewTouchCtrl : MonoBehaviour {
	RaycastHit hit;
	Ray ray;

	void Update()
	{
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		//Debug.Log(ray);
		if (Physics.Raycast(ray, out hit))
		{
			Debug.Log(" you clicked on " + hit.collider.gameObject.name);

			if (hit.collider.gameObject.tag == "Treasures")
			{
				hit.collider.gameObject.GetComponent<Animator>().SetTrigger("open");
				TreasureAttributes tr = hit.collider.gameObject.GetComponent<TreasureAttributes> ();
				TreasureSetupController.currTargetName = tr.getTargetImg ();
				Debug.Log("The target image is" + TreasureSetupController.currTargetName);
				SceneManager.LoadScene ("HunterCamera");
			}
		}
	}
}
