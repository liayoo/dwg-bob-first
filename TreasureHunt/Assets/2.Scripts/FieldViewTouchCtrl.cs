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
				// Load appropriate scene accroding to the catchGameFlag
				int catchGameFlag = tr.getCatchGameFlag ();
				SceneManager.LoadScene ("H_CatchView2D");////////////
				/*
				// catchGameFlag 1, 2, 3 = slime pang (easy, normal, hard)
				if (catchGameFlag < 3) 
				{
					SceneManager.LoadScene ("H_CatchView");
				} 
				// catchGameFlag 4 = Quiz
				else 
				{
					SceneManager.LoadScene ("H_CatchView2D");
				}
				*/
			}
		}
	}
}
