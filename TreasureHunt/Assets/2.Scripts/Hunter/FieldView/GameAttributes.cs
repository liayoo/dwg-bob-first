using UnityEngine;
using System.Collections;

public class GameAttributes : MonoBehaviour {

	string gameID;
	string gameName;
	int treasureCount;
	string makerID;
	int status;
	int participant;

	public void SetAsChildOf(GameObject parent){
		this.transform.parent = parent.transform;
	}

	public void SetAttributes(string gId, string gName, int tCount, string makerId, int stat){
		gameID = gId;
		gameName = gName;
		treasureCount = tCount;
		makerID = makerId;
		status = stat;
	}
}
