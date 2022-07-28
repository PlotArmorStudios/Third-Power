using System;
using UnityEngine;

namespace UITrigger
{
    public class AltarUITriggerDetector : UITriggerDetector
    {
        private AltarInput _altar;

        protected override void Start()
        {
            base.Start();
            _altar = _referenceInteractable.GetComponent<AltarInput>();
        }
        
        protected override void OnTriggerStay(Collider other)
        {
            base.OnTriggerStay(other);
            var player = other.GetComponent<Controller>();
            if (!player) return;
            
            if (_dot > -0.5)
            {
                Debug.Log("Activate altar input");
                _altar.ActivateInteractInput();
            }
            else
            {
                Debug.Log("DeActivate altar input");
                _altar.DeactivateInteractInput();
            }
        }

        private void OnDisable()
        {
            _altar.Interact.action.Disable();
        }
    }
}