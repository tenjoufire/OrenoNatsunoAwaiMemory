using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SwingCount : MonoBehaviour
{

    public GameObject pc;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        PlayerController pco = pc.GetComponent<PlayerController>();

        this.GetComponent<Text>().text = "残りスイング数：" + pco.SwingCount().ToString();

    }
}
