using UnityEngine;

public class ToggleComponents : MonoBehaviour
{
    private Death _death;
    
    private void OnEnable()
    {
        _death = GetComponent<Death>();
        _death.OnDie += ToggleOffComponents;
    }

    private void OnDisable()
    {
        _death.OnDie -= ToggleOffComponents;
    }

    public void ToggleOffComponents()
    {
        var behaviours = GetComponents<Behaviour>();
        foreach (var behaviour in behaviours)
        {
            if (behaviour != this)
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