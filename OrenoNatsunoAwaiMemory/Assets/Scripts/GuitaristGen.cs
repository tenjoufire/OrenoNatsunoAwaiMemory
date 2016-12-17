using UnityEngine;
using System.Collections;

public class GuitaristGen : MonoBehaviour
{

    public GameObject[] guitarists;
    public int index;
    private GameObject guitarist;

    // Use this for initialization
    void Start()
    {
        guitarist = (GameObject)Instantiate(
            guitarists[index],
            new Vector3(0, 0.5f, -1.8f),
            Quaternion.identity
            );

    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject GetGuitarist()
    {
        return guitarist;
    }
}
