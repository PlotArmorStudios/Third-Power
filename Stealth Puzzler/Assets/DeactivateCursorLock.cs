using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateCursorLock : MonoBehaviour
{
    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}
