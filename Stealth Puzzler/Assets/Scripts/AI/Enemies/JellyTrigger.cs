using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JellyTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _jellyController;
    [SerializeField] private Animator _jellyAnimator;
    private bool _jellyCanExpand = true;
    [SerializeField] private InputActionReference _move;
    [SerializeField] private InputActionReference _switchController;
    private bool _isInvincible = false;
    [SerializeField] private float _invicibilityTimeInitial = 1f;
    [SerializeField] private float _invicibilityTimeLeft;

    [SerializeField] private float _jellyUpwardForce = 7f;
    [SerializeField] private LayerMask _jellyInteractsWith;
    private bool _hasHitGround;

    private void Start()
    {
        
        AnimatorStateInfo state = _jellyAnimator.GetCurrentAnimatorStateInfo(0);//could replace 0 by any other animation layer index
        _jellyAnimator.Play(state.fullPathHash, -1, Random.Range(0f, 1f));
        _invicibilityTimeLeft = 0;
        print(_jellyInteractsWith.value);
    }
    private void Update()
    {
        InvincibilityCheck();
    }

    private void InvincibilityCheck()
    {
        if (_isInvincible)
        {
            _invicibilityTimeLeft -= Time.deltaTime;
        }
        if (_invicibilityTimeLeft < 0)
        {
            _isInvincible = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CubeController>())
        {
            _move.action.Disable();
            _invicibilityTimeLeft = _invicibilityTimeInitial;
            ExpandJelly();
            AkSoundEngine.PostEvent("Play_Jelly_Impact_Cube", gameObject);
        }
        else if (other.gameObject.GetComponent<PlayerController>() && _invicibilityTimeLeft <= 0)
        {
            print("humanoid hit");
            other.gameObject.GetComponent<Health>().TakeHit();
            ExpandJelly();
        }
        else if ((_jellyInteractsWith.value & (1 << other.gameObject.layer)) > 0 && !_hasHitGround)
        {
            _hasHitGround = true;
            Debug.Log("hitting ground sound");
            AkSoundEngine.PostEvent("Play_Jelly_Impact_Cube", gameObject);
        }
    }

    private void ExpandJelly()
    {
        if (_jellyCanExpand)
        {
            _jellyCanExpand = false;
            _jellyAnimator.SetTrigger("Grow");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_switchController.action.triggered)
        {
            _isInvincible = true;
            Rigidbody _rb = GetComponent<Rigidbody>();
            if (_rb)
                _rb.AddForce(Vector3.up * _jellyUpwardForce, ForceMode.Impulse);
        }
    }
}
