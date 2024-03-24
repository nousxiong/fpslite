using UnityEngine;
public class CameraLook : MonoBehaviour
{
    public bool IsVerticalRotating => canRotateCamera && Mathf.Abs(inputManager.cameraInput.y) > 0.001f;
    
    Transform _playerTransform;
    // PlayerController _playerController;
    [HideInInspector] public InputManager inputManager;
    [Range(0,100)] public float sensitivity;
    [HideInInspector] public float sensitivityMultiplier;
    float _xRotation;
    [HideInInspector] public Vector2 rot, additionalRot;
    [HideInInspector] public bool canRotateCamera;
    Transform _cameraPositionReference;
    void Awake()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        // _playerController = _playerTransform.gameObject.GetComponent<PlayerController>();
        _cameraPositionReference = _playerTransform.GetComponent<SetCameraReferencePosition>().cameraReference;
        // inputManager = _playerController.inputManager;
        inputManager = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>();
        _playerTransform.forward = transform.parent.transform.forward;
    }
    void Start() 
    {
        sensitivityMultiplier = 1;
        canRotateCamera = true;
    }
    void Update() 
    {
        SetRotationValues();
    }
    void LateUpdate()
    {
        RotateCamera();
        SetPosition(_cameraPositionReference.position);
    }
    void SetRotationValues()
    {
        if (!canRotateCamera)
        {
            return;
        }
        Vector3 input = inputManager.cameraInput;
        rot.x -= input.x * (sensitivity* sensitivityMultiplier) * Time.fixedDeltaTime   ;
        rot.x = Mathf.Clamp(rot.x+ additionalRot.x, -90, 90);
        rot.y = input.y * (sensitivity * sensitivityMultiplier) + additionalRot.y;
        _xRotation = transform.rotation.x + rot.x;
    }
    void RotateCamera()
    {
        if( canRotateCamera)
        {
            transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);
            _playerTransform.Rotate(_playerTransform.up, rot.y*Time.fixedDeltaTime);
            transform.parent.parent.Rotate(_playerTransform.up, rot.y * Time.fixedDeltaTime);
        }
    }
    void SetPosition(Vector3 position)
    {
        transform.parent.parent.position = position;
    }

}