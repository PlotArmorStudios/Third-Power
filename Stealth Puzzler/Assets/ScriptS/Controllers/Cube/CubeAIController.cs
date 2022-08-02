using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeAIController : MonoBehaviour
{
    public float VerticalInput;
    [SerializeField] private float _rollSpeed = 5;

    [Header("Grid Snapping")] [SerializeField]
    private float _snapSpeed = .3f;

    public Rigidbody Rigidbody;
    private List<Vector3> _directions = new List<Vector3>();
    private Vector3 _newdirection;

    private Grid _grid;
    public Drop Drop { get; set; }

    public float FallTimer { get; set; }
    [field: SerializeField] public bool IsFalling { get; set; }

    public bool IsMoving { get; set; }

    private void OnEnable()
    {
        SnapToGrid();
    }

    private void OnValidate()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    private void Awake()
    {
        _grid = FindObjectOfType<Grid>();
    }

    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Drop = GetComponent<Drop>();

        //Initialize directions to snap to
        _directions.Add(Vector3.forward);
        _directions.Add(Vector3.back);
        _directions.Add(Vector3.right);
        _directions.Add(Vector3.left);
    }

    private void Update()
    {
        if (IsMoving) return;

        if (VerticalInput >= 0.1f) Tumble(Vector3.right);
        if (IsFalling) FallTimer += Time.deltaTime;
        else FallTimer = 0;
    }

    public void SetForwardInput()
    {
        VerticalInput = 1;
    }

    public void ResetForwardInput()
    {
        VerticalInput = 0;
    }
    
    private void Tumble(Vector3 dir)
    {
        float largestDot = 0;
        int closestAxis = 0;

        for (int currentAxis = 0; currentAxis < _directions.Count; currentAxis++)
        {
            var dot = Vector3.Dot(dir, _directions[currentAxis]);
            if (dot > largestDot)
            {
                largestDot = dot;
                closestAxis = currentAxis;
            }
        }

        _newdirection = _directions[closestAxis];

        var anchor = Rigidbody.transform.position + (Vector3.down + _newdirection) * 0.5f;
        var axis = Vector3.Cross(Vector3.up, _newdirection);
        PlayTumbleSound();
        StartCoroutine(Roll(anchor, axis));
    }

    private IEnumerator Roll(Vector3 anchor, Vector3 axis)
    {
#if UNITY_EDITOR
        float currTime = Time.time;
#endif

        //enable the ability to be detected by enemies
        ControllerManager.Instance.ActivatePlayer();

        IsMoving = true;
        float rotationRemaining = 90;

        while (rotationRemaining > 0)
        {
            float rotateAmount = Mathf.Min(Time.deltaTime * _rollSpeed, rotationRemaining);
            Rigidbody.transform.RotateAround(anchor, axis, rotateAmount);
            rotationRemaining -= rotateAmount;
            yield return null;
        }

        SnapToGrid();
        IsMoving = false;

        //disable the ability to be detected by enemies
        ControllerManager.Instance.DeactivatePlayer();
#if UNITY_EDITOR && DEBUGLOG
        Debug.Log("Roll took " + (Time.time - currTime) + "seconds");
#endif
    }

    private void PlayTumbleSound()
    {
        AkSoundEngine.PostEvent("Play_Cube_Movement", gameObject);
    }

    private void SnapToGrid()
    {
        Vector3 snappedPostion = _grid.GetNearestPointOnGrid(Rigidbody.transform.position);
        snappedPostion.y = Rigidbody.transform.position.y;
        Rigidbody.transform.position = Vector3.Lerp(Rigidbody.transform.position, snappedPostion, _snapSpeed);
#if DEBUGLOG
        Debug.Log("Snapped to " + _grid.GetNearestPointOnGrid(Rigidbody.transform.position));
#endif
    }

    public void StartCube()
    {
        VerticalInput = 1;
    }

    public void StopCube()
    {
        VerticalInput = 0;
    }
}