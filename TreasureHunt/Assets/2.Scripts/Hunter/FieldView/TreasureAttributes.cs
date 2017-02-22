using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TreasureAttributes : MonoBehaviour
{

    public string treasureID;
    public string treasureName;
    public string treasureDescription;
    public string gameID;
    public Vector3 treasureLocation;
    public int treasurePoint;
    public int catchGameFlag;
    public string targetImg;
    public string treasureImg;
    public string question;
    public string answer;

    public void setAsChildOf(GameObject parent)
    {
        this.transform.parent = parent.transform;
    }

    public void setAttributes(string trId, string trName, string trDes, string gameId,
       Vector3 trLoc, int trPoint, int trCatchGame, string trTargetImg, string trTreasureImg, string question, string answer)
    {
        treasureID = trId;
        treasureName = trName;
        treasureDescription = trDes;
        gameID = gameId;
        treasureLocation = trLoc;
        treasurePoint = trPoint;
        catchGameFlag = trCatchGame;
        targetImg = trTargetImg;
        treasureImg = trTreasureImg;
        this.question = question;
        this.answer = answer;
    }

    public string getTargetImg()
    {
        return targetImg;
    }

	public int getCatchGameFlag()
	{
		return catchGameFlag;
	}

	public int getPoint()
	{
		return treasurePoint;
	}

	public string getTreasureID()
	{
		return treasureID;
	}
}
