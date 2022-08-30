using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListenerFollowCamera : MonoBehaviour
{
    public static ListenerFollowCamera Instance { get; internal set; }

    // Start is called before the first frame update

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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
