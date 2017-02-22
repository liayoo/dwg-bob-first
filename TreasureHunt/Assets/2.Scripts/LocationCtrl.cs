using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LocationCtrl : MonoBehaviour, IPointerDownHandler
{

    public static LocationCtrl instance = null;

    RaycastHit hit;
    Ray ray;
    public GameObject canvas;
    public string location;

    void Awake()
    {
        Debug.Log("Awake");
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Vector2 pos = eventData.position;
        //location = "(" + pos.x + ",0,"+pos.y+")";
        //Debug.Log(location);
        gameObject.SetActive(false);
        //Debug.Log(ray);
        //if (canvas.GetComponent<GraphicRaycaster>().Raycast(ray, out hit))
        //{           
        //    Debug.Log(hit.point);
        //}
    }
}
