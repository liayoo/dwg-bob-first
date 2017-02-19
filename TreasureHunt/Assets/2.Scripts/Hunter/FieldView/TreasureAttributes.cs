using UnityEngine;
using System.Collections;

public class TreasureAttributes : MonoBehaviour {

	string treasureID;
	string treasureName;
	string treasureDescription;
	string gameID;
	Vector3 treasureLocation;
	int treasurePoint;
	int catchGameFlag;
	string treasureImageName;
	string targetImageName;

	public void setAsChildOf(GameObject parent){
		this.transform.parent = parent.transform;
	}

	public void setAttributes(string trId, string trName, string trDes, string gameId, 
		Vector3 trLoc, int trPoint, int trCatchGame, string trImg, string targetImg){
		treasureID = trId;
		treasureName = trName;
		treasureDescription = trDes;
		gameID = gameId;
		treasureLocation = trLoc;
		treasurePoint = trPoint;
		catchGameFlag = trCatchGame;
		treasureImageName = trImg;
		targetImageName = targetImg;
	}

}
