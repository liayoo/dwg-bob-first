using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TreasureBoxCtrl : MonoBehaviour {
    public Image hp;
    public GameObject modelExplosion;
    public AudioClip kick;
    public AudioClip destroy;

    private AudioSource audioSouce;

    void Start()
    {
        audioSouce = GetComponent<AudioSource>();
    }
	void OnTriggerEnter(Collider coll)
    {
        Debug.Log("Collide");
        if (coll.gameObject.tag == "Slime")
        {
            Debug.Log("Treasure attacked");
            audioSouce.clip = kick;
            audioSouce.Play();
            hp.fillAmount -= 0.4f;
            if (hp.fillAmount <= 0)
            {
                StartCoroutine(DestroyTreasureBox());
            }
        }
    }

    IEnumerator DestroyTreasureBox()
    {
        GameObject explosion = (GameObject)Instantiate(modelExplosion, gameObject.transform.position, gameObject.transform.rotation);
        explosion.transform.SetParent(gameObject.transform);
        explosion.transform.localScale = Vector3.one;
        audioSouce.clip = destroy;
        audioSouce.Play();
        yield return new WaitForSeconds(0.4f);        
        Destroy(explosion);
        GameObject.FindGameObjectWithTag("Slime").GetComponent<SlimeCtrl>().GameOver();
        MiniGame2Manager.instance.GameOver();
        gameObject.SetActive(false);
    }    
}
