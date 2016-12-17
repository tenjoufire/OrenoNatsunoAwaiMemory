using UnityEngine;
using System.Collections;

public class CameraLooksOne : MonoBehaviour {

    public GameObject target;
    public GameObject lookTarget;
    private Vector3 offset;
    private Transform look;
    // Use this for initialization
    void Start () {

        //自分自身とtargetとの相対距離を求める
        offset = GetComponent<Transform>().position - target.GetComponent<Transform>().position;

	
	}
	
	// Update is called once per frame
	void Update () {

        transform.position = target.GetComponent<Transform>().position + offset;
        //look = target.transform;
        //look.position = new Vector3(target.transform.position.x, target.transform.position.y + 1, target.transform.position.z);
        //transform.LookAt(look);


    }
}
