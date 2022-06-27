using UnityEngine;

public class ToggleComponents : MonoBehaviour
{
    private Health _health;
    
    private void OnEnable()
    {
        _health = GetComponent<Health>();
        _health.OnDie += ToggleOffComponents;
    }

    private void OnDisable()
    {
        _health.OnDie -= ToggleOffComponents;
    }

    public void ToggleOffComponents()
    {
        var behaviours = GetComponents<Behaviour>();
        foreach (var behaviour in behaviours)
        {
            if (behaviour != this && !(behaviour is Animator))
                behaviour.enabled = false;
        }
    }

    public void ToggleOnComponents()
    {
        var behaviours = GetComponents<Behaviour>();
        foreach (var behaviour in behaviours)
        {
            behaviour.enabled = true;
        }
    }
}