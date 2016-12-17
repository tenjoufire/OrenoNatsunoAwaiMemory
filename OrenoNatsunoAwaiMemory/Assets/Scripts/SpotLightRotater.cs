using UnityEngine;
using System.Collections;

public class SpotLightRotater : MonoBehaviour {

    public float speedX = 2.0f;
    public float speedZ = 2.0f;
    public float maxAngleX = 50;

    private float xAngle;
    private float defaultAngleX;
    private int xAngleFlag = 1;
    //private float zAngle = 0f;
	// Use this for initialization
	void Start () {
        defaultAngleX = transform.rotation.eulerAngles.x;
	
	}
	
	// Update is called once per frame
	void Update () {
        xAngle = Time.deltaTime * speedX;
        if (transform.rotation.eulerAngles.x > defaultAngleX + maxAngleX)
            xAngleFlag *= -1;
        else if (transform.rotation.eulerAngles.x < defaultAngleX - maxAngleX)
            xAngleFlag *= -1;
        transform.Rotate(xAngle * xAngleFlag, 0, Time.deltaTime * speedZ);
	
	}
}
