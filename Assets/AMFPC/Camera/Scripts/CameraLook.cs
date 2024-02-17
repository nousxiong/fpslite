using UnityEngine;
public class CameraLook : MonoBehaviour
{
    private Transform _playerTransform;
    private PlayerController _playerController;
    [HideInInspector] public InputManager inputManager;
    [Range(0,100)] public float sensitivity;
    [HideInInspector] public float sensitivityMultiplier;
    private float _xRotation;
    [HideInInspector] public Vector2 rot, additionalRot;
    [HideInInspector] public bool canRotateCamera;
    private Transform _cameraPositionReference;
    private void Awake()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _playerController = _playerTransform.gameObject.GetComponent<PlayerController>();
        _cameraPositionReference = _playerTransform.GetComponent<SetCameraReferencePosition>().cameraReference;
        inputManager = _playerController.inputManager;
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
    private void LateUpdate()
    {
        RotateCamera();
        SetPosition(_cameraPositionReference.position);
    }
    private void SetRotationValues()
    {
        if (!canRotateCamera) return;
        Vector3 _input = inputManager.cameraInput;
        rot.x -= _input.x * (sensitivity* sensitivityMultiplier) * Time.fixedDeltaTime   ;
        rot.x = Mathf.Clamp(rot.x+ additionalRot.x, -90, 90);
        rot.y = _input.y * (sensitivity * sensitivityMultiplier) + additionalRot.y;
        _xRotation = transform.rotation.x + rot.x;
    }
    private void RotateCamera()
    {
        if( canRotateCamera)
        {
            transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);
            _playerTransform.Rotate(_playerTransform.up, rot.y*Time.fixedDeltaTime);
            transform.parent.parent.Rotate(_playerTransform.up, rot.y * Time.fixedDeltaTime);
        }
    }
    private void SetPosition(Vector3 position)
    {
        transform.parent.parent.position = position;
    }

}