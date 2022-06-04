using System;
using System.Collections;
using System.Collections.Generic;
using Helpers;
using UnityEngine;

public class SceneLoadTrigger : MonoBehaviour
{
    [SerializeField] private string _sceneToLoad;
    
    private SceneLoader _sceneLoader;

    private void Start()
    {
        _sceneLoader = SceneLoader.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        _sceneLoader.LoadScene(_sceneToLoad);
    }
}
