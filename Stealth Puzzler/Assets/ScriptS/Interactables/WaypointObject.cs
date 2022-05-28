using UnityEngine;

public class WaypointObject : MonoBehaviour
{
    [SerializeField] private WayPoints _wayPoints;
    [SerializeField] private float _moveSpeed = 5f;
    
    private Transform _currentWayPoint;
    [SerializeField] private float _distanceThreshold = .1f;

    private void Start()
    {
        _currentWayPoint = _wayPoints.GetNextWayPoint(_currentWayPoint);
        transform.position = _currentWayPoint.position;

        _currentWayPoint = _wayPoints.GetNextWayPoint(_currentWayPoint);
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _currentWayPoint.position, _moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, _currentWayPoint.position) < _distanceThreshold)
        {
            _currentWayPoint = _wayPoints.GetNextWayPoint(_currentWayPoint);
        }
    }
}