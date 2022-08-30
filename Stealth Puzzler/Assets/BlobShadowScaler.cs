using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BlobShadowScaler : MonoBehaviour
{
    [SerializeField] private float _groundDistanceScale = 1;
    private PlayerController _player;
    private DecalProjector _decal;
    private Vector3 _height;

    private void OnEnable()
    {
        _decal = GetComponent<DecalProjector>();
        _player = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, out hit, 1000)) _height = hit.point;
    
        var position = transform.position;
        position.y = _height.y;
        transform.position = position;
        
        // var playerDistanceToGround = Vector3.Distance(_player.transform.position, _height);
        // if (playerDistanceToGround < .1f)
        //     return;
        //
        // var distanceScale = _groundDistanceScale / playerDistanceToGround;
        // transform.localScale = new Vector3(distanceScale, distanceScale, distanceScale);
    }
}