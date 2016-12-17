using UnityEngine;
using System.Collections;

public class TitleController : MonoBehaviour {

    //とりあえずコピペで文字の点滅しておく。いずれ自作に変えるかも←点滅が思ってたのと違ったんでαチャンネルの変動は自作に変えた
    private GameObject textObject; //点滅させたい文字

    private int alphachanger = 0;//alphaの変動にかかわる

    // Use this for initialization
    void Start () {
        textObject = GameObject.Find("Caution");
    }
	
	// Update is called once per frame
	void Update () {

        //一定時間ごとに点滅

        float alpha = textObject.GetComponent<CanvasRenderer>().GetAlpha();//文字列に現在のα値を取得
        if(alpha >= 1.0f)
        {
            alphachanger = 0;//alphaを減少させる
        }
        if(alpha <= 0f)
        {
            alphachanger = 1;//alphaを増大させる
        }

        if (alphachanger == 0)
            {
            alpha -= 0.03f;
            }

        if (alphachanger == 1)
            {
            alpha += 0.03f;
            }
        textObject.GetComponent<CanvasRenderer>().SetAlpha(alpha);
    }
}
