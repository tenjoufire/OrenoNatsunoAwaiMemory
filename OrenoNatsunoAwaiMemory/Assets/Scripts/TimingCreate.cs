using UnityEngine;
using System.Collections;
using System.IO;

public class TimingCreate : MonoBehaviour {

    private AudioSource _audioSource;
    private float _startTime = 0;
    private CSVEdit _CSVEdit;

    private bool _isPlaying = false;
    public GameObject startButton;

    void Start()
    {
        _audioSource = GameObject.Find("Music1").GetComponent<AudioSource>();
        _CSVEdit = GameObject.Find("GameObject").GetComponent<CSVEdit>();
    }

    void Update()
    {
        if (_isPlaying)
        {
            DetectKeys();
        }
    }

    public void StartMusic()
    {
        startButton.SetActive(false);
        _audioSource.Play();
        _startTime = Time.time;
        _isPlaying = true;
    }

    void DetectKeys()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            WriteNotesTiming(0);//ノート1つと仮定してるので1パターンのみ生成
        }



    }

    void WriteNotesTiming(int num)
    {
        Debug.Log(GetTiming());
        _CSVEdit.WriteCSV(GetTiming().ToString() + "," + num.ToString());
    }

    float GetTiming()
    {
        return Time.time - _startTime;
    }
}
