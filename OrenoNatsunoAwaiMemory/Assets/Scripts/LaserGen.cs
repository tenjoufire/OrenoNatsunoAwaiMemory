using UnityEngine;
using System.Collections;

public class LaserGen : MonoBehaviour {

    public GameObject laser;
    public int XWidth;
    public int YHeightL;
    public int YHeightH;
    public int ZWidth;
    public int laserLifeTime;
    public int interval;

    private Transform startPoint;
    private bool isRunning;

	// Use this for initialization
	void Start () {
        startPoint = transform;
    }
	
	// Update is called once per frame
	void Update () {
        StartCoroutine("Laser");
        StartCoroutine("Wait");
    }

    //レーザーの発生頻度を決定するコルーチン
    IEnumerator Wait()
    {
        //このコルーチンが実行中は新たにコルーチンを実行しないようにする
        if (isRunning) yield break;

        isRunning = true;
        System.Random r = new System.Random();
        float time = r.Next(interval) * 0.1f;
        yield return new WaitForSeconds(time);
        isRunning = false;
    }

    //レーザーを新しく生成するコルーチン
    IEnumerator Laser()
    {
        //このコルーチンが実行中は新たにコルーチンを実行しないようにする
        if (isRunning) yield break;

        //新規オブジェクト生成
        GameObject l = (GameObject)Instantiate(
            laser,
            new Vector3(startPoint.position.x, startPoint.position.y, startPoint.position.z),
            Quaternion.identity);
        //乱数生成（これらがレーザーの向きになる）
        System.Random rx = new System.Random();
        System.Random ry = new System.Random();
        System.Random rz = new System.Random();
        float x = rx.Next(-XWidth, XWidth);
        float y = ry.Next(YHeightL, YHeightH);
        float z = rz.Next(5, ZWidth);

        //レーザーの向きを設定（LineRendererのプロパティを設定）
        l.GetComponent<LineRenderer>().SetPosition(1, new Vector3(x, y, z));
        //レーザーの出現時間を設定
        yield return new WaitForSeconds(laserLifeTime);
        //このレーザーを削除
        Destroy(l);
        
    }
}
