using UnityEngine;
using System.Collections;

public class PointLightFlush : MonoBehaviour
{

    public float interval;
    public bool changeColor;

    private int colorIndex = 0;

    // Use this for initialization
    void Start()
    {
        StartCoroutine("Flushing");
        if (changeColor) StartCoroutine("ChangeColor");
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Flushing()
    {
        while (true)
        {
            GetComponent<Light>().enabled = true;

            yield return new WaitForSeconds(interval);

            GetComponent<Light>().enabled = false;

            yield return new WaitForSeconds(interval);
        }
    }

    IEnumerator ChangeColor()
    {
        while (true)
        {
            Color[] colors = { Color.red, Color.green, Color.blue, Color.yellow };
            System.Random r = new System.Random();
            colorIndex = r.Next(colors.Length - 1);
            gameObject.GetComponent<Light>().color = colors[colorIndex];

            yield return new WaitForSeconds(interval);
        }
    }
}