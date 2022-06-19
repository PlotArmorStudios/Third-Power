using UnityEngine;

public abstract class Obstacle : MonoBehaviour
{
    [Tooltip("Used to identify this particular object. Naming convention: 'Button 1.2' for the second button in the first level.")]
    [SerializeField] protected string _obstacleID;
}