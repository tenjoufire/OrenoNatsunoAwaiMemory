using UnityEngine;
using System.Collections;

public class NotesScript : MonoBehaviour {

    public int lineNum;
    private GameManager _gameManager;
    private CheckShaking chk;

    // Use this for initialization
    void Start () {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        chk = GameObject.Find("KinectAvatar").GetComponent<CheckShaking>();

    }


    void CheckInput(KeyCode key)
    {
        if (chk.isShaking())
        {
            if (Mathf.Abs(this.transform.position.y - _gameManager.GetJudge()) < 3) {//判定ライン(画面の底辺から80の位置)との相対距離で判定つける
                _gameManager.GoodTimingFunc(1);//perfect
                Destroy(this.gameObject);
                }
            if (Mathf.Abs(this.transform.position.y - _gameManager.GetJudge()) > 3 && Mathf.Abs(this.transform.position.y - _gameManager.GetJudge()) < 5)
            {
                _gameManager.GoodTimingFunc(2);//great
                Destroy(this.gameObject);
            }
            if (Mathf.Abs(this.transform.position.y - _gameManager.GetJudge()) > 5 && Mathf.Abs(this.transform.position.y - _gameManager.GetJudge()) < 7)
            {
                _gameManager.GoodTimingFunc(3);//good
                Destroy(this.gameObject);
            }
            if (Mathf.Abs(this.transform.position.y - _gameManager.GetJudge()) > 7)
            {
                Debug.Log(this.transform.position.y);
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
                CheckInput(KeyCode.Space);
                break;
                //ノーツ増やしたらここにライン番号入れるよ
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update () {
        this.transform.position += Vector3.down*60*Time.deltaTime;
        if(this.transform.position.y < -1f)
        {
            Destroy(this.gameObject);//下まで行ったら消滅させる
            _gameManager.GoodTimingFunc(0);//miss
        }
	}
}
