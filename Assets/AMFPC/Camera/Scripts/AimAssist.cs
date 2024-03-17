using System;
using UnityEngine;

public class AimAssist : MonoBehaviour
{
    [Range(0, 2)] public float radius;
    private bool _targetDetected;
    private Transform _target;
    private CameraLook cameraLook;

    private float xAngle, yAngle;
    [Range(0,100)] public float strength;
    private Vector2 _targetRot;

    void Start()
    {
        cameraLook = GetComponent<CameraLook>();
    }

    void OnEnable()
    {
        _targetDetected = false;
    }

    void OnDisable()
    {
        _targetDetected = false;
        _target = null;
        cameraLook.additionalRot = Vector3.zero;
    }

    void Update()
    {
        SetAimAssistValues();
        SetTargetRotationValues();
    }
    private void FixedUpdate()
    {
        RaycastAimAssistSphere();
    }
    public void RaycastAimAssistSphere()
    {
        if (!Physics.SphereCast(
                transform.position, 
                radius, 
                transform.forward, 
                out RaycastHit Hit, 
                99,
                LayerMask.GetMask("Enemy")))
        {
            _targetDetected = false;
            return;
        }
        _targetDetected = Hit.transform.gameObject.GetComponent<AimAssistTarget>();
        if (_targetDetected)
        {
            _target = Hit.transform.gameObject.GetComponent<AimAssistTarget>().target;
        }
    }
    private void SetAimAssistValues()
    {
        Vector3 _lookDir = _targetDetected ? (_target.position - transform.position) : transform.forward; 
        Vector3 X_Projection = Vector3.ProjectOnPlane(_lookDir, transform.up);
        Vector3 Y_Projection = Vector3.ProjectOnPlane(_lookDir, transform.right);

        xAngle = -(Vector3.Angle(X_Projection, transform.forward));
        yAngle = Vector3.Angle(Y_Projection, transform.forward);

        Vector3 Screen_X_Projection = Vector3.ProjectOnPlane(X_Projection, transform.forward);
        Vector3 Screen_Y_Projection = Vector3.ProjectOnPlane(Y_Projection, transform.forward);

        if (Vector3.Dot(Screen_X_Projection.normalized, transform.right) > 0)
        {
            xAngle = -xAngle;
        }
        if (Vector3.Dot(Screen_Y_Projection.normalized, transform.up) < 0)
        {
            yAngle = -yAngle;
        }
    }
    private void SetTargetRotationValues()
    {
        if(_targetDetected)
        {
            _targetRot.x = -(yAngle / 5) * strength * Time.fixedDeltaTime;
            _targetRot.y = (xAngle * 15) * strength * Time.fixedDeltaTime;
            cameraLook.additionalRot = _targetRot;
        }
        else
        {
            cameraLook.additionalRot = Vector3.zero;
        }
    }
}
