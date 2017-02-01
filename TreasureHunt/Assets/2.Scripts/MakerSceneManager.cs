using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Vuforia;

public class MakerSceneManager: MonoBehaviour {
	static Camera camera;
	float height;
	float width;
	CloudUpLoadManager uploader;

	void start()
	{
		Debug.Log("Start");
	}

	public void uploadToCloud()
	{
		camera = Camera.main;
		if (camera == null) {
			Debug.Log("Null Camera");
			return;
		}
		width = camera.pixelWidth;
		height = camera.pixelHeight;
		Texture2D tex = new Texture2D((int)width, (int)height, TextureFormat.RGB24, false);
		RenderTexture rt = new RenderTexture((int)width, (int)height, 24);
		camera.targetTexture = rt;
		camera.Render();
		RenderTexture.active = rt;
		tex.ReadPixels(camera.pixelRect, 0, 0);
		camera.targetTexture = null;
		RenderTexture.active = null;
		GameObject.DestroyImmediate(rt);
		if (tex != null)
		{
			string targetName = System.DateTime.Now.ToString("yyMMdd_hhmmss");
			Debug.Log("<color=red>Uploading Image</color>");
			StartCoroutine(CloudUpLoadManager.PostNewTarget(tex, targetName));
		}
	}
}
