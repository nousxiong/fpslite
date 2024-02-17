using UnityEngine;

public class Climbing : MonoBehaviour
{
    [HideInInspector] public bool mechanicEnabled = true;

    [HideInInspector] public bool canClimb;
    private PlayerController _playerController;
    [Range(0, 35)] public float climbingSpeed;
    public LayerMask climbingLayerMask;

    private PlayerState _playerState;
    private Collisions _collisions;
    private bool _climbing, _resetMechanics, _managedMechanics;
    private Vector3 _climbingMovement, _input,_lastMoveDir;
    private int _mechanicIndex;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _playerState = _playerController.playerState;
        _collisions = _playerController.collisions;
    }
    void Start()
    {
        mechanicEnabled = true;
        canClimb = true;
        _collisions.OnLand += ResetClimbingVelocity;
        _mechanicIndex = _playerController.velocityVectorsCount;
        _playerController.velocityVectorsCount++;
    }
    void Update()
    {
        if (!mechanicEnabled)
        {
            SetVelocityVector(Vector3.zero);
            _climbing = false;
            return;
        }
        SetClimbingMovementVector();
        SetClimbingVelocity();
        ManageMovementMechanics();
        CheckForClimbing();
    }
    private void CheckForClimbing()
    {
        RaycastHit _hit = _collisions.frontHit;
        bool _frontCollision = _collisions.frontCollision;
        bool _climbableSurface = false;
        if (_frontCollision)
        {
            _climbableSurface = (climbingLayerMask.value & (1 << _hit.transform.gameObject.layer)) > 0;
        }

        if (_frontCollision && _climbableSurface && canClimb)
        {
            _input = _playerController.inputManager.moveInput;
            Vector3 _moveDir = (transform.right * _input.x + transform.forward * _input.y);
            _lastMoveDir = (_moveDir != Vector3.zero) ? _moveDir : _lastMoveDir;
            float _inputlAngle = Vector3.Angle(_lastMoveDir, _hit.normal);

            _climbing = (!_playerState.crouching && _inputlAngle > 90);
        }
        else 
        { 
            _climbing = false; 
        }
    }
    private void SetClimbingMovementVector()
    {
        if (!_climbing) return;
        _input = _playerController.inputManager.moveInput;
        Vector3 _dir = Vector3.Cross(_collisions.frontHit.normal, -transform.right);
        Vector3 _moveVector = (transform.right * _input.x + _dir * _input.y);

        _climbingMovement = Vector3.ClampMagnitude(_moveVector, 1) * climbingSpeed;
        _playerController.jumping.jumpDirection = -transform.forward + Vector3.up;
    }
    private void SetClimbingVelocity()
    {
        Vector3 _moveVector = _climbing ? _climbingMovement : Vector3.zero;
        SetVelocityVector(_moveVector);
    }
    private void ManageMovementMechanics()
    {
        if (_climbing && !_managedMechanics)
        {
            _resetMechanics = false;
            _managedMechanics = true;
            _playerController.defaultMovement.mechanicEnabled = false;
            _playerController.slopeSliding.mechanicEnabled = false;
            _playerController.gravity.mechanicEnabled = false;
            _playerController.sliding.mechanicEnabled = false;
            _playerController.jumping.ResetJumping();
            _playerController.jumping.forceJump = true;
            _playerState.climbing = true;
        }
        else if (!_resetMechanics && !_climbing)
        {
            _playerController.defaultMovement.mechanicEnabled = true;
            _playerController.slopeSliding.mechanicEnabled = true;
            _playerController.gravity.mechanicEnabled = true;
            _playerController.sliding.mechanicEnabled = true;
            _playerState.climbing = false;
            _resetMechanics = true;
            _managedMechanics = false;
            _playerController.jumping.jumpDirection = Vector3.up;
            _playerController.jumping.forceJump = false;
        }
    }
    private void ResetClimbingVelocity()
    {
        SetVelocityVector(Vector3.zero);
    }
    public void EnableMovementMechanic(bool value)
    {
        mechanicEnabled = value;
    }
    public void SetVelocityVector(Vector3 vector)
    {
        _playerController.velocities[_mechanicIndex] = vector;
    }
}
