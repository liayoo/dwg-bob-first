using UnityEngine;
using System.Collections;

namespace IA.Plugin
{
    public class TargetImageController : MonoBehaviour
    {
		public static TargetImageController instance = null;
        public string targetImage;
        public string locationUpdates;
		void Awake()
		{
			//Debug.Log ("Awake");
			if (instance == null) 
			{
				instance = this;
			} 
			else if (instance != this) 
			{
				Destroy (gameObject);
			}
		}
        public TargetImageController()
        {

        }
    }
}
