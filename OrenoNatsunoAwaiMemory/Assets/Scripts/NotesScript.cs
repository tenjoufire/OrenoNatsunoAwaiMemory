using UnityEngine;
using System.Collections;

public class NotesScript : MonoBehaviour {

    public int lineNum;
    private GameManager _gameManager;
    private CheckShaking chk;

    // Use this for initialization
    void Start () {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        chk = _gameManager.chk;

    }


    void CheckInput(KeyCode key)
    {
        if ((key == KeyCode.Space&&chk.UpShaking())||(key == KeyCode.A&&chk.DownShaking()))
        {
            if (Mathf.Abs(this.transform.position.y - _gameManager.GetJudge()) < 30) {//判定ライン(画面の底辺から80の位置)との相対距離で判定つける
                _gameManager.GoodTimingFunc(1);//perfect
                Destroy(this.gameObject);
                }
            /*
            if (Mathf.Abs(this.transform.position.y - _gameManager.GetJudge()) > 3 && Mathf.Abs(this.transform.position.y - _gameManager.GetJudge()) < 5)
            {
                _gameManager.GoodTimingFunc(2);//great
                Destroy(this.gameObject);
            }*/
            if (Mathf.Abs(this.transform.position.y - _gameManager.GetJudge()) > 30 && Mathf.Abs(this.transform.position.y - _gameManager.GetJudge()) < 40)
            {
                _gameManager.GoodTimingFunc(3);//good
                Destroy(this.gameObject);
            }
            if (Mathf.Abs(this.transform.position.y - _gameManager.GetJudge()) > 40)
            {
                //Debug.Log(this.transform.position.y);
                _gameManager.GoodTimingFunc(4);//bad
                Destroy(this.gameObject);
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        switch (lineNum)
        {
            case 0:
                CheckInput(KeyCode.Space);//UpShakingに対応
                break;
            //ノーツ増やしたらここにライン番号入れるよ
            case 1:
                CheckInput(KeyCode.A);//DownShakingに対応
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update () {
        this.transform.position += Vector3.down*400*Time.deltaTime;
        if(this.transform.position.y < -1f)
        {
            Destroy(this.gameObject);//下まで行ったら消滅させる
            _gameManager.GoodTimingFunc(0);//miss
        }
	}
}
