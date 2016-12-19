using UnityEngine;
using System.Collections;
using System.IO;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public GameObject[] notes;
    private float[] _timing;
    private int[] _lineNum;

    public string filePass;
    private int _notesCount = 0;

    private AudioSource _audioSource;
    private float _startTime = 0;

    public float timeOffset = -1;

    private bool _isPlaying = false;
    public GameObject parentObject;//お前がprefabノーツのママになるんだよ！
    private GameObject OPPanel;
    private Image OPPanelColor;//最初に出すパネルの画像
    private GameObject _backScreen;//音ゲー要素のパネル

    private float titleTime = 5f;//パネルを消し始めるまでの時間
    private GameObject _JudgeLine;//判定ライン


    private Image hanteiImage;//判定用画像のImage

    public Text songTitle;

    public Text scoreText;
    int score = 0;
    public Text comboText;
    int combo = 0;

    //判定用画像(120*35サイズが望ましい)
    public Sprite miss;
    public Sprite bad;
    public Sprite good;
    public Sprite great;
    public Sprite perfect;

    //効果音
    private AudioSource seSource;

    public AudioClip se1;
    public AudioClip se2;
    public AudioClip se3;
    public AudioClip se4;
    public AudioClip se0;

    public CheckShaking chk;


    void Start()
    {
        hanteiImage = GameObject.Find("JudgeResult").GetComponent<Image>();//判定用画像を変更可能にしておく
        _audioSource = GameObject.Find("GameMusic").GetComponent<AudioSource>();
        seSource = GameObject.Find("SE").GetComponent<AudioSource>();
        _JudgeLine = GameObject.Find("JudgeLine");
        _backScreen = GameObject.Find("BackScreen");

        OPPanel = GameObject.Find("OPPanel");
        OPPanelColor = GameObject.Find("OPPanel").GetComponent<Image>();
        songTitle.text = _audioSource.clip.name;
        _timing = new float[1024];
        _lineNum = new int[1024];
        LoadCSV();
    }

    void Update()
    {
        if (_isPlaying)
        {
            if (PlayerPrefs.GetInt("gameOrMv") == 0)//音ゲーモード
            {
                CheckNextNotes();
                scoreText.text = "SCORE:" + score.ToString();//スコア更新
                comboText.text = "COMBO:" + combo.ToString();//コンボ数更新
            }
            if (PlayerPrefs.GetInt("gameOrMv") == 1)//MVモード
            {
                //_backScreen.SetActive(false);//透明になったらパネル破壊
                parentObject.SetActive(false);//透明になったらパネル破壊
            }
        }

        if (!_isPlaying)
        {
            titleTime -= Time.deltaTime;
            if (OPPanel.activeSelf && titleTime <= 0)
            {
                var color1 = OPPanelColor.color;
                var color2 = songTitle.color;
                color1.a -= Time.deltaTime * 0.5f;
                color2.a -= Time.deltaTime * 0.5f;
                OPPanelColor.color = color1;
                songTitle.color = color2;
                if (OPPanelColor.color.a <= 0f)
                {
                    OPPanel.SetActive(false);
                    StartGame();//ゲーム開始
                }

            }
        }
        if ((!_audioSource.isPlaying && !OPPanel.activeSelf) || Input.GetKeyDown(KeyCode.Q))//曲終了の判定
        {
            //Debug.Log("ｳﾜｰ!!ｷｮｸｵﾜｯﾀｧｧｧｧｧｧｧｧｧｧ");
            StartCoroutine("GoToResult");
        }
    }

    public float GetJudge()//判定の位置のゲッター
    {
        return _JudgeLine.transform.position.y;
    }

    public void StartGame()
    {
        _startTime = Time.time;
        _audioSource.Play();
        _isPlaying = true;
    }



    void CheckNextNotes()
    {
        while (_timing[_notesCount] + timeOffset < GetMusicTime() && _timing[_notesCount] != 0)
        {
            SpawnNotes(_lineNum[_notesCount]);
            _notesCount++;
        }
    }

    void SpawnNotes(int num)
    {
        //Debug.Log("spawn notes.");//Debug
        //Debug.Log(_notesCount);
        GameObject prefab = (GameObject)Instantiate(notes[num],
            new Vector3(0, _JudgeLine.transform.position.y + 800f, 0),
            //transform.InverseTransformPoint(0, _JudgeLine.transform.position.y, 0),
            Quaternion.identity);//このリキャストたぶんいらない

        prefab.transform.SetParent(_JudgeLine.transform, false);//Canvasの子としてノーツを生成
        Debug.Log(prefab.transform.position.y + "  " + _JudgeLine.transform.position.y);
    }

    void LoadCSV()
    {
        int i = 0, j;
        TextAsset csv = Resources.Load(filePass) as TextAsset;
        StringReader reader = new StringReader(csv.text);
        while (reader.Peek() > -1)
        {

            string line = reader.ReadLine();
            string[] values = line.Split(',');
            for (j = 0; j < values.Length; j++)
            {
                _timing[i] = float.Parse(values[0]) - 1.45f; //ノーツだすタイミングを調整     
                _lineNum[i] = int.Parse(values[1]);
            }
            i++;
        }
    }

    float GetMusicTime()
    {
        return Time.time - _startTime;
    }

    public void GoodTimingFunc(int num)//判定ライン上でキー入力できたときの反応
    {

        switch (num)
        {
            case 0:
                hanteiImage.sprite = miss;
                combo = 0;
                seSource.clip = se0;
                seSource.Play();
                break;
            case 1:
                hanteiImage.sprite = perfect;
                score += 1000 + (combo * 50);
                combo++;
                seSource.clip = se1;
                seSource.Play();
                break;
            case 2:
                hanteiImage.sprite = great;
                score += 500 + (combo * 50);
                combo++;
                seSource.clip = se2;
                seSource.Play();
                break;
            case 3:
                hanteiImage.sprite = good;
                score += 100 + (combo * 50);
                combo++;
                seSource.clip = se3;
                seSource.Play();
                break;
            case 4:
                hanteiImage.sprite = bad;
                combo = 0;
                seSource.clip = se4;
                seSource.Play();
                break;
        }

    }

    IEnumerator GoToResult()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("Menu");
    }
}

