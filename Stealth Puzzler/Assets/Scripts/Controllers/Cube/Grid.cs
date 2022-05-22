using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
   [SerializeField] private float size = 1f;
   [SerializeField] private int _gizmoSize = 40;

   public Vector3 GetNearestPointOnGrid(Vector3 position)
   {
      position -= transform.position;
      int xCount = Mathf.RoundToInt(position.x / size);
      int yCount = Mathf.RoundToInt(position.y / size);
      int zCount = Mathf.RoundToInt(position.z / size);

      Vector3 result = new Vector3(xCount*size, yCount * size, zCount * size );
      
      result += transform.position;
      return result;
   }
   
   private void OnDrawGizmos()
   {
      Gizmos.color = Color.yellow;
      
      for (float x = 0; x < _gizmoSize; x += size)
      {
         for (float z = 0; z < _gizmoSize; z += size)
         {
            var point = GetNearestPointOnGrid(new Vector3(x, 0f, z));
            Gizmos.DrawSphere(point, 0.1f);
         }
      }
   }
}
