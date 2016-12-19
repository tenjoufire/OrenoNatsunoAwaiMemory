using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreCount : MonoBehaviour
{

    public GameObject pc;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        PointCalicuration pco = pc.GetComponent<PointCalicuration>();

        this.GetComponent<Text>().text = "かっこよさ指数：" + pco.ScoreCheck().ToString();
    }
}
