using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SwichToNextScene : MonoBehaviour
{

    public string nextSceneName;
    public int waitTime;
    // Use this for initialization
    void Start()
    {
        StartCoroutine("SwichScene");
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SwichScene()
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(nextSceneName);
    }
}
