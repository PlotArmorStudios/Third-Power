using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;

public class InputToggler : MonoBehaviour
{
   private CinemachineInputProvider _inputProvider;
   private List<AltarInput> _altars;

   private void OnEnable()
   {
      _inputProvider = GetComponent<CinemachineInputProvider>();
      _altars = FindObjectsOfType<AltarInput>().ToList();
      
      foreach (var altar in _altars)
      {
         altar.OnActivateCamInput += HandleEnableInput;
         altar.OnDeactivateCamInput += HandleDisableInput;
      }
   }

   private void OnDisable()
   {
      foreach (var altar in _altars)
      {
         altar.OnActivateCamInput -= HandleEnableInput;
         altar.OnDeactivateCamInput -= HandleDisableInput;
      }
   }

   private void HandleEnableInput()
   {
      _inputProvider.enabled = true;
   }

   private void HandleDisableInput()
   {
      _inputProvider.enabled = false;
   }
}
