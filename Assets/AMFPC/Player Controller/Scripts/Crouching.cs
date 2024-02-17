using UnityEngine;

public class Crouching : MonoBehaviour
{
    [HideInInspector] public bool mechanicEnabled;

    private PlayerController _playerController;
    public CrouchMode crouchMode;
    [Range(1,100)] 
    public float crouchingSpeed;
    [Range(0, 5)] 
    public float crouchHeight;
    public enum CrouchMode
    {
        Hold, Toggle
    }

    private CharacterController _CC;
    private Collisions _collisions;
    private float _targetHeight, _refHeight, _lastRefHeight, _CCInitialHeight;
    private bool _isCrouching;
    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }
    void Start()
    {
        mechanicEnabled = true;
        _CC = _playerController.characterController;
        _refHeight = _CCInitialHeight = _lastRefHeight = _playerController.CCInitialHeight;
        _collisions = _playerController.collisions;
        _playerController.inputManager.onCrouchInputDown.AddListener(() => Crouch(!_isCrouching));
        GetComponent<HealthManager>().OnDeath.AddListener(() => SetMechanic(false));
        _targetHeight = _playerController.CCInitialHeight;
    }
    private void LateUpdate()
    {
        CancelHoldCrouch(); // uncrouch when no crouch input / hold mode
    }
    private void FixedUpdate()
    {
        ManageCrouching();
    }
    private void ManageCrouching()
    {
        _refHeight = Mathf.Lerp(_refHeight, _targetHeight,crouchingSpeed*Time.deltaTime);
        float delta = _refHeight - _lastRefHeight;
        _lastRefHeight = _refHeight;
        _CC.height += delta;
        if(_collisions.isGrounded)
        _CC.transform.position = transform.position + 0.5f * delta * Vector3.up;
    }
    private void CancelHoldCrouch()
    {
        if (crouchMode == CrouchMode.Hold && !_playerController.inputManager.crouch && _isCrouching)
        Crouch(false);
    }
    public void Crouch(bool value)
    {
        if (!mechanicEnabled)
        {
            _targetHeight = _CCInitialHeight;
            _playerController.playerState.crouching = _isCrouching = false;
            return;
        }
        
        bool _spaceAboveClear = _collisions.aboveDistance >= (_CCInitialHeight / 2 + _CC.skinWidth);
        if (_isCrouching && !_spaceAboveClear) return;
        _isCrouching = value;
        _targetHeight = _isCrouching ? crouchHeight : _CCInitialHeight;
        _playerController.playerState.crouching = _isCrouching;
    }
    public void EnableMovementMechanic(bool value)
    {
        mechanicEnabled = value;
    }
    private void SetMechanic(bool value)
    {
        mechanicEnabled = value;
    }

}