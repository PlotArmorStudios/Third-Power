using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var player = other.gameObject.GetComponent<PlayerController>();
        var cube = other.gameObject.GetComponent<CubeController>();
        
        if (cube)
            ControllerManager.Instance.SwitchControllers();
        if (player && player.IsVulnerable)
            player.GetComponent<Health>().TakeHit();
    }
}