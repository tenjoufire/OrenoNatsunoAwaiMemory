using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameORMV : MonoBehaviour {

    public Text gameMessage;
    public Text mvMessage;

    private bool playGame = true;
	// Use this for initialization
	void Start () {
        //intが0ならばGAME，1ならばMVを再生する
        PlayerPrefs.SetInt("gameOrMv", 0);
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (playGame)
            {
                playGame = false;
                gameMessage.text = "Press space key";
                gameMessage.fontSize = 10;
                mvMessage.text = "MV";
                mvMessage.fontSize = 14;
                PlayerPrefs.SetInt("gameOrMv", 1);
            }
            else
            {
                playGame = true;
                gameMessage.text = "Game";
                gameMessage.fontSize = 14;
                mvMessage.text = "Press space key";
                mvMessage.fontSize = 10;
                PlayerPrefs.SetInt("gameOrMv", 0);
            }
        }

        Debug.Log(PlayerPrefs.GetInt("gameOrMv"));
	
	}
}
