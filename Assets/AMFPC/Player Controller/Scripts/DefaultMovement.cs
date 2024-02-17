using UnityEngine;
public class DefaultMovement : MonoBehaviour
{
    [HideInInspector] public bool mechanicEnabled;

    private PlayerController playerController;
    [Range(0, 35)] public float runSpeed, walkSpeed, strafeSpeed, crouchSpeed, crouchStrafeSpeed;
    [Range(0, 1)] public float strafeControl;

    [HideInInspector] public float speed;
    [HideInInspector] public bool forceWalkSpeed, canRun, canDefaultMove;
    [HideInInspector] public Vector3 moveDir;

    private Vector3 _moveVector, _strafeMoveVector, _lastMoveDir;
    private int _mechanicIndex;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        mechanicEnabled = canDefaultMove = canRun = true;
        _mechanicIndex = playerController.velocityVectorsCount;
        playerController.velocityVectorsCount++;
        playerController.collisions.OnLand += ResetLastMoveVector;
        GetComponent<HealthManager>().OnDeath.AddListener(() => SetMechanic(false));
    }
    void Update()
    {
        SetMoveVector();
        ManagePlayerSpeed();
        SetStrafeVelocity();
    }
    private void ManagePlayerSpeed()
    {
        Vector2 _input = playerController.inputManager.moveInput;
        if (_input == Vector2.zero)
        {
            speed = walkSpeed;
            return;
        }
        float _standingSpeed = (Mathf.Abs(_input.x) > _input.y) ? walkSpeed : runSpeed;
        _standingSpeed = forceWalkSpeed ? walkSpeed : _standingSpeed;
        float _groundSpeed = playerController.playerState.crouching ? crouchSpeed : _standingSpeed;
        float _inAirSpeed = playerController.playerState.crouching ? crouchStrafeSpeed : strafeSpeed;
        speed = playerController.collisions.isGrounded ? _groundSpeed : _inAirSpeed;
    }
    private void SetMoveVector()
    {
        if (!mechanicEnabled)
        {
            _moveVector = _strafeMoveVector = Vector3.zero;
            SetVelocityVector(_moveVector);
            return;
        }
        Vector2 _input = playerController.inputManager.moveInput;
        moveDir = (transform.right * _input.x + transform.forward * _input.y);

        if (canDefaultMove)
        {
            if (playerController.collisions.slopeAngle > playerController.characterController.slopeLimit)
            {
                _moveVector = Vector3.ProjectOnPlane(moveDir, Vector3.up).normalized * speed;
            }
            else
            {
                _moveVector = Vector3.ProjectOnPlane(moveDir, playerController.collisions.slopeNormal).normalized * speed;
            }
        }
        else
        {
            _moveVector = Vector3.zero;
        }
        _moveVector = playerController.collisions.isGrounded ? _moveVector : _strafeMoveVector;
        SetVelocityVector(_moveVector);
    }
    private void SetStrafeVelocity()
    {
        if (playerController.collisions.isGrounded)
        {
            _lastMoveDir = moveDir;
        }
        _lastMoveDir = (_lastMoveDir + moveDir * strafeControl / 10f).normalized;
        _strafeMoveVector = _lastMoveDir * speed;
    }
    private void ResetLastMoveVector()
    {
        _lastMoveDir = Vector3.zero;
    }
    public void SetVelocityVector(Vector3 vector)
    {
        playerController.velocities[_mechanicIndex] = vector;
    }
    private void SetMechanic(bool value)
    {
        mechanicEnabled = value;
    }
}
