using UnityEngine;

public class UITriggerDetector : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        var playerForward = other.gameObject.transform.forward;
        var triggerForward = gameObject.transform.forward;
        var dot = Vector3.Dot(playerForward, triggerForward);
        if(dot > 0)
            Debug.Log("Show UI");
    }
}