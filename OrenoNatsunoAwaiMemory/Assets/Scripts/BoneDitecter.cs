using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BoneDitecter : MonoBehaviour {

    public BodySourceManager BodyManager;
    public int waitingTime;
    public Text message;

    private bool flag = true;
    private Color defaultColor;

    // Use this for initialization
    void Start () {
        defaultColor = message.color;
	
	}
	
	// Update is called once per frame
	void Update () {

        if (BodyManager == null)
        {
            //Debug.Log("BodyManager == null");
            return;
        }

        // Bodyデータを取得する
        var data = BodyManager.GetData();
        if (data == null)
        {
            //Debug.Log("No Body Data");
            return;
        }
        // 最初に追跡している人を取得する
        var body = data.FirstOrDefault(b => b.IsTracked);
        if (body == null)
        {
            //Debug.Log("No Tracking");
            message.color = defaultColor;
            message.text = "Please stand in front of the Kinect";
            StopCoroutine("Tracking");
            flag = true;
            return;
        }
        if (flag) StartCoroutine("Tracking");
        
    }

    IEnumerator Tracking()
    {
        flag = false;
        message.color = new Color(1, 0.8f, 0.8f);
        message.text = "Detecting...";
        yield return new WaitForSeconds(waitingTime);
        //ここに次のシーンへの処理をかく
        SceneManager.LoadScene("UnityChanWithKinect");
    }
}
