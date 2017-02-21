using Vuforia;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FieldViewTouchCtrl : MonoBehaviour {
	RaycastHit hit;
	Ray ray;
    int count = 0;
	void Update()
	{
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		//Debug.Log(ray);
		if (Physics.Raycast(ray, out hit))
		{
			Debug.Log(" you clicked on " + hit.collider.gameObject.name);

			if (hit.collider.gameObject.tag == "Treasures")
			{				
				TreasureAttributes tr = hit.collider.gameObject.GetComponent<TreasureAttributes> ();
				TreasureSetupController.currTargetName = tr.getTargetImg ();
                GameManager.instance.treasure_id = int.Parse(tr.treasureID);
                GameManager.instance.point = tr.treasurePoint;
                GameManager.instance.minigameCat = tr.catchGameFlag;                
                Debug.Log("The target image is" + TreasureSetupController.currTargetName);
                if (count == 0)
                {
                    GameManager.instance.MoveScene("H_CatchView");
                    count++;
                }
			}
		}
	}
}
