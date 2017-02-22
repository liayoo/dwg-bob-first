
using UnityEngine;
using System.Collections;
using SimpleJSON;
using UnityEngine.UI;
using System;

public class InventorySetupController : MonoBehaviour
{
    public static InventorySetupController instance = null;
    public Sprite targetImg;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public GameObject scrollbar;
    public GameObject treasureList;

    public void ForEachItem(string data, int isUsed)
    {
        // Destroy old treasure lists, that is, usedLists or old unusedists
        GameObject content = GameObject.Find("Canvas/Panel/Scroll View/Viewport/Content");
        for (int i = 0; i < content.transform.childCount; i++)
        {
            Destroy(content.transform.GetChild(i).gameObject);
        }
        // make new treasure lists, that is, myGameLists
        var jsonData = JSON.Parse(data);
        var treasures = jsonData["user_info"];
        // for each treasure
        for (int i = 0; i < treasures.Count; i++)
        {
            var curr = treasures[i];
            if (curr["used"].AsInt == isUsed)
            {
                // make new treasure list
                GameObject newTreasure = (GameObject)Instantiate(treasureList, new Vector3(0, 0, 0), Quaternion.identity);
                newTreasure.transform.parent = scrollbar.transform.FindChild("Viewport/Content");
                newTreasure.transform.localScale = Vector3.one;
                newTreasure.tag = "TreasureList";
                // attach attributes
                newTreasure.name = curr["treasure_id"].AsInt.ToString();
                newTreasure.transform.FindChild("TreasureName").GetComponent<Text>().text = curr["treasure_name"];
                newTreasure.transform.FindChild("Description").GetComponent<Text>().text = curr["description"];
                newTreasure.transform.FindChild("Point").GetComponent<Text>().text += curr["point"].AsInt.ToString();
                newTreasure.transform.FindChild("DateTime").GetComponent<Text>().text = curr["date_time"];
                
                targetImg = Utility.CreateSprite(newTreasure.name + "_");
                
                newTreasure.transform.FindChild("Image").GetComponent<Image>().sprite = targetImg;
                
                // attach onclick event
                if (isUsed == 0)
                {
                    Button btn = newTreasure.GetComponent<Button>();
                    btn.onClick.AddListener
                    (
                        delegate {
                            CacheController.instance.GetContent("UnusedInventoryPopup", curr["treasure_id"].AsInt.ToString());
                        }
                    );
                }
            }
        }
    }
}
