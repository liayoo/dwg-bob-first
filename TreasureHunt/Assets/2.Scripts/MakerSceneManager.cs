using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Vuforia;


namespace IA.Plugin
{
    public class MakerSceneManager : MonoBehaviour
    {
        static Camera camera;
        float height;
        float width;
        public string targetName;
        public string location;
        CloudUpLoadManager uploader;
        IAPlugin plugin;
        public bool callback;

		public GameObject CameraPanel;
		public GameObject MakerCamera;

        public static MakerSceneManager instance = null;

        void Awake()
        {
            Debug.Log("Awake");
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        void start()
        {
            Debug.Log("Start");
            targetName = "";
            callback = false;
            plugin = IAPlugin.pluginWithGameObjectName(gameObject.name);
        }

        public IEnumerator uploadToCloud()
        {
            camera = Camera.main;
            if (camera == null)
            {
                Debug.Log("Null Camera");
                StopCoroutine(uploadToCloud());
            }
            width = camera.pixelWidth;
            height = camera.pixelHeight;
            Texture2D tex = new Texture2D((int)width, (int)width, TextureFormat.RGB24, false);
            RenderTexture rt = new RenderTexture((int)width, (int)height, 24);
            camera.targetTexture = rt;
            camera.Render();
            RenderTexture.active = rt;

            /* hyunSo
			 * Note that Vuforia has a 2MB limit on the image to be uploaded.
			 * The image is cropped here to ensure we don't exceed that limit.
			 * TODO: know the size before upload for notification
			 * 		 (user for restrict the size within the 2MB limit)
			 */
            Rect cropImageRect = new Rect(0, (height - width) / 2, width, width);
            tex.ReadPixels(cropImageRect, 0, 0);

            camera.targetTexture = null;
            RenderTexture.active = null;
            GameObject.DestroyImmediate(rt);
            if (tex != null)
            {
                targetName = System.DateTime.Now.ToString("yyMMdd_hhmmss");
                Debug.Log("<color=red>Uploading Image</color>");
                StartCoroutine(CloudUpLoadManager.PostNewTarget(tex, targetName));
				CameraPanel.SetActive(false);
				MakerCamera.SetActive(false);
            }

            while (!callback)
            {
                yield return null;
            }
				
            //GameObject.Find("Canvas").GetComponent<TargetImageController>().targetImage = targetName;
            GameObject.Find("Canvas").GetComponent<TargetImageController>().locationUpdates = location;
        }

        public void getLocationChangeNotification(string text)
        {
            Debug.Log("getting new user location from android");
            location = plugin.updateLocation();
        }
    }
}