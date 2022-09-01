using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListenerFollowCamera : MonoBehaviour
{
    public static ListenerFollowCamera Instance;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SetCamera()
    {
        transform.position = Camera.main.transform.position;
        transform.forward = Camera.main.transform.forward;
    }

    private void Update()
    {
        SetCamera();
    }

}
