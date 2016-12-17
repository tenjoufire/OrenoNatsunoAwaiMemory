using UnityEngine;
using System.Collections;

public class ParticleGen : MonoBehaviour {

    public GameObject particleGen;
    public GameObject target;

    private ParticleSystem particle;
    private CheckShaking chk;
    private int flag = 0;

	// Use this for initialization
	void Start () {

        //子要素のparticleをとってくる
        particle = gameObject.GetComponentInChildren<ParticleSystem>();
        particle.Stop();
        chk = target.GetComponent<CheckShaking>();
	
	}
	
	// Update is called once per frame
	void Update () {
	
        if(flag > 10)
        {
            if (chk.isShaking())
            {
                flag = 0;
                particle.Play();
            }
            else
            {
                particle.Stop();
            }
        }
        flag++;
	}

    
}
