using UnityEngine;

public class WaypointObject : MonoBehaviour
{
    public enum Mode
    {
        Loop,
        PingPong,
        Once
    }

    [SerializeField] private WayPoints _wayPoints;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _delayAtPoints = 0;
    
    [Tooltip("Allows object to move to next waypoint at shorter or farther distances. Allows for curved movement.")]
    [SerializeField] private float _distanceThreshold = .1f;
    
    private Transform _currentWayPoint;
    private bool waiting;

    public bool IsActive;
    [SerializeField] private Mode mode;

    private void Start()
    {
        _currentWayPoint = _wayPoints.GetNextWayPoint(_currentWayPoint);
        transform.position = _currentWayPoint.position;

        _currentWayPoint = _wayPoints.GetNextWayPoint(_currentWayPoint);
    }

    private void Update()
    {
        if (!IsActive || waiting) return;
        
        transform.position = Vector3.MoveTowards(transform.position, _currentWayPoint.position, _moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, _currentWayPoint.position) < _distanceThreshold)
        {
            _currentWayPoint = _wayPoints.GetNextWayPoint(_currentWayPoint, mode);
            if (_delayAtPoints > 0)
            {
                waiting = true;
                Invoke("DoneWaiting", _delayAtPoints);
            }
        }
    }
    
    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    private void DoneWaiting()
    {
        waiting = false;
    }
}