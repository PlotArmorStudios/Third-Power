using UnityEngine;

public class WaypointObject : MonoBehaviour
{
    [SerializeField] private WayPoints _wayPoints;
    [SerializeField] private float _moveSpeed = 5f;
    
    [SerializeField] private float _distanceThreshold = .1f;
    private Transform _currentWayPoint;

    public bool IsActive;
    private void Start()
    {
        _currentWayPoint = _wayPoints.GetNextWayPoint(_currentWayPoint);
        transform.position = _currentWayPoint.position;

        _currentWayPoint = _wayPoints.GetNextWayPoint(_currentWayPoint);
    }

    private void Update()
    {
        if (!IsActive) return;
        
        transform.position = Vector3.MoveTowards(transform.position, _currentWayPoint.position, _moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, _currentWayPoint.position) < _distanceThreshold)
        {
            _currentWayPoint = _wayPoints.GetNextWayPoint(_currentWayPoint);
        }
    }
}