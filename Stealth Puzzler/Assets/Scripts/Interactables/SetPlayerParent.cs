using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayerParent : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerController>();
        var cube = other.GetComponent<CubeController>();

        if (player)
            player.transform.SetParent(transform);
        if (cube)
            cube.transform.SetParent(transform);
    }

    private void OnTriggerExit(Collider other)
    {
        var player = other.GetComponent<PlayerController>();
        var cube = other.GetComponent<CubeController>();

        if (player)
            player.transform.SetParent(null);
        if (cube)
            cube.transform.SetParent(null);
    }
}