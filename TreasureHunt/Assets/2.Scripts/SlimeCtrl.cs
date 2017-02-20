using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SlimeCtrl : MonoBehaviour {

    public GameObject modelExplosion;        
    public AnimationClip dead;    

    private Transform slimeTr;
    private NavMeshAgent nav;
    private Animation anim;
    private bool alreadyDie = false;
    private bool isAttack = false;
    private bool isOver = false;
    private Transform targetTr;
    private GameObject gameOver;
    // Use this for initialization
    void Start () {
        slimeTr = gameObject.transform;
        targetTr = GameObject.Find("ImageTarget(Clone)").transform.FindChild("TreasureBox");
        nav = gameObject.GetComponent<NavMeshAgent>();
        anim = gameObject.GetComponent<Animation>();
        gameOver = GameObject.FindGameObjectWithTag("GameOver");
        StartCoroutine(CheckSlimeState());      
    }
    // When touched
    public void Die()
    {        
        if (!alreadyDie && !isOver)
        {
            alreadyDie = true;
            StopCoroutine(CheckSlimeState());            
            StartCoroutine(DieSlowly());
        }
    }
    // Wait for all functions completed until the objects destroyed. 
    IEnumerator DieSlowly()
    {        
        nav.Stop();
        anim.clip = dead;
        anim.Play();        
        GameObject explosion = (GameObject)Instantiate(modelExplosion, slimeTr.position, slimeTr.rotation);
        explosion.transform.SetParent(slimeTr);
        explosion.transform.localScale = Vector3.one;
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(0.4f);
       
        Destroy(explosion);
        MiniGame2Manager.instance.dieCount++;
        Destroy(gameObject);
    }
    IEnumerator CheckSlimeState()
    {
        while (!alreadyDie)
        {
            yield return new WaitForSeconds(0.2f);
            if (!isOver)
            {
                float distance = Vector3.Distance(slimeTr.position, targetTr.position);

                if (distance < 45f)
                {
                    //nav.Stop();
                    isAttack = true;
                }
            }
        }
    }    
    public void GameOver()
    {
        StopAllCoroutines();
        nav.Stop();
        gameOver.transform.localScale = Vector3.one;
        isOver = true;
    }
}
