using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PointText : MonoBehaviour {

    public Slider slider;

	public void SetValue () {
        gameObject.GetComponent<Text>().text = slider.value.ToString();
	}
}
