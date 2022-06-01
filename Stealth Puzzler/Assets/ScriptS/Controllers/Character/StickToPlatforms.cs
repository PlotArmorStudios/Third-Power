using UnityEngine;

public class StickToPlatforms : MonoBehaviour
{
    //Stick to platforms
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private float _maxRayDistance;
    private Transform _groundedObject;
    private Vector3? _groundedObjectLastPosition;
    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        StickToMovingPlatform();
    }
    
    private void FixedUpdate()
    {
        CheckForObjectBelow();
    }

    private void StickToMovingPlatform()
    {
        if (_groundedObject != null)
        {
            if (_groundedObjectLastPosition.HasValue &&
                _groundedObjectLastPosition.Value != _groundedObject.position)
            {
                Vector3 delta = _groundedObject.position - _groundedObjectLastPosition.Value;
                delta.y = 0;
                _rigidbody.transform.position += delta;
            }

            _groundedObjectLastPosition = _groundedObject.position;
        }
        else
        {
            _groundedObjectLastPosition = null;
        }
    }


    private void CheckForObjectBelow()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        
        var raycastHit = Physics.Raycast(ray, out hit, _maxRayDistance, _groundLayerMask);
        Debug.DrawRay(transform.position, Vector3.down * _maxRayDistance, Color.red);

        if (raycastHit)
        {
            _groundedObject = hit.collider.transform;
        }
        else
        {
            _groundedObject = null;
        }
    }
}