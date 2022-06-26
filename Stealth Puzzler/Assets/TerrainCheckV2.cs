using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainCheckV2 : MonoBehaviour
{
    [SerializeField] private Transform _leftFoot;
    [SerializeField] private Transform _rightFoot;
    [SerializeField] private float _maxRayDistance = 1f;
    [SerializeField] private LayerMask _cementLayerMask;
    [SerializeField] private LayerMask _woodLayerMask;

    private void FixedUpdate()
    {
        Ray leftFootRay = new Ray(_leftFoot.position, Vector3.down);
        Ray rightFootRay = new Ray(_rightFoot.position, Vector3.down);
        RaycastHit hit;
        //Debug.DrawRay(_leftFoot.position, Vector3.down * _maxRayDistance);
        //Debug.DrawRay(_rightFoot.position, Vector3.down * _maxRayDistance);

        if (Physics.Raycast(leftFootRay, out hit, _maxRayDistance))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Cement"))
            {
              AkSoundEngine.SetSwitch("Material", "Cement", gameObject);
              Debug.Log("Cement");
            }

            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Metal"))
            {
                AkSoundEngine.SetSwitch("Material", "Metal", gameObject);
                Debug.Log("Metal");
            }
        }

        if (Physics.Raycast(rightFootRay, out hit, _maxRayDistance))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Cement"))
            {
                AkSoundEngine.SetSwitch("Material", "Cement", gameObject);
            }

            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Metal"))
            {
                AkSoundEngine.SetSwitch("Material", "Metal", gameObject);
            }
        }
    }
}
