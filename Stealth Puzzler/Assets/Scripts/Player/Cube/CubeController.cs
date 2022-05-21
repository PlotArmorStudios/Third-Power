using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    private Rigidbody _rigidbody;

    private float _horizontal;
    private float _vertical;
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        ReadInput();
    }

    private void ReadInput()
    {
    }
}
