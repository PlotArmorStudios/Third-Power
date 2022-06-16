using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    [Header("Movement/Cam Rotation Sync")]
    public Camera Cam;
    public Transform CamTransform;
}