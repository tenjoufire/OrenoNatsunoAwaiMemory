using UnityEngine;
using System.Collections;

public class GuitarPosition : MonoBehaviour
{
    Quaternion defaultRotation;

    // Use this for initialization
    void Start()
    {
        defaultRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Inverse(transform.parent.rotation) * defaultRotation;

    }
}
