using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var player = other.gameObject.GetComponent<PlayerController>();
        if (!player) return;
        player.GetComponent<Health>().TakeHit();
    }
}