using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireworksSeed : MonoBehaviour {

    //ゲームでの点数を反映させる点数
    //今は仮に配置

    
    public static int score = 0;


    public Text tensu;  //スコアを格納する用
    public Text comment; //コメント


    //花火
    [SerializeField]
    public ParticleSystem seed1;
    public ParticleSystem seed2;
    public ParticleSystem seed3;
    public ParticleSystem seed4;


    void Awake()
    {
        score = PlayerPrefs.GetInt("Real");
    }
    // Use this for initialization
    void Start () {
        
        //tensu.text = score.ToString();    //スコアを格納    
    }

    // Update is called once per frame
    void Update () {
        
    }

    public void DelayStart()
    {
        Invoke("ShowText", 3.0f); //3秒後にコメント表示
    }

    public void Hanabi(){

        //点滅しているテキストを非表示に
        MyCanvas.SetActive("Click",false);
        MyCanvas.SetActive("Button",false);

        //スコアが悪かったら花火しょぼい
        if(score < 30){
            seed1.Play();
        }
        //スコアが良かったら花火いっぱい
        else if(score > 30){
            seed2.Play();
            seed3.Play();
            seed4.Play();
        }
    }

    //結果表示
    public void ShowText()
    {
        MyCanvas.SetActive("tensu",true);
        MyCanvas.SetActive("パーセント", true);
        MyCanvas.SetActive("comment", true);
        MyCanvas.SetActive("HomeButton",true);

        if (score < 30)
        {
            comment.text = "まだまだキモオタですね...";            
        }
        else if (score > 30)
        {
            comment.text = "君なら現実でもリア充できる！！！";            
        }
    }

    //soundスクリプトに値を渡すため
    public static int GetScore()
    {
        return score;
    }
}