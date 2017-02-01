/*
 * Special Thanks to tompuz...
 * reference: https://github.com/tompuz/magistrinis
 */

using System;
using UnityEngine;
using Vuforia;

namespace Assets.Scripts.CloudRecognition
{
	/// <summary>
	/// This MonoBehaviour implements the Cloud Reco Event handling for this sample.
	/// It registers itself at the CloudRecoBehaviour and is notified of new search results.
	/// </summary>
	public class SimpleCloudHandler : MonoBehaviour, ICloudRecoEventHandler
	{
		// CloudRecoBehaviour reference to avoid lookups
		private CloudRecoBehaviour mCloudRecoBehaviour;
		// ImageTracker reference to avoid lookups
		private ObjectTracker mImageTracker;
		private bool mIsScanning = false;
		private string mTargetMetadata = "";
		/// can be set in the Unity inspector to reference a ImageTargetBehaviour that is used for augmentations of new cloud reco results.
		public ImageTargetBehaviour ImageTargetTemplate;

		/// register for events at the CloudRecoBehaviour
		void Start()
		{
			// register this event handler at the cloud reco behaviour
			mCloudRecoBehaviour = GetComponent<CloudRecoBehaviour>();
			if (mCloudRecoBehaviour)
			{
				mCloudRecoBehaviour.RegisterEventHandler(this);
			}
		}

		/// called when TargetFinder has been initialized successfully
		public void OnInitialized()
		{
			// get a reference to the Image Tracker, remember it
			mImageTracker = (ObjectTracker) TrackerManager.Instance.GetTracker<ObjectTracker>();
			Debug.Log ("Cloud Reco initialized");
		}

		/// visualize initialization errors
		public void OnInitError(TargetFinder.InitState initError)
		{
			Debug.Log ("Cloud Reco init error " + initError.ToString());
		}

		/// visualize update errors
		public void OnUpdateError(TargetFinder.UpdateState updateError)
		{
			Debug.Log ("Cloud Reco update error " + updateError.ToString());
		}

		/// when we start scanning, unregister Trackable from the ImageTargetTemplate, then delete all trackables
		public void OnStateChanged(bool scanning)
		{
			mIsScanning = scanning;
			if (scanning)
			{
				// clear all known trackables
				ObjectTracker tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
				tracker.TargetFinder.ClearTrackables(false);
			}
		}

		/// Handles new search results
		/// <param name="targetSearchResult"></param>
		public void OnNewSearchResult(TargetFinder.TargetSearchResult targetSearchResult)
		{
			// duplicate the referenced image target
			GameObject newImageTarget = Instantiate(ImageTargetTemplate.gameObject) as GameObject;
			GameObject augmentation = null;
			string model_name = targetSearchResult.MetaData;

			if (augmentation != null)
				augmentation.transform.parent = newImageTarget.transform;

			// enable the new result with the same ImageTargetBehaviour:
			ImageTargetAbstractBehaviour imageTargetBehaviour =
				mImageTracker.TargetFinder.EnableTracking(targetSearchResult, newImageTarget);
			Debug.Log("Metadata value is " + model_name);
			mTargetMetadata = model_name;

			if (!mIsScanning)
			{
				// stop the target finder
				mCloudRecoBehaviour.CloudRecoEnabled = true;
			}
		}
	}
}
