using Vuforia;
using UnityEngine;
using System.Collections;

public class TouchCtrl : MonoBehaviour {
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

            if (hit.collider.gameObject.tag == "Treasures")
            {
                hit.collider.gameObject.GetComponent<Animator>().SetTrigger("open");
            }
        }
    }
}
