using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace UITrigger
{
    public class UITriggerDetector : MonoBehaviour
    {
        private GameObject _uiObject;
        [SerializeField] private GameObject _referenceInteractable;
        [SerializeField] private GameObject _canvasObject;
        [SerializeField] private string _interactableText;

        private void OnValidate()
        {
            if (_referenceInteractable)
                transform.rotation = _referenceInteractable.transform.rotation;
        }

        private void Start()
        {
            if (_referenceInteractable)
                transform.rotation = _referenceInteractable.transform.rotation;

            var canvas = Instantiate(_canvasObject, transform.position, Quaternion.identity);
            canvas.transform.parent = transform;
            canvas.GetComponent<CanvasObject>().InteractText.text = _interactableText;
            _uiObject = canvas;
            canvas.gameObject.SetActive(false);
        }

        private void OnTriggerExit(Collider other)
        {
            _uiObject.gameObject.SetActive(false);
        }

        private void OnTriggerStay(Collider other)
        {
            var player = other.GetComponent<Controller>();
            if (!player) return;

            var playerForward = transform.worldToLocalMatrix.MultiplyVector(player.gameObject.transform.forward);
            var triggerForward = transform.worldToLocalMatrix.MultiplyVector(gameObject.transform.forward);
            var dot = Vector3.Dot(playerForward, triggerForward);

            Debug.Log("Dot: " + dot);

            if (dot > -0.5) _uiObject.gameObject.SetActive(true);
            else _uiObject.gameObject.SetActive(false);
        }
    }
}