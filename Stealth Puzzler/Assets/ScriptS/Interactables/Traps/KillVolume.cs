using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillVolume : MonoBehaviour
{
    [SerializeField]
    private float interval;
    private float timeLastHurt;

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
        if (timeLastHurt + interval > Time.time)
        {
            return;
        }
        var player = other.gameObject.GetComponent<PlayerController>();
        var cube = other.gameObject.GetComponent<CubeController>();

        if (cube)
        {
            ControllerManager.Instance.ForceHuman();
            ControllerManager.Instance.SwitchingBlocked = true;
        }
        if (player && player.IsVulnerable)
        {
            player.GetComponent<Health>().TakeHit();
            timeLastHurt = Time.time;
        }
    }
}
