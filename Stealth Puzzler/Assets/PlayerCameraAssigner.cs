using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This will assign the player's camera variables when the cam rig loads.
/// </summary>
public class PlayerCameraAssigner : MonoBehaviour
{
   private void Start()
   {
      var controllers = FindObjectsOfType<Controller>();
      
      foreach (var controller in controllers)
      {
         controller.Cam = GetComponent<Camera>();
         controller.CamTransform = GetComponent<Camera>().transform;
      }
   }
}
