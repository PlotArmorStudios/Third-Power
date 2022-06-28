using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    [Header("Movement/Cam Rotation Sync")] public Camera Cam;
    public Transform CamTransform;

    public void InitializeCam(Camera main)
    {
        Cam = main;
        CamTransform = main.transform;
    }
}