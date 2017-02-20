using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MyPointsCtrl : MonoBehaviour {

	void Start () {
		GetComponent<Text>().text = LoginButtonCtrl.totPoint.ToString();
	}

}
