using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayerParent : MonoBehaviour
{
    private PlayerController _player;
    private CubeController _cube;


    private void OnTriggerEnter(Collider other)
    {
        _player = other.GetComponent<PlayerController>();
        _cube = other.GetComponent<CubeController>();

        if (_player)
            _player.transform.SetParent(transform);
        if (_cube)
            _cube.transform.SetParent(transform);
    }

    private void OnTriggerExit(Collider other)
    {
        if (_player || _cube)
        {
            ControllerManager.Instance.DeparentControllers();
        }
    }
}