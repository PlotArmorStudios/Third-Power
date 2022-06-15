using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth _playerHealth;
    public LayerMask WhatIsEnemy;
    //States
    public bool IsDead;
    public float LowDangerRange, MedDangerRange, HighDangerRange;
    public bool PlayerInLowDanger, PlayerInMediumDanger, PlayerInHighDanger;

    //so UI icons face camera
    public Camera MainCamera;

    //placeholder UI icons
    [SerializeField] private GameObject _lowDangerIcon, _medDangerIcon, _highDangerIcon;
    private void OnEnable()
    {
        _playerHealth.OnDie += SetInactiveAlertUI;
        _playerHealth.OnDie += ToggleDeath;
        MainCamera = Camera.main;
    }

    private void ToggleDeath()
    {
        IsDead = true;
    }

    private void OnDisable()
    {
        _playerHealth.OnDie -= SetInactiveAlertUI;
        _playerHealth.OnDie -= ToggleDeath;
    }

    // Update is called once per frame
    void Update()
    {
        //Check if Player in low danger
        PlayerInLowDanger = Physics.CheckSphere(transform.position, LowDangerRange, WhatIsEnemy);

        //Check if Player in medium danger
        PlayerInMediumDanger = Physics.CheckSphere(transform.position, MedDangerRange, WhatIsEnemy);

        //Check if Player in high danger
        PlayerInHighDanger = Physics.CheckSphere(transform.position, HighDangerRange, WhatIsEnemy);
        
        if (IsDead) return;
        
        if (!PlayerInLowDanger) //No danger
        {
            SetInactiveAlertUI();
            return;
        }
        if (PlayerInLowDanger && !PlayerInMediumDanger && !PlayerInHighDanger) //low danger
        {
            _lowDangerIcon.SetActive(true);
            _lowDangerIcon.transform.LookAt(MainCamera.transform.position);
        }
        else
            _lowDangerIcon.SetActive(false);

        if (PlayerInMediumDanger && !PlayerInHighDanger) //med danger
        {
            _medDangerIcon.SetActive(true);
            _medDangerIcon.transform.LookAt(MainCamera.transform.position);
        }
        else
            _medDangerIcon.SetActive(false);
        
        if (PlayerInHighDanger) //high danger
        {
            _highDangerIcon.SetActive(true);
            _highDangerIcon.transform.LookAt(MainCamera.transform.position);
        }
        else
            _highDangerIcon.SetActive(false);
    }

    private void SetInactiveAlertUI()
    {
        _lowDangerIcon.SetActive(false);
        _medDangerIcon.SetActive(false);
        _highDangerIcon.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, LowDangerRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, MedDangerRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, HighDangerRange);
    }
}
