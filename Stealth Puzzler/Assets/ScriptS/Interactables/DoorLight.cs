using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLight : MonoBehaviour
{
    private static Color _offColour, _onColour, _onEmission, _offEmission;
    private Material _material;
    
    private void Awake() 
    {
        _material = GetComponent<Renderer>().material;
        _offColour = new Color(1f,0,0);
        _offEmission = new Color(0.5f,0,0);
        _onColour = new Color(0,1f,0);
        _onEmission = new Color(0,0.5f,0);
        TurnOff();
    }

    public void TurnOn()
    {
        _material.color = _onColour;
        _material.SetColor("_EmissionColor", _onEmission);
    }

    public void TurnOff()
    {
        _material.color = _offColour;
        _material.SetColor("_EmissionColor", _offEmission);
    }
}
