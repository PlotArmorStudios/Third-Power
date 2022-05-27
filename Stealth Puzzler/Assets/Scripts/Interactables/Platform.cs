using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
   [SerializeField] private Vector3 _start;
   [SerializeField] private Vector3 _end;
   [SerializeField] private float _speed;

   private void Update()
   {
      transform.position = Vector3.Lerp(_start, _end, _speed);
   }
}
