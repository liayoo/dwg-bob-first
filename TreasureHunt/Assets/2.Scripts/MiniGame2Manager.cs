using UnityEngine;
using System.Collections;

public class MiniGame2Manager : MonoBehaviour {

    public static MiniGame2Manager instance = null;
    public GameObject slimeModel;
    public Texture[] color;
    public int monsterCount = 10;
    public int dieCount = 0;
    public GameObject mainCanvas;
    public GameObject gameCanvas;
    public GameObject rewardCanvas;
    public float monsterSpeed;

    private int count = 0;
    private Transform parentTr;
    private Transform targetTr;
    private bool gameOver = false;
    
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
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        parentTr = GameObject.Find("ImageTarget(Clone)").transform;
        targetTr = parentTr.FindChild("TreasureBox").transform;
        mainCanvas.SetActive(false);     
        gameCanvas.SetActive(true);
        int cat = GameManager.instance.minigameCat;
        switch (cat)
        {
            case 1:
                monsterCount = 10;
                monsterSpeed = 25;
                break;
            case 2:
                monsterCount = 20;
                monsterSpeed = 30;
                break;
            case 3:
                monsterSpeed = 35;
                monsterCount = 30;
                break;
        }
        StartCoroutine(InstSlime());       
    }

    void Update()
    {
        if(dieCount == monsterCount && !gameOver)
        {
            GameOver();
            
            gameCanvas.SetActive(false);
            targetTr.gameObject.GetComponent<Animator>().SetTrigger("open");
            rewardCanvas.SetActive(true);
            gameOver = true;
        }
    }
    // Instantiate slimes at random position and with random color every 1 second.
    IEnumerator InstSlime()
    {
        while (count < monsterCount)
        {
            
            GameObject slime = Instantiate(slimeModel);
            count++;
            Transform slimeTr = slime.transform;

            slimeTr.SetParent(parentTr, false);
            slimeTr.localPosition = GetRandomPostion();
            slimeTr.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            slimeTr.FindChild("ModelSlime").GetComponent<SkinnedMeshRenderer>().material.mainTexture = color[Random.Range(0, color.Length)];
            NavMeshAgent nav = slime.GetComponent<NavMeshAgent>();
            nav.destination = targetTr.position;
            nav.speed = monsterSpeed;
            yield return new WaitForSeconds(1f);
            
        }        
    }
    // Get raandom position.
    private Vector3 GetRandomPostion()
    {
        float x = 0;
        float z = 0;
        while((x > -0.2f && x < 0.2f) && (z > -0.2f && z < 0.2f))// Avoid the treasure box's positon.
        {
            x = Random.Range(-1f, 1f);
            z = Random.Range(-1f, 1f);
        }
        return new Vector3(x, 0, z);
    }
    public void GameOver()
    {
        StopAllCoroutines();
        gameOver = true;
    }
    public void Retry()
    {
        GameObject[] slimes = GameObject.FindGameObjectsWithTag("Slime");
        foreach(GameObject slime in slimes)
        {
            Destroy(slime);
        }
        monsterCount = 10;
        dieCount = count = 0;
        StartCoroutine(InstSlime());
        GameObject.FindGameObjectWithTag("GameOver").transform.localScale = Vector3.zero;
        targetTr.gameObject.SetActive(true);
        targetTr.gameObject.GetComponent<TreasureBoxCtrl>().hp.fillAmount = 1;
        gameOver = false;
    }
    public void GetReward()
    {
        string str = "{\"flag\":19,\"usn\":" + LoginButtonCtrl.userID + ",\"treasure_id\":" + GameManager.instance.treasure_id + ",\"point\":" + GameManager.instance.point+"}";
        NetworkManager.instance.SendData(str);        
    }
   
}
