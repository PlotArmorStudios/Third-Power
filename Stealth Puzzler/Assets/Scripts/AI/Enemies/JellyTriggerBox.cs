using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class JellyTriggerBox : MonoBehaviour
{
    [SerializeField] private GameObject _jellyController;
    [SerializeField] private Rigidbody _jellyRb;
    [SerializeField] private float _initialForceMultiplier;
    public static event Action OnJellyTriggerBoxEntered;
    [SerializeField] private float _flipTime = 0.75f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CubeController>())
        {
            _jellyRb.useGravity = true;
            _jellyRb.isKinematic = false;
            _jellyRb.AddForce(Vector3.down * _initialForceMultiplier);
            LeanTween.value(_jellyController, RotateCallback, 180f, 0f, _flipTime);
        }
        else if (other.gameObject.GetComponent<PlayerController>())
        {
            _jellyRb.useGravity = true;
            _jellyRb.isKinematic = false;
            _jellyRb.AddForce(Vector3.down * _initialForceMultiplier);
            LeanTween.value(_jellyController, RotateCallback, 180f, 0f, _flipTime);
        }
    }
    private void RotateCallback(float c)
    {
        var rotationVector = _jellyController.transform.rotation.eulerAngles;
        rotationVector.z = c;
        _jellyController.transform.rotation = Quaternion.Euler(rotationVector);
    }
}
