using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PanelController : MonoBehaviour {
       

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {

	}

    //Game1～3選んだら表示
    //Gameボタンにつける
    public void ShowButton()
    {
        MyCanvas.SetActive("ConfirmText", true);
        MyCanvas.SetActive("Panel",true);
        MyCanvas.SetActive("YesButton",true);
        MyCanvas.SetActive("NoButton", true);
    }

    public void ShowButton2()
    {
        MyCanvas.SetActive("ConfirmText", true);
        MyCanvas.SetActive("Panel", true);
        MyCanvas.SetActive("YesButton2", true);
        MyCanvas.SetActive("NoButton2", true);
    }

    public void ShowButton3()
    {
        MyCanvas.SetActive("ConfirmText", true);
        MyCanvas.SetActive("Panel", true);
        MyCanvas.SetActive("YesButton3", true);
        MyCanvas.SetActive("NoButton3", true);
    }

    //選択肢を選びなおすときに非表示にする
    //NoButtonにつける
    public void HideButton()
    {
        MyCanvas.SetActive("ConfirmText", false);
        MyCanvas.SetActive("Panel", false);
        MyCanvas.SetActive("YesButton", false);
        MyCanvas.SetActive("NoButton", false);
    }

    public void HideButton2()
    {
        MyCanvas.SetActive("ConfirmText", false);
        MyCanvas.SetActive("Panel", false);
        MyCanvas.SetActive("YesButton2", false);
        MyCanvas.SetActive("NoButton2", false);
    }

    public void HideButton3()
    {
        MyCanvas.SetActive("ConfirmText", false);
        MyCanvas.SetActive("Panel", false);
        MyCanvas.SetActive("YesButton3", false);
        MyCanvas.SetActive("NoButton3", false);
    }
}
