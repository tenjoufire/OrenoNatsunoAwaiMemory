using UnityEngine;
using System.Collections;

public class PointCalicuration : MonoBehaviour {

    private int plPoint = 10;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    // 点数計算を行う
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("bomb"))
        {

            plPoint--;

        }

        else if (collider.gameObject.CompareTag("targetWM"))
        {
            plPoint = plPoint + 2;
        }

        Debug.Log("リア充度："+plPoint);

    }

    public int ScoreCheck()
    {
        return plPoint;
    }

}
