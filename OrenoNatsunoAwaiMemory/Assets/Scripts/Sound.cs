using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Sound : MonoBehaviour {

	 //音声ファイル格納用変数
    public AudioClip sound01;
    public AudioClip sound02;
    private AudioSource audioSource;

    public Text tensu;
    int score;

    // Use this for initialization
    void Start () {
		audioSource = gameObject.GetComponent<AudioSource>();
        score = FireworksSeed.GetScore();
        tensu.text = score.ToString();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void SoundPlay(){
		if(score < 30){
            audioSource.clip = sound01;
            audioSource.Play ();
        }
        else if(score > 30){
            audioSource.clip = sound02;
            audioSource.Play ();
        }
	}
	
}
