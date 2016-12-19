using UnityEngine;
using System.Collections;

public class MekakushiController : MonoBehaviour
{

    public GameObject tpc;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        PlayerController pc = tpc.GetComponent<PlayerController>();

        // flagの値で目隠しをするかどうか
        if (pc.FlagTrigger() == 0)
        {
            GameObject.Find("meui").GetComponent<Canvas>().enabled = false;
        }
        else if (pc.FlagTrigger() == 1)
        {
            GameObject.Find("meui").GetComponent<Canvas>().enabled = true;
        }

    }
}
