using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GoNextCcene : MonoBehaviour {

    public int waitingTime;
    public string NextSceneName;

	// Use this for initialization
	void Start () {
        StartCoroutine("NextScene");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator NextScene()
    {
        yield return new WaitForSeconds(waitingTime);
        SceneManager.LoadScene(NextSceneName);
    }
}
