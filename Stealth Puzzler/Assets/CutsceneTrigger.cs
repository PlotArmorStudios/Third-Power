using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CutsceneTrigger : MonoBehaviour
{
   [SerializeField] private UnityEvent _cutsceneEvent;

   private void OnTriggerEnter(Collider other)
   {
      var player = other.gameObject.GetComponent<Controller>();
      if (!player) return;
      _cutsceneEvent?.Invoke();
   }
}
