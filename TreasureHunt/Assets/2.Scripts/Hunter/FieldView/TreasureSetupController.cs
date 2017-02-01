using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
//using SimpleJSON;

public class TreasureSetupController : MonoBehaviour {
	
	//infos included in json:
	//	usn, game_id, 
	//	game_name, treasure_count, maker_id, status, participant
	//	treasure_id, treasure_name, description, game_id, location, point, catchgame_cat, target_img_url

	public GameObject treasure;
	public static List<string> m_Data = new List<string>();

	void ForEachGame(){
		/*
		string data = "";
		foreach(string info in m_Data){ 
			if(info!=null){
				data = info;
				m_Data.Remove(info);
				break;
			}
		}
		if(data.Length != 0){
			var jsonData = JSON.Parse(data);
			int howMany = jsonData ["treasure_count"].AsInt;
			for(int i=0; i<howMany; i++){
				MakeNewTreasure ();
			}
		}
		*/
		/*
		//todo: get 
		GameObject newGame = (GameObject) Instantiate ();
		int howMany;
		for (int i = 0; i < howMany; i++) {
			MakeNewTreasure ();
		}
		return;
		*/
	}


	TreasureAttributes MakeNewTreasure(GameObject parent, string trId, string trName, string trDes, 
		string gameId, string trLoc, int trPoint, int trCatchGame, string trTargetImg){
		TreasureAttributes newTreasure = (TreasureAttributes) Instantiate (treasure, StringToVector3(trLoc), Quaternion.identity);
		newTreasure.setAttributes (trId, trName, trDes, gameId, trLoc, trPoint, trCatchGame, trTargetImg);
		newTreasure.setAsChildOf (parent);
		return newTreasure; 
	}

	Vector3 StringToVector3 (string str){

		// define where to splict string
		char[] delimiterChars = { ',' };

		// get rid of empty spaces
		str.Replace(" ", "");

		// get rid of parentheses
		if (str [0] == '(' && str [str.Length - 1] == ')') {
			str = str.Substring (1, str.Length - 2);
		}

		// divide string
		string[] words = str.Split (delimiterChars);

		//check if it's parse properly
		if (words.Length != 3) {
			Debug.Log ("incorrect treasue location format in StringToVector3");
		}

		// get x, y, z of target vector3
		float x = float.Parse (words[0], CultureInfo.InvariantCulture.NumberFormat);
		float y = float.Parse (words[1], CultureInfo.InvariantCulture.NumberFormat);
		float z = float.Parse (words[2], CultureInfo.InvariantCulture.NumberFormat);

		//Todo: if y should be 0, double check and make sure it must be 0

		//construct vector3 from x, y, z and return it;
		Vector3 result = new Vector3 (x, y, z);
		return result;
	}

}
