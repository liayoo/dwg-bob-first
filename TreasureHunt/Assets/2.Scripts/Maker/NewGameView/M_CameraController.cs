using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace IA.Plugin
{
    public class M_CameraController : MonoBehaviour
    {

        public Button captureButton;
        public GameObject CameraPanel;
        public GameObject MakerCamera;
        public GameObject MainPanel;
        public GameObject PopupPanel;

        void Start()
        {
            captureButton.onClick.AddListener(OnCaptureButton);
        }

        void OnCaptureButton()
        {
            StartCoroutine(MakerSceneManager.instance.uploadToCloud());
            CameraPanel.SetActive(false);
            MakerCamera.SetActive(false);
            MainPanel.SetActive(true);
            PopupPanel.SetActive(true);
        }
    }

}
