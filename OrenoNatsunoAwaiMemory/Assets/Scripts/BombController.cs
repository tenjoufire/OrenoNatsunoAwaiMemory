using UnityEngine;
using System.Collections;

public class BombController : MonoBehaviour
{

    public GameObject exPlo;
    public GameObject exploPos;
    // public GameObject kc;

    public GameObject go;

    private int bPoint = -1;

    private AudioSource sound01;
    private bool bTrigger = false;

    public bool BTrigger
    {
        set
        {
            this.bTrigger = value;
        }

        get
        {
            return this.bTrigger;
        }

    }

    // Use this for initialization
    void Start()
    {

        var audio = GameObject.Find("bakuhatsu");
        sound01 = audio.GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {

        // 爆発するよ
        /* if (Input.GetKeyDown(KeyCode.Z))
         {
             Instantiate(exPlo, exploPos.transform.position, Quaternion.identity);
             sound01.PlayOneShot(sound01.clip);
             Destroy(go, 0);

         } */

    }

    // 爆弾に衝突した時
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("bat"))
        {

            Destroy(go, 0);
            sound01.PlayOneShot(sound01.clip);
            Instantiate(exPlo, exploPos.transform.position, Quaternion.identity);
            bTrigger = true;

        }
    }

    public bool triggerBomb()
    {
        return bTrigger;
    }

    public int bombPoint()
    {
        return bPoint;
    }

}
