using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpherePointGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _objectToMove;
    [SerializeField] private float _moveSpeed = 3;
    [SerializeField] private float _rotateSpeed = 10;
    [Tooltip("The minimum time a Harpy will remain still for before moving again")][SerializeField] private float _minWaitTime = 3f;
    [Tooltip("The maximum time a Harpy will remain still for before moving again")][SerializeField] private float _maxWaitTime = 7f;
    private MeshRenderer _mr;
    
    private void Awake() 
    {
        _mr = GetComponent<MeshRenderer>();
        _mr.enabled = false;
    }

    private void Start()
    {
        StartCoroutine(MoveObject());
    }

    private IEnumerator MoveObject()
    {
        while(true)
        {
         
            Vector3 newLocation = transform.TransformPoint(Random.insideUnitSphere / 2);
            Vector3 rotationDiff = newLocation - _objectToMove.transform.position;
            Vector3 lookPos = newLocation - _objectToMove.transform.position;
            lookPos.y = 0;
            //_objectToMove.transform.rotation = Quaternion.LookRotation(lookPos);
            Quaternion targetRot = Quaternion.LookRotation(lookPos);
            while (Vector3.Distance(_objectToMove.transform.position, newLocation) > 0.02)
            {
                _objectToMove.transform.position = Vector3.MoveTowards(_objectToMove.transform.position, newLocation, Time.deltaTime * _moveSpeed);
                _objectToMove.transform.rotation = Quaternion.Slerp(_objectToMove.transform.rotation, targetRot, Time.deltaTime * _rotateSpeed);
                yield return null;
            }
            yield return new WaitForSeconds(Random.Range(_minWaitTime, _maxWaitTime));
        }
    }
}
