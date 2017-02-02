using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScrollBarCtrl : MonoBehaviour {

	public Scrollbar scrollbar;
	private bool dragging;

	// only show scroll bar when scrolling

	void Update()
	{
		if (dragging) 
		{
			scrollbar.gameObject.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);
		} 
		else 
		{
			scrollbar.gameObject.GetComponent<RectTransform> ().localScale = new Vector3 (0, 0, 0);
			GetComponent<ScrollRect>().verticalScrollbarSpacing = -20;
		}
	}

	// Following functions are linked to event triggers of ScrollView object
	public void StartDrag()
	{
		dragging = true;
	}

	public void EndDrag()
	{
		dragging = false;
	}
}
