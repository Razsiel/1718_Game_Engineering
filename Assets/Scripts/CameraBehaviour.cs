using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    private Camera _camera;

    // Use this for initialization
    void Start ()
    {
        _camera = gameObject.GetComponent<Camera>();
    }
    
    // Update is called once per frame
    void Update () {
        
    }
}
