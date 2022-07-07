using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace UITrigger
{
    public class UITriggerDetector : MonoBehaviour
    {
        [SerializeField] protected GameObject _referenceInteractable;
        [SerializeField] private GameObject _canvasObject;
        
        private GameObject _uiObject;
        protected float _dot;

        private void OnValidate()
        {
            if (_referenceInteractable)
                transform.rotation = _referenceInteractable.transform.rotation;
        }

        protected virtual void Start()
        {
            if (_referenceInteractable)
                transform.rotation = _referenceInteractable.transform.rotation;

            // var canvas = Instantiate(_canvasObject, transform.position, Quaternion.identity);
            // canvas.transform.parent = transform;
            // canvas.GetComponent<CanvasObject>().InteractText.text = _interactableText;
            _uiObject = _canvasObject;
            _uiObject.gameObject.SetActive(false);
        }

        private void OnTriggerExit(Collider other)
        {
            _uiObject.gameObject.SetActive(false);
        }

        protected virtual void OnTriggerStay(Collider other)
        {
            var player = other.GetComponent<Controller>();
            if (!player) return;

            var playerForward = transform.worldToLocalMatrix.MultiplyVector(player.gameObject.transform.forward);
            var triggerForward = transform.worldToLocalMatrix.MultiplyVector(gameObject.transform.forward);
            _dot = Vector3.Dot(playerForward, triggerForward);

#if DebugLog
            Debug.Log("Dot: " + _dot);
#endif

            if (_dot > -0.5) _uiObject.gameObject.SetActive(true);
            else _uiObject.gameObject.SetActive(false);
        }
    }
}