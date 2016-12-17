using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ForceAspectRatio : MonoBehaviour
{
    public float horizontal = 16;
    public float vertical = 9;
    public float fieldOfView = 6;

    void Update()
    {
        GetComponent<Camera>().aspect = horizontal / vertical;
        GetComponent<Camera>().fieldOfView = fieldOfView;
    }
}
