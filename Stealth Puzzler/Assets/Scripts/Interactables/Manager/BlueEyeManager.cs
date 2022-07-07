using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BlueEyeManager : MonoBehaviour
{
    [SerializeField] private List<BlueEye> _blueEyeList;
    [SerializeField] private UnityEvent _onAllBlueEyesLit;

    private void OnEnable()
    {
        BlueEye.OnEyeHit += CheckEyes;
    }

    private void OnDisable()
    {
        BlueEye.OnEyeHit -= CheckEyes;
    }
    private void CheckEyes()
    {
        foreach (BlueEye _blueEye in _blueEyeList)
        {
            if (!_blueEye.IsLit) { return; }
        }
        _onAllBlueEyesLit?.Invoke();
    }
}