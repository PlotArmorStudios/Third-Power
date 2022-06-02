using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TerrainChecker : MonoBehaviour
{
    private enum Current_Terrain { Cement, Wood };


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    

    private void CheckTerrain()
    {
        RaycastHit[] hit;

        hit = Physics.RaycastAll(transform.position, Vector3.down, 0.5f);

        foreach (RaycastHit rayhit in hit)
        {
            if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("Cement"))
            {
                AkSoundEngine.SetSwitch("Material", "Cement", gameObject);
                Debug.Log("Cement");
            }
            else if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("Wood"))
            {
                AkSoundEngine.SetSwitch("Material", "Wood", gameObject);
            }

        }
    }

    void Update()
    {
        CheckTerrain();

    }
}
