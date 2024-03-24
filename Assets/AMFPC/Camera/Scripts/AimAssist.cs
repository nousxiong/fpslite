using UnityEngine;

// ReSharper disable once CheckNamespace
public class AimAssist : MonoBehaviour
{
    // public Vector3 boxHalfExtents = new Vector3(.5f, 4f, .5f);
    [Range(0, 10)] public float radius;
    bool _targetDetected;
    Transform _target;
    CameraLook _cameraLook;

    float _xAngle, _yAngle;
    [Range(0,100)] public float strength;
    Vector2 _targetRot;
    InputManager _inputManager;
    bool _off;

    void Awake()
    {
        _inputManager = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>();
    }

    void Start()
    {
        _cameraLook = GetComponent<CameraLook>();
        _inputManager.onAimAssistEnabled.AddListener(Enable);
        _inputManager.onAimAssistDisabled.AddListener(Disable);
    }

    void Enable()
    {
        _targetDetected = false;
        _off = false;
    }

    void Disable()
    {
        _targetDetected = false;
        _target = null;
        _cameraLook.additionalRot = Vector3.zero;
        _off = true;
    }

    void Update()
    {
        if (_off)
        {
            return;
        }
        SetAimAssistValues();
        SetTargetRotationValues();
    }
    void FixedUpdate()
    {
        if (_off)
        {
            return;
        }
        RaycastAimAssistSphere();
    }
    
    void RaycastAimAssistSphere()
    {
        // if (!Physics.BoxCast(
        //         transform.position,
        //         boxHalfExtents,
        //         transform.forward, 
        //         out RaycastHit hit, 
        //         transform.rotation,
        //         99,
        //         LayerMask.GetMask("Enemy")))
        // {
        //     targetDetected = false;
        //     return;
        // }
        if (!Physics.SphereCast(
                transform.position, 
                radius, 
                transform.forward, 
                out RaycastHit hit, 
                99,
                LayerMask.GetMask("Enemy")))
        {
            _targetDetected = false;
            return;
        }
        _targetDetected = hit.transform.gameObject.GetComponent<AimAssistTarget>();
        if (_targetDetected)
        {
            _target = hit.transform.gameObject.GetComponent<AimAssistTarget>().target;
        }
    }
    void SetAimAssistValues()
    {
        Vector3 lookDir = _targetDetected ? (_target.position - transform.position) : transform.forward; 
        Vector3 xProjection = Vector3.ProjectOnPlane(lookDir, transform.up);
        Vector3 yProjection = Vector3.ProjectOnPlane(lookDir, transform.right);

        Vector3 forward = transform.forward;
        _xAngle = -(Vector3.Angle(xProjection, forward));
        _yAngle = Vector3.Angle(yProjection, forward);

        Vector3 screenXProjection = Vector3.ProjectOnPlane(xProjection, forward);
        Vector3 screenYProjection = Vector3.ProjectOnPlane(yProjection, forward);

        if (Vector3.Dot(screenXProjection.normalized, transform.right) > 0)
        {
            _xAngle = -_xAngle;
        }
        if (Vector3.Dot(screenYProjection.normalized, transform.up) < 0)
        {
            _yAngle = -_yAngle;
        }
    }
    void SetTargetRotationValues()
    {
        if(_targetDetected)
        {
            _targetRot.x = -(_yAngle / 5) * strength * Time.fixedDeltaTime;
            _targetRot.y = (_xAngle * 15) * strength * Time.fixedDeltaTime;
            _cameraLook.additionalRot = _targetRot;
        }
        else
        {
            _cameraLook.additionalRot = Vector3.zero;
        }
    }
}
