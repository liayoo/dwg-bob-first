using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

namespace IA.Plugin {
	public class TreasureImageToggle : MonoBehaviour {
		public Toggle treasureImg;
		public Sprite unselected;
		public Sprite selected;

		void Start()
		{
			changeGraphic (treasureImg);
			treasureImg.GetComponent<Toggle>().onValueChanged.AddListener (delegate {
				changeGraphic (treasureImg);
				M_PopupController.instance.treasureImage = Convert.ToInt32(treasureImg.name);
			});
		}

		public void changeGraphic(Toggle t)
		{
			if (t.isOn) {
				((Image)t.targetGraphic).overrideSprite = selected;
			} else {
				((Image)t.targetGraphic).overrideSprite = unselected;
			}
		}
	}
}