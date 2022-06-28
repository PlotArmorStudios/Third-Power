using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarCube : MonoBehaviour
{
    [SerializeField] private float _hoverFrequency;
    [SerializeField] private float _hoverMagnitude;
    private Vector3 _position;

    private void Start()
    {
        _position = transform.position;
    }

    void Update()
    {
        transform.Rotate(Vector3.up, Space.World);

        float x = _position.x;
        float y = _position.y + Mathf.Sin(Time.time * _hoverFrequency) * _hoverMagnitude;
        float z = _position.z;

        transform.position = new Vector3(x, y, z);
    }
}