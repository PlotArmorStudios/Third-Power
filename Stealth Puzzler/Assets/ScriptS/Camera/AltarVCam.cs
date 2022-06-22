using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarVCam : MonoBehaviour
{
    [SerializeField] private AltarInput _altar;
    
    private void OnEnable()
    {
        _altar.OnActivateUI += ActivatePriority;
        _altar.OnDeactivateUI += DeactivatePriority;
    }
    
    private void OnDisable()
    {
        _altar.OnActivateUI -= ActivatePriority;
        _altar.OnDeactivateUI -= DeactivatePriority;
    }

    private void ActivatePriority()
    {
    }
  
    private void DeactivatePriority()
    {
        
    }

}
