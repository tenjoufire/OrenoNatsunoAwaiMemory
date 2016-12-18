using UnityEngine;
using System.Collections;

public class SuicaController : MonoBehaviour
{

    //   public Rigidbody rb;
    // public GameObject kc;

    public GameObject suica;
    public GameObject ss;
    public GameObject suicaPos;

   // private int sPoint = 2;

    private AudioSource sound02;

    private bool sTrigger = false;

    public bool STrigger
    {
        set
        {
            this.sTrigger = value;
        }

        get
        {
            return this.sTrigger;
        }

    }

    // Use this for initialization
    void Start()
    {

        //   rb = GetComponent<Rigidbody>();

        var audio = GameObject.Find("SuicaOto");
        sound02 = audio.GetComponent<AudioSource>();

    }


    // Update is called once per frame
    void Update()
    {
        // if (Input.GetMouseButtonDown(0))
        // {
        //        }
    }

    // "bat"タグを持つオブジェクトに衝突した時
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("bat"))
        {

            Destroy(suica, 0);
            sound02.PlayOneShot(sound02.clip);
            Instantiate(ss, suicaPos.transform.position, Quaternion.identity);
            sTrigger = true;

        }
    }

    /*
    public bool triggerMeron()
    {
        return sTrigger;
    }

    public int suicaPoint()
    {
        return sPoint;
    }
    */

}
