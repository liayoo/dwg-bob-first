using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TreasureAttributes : MonoBehaviour {

	string treasureID;
	string treasureName;
	string treasureDescription;
	string gameID;
	Vector3 treasureLocation;
	int treasurePoint;
	int catchGameFlag;
	string targetImg;
	string treasureImg;

	public void setAsChildOf(GameObject parent){
		this.transform.parent = parent.transform;
	}

	public void setAttributes(string trId, string trName, string trDes, string gameId, 
		Vector3 trLoc, int trPoint, int trCatchGame, string trTargetImg, string trTreasureImg){
		treasureID = trId;
		treasureName = trName;
		treasureDescription = trDes;
		gameID = gameId;
		treasureLocation = trLoc;
		treasurePoint = trPoint;
		catchGameFlag = trCatchGame;
		targetImg = trTargetImg;
		treasureImg = trTreasureImg;
	}

	public string getTargetImg(){
		return targetImg;
	}

	public int getCatchGameFlag(){
		return catchGameFlag;
	}

	public int getPoint(){
		return treasurePoint;
	}

	public string getTreasureID(){
		return treasureID;
	}
}
