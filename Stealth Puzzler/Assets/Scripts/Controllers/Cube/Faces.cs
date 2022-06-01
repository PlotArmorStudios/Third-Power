using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Faces : MonoBehaviour
{
    [SerializeField] private List<Reflector> _faces;

    public Reflector GetReflector()
    {
        return _faces[0].GetComponent<Reflector>();
    }
}

