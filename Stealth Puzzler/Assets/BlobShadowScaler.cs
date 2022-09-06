using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BlobShadowScaler : MonoBehaviour
{
    [SerializeField] private DecalProjector _decal;
    [SerializeField] private float _groundDistanceScale = 1;
    [SerializeField] private Transform _feet;
    private Vector3 _height;

    private void Update()
    {
        var position = _feet.position;
        position.y = _height.y;
        _decal.transform.position = position;
        
        var playerDistanceToGround = Vector3.Distance(_feet.position, _height);
        if (playerDistanceToGround < .1f)
            return;
#if DebugLog
        Debug.Log("Player distance to ground is: " + playerDistanceToGround);
#endif
        if (playerDistanceToGround < .45f)
            playerDistanceToGround = .4f;
        
        var distanceScale = _groundDistanceScale / playerDistanceToGround;
#if DebugLog
        Debug.Log("Distance scale is: " + distanceScale);
#endif
        _decal.transform.localScale = new Vector3(distanceScale, distanceScale, distanceScale);
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, out hit)) _height = hit.point;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_height, .5f);
    }
}