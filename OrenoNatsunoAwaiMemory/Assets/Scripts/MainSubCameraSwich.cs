using UnityEngine;
using System.Collections;

public class MainSubCameraSwich : MonoBehaviour
{

    public GameObject mainCamera;
    public GameObject[] subCameras;
    public int interval;
    public bool auto = true;

    // Use this for initialization
    void Start()
    {
        //mainCamera.SetActive(false);
        foreach (var sub in subCameras)
        {
            sub.SetActive(false);
        }
        if (auto) StartCoroutine("MainSubChange");
    }

    // Update is called once per frame
    void Update()
    {


    }

    IEnumerator MainSubChange()
    {
        while (true)
        {
            System.Random r = new System.Random();
            int subIndex = r.Next(subCameras.Length);

            if (mainCamera.activeSelf)
            {
                mainCamera.SetActive(false);
                subCameras[subIndex].SetActive(true);
            }
            else
            {
                foreach (var sub in subCameras)
                {
                    sub.SetActive(false);
                }
                mainCamera.SetActive(true);
            }

            yield return new WaitForSeconds(interval);
        }
    }

}
