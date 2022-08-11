using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CutsceneTrigger : MonoBehaviour
{
   [SerializeField] private Transform _cubeLocation;
   [SerializeField] private UnityEvent _cutsceneEvent;
   
   private void OnTriggerEnter(Collider other)
   {
      var player = other.gameObject.GetComponent<Controller>();
      if (!player) return;
      _cutsceneEvent?.Invoke();

      StartCoroutine(FlipCubeToOriginalRotation(player));
   }

   private IEnumerator FlipCubeToOriginalRotation(Controller player)
   {
      yield return new WaitForSeconds(1f);
      ControllerManager.Instance.CubeController.transform.position = _cubeLocation.position;
      ControllerManager.Instance.CubeController.transform.forward = Vector3.right;
   }
}
