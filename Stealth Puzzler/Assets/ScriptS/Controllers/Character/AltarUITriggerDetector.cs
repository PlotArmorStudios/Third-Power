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
            
            if (_dot > -0.5) _altar.Interact.action.Enable();
            else _altar.Interact.action.Disable();
        }

        private void OnDisable()
        {
            _altar.Interact.action.Disable();
        }
    }
}