using UnityEngine;

public class FogCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var player = other.gameObject.GetComponent<PlayerController>();
        var cube = other.gameObject.GetComponent<CubeController>();

        if (cube)
            ControllerManager.Instance.ForceSwitch();
        if (player)
            player.GetComponent<Health>().TakeHit();
    }
}