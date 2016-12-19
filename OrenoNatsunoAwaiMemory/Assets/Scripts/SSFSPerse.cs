using UnityEngine;
using System.Collections;

public class SSFSPerse : MonoBehaviour {

    public float initRange = 1.0f;
    public float interval;

    private Renderer renderer;
	// Use this for initialization
	void Start () {

        renderer = GetComponent<Renderer>();
        StartCoroutine("PhaseMove");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator PhaseMove()
    {
        float range = initRange;
        while (true)
        {
            while (range >= 0f)
            {
                renderer.material.EnableKeyword("_Phase");
                renderer.material.SetFloat("_Phase", range);
                range -= 0.05f;
                yield return new WaitForSeconds(0.01f);
            }

            yield return new WaitForSeconds(interval);

            while(range <= 1.01f)
            {
                renderer.material.EnableKeyword("_Phase");
                renderer.material.SetFloat("_Phase", range);
                range += 0.05f;
                yield return new WaitForSeconds(0.01f);
            }
            yield return new WaitForSeconds(interval + interval);
        }
    }
}
