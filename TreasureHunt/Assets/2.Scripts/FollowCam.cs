using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour {
    
    public float dist;//set distance
    public float height;//set height
    public float dampTrace = 20f;//to make tracing soft  

    private Transform targetTr;
    private Transform tr;    

    // Use this for initialization
    void Start () {
        tr = GetComponent<Transform>();
        
    }
	
	
	void LateUpdate () {
        
        targetTr = GameObject.FindGameObjectWithTag("TreasureBox").GetComponent<Transform>();
        tr.position = Vector3.Lerp(tr.position, targetTr.position - (targetTr.forward * dist) + (Vector3.up * height), Time.deltaTime * dampTrace);
        tr.LookAt(targetTr.position);
        
    }
    

   
}
