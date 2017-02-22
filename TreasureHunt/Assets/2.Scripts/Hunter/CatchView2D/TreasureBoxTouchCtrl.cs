using Vuforia;
using UnityEngine;

using System.Collections;

public class TreasureBoxTouchCtrl : MonoBehaviour {
	RaycastHit hit;
	Ray ray;
    static Camera camera;
    float height;
    float width;
    int count = 0;
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
                camera = Camera.main;
                if (camera == null)
                {
                    Debug.Log("Null Camera");
                }
                width = camera.pixelWidth;
                height = camera.pixelHeight;
                Texture2D tex = new Texture2D((int)width, (int)width, TextureFormat.RGB24, false);
                RenderTexture rt = new RenderTexture((int)width, (int)height, 24);
                camera.targetTexture = rt;
                camera.Render();
                RenderTexture.active = rt;

                Rect cropImageRect = new Rect(0, (height - width) / 2, width, width);
                tex.ReadPixels(cropImageRect, 0, 0);

                Utility.SaveImage(GameManager.instance.treasure_id + "_.jpg", tex.EncodeToPNG());

                camera.targetTexture = null;
                RenderTexture.active = null;
                GameObject.DestroyImmediate(rt);
                //hit.collider.gameObject.GetComponent<Animator>().SetTrigger("open");
                //CacheController.instance.GetContent ("QuizSetup", TreasureSetupController.currTreasureID.ToString());
                if (count == 0)
                {
                    count++;
                    QuizGameSetup.instance.QuizSetup();                    
                }
			}
		}
	}
}
