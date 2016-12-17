using UnityEngine;
using Windows.Kinect;
using System.Collections;
using System.Linq;

public class CheckShaking : MonoBehaviour {

    public BodySourceManager BodyManager;
    public float delta;

    private Quaternion prevRightWristPos = new Quaternion();
    private float prevRightHandPos = 0f;
    private bool trigger = false;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        trigger = false;

        if (BodyManager == null)
        {
            Debug.Log("_BodyManager == null");
            return;
        }

        // Bodyデータを取得する
        var data = BodyManager.GetData();
        if (data == null)
        {
            return;
        }
        // 最初に追跡している人を取得する
        var body = data.FirstOrDefault(b => b.IsTracked);
        if (body == null)
        {
            return;
        }
        /*
        // 床の傾きを取得する
        var floorPlane = BodyManager.FloorClipPlane;
        var comp = Quaternion.FromToRotation(
            new Vector3(floorPlane.X, floorPlane.Y, floorPlane.Z), Vector3.up);

        // 関節の回転を取得する
        var joints = body.JointOrientations;

        //右ひじ関節の回転を取得する
        var rightWristOrientation = joints[JointType.WristRight].Orientation.ToQuaternion(comp);
        //Debug.Log("X:" + rightElbowOrientation.X + " Y:" + rightElbowOrientation.Y + " Z:" + rightElbowOrientation.Z + " W:" + rightElbowOrientation.W);
        //Debug.Log(rightWristOrientation.eulerAngles.magnitude);

        //直前の回転角との差を見る
        if(System.Math.Abs(rightWristOrientation.eulerAngles.magnitude - prevRightWristPos.eulerAngles.magnitude) > delta)
        {
            trigger = true;
            Debug.Log("meu");
        }

        //現在の回転角を保存
        prevRightWristPos = rightWristOrientation;
        */

        //右手の位置を保存
        var rightHandPos = body.Joints[JointType.HandRight].Position;
        Debug.Log(rightHandPos.Y);

        //直前の右手の位置との差を見る
        if(System.Math.Abs(rightHandPos.Y - prevRightHandPos) > delta && rightHandPos.Y < prevRightHandPos)
        {
            trigger = true;
            Debug.Log("meu");
        }

        //現在の右手の位置を保存
        prevRightHandPos = rightHandPos.Y;


    }

    public bool isShaking()
    {
        return trigger;
    }
}
