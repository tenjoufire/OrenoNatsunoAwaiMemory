using UnityEngine;
using Windows.Kinect;
using System.Linq;
using System.Collections;

public class PlayerMotion : MonoBehaviour
{

    // kinect関係の変数
    public BodySourceManager BodyManager;
    public float delta;

    private Quaternion prevRightWristPos = new Quaternion();
    private float prevRightHandPos = 0f;
    private bool trigger = false;

    public bool Trigger
    {

        set
        {
            this.trigger = value;
        }

        get
        {
            return this.trigger;
        }
    }

    private int counter = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    public void Update()
    {

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
        //右手の位置を保存
        var rightHandPos = body.Joints[JointType.HandRight].Position;
     //   Debug.Log("rY:" + rightHandPos.Y);
        // 頭の位置を保存
         var headPos = body.Joints[JointType.Head].Position;
        //  Debug.Log("hY:"+headPos.Y);

        // 必ず最初は直前の座標が頭の座標より高いように
        if (counter == 0 && prevRightHandPos > headPos.Y)
        {

            counter = 1;

        }

        // if (prevRightHandPos>headPos.Y) {

        // 直前の右手の位置との差を見る
        // 腕を振る動作がなるべく大きくなるように，判定厳しく
        if (counter >= 1 && System.Math.Abs(rightHandPos.Y - prevRightHandPos) > delta)
        {
            counter++;
        }

        if (counter == 3)
        {
            trigger = true;
         //   Debug.Log("meu");

            counter = 0;
        }

        //現在の右手の位置を保存
        prevRightHandPos = rightHandPos.Y;

        //}
    }

    public bool isShaking()
    {
        return trigger;
    }


}
