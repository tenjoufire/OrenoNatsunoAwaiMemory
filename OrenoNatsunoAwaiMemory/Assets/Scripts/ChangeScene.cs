﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void goMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void goGame1()
    {
        SceneManager.LoadScene("Game1");
    }

    public void goGame2()
    {
        SceneManager.LoadScene("Game2");
    }

    public void goGame3()
    {
        SceneManager.LoadScene("Game3");
    }
}