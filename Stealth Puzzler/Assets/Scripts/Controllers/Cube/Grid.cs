using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
   [SerializeField] private float _size = 1f;
   [SerializeField] private int _gizmoSize = 40;
   
   [Range(1, 5)] [SerializeField] private float _gridScaleX = 1f;
   [Range(1, 5)] [SerializeField] private float _gridScaleY = 1f;
   [Range(1, 5)] [SerializeField] private float _gridScaleZ = 1f;

   private Vector3 _defaultPosition = new Vector3(.0f, 0, .0f);
   
   private void OnValidate()
   {
      transform.position = _defaultPosition;
   }

   private void Start()
   {
      transform.position = _defaultPosition;
   }

   public Vector3 GetNearestPointOnGrid(Vector3 position)
   {
      position -= transform.position;
      int xCount = Mathf.RoundToInt(position.x / _gridScaleX);
      int yCount = Mathf.RoundToInt(position.y / _gridScaleY);
      int zCount = Mathf.RoundToInt(position.z / _gridScaleZ);

      Vector3 result = new Vector3(xCount*_gridScaleX, yCount * _gridScaleY, zCount * _gridScaleZ);
      
      result += transform.position;
      return result;
   }
   
   private void OnDrawGizmos()
   {
      Gizmos.color = Color.yellow;
      
      for (float x = 0; x < _gizmoSize; x += _gridScaleX)
      {
         for (float z = 0; z < _gizmoSize; z += _gridScaleZ)
         {
            var point = GetNearestPointOnGrid(new Vector3(x, 0f, z));
            Gizmos.DrawSphere(point, 0.1f);
         }
      }
   }
}
