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
        Debug.DrawRay(_leftFoot.position, Vector3.down * _maxRayDistance);
        Debug.DrawRay(_rightFoot.position, Vector3.down * _maxRayDistance);

        if (Physics.Raycast(leftFootRay, out hit, _maxRayDistance))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Cement"))
            {
              AkSoundEngine.SetSwitch("Material", "Cement", gameObject);
              //Debug.Log("Left Cement");
            }

            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Wood"))
            {
                AkSoundEngine.SetSwitch("Material", "Wood", gameObject);
               // Debug.Log("Left Wood");
            }
        }

        if (Physics.Raycast(rightFootRay, out hit, _maxRayDistance))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Cement"))
            {
                AkSoundEngine.SetSwitch("Material", "Cement", gameObject);
               // Debug.Log("Right Cement");
            }

            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Wood"))
            {
                AkSoundEngine.SetSwitch("Material", "Wood", gameObject);
                //Debug.Log("Right Wood");
            }
        }


    }

    // Start is called before the first frame update
   

    //private void CheckTerrain()
    //{

    //    RaycastHit[] hit;

    //    hit = Physics.RaycastAll(transform.position, Vector3.down, 1.0f);

    //    foreach (RaycastHit rayhit in hit)
    //    {
    //        if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("Cement"))
    //        {
    //            Debug.Log("Cement");
    //        }
    //        else if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("Wood"))
    //        {
    //            Debug.Log("Wood");
    //        }
    //    }
    //}


    //// Update is called once per frame
    //void Update()
    //{
    //    CheckTerrain();
    //    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) , Color.red);

    //}
}
