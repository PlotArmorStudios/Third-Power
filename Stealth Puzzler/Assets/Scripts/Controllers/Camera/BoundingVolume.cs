using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BoundingVolume : MonoBehaviour
{
    [SerializeField] private CinemachineConfiner _confiner;
    private void Start()
    {
        _confiner = FindObjectOfType<CinemachineConfiner>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CubeController>() || other.gameObject.GetComponent<PlayerController>())
        {
            _confiner.m_BoundingVolume = GetComponent<BoxCollider>();
        }
    }
}