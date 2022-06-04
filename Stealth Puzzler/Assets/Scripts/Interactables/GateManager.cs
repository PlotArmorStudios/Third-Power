using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateManager : MonoBehaviour
{
    [SerializeField] private Transform _gateTransform;
    [SerializeField] private Transform _openPosition;
    [SerializeField] private Transform _closedPosition;
    [SerializeField] private float _duration = 1f;

    [ContextMenu("Open Gate")]
    public void OpenGate()
    {
        StopCoroutine(MoveGateDown());
        StartCoroutine(MoveGateUp());
    }

    [ContextMenu("Close Gate")]
    public void CloseGate()
    {
        StopCoroutine(MoveGateUp());
        StartCoroutine(MoveGateDown());
    }

    private IEnumerator MoveGateUp()
    {
        float elapsedTime = 0;

        while (elapsedTime < _duration)
        {
            _gateTransform.position = Vector3.Lerp(_gateTransform.position, _openPosition.position, elapsedTime / _duration);
            elapsedTime+= Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator MoveGateDown()
    {
        float elapsedTime = 0;

        while (elapsedTime < _duration)
        {
            _gateTransform.position = Vector3.Lerp(_gateTransform.position, _closedPosition.position, elapsedTime / _duration);
            elapsedTime+= Time.deltaTime;
            yield return null;
        }
    }
}