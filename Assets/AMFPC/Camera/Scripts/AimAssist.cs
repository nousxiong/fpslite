using UnityEngine;

// ReSharper disable once CheckNamespace
public class AimAssist : MonoBehaviour
{
    public Vector3 boxHalfExtents = new Vector3(.5f, 4f, .5f);
    [Range(0, 2)] public float radius;
    bool targetDetected;
    Transform target;
    CameraLook cameraLook;

    float xAngle, yAngle;
    [Range(0,100)] public float strength;
    Vector2 targetRot;
    InputManager inputManager;
    public bool off;

    void Awake()
    {
        inputManager = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>();
    }

    void Start()
    {
        cameraLook = GetComponent<CameraLook>();
        inputManager.onAimAssistEnabled.AddListener(Enable);
        inputManager.onAimAssistDisabled.AddListener(Disable);
    }

    void Enable()
    {
        targetDetected = false;
        off = false;
    }

    void Disable()
    {
        targetDetected = false;
        target = null;
        cameraLook.additionalRot = Vector3.zero;
        off = true;
    }

    void Update()
    {
        if (off)
        {
            return;
        }
        SetAimAssistValues();
        SetTargetRotationValues();
    }
    void FixedUpdate()
    {
        if (off)
        {
            return;
        }
        RaycastAimAssistSphere();
    }
    
    void RaycastAimAssistSphere()
    {
        if (!Physics.BoxCast(
                transform.position,
                boxHalfExtents,
                transform.forward, 
                out RaycastHit hit, 
                transform.rotation,
                99,
                LayerMask.GetMask("Enemy")))
        {
            targetDetected = false;
            return;
        }
        // if (!Physics.SphereCast(
        //         transform.position, 
        //         radius, 
        //         transform.forward, 
        //         out RaycastHit hit, 
        //         99,
        //         LayerMask.GetMask("Enemy")))
        // {
        //     targetDetected = false;
        //     return;
        // }
        targetDetected = hit.transform.gameObject.GetComponent<AimAssistTarget>();
        if (targetDetected)
        {
            target = hit.transform.gameObject.GetComponent<AimAssistTarget>().target;
        }
    }
    void SetAimAssistValues()
    {
        Vector3 lookDir = targetDetected ? (target.position - transform.position) : transform.forward; 
        Vector3 xProjection = Vector3.ProjectOnPlane(lookDir, transform.up);
        Vector3 yProjection = Vector3.ProjectOnPlane(lookDir, transform.right);

        Vector3 forward = transform.forward;
        xAngle = -(Vector3.Angle(xProjection, forward));
        yAngle = Vector3.Angle(yProjection, forward);

        Vector3 screenXProjection = Vector3.ProjectOnPlane(xProjection, forward);
        Vector3 screenYProjection = Vector3.ProjectOnPlane(yProjection, forward);

        if (Vector3.Dot(screenXProjection.normalized, transform.right) > 0)
        {
            xAngle = -xAngle;
        }
        if (Vector3.Dot(screenYProjection.normalized, transform.up) < 0)
        {
            yAngle = -yAngle;
        }
    }
    void SetTargetRotationValues()
    {
        if(targetDetected)
        {
            targetRot.x = -(yAngle / 5) * strength * Time.fixedDeltaTime;
            targetRot.y = (xAngle * 15) * strength * Time.fixedDeltaTime;
            cameraLook.additionalRot = targetRot;
        }
        else
        {
            cameraLook.additionalRot = Vector3.zero;
        }
    }
}
